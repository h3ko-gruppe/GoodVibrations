using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using GoodVibrations.Interfaces.Services;
using Microsoft.Band;
using Microsoft.Band.Portable.Tiles.Pages;
using PCLStorage;
//using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.Shared
{
    //public class MicrosoftBandService //: IMicrosoftBandService, IDisposable
    //{
    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class MicrosoftBandService : IMicrosoftBandService, IDisposable
	{
		private static CancellationToken _cancellationToken;
        private const string _bandTileId = "d012ad68-b25c-4bb7-a586-6bace6b9601d";
		

        public async Task ConnectAndReadData()
		{
			_cancellationToken = CancellationToken.None;
			await AddBandTileIfNotExist();
			await ClearTilePages();
		}

        public async Task NotifyIfConnected (Guid notificationId, string eventId, string notificationName) {

            if (ConnectedBand != null) {
                var message = $"Received sound '{eventId} ' on '{notificationName} '.";
                await ConnectedBand.NotificationManager.SendMessageAsync (notificationId, "Notification Info", message, DateTime.Now);
                await ConnectedBand.NotificationManager.VibrateAsync (Microsoft.Band.Portable.Notifications.VibrationType.NotificationAlarm);
            }        
        }

		public void Dispose()
		{
		}

		private void ErrorHandler(NSError error)
		{

		}

		public async Task AddBandTile()
		{
			try
			{
				var band = await GetConnectedBand();
				if (band != null)
				{
                    try
                    {                       
                        var imageSmall = await LoadImage("BandTileSmall.png");
                        var imageLarge = await LoadImage("BandTileLarge.png");
                        var tile = new Microsoft.Band.Portable.Tiles.BandTile (new Guid (_bandTileId), "Alarm", imageLarge, imageSmall);

                        var button = new PageRect (10, 10, 200, 90);
                        var panel = new Panel () { Rect = new PageRect (0, 0, 220, 150) };
                        tile.PageLayouts.Add (new PageLayout (panel));

                        // Create the Tile on the Band.
                        await band.TileManager.AddTileAsync (tile);
                        //await band.TileManager.SetPagesTaskAsync(new PageData(),tile.TileId);
                    } 
                    catch (Exception x) {
                        string s = x.Message;
                    }
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

        private async Task<Microsoft.Band.Portable.BandImage> LoadImage (string filename)
        {
            var path = Path.Combine (NSBundle.MainBundle.ResourcePath, filename);
            var imgStream = await LoadIconAsStream (path);
            var imageSmall = await Microsoft.Band.Portable.BandImage.FromStreamAsync (imgStream);
            return imageSmall;
        }

        public async Task GetBandTiles()
		{
			var band = await GetConnectedBand();
			if (band != null)
			{
				BandTiles = await band.TileManager.GetTilesAsync();				
			}
		}


		public async Task RemoveBandTile(string bandTileId)
        {
			var band = await GetConnectedBand();
			if (band != null)
			{
				//using (band)
                {
                    var tileExits = CheckIfTileExists (band);
                    if (tileExits) {
                       // Remove the Tile from the Band, if present. An application won't need to do this everytime it runs. 
                        // But in case you modify this sample code and run it again, let's make sure to start fresh.
                        await band.TileManager.RemoveTileAsync (new Guid (bandTileId));
                    }
                }
			}
		}

		private async Task ClearTilePages ()
        {
            var band = await GetConnectedBand ();
            if (band != null) {
                var tileId = new Guid (""); //Const.BandTileId);
                                            //await band.TileManager.RemovePagesAsync(tileId, _cancellationTokenSource.Token);
                                            //await band.TileManager.SetPagesAsync(tileId, _cancellationTokenSource.Token,
                                            //           new PageData(new Guid(Constants.Band.BandTileId), 0,
                                            // new TextButtonData(1, ""))); //Localization.InitiateEmergencyCall)));
            }
        }

        //private async void HandleTileButtonPressed(object sender, BandTileEventArgs<IBandTileButtonPressedEvent> e)
        //{
        //	//if (_emergencyClient == null)
        //	//    return;

        //	//var result = await _emergencyClient.InitiateEmergencyCall(CancellationToken.None);

        //	//// Send a notification.
        //	//var callresult = result.StatusCode == HttpStatusCode.OK ? "succeeded" : "failed";
        //	//await ((BandClient) sender).NotificationManager.SendMessageAsync(new Guid(Constants.Band.BandTileId), "Robin",
        //	//        $"Call {callresult}",
        //	//        DateTimeOffset.Now);
        //}

        private async Task AddBandTileIfNotExist ()
        {
            var band = await GetConnectedBand ();
            if (band != null) {
                await GetBandTiles ();
                var tileExits = CheckIfTileExists (band);
                if (!tileExits)
                    await AddBandTile ();
            }
        }

        private bool CheckIfTileExists (Microsoft.Band.Portable.BandClient band)
        {
            var tileExits = BandTiles.Any (x => x.Id == new Guid (_bandTileId));
            return tileExits;
        }

        private async Task<Microsoft.Band.Portable.BandClient> GetConnectedBand()
		{
			// Get the list of Microsoft Bands paired to the phone.
            var pairedBands = await Microsoft.Band.Portable.BandClientManager.Instance.GetPairedBandsAsync();
            var bands = new List<Microsoft.Band.Portable.BandDeviceInfo> (pairedBands);
            if (bands.Count < 1)
			{
				//no Bands paired with this device
				return null;
			}

			// Connect to Microsoft Band.
			if (ConnectedBand == null)
			{
				var bandClient = pairedBands.First();
                var connected = await Microsoft.Band.Portable.BandClientManager.Instance.ConnectAsync(bandClient);
				//if (bandClient.IsConnected)
				ConnectedBand = connected;
                //OnConnectionStateChanged(new WearableConnectionChangedEventArgs(ConnectionState.Connected));
			}
			return ConnectedBand;
		}

		//private static async Task<IStream> LoadIcon(string uri)
		//{
		//	NSError errorHandler = new NSError();
		//	var imageFile = await FileSystem.Current.GetFileFromPathAsync(uri, _cancellationToken);

		//	using (var fileStream = await imageFile.OpenAsync(PCLStorage.FileAccess.Read))
		//	{
		//		var bandIcon = new Band(new UIKit.UIImage(uri), out errorHandler);
		//		return bandIcon;
		//	}
		//}

        private static async Task<Stream> LoadIconAsStream (string filename)
        {
            var imageFile = await FileSystem.Current.GetFileFromPathAsync (filename, _cancellationToken);
            var fileStream = await imageFile.OpenAsync (PCLStorage.FileAccess.Read);
            return fileStream;
        }

		private IEnumerable<Microsoft.Band.Portable.Tiles.BandTile> _bandTiles;
        public IEnumerable<Microsoft.Band.Portable.Tiles.BandTile> BandTiles
		{
			get
			{
				return _bandTiles;
			}

			set
			{
				_bandTiles = value;
			}
		}

		private Microsoft.Band.Portable.BandClient _connectedBand;
		public Microsoft.Band.Portable.BandClient ConnectedBand
		{
			get
			{
				return _connectedBand;
			}

			set
			{
				_connectedBand = value;
			}
		}
	}
}