using System;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using ReactiveUI;

namespace GoodVibrations.Shared
{
    public class MicrosoftBandService : IMicrosoftBandService, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IBandClient _connectedBand;
        private EmergencyApiClient _emergencyClient;

        public ConnectionState ConnectionState
            => _connectedBand == null ? ConnectionState.Disconnected : ConnectionState.Connected;

        public event EventHandler<WearableConnectionChangedEventArgs> ConnectionStateChanged;

        public MicrosoftBandService(IAuthenticationService authentication, ISuspensionHost suspension)
        {
            // Create a client when credentials change
            authentication.User.Select(val =>
            {
                if (val?.Credentials?.Email == null || val.Credentials.Password == null)
                    return null;

                var credentials = new NetworkCredential(val.Credentials.Email, val.Credentials.Password);

                return new EmergencyApiClient(Const.RobinServiceUrl, credentials);
            }).Subscribe(val => _emergencyClient = val);

            // Combine all app states to a distinct boolean stream so we now when we need to cancel / continue Band connection
            var appState = Observable.Merge(suspension.IsLaunchingNew, suspension.IsResuming, suspension.IsUnpausing).Select(_ => true)
               .Merge(suspension.ShouldPersistState.Select(_ => false)).DistinctUntilChanged();

            // When user change or appState change we either need to connect or disconnect from band
            // On succesful connect we show a nice dialog.
            appState
                .CombineLatest(authentication.User, (started, user) => new {started, user})
                .ObserveOnDispatcher()
                .Subscribe(async val =>
                {
                    if (val?.user == null || !val.started)
                        CancelBandConnection();
                    else
                        await ConnectAndReadData();
                }, ex => {/* TODO: @rherlt Handle exception */});

            // Show a nice popup when we connect to a band
            Observable.FromEventPattern<WearableConnectionChangedEventArgs>(handler => ConnectionStateChanged += handler,
                handler => ConnectionStateChanged -= handler)
                .Select(val => val.EventArgs.ConnectionState)
                .DistinctUntilChanged()
                .Where(state => state == ConnectionState.Connected)
                .ObserveOnDispatcher()
                .Subscribe(async _ =>
                {
                    var box = new MessageDialog(Localize.Text["BandConnected_Info"], Localize.Text["BandConnected_Title"]);
                    await box.ShowAsync();
                });
        }

        public async Task ConnectAndReadData()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            await AddBandTileIfNotExist();
            await ClearTilePages();
            await StartReadingFromBand();
        }

        public void Dispose()
        {
            CancelBandConnection();
        }

        public void CancelBandConnection()
        {
            OnConnectionStateChanged(new WearableConnectionChangedEventArgs(ConnectionState.Disconnected));
            if (_connectedBand != null)
                _connectedBand.TileManager.TileButtonPressed -= HandleTileButtonPressed;

            _connectedBand?.Dispose();
            _cancellationTokenSource?.Cancel();
        }

        public async Task AddBandTile()
        {
            try
            {
                var band = await GetConnectedBand();
                if (band != null)
                {
                    // Connect to Microsoft Band.
                    using (band)
                    {
                        // Create a Tile with a TextButton on it.
                        var tileId = new Guid(Const.BandTileId);
                        var tile = new BandTile(tileId)
                        {
                            Name = Localization.BandTileTitle,
                            TileIcon = await LoadIcon("ms-appx:///Assets/BandShield48x48.png"),
                            SmallIcon = await LoadIcon("ms-appx:///Assets/BandShield24x24.png")
                        };
                        var button = new TextButton {ElementId = 1, Rect = new PageRect(10, 10, 200, 90)};
                        var panel = new FilledPanel(button) {Rect = new PageRect(0, 0, 220, 150)};
                        tile.PageLayouts.Add(new PageLayout(panel));

                        // Create the Tile on the Band.
                        await band.TileManager.AddTileAsync(tile, _cancellationTokenSource.Token);
                        await band.TileManager.SetPagesAsync(tileId, _cancellationTokenSource.Token,
                            new PageData(new Guid(Const.BandPageDataId), 0,
                                new TextButtonData(1, Localization.InitiateEmergencyCall)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RemoveBandTile()
        {
            var band = await GetConnectedBand();
            if (band != null)
            {
                using (band)
                {
                    var tileExits = await CheckIfTileExists(band);
                    if (tileExits)
                    {
                        // Create a Tile with a TextButton on it.
                        var tileId = new Guid(Const.BandTileId);
                        // Remove the Tile from the Band, if present. An application won't need to do this everytime it runs. 
                        // But in case you modify this sample code and run it again, let's make sure to start fresh.
                        await band.TileManager.RemoveTileAsync(tileId, _cancellationTokenSource.Token);
                    }
                }
            }
        }

        protected virtual void OnConnectionStateChanged(WearableConnectionChangedEventArgs e)
        {
            ConnectionStateChanged?.Invoke(this, e);
        }

        private async Task ClearTilePages()
        {
            var band = await GetConnectedBand();
            if (band != null)
            {
                var tileId = new Guid(Const.BandTileId);
                await band.TileManager.RemovePagesAsync(tileId, _cancellationTokenSource.Token);
                await band.TileManager.SetPagesAsync(tileId, _cancellationTokenSource.Token,
                    new PageData(new Guid(Const.BandPageDataId), 0,
                        new TextButtonData(1, Localization.InitiateEmergencyCall)));
            }
        }

        private async void HandleTileButtonPressed(object sender,
            BandTileEventArgs<IBandTileButtonPressedEvent> e)
        {
            if (_emergencyClient == null)
                return;

            var result = await _emergencyClient.InitiateEmergencyCall(CancellationToken.None);

            // Send a notification.
            var callresult = result.StatusCode == HttpStatusCode.OK ? "succeeded" : "failed";
            await
                ((IBandClient) sender).NotificationManager.SendMessageAsync(new Guid(Const.BandTileId), "Robin",
                    $"Call {callresult}",
                    DateTimeOffset.Now);
        }


        private async Task StartReadingFromBand()
        {
            var band = await GetConnectedBand();
            if (band != null)
            {
                band.TileManager.TileButtonPressed -= HandleTileButtonPressed;
                band.TileManager.TileButtonPressed += HandleTileButtonPressed;
                await band.TileManager.StartReadingsAsync(_cancellationTokenSource.Token);
            }
        }

        private async Task AddBandTileIfNotExist()
        {
            var band = await GetConnectedBand();
            if (band != null)
            {
                var tileExits = await CheckIfTileExists(band);
                if (!tileExits)
                    await AddBandTile();
            }
        }

        private async Task<bool> CheckIfTileExists(IBandClient band)
        {
            var tiles = await band.TileManager.GetTilesAsync(_cancellationTokenSource.Token);
            var tileExits = tiles.Any(x => x.TileId == new Guid(Const.BandTileId));
            return tileExits;
        }


        private async Task<IBandClient> GetConnectedBand()
        {
            // Get the list of Microsoft Bands paired to the phone.
            var pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Length < 1)
            {
                //no Bands paired with this device
                return null;
            }

            // Connect to Microsoft Band.
            if (_connectedBand == null)
            {
                _connectedBand = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
                OnConnectionStateChanged(new WearableConnectionChangedEventArgs(ConnectionState.Connected));
            }
            return _connectedBand;
        }

        private static async Task<BandIcon> LoadIcon(string uri)
        {
            var imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

            using (var fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                var bitmap = new WriteableBitmap(1, 1);
                await bitmap.SetSourceAsync(fileStream);
                return bitmap.ToBandIcon();
            }
        }
    }
}