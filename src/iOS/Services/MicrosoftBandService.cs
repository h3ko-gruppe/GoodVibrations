using System;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using GoodVibrations.Enums;
using GoodVibrations.EventArgs;
using GoodVibrations.Interfaces.Services;
using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using ReactiveUI;
using UIKit;

namespace GoodVibrations.iOS
{
    public class MicrosoftBandService : IMicrosoftBandService, IDisposable
    {
        public static readonly string BandTileId = "d10120b9-a692-4b77-a6bd-0c2edaf04ae8";
        public static readonly string BandPageDataId = "348e2117-ef39-4239-bccb-b99b4903f99b";

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        //private Microsoft.Band.Portable.BandClient _connectedBand;
        private BandClient _connectedBand;
        //private EmergencyApiClient _emergencyClient;

        public ConnectionState ConnectionState
            => _connectedBand == null ? ConnectionState.Disconnected : ConnectionState.Connected;

        public event EventHandler<WearableConnectionChangedEventArgs> ConnectionStateChanged;

        public MicrosoftBandService(IAuthentificationSerivce authentication, ISuspensionHost suspension)
        {
            // Create a client when credentials change
            //authentication.User.Select(val =>
            //{
            //    if (val?.Credentials?.Email == null || val.Credentials.Password == null)
            //        return null;

            //    var credentials = new NetworkCredential(val.Credentials.Email, val.Credentials.Password);

            //    return new EmergencyApiClient(Const.RobinServiceUrl, credentials);
            //}).Subscribe(val => _emergencyClient = val);

            // Combine all app states to a distinct boolean stream so we now when we need to cancel / continue Band connection
            //var appState = Observable.Merge(suspension.IsLaunchingNew, suspension.IsResuming, suspension.IsUnpausing).Select(_ => true)
            //   .Merge(suspension.ShouldPersistState.Select(_ => false)).DistinctUntilChanged();

            // When user change or appState change we either need to connect or disconnect from band
            // On succesful connect we show a nice dialog.
            //appState
            //    //.CombineLatest(authentication.User, (started, user) => new {started, user})
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Subscribe(async val =>
            //    {
            //        if (val)
            //            //if (val?.user == null || !val.started)
            //            //    CancelBandConnection();
            //            //else
            //            await ConnectAndReadData();
            //        else
            //            CancelBandConnection();
            //    }, ex => {/* TODO: @rherlt Handle exception */});

            // Show a nice popup when we connect to a band
            Observable.FromEventPattern<WearableConnectionChangedEventArgs>(handler => ConnectionStateChanged += handler,
                handler => ConnectionStateChanged -= handler)
                      .Select(val => val.EventArgs.State)
                .DistinctUntilChanged()
                .Where(state => state == ConnectionState.Connected)
                      .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async _ =>
                {
                    //var box = new MessageDialog(Localize.Text["BandConnected_Info"], Localize.Text["BandConnected_Title"]);
                    //await box.ShowAsync();
                    await App.Current.MainPage.DisplayAlert("Bad connection", "Bad connection", "Ok");
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
                _connectedBand.ButtonPressed -= HandleTileButtonPressed;

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
                        var tileId = new NSUuid(BandTileId);
                        var tile = new BandTile();

                        NSError error;

                        tile.SetName("TileName", out error);
                        tile.SetTileIcon(LoadIcon("dummy.png"), out error);
                        tile.SetSmallIcon(LoadIcon("dummy.png"), out error);

                        var button = new TextButton { ElementId = 1, Rect = new PageRect(10, 10, 200, 90) };
                        var panel = new FilledPanel(new PageRect(0, 0, 220, 150));
                        panel.AddElement(button);
                        tile.PageLayouts.Add(new PageLayout(panel));

                        // Create the Tile on the Band.

                        var btnData = TextButtonData.Create(1, "Initiate Call", out error);

                        var pageData = PageData.Create(new NSUuid(BandPageDataId), 0, new PageElementData[] { btnData });

                        await band.TileManager.AddTileTaskAsync(tile);
                        await band.TileManager.SetPagesTaskAsync(new PageData[] { pageData }, tileId);
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
                        var tileId = new NSUuid(BandTileId);
                        // Remove the Tile from the Band, if present. An application won't need to do this everytime it runs. 
                        // But in case you modify this sample code and run it again, let's make sure to start fresh.
                        await band.TileManager.RemoveTileTaskAsync(tileId);
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
                NSError error;
                var btnData = TextButtonData.Create(1, "Initiate Call", out error);

                var pageData = PageData.Create(new NSUuid(BandPageDataId), 0, new PageElementData[] { btnData });

                var tileId = new NSUuid(BandTileId);
                await band.TileManager.RemovePagesTaskAsync(tileId);
                await band.TileManager.SetPagesTaskAsync(new PageData[] { pageData }, tileId);
            }
        }

        private async void HandleTileButtonPressed(object sender,
                           TileButtonPressedEventArgs e)
        {
            //if (_emergencyClient == null)
            //    return;

            //var result = await _emergencyClient.InitiateEmergencyCall(CancellationToken.None);

            // Send a notification.
            // TODO: invoke tile action

            System.Diagnostics.Debug.WriteLine($"clicked name: {e.TileButtonEvent.TileName} EventType: {e.TileButtonEvent.EventType}");
            await App.Current.MainPage.DisplayAlert("Clicked Band Tile", $"clicked name: {e.TileButtonEvent.TileName} EventType: {e.TileButtonEvent.EventType}", "Ok");

            //var callresult = result.StatusCode == HttpStatusCode.OK ? "succeeded" : "failed";
            //await
            //    ((BandClient) sender).NotificationManager.SendMessageAsync(new Guid(Const.BandTileId), "Robin",
            //        $"Call {callresult}",
            //        DateTimeOffset.Now);
        }


        private async Task StartReadingFromBand()
        {
            var band = await GetConnectedBand();
            if (band != null)
            {
                band.ButtonPressed -= HandleTileButtonPressed;
                band.ButtonPressed += HandleTileButtonPressed;

                //await band.TileManager.StartReadingsAsync(_cancellationTokenSource.Token);
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

        private async Task<bool> CheckIfTileExists(BandClient band)
        {
            var tiles = await band.TileManager.GetTilesTaskAsync();
            //(BandTile[] arg1, Foundation.NSError arg2) =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"get tiles async error: {arg2}");});

            var tileExits = tiles.Any(x => x.TileId == new NSUuid(BandTileId));
            return tileExits;
        }


        private async Task<BandClient> GetConnectedBand()
        {
            // Get the list of Microsoft Bands paired to the phone.
            var pairedBands = BandClientManager.Instance.AttachedClients;
            if (!pairedBands.Any())
            {
                //no Bands paired with this device
                return null;
            }

            // Connect to Microsoft Band.
            if (_connectedBand == null)
            {
                var bandClient = pairedBands[0];
                await BandClientManager.Instance.ConnectTaskAsync(bandClient);

                _connectedBand = bandClient;

                //OnConnectionStateChanged(new WearableConnectionChangedEventArgs(ConnectionState.Connected));
            }
            return _connectedBand;
        }

        private static BandIcon LoadIcon(string uri)
        {
            NSError error;

            var bandIcon = BandIcon.FromUIImage(UIImage.FromFile(uri), out error);
            var bandImage = new BandImage(uri);
            var test = BandIcon.FromBandImage(bandImage, out error);
            // TODO: Can not create a bandIcon
            System.Diagnostics.Debugger.Break();
            return bandIcon;
        }
    }
}