using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using Microsoft.Band.Portable;
using Microsoft.Band.Portable.Tiles;
using Microsoft.Band.Portable.Tiles.Pages;
using Microsoft.Band.Portable.Tiles.Pages.Data;
using PCLStorage;
using Xamarin.Forms;

namespace GoodVibrations.Services
{
   public class MicrosoftBandService2 : IMicrosoftBandService, IDisposable
	{
		private static CancellationToken _cancellationToken;
        private const string _bandTileId = "d012ad68-b25c-4bb7-a586-6bace6b9601d";
        private static IImageService _imageService;

        public MicrosoftBandService2 (IImageService imageService)
        { 
            _imageService = imageService;
        }

        public async Task ConnectAndReadData()
		{
			_cancellationToken = CancellationToken.None;
            await RemoveBandTile (_bandTileId);
			await AddBandTileIfNotExist();
			//await ClearTilePages();
		}

        public async Task NotifyIfConnected (string eventId, string notificationName) {

            if (ConnectedBand != null) {
                var message = $"Received sound '{eventId} ' on '{notificationName} '.";
                await ConnectedBand.NotificationManager.SendMessageAsync (new Guid(_bandTileId), "Notification Info", message, DateTime.Now);
                await ConnectedBand.NotificationManager.VibrateAsync (Microsoft.Band.Portable.Notifications.VibrationType.NotificationAlarm);
            }        
        }

		public void Dispose()
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
                        var tile = new Microsoft.Band.Portable.Tiles.BandTile (new Guid (_bandTileId), "Good Vibrations", imageLarge, imageSmall);

                        //var button = new PageRect (10, 10, 200, 90);
                        //var panel = new FlowPanel () { Rect = new PageRect (0, 0, 220, 150) };
                        var pageLayout = new PageLayout {
                            
                            Root = new ScrollFlowPanel {
                                Rect = new PageRect (0, 0, 245, 105),
                                Orientation = FlowPanelOrientation.Vertical,
                                Elements = {
                                    new TextBlock {
                                        ElementId = 1,
                                        Rect = new PageRect(0, 0, 229, 30),
                                        ColorSource = ElementColorSource.BandBase,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        VerticalAlignment = VerticalAlignment.Bottom,
                                    },
                                    new TextButton {
                                        ElementId = 2,
                                        Rect = new PageRect(0, 0, 229, 43),
                                        PressedColor = new BandColor(0, 127, 0)
                                    },
                                    //new Image {
                                    //    ElementId = imageId,
                                    //    Rectangle = new PageRect(0, 0, 229, 46),
                                    //    Color = new BandColor(127, 127, 0),
                                    //    VerticalAlignment = VerticalAlignment.Center,
                                    //    HorizontalAlignment = HorizontalAlignment.Center
                                    //}
                                }
                            }
                        };

                        // add the page layout to the tile
                        tile.PageLayouts.Add (pageLayout);
                        // add additional images
                        //tile.PageImages.Add (await BandImage.FromStreamAsync (additionalImageStream));
                        // add the tile to the Band
                        await band.TileManager.AddTileAsync (tile);
                          
                        // declare the data for the page
                        //var pageData = new PageData
                        //                        {
                        //                            PageId = 1,
                        //                            PageLayoutIndex = pageIndex,
                        //                            Data = {
                        //        new TextBlockData {
                        //            ElementId = titleId,
                        //            Text = "Buttons"
                        //        },
                        //        new TextButtonData {
                        //            ElementId = buttonId,
                        //            Text = "Press Me!"
                        //        },
                        //        new ImageData {
                        //            ElementId = imageId,
                        //            ImageIndex = 0
                        //        }
                        //    }
                        //};
                        //// apply the data to the tile
                        //await tileManager.SetTilePageDataAsync(tileId, pageData);

                    } 
                    catch (Exception x) {
                        System.Diagnostics.Debug.WriteLine($"Error while: {nameof(AddBandTile)} Error: {x}");
                    }
				}
			}
			catch (Exception ex)
			{
                System.Diagnostics.Debug.WriteLine($"Error while: {nameof(AddBandTile)} With {nameof(GetConnectedBand)} Error: {ex}");
				throw;
			}
		}

        private async Task<Microsoft.Band.Portable.BandImage> LoadImage (string filename)
        {           
            var path = Path.Combine (_imageService.BundlePrefix, filename);
            var imgStream = await LoadIconAsStream (path);
            var imageSmall = await Microsoft.Band.Portable.BandImage.FromStreamAsync (imgStream);
            return imageSmall;
        }

        public async Task<IEnumerable<BandTile>> GetBandTiles()
		{
            if (_bandTiles != null)
                return _bandTiles;
            else {

                var band = await GetConnectedBand ();
                if (band != null) {
                    _bandTiles = await band.TileManager.GetTilesAsync ();
                }

                return _bandTiles;
            }
		}


		public async Task RemoveBandTile(string bandTileId)
        {
			var band = await GetConnectedBand();
			if (band != null)
			{			
                var tileExits = await CheckIfTileExists (band);
                if (tileExits) {
                   // Remove the Tile from the Band, if present. An application won't need to do this everytime it runs. 
                    // But in case you modify this sample code and run it again, let's make sure to start fresh.
                    await band.TileManager.RemoveTileAsync (new Guid (bandTileId));
                }
			}
		}

		private async Task ClearTilePages ()
        {
            //var band = await GetConnectedBand ();
            //if (band != null) {
            //    var tileId = new Guid (_bandTileId); 
            //    await band.TileManager.RemoveTilePagesAsync(tileId);
            //    await band.TileManager.SetPagesAsync (tileId);
            //    new PageData(new Guid(Constants.Band.BandTileId), 0,
            //    new TextButtonData(1, ""))); //Localization.InitiateEmergencyCall)));
            //}
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
                var tileExits = await CheckIfTileExists (band);
                if (!tileExits)
                    await AddBandTile ();
            }
        }

        private async Task<bool> CheckIfTileExists (Microsoft.Band.Portable.BandClient band)
        {
            var id = new Guid (_bandTileId);
            var tiles = await GetBandTiles ();
            var exists = tiles.Any (x => x.Id == id);
            return exists;
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