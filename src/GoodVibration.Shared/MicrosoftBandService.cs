using System;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodVibrations.Consts;
using GoodVibrations.Interfaces.Services;
using Foundation;
using Microsoft.Band;
using Microsoft.Band.Tiles;
using Microsoft.Band.Tiles.Pages;
using ReactiveUI;

namespace GoodVibrations.Shared
{
	public class MicrosoftBandService : IMicrosoftBandService, IDisposable
	{
		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		NSError bla = new NSError();
		private BandClient _connectedBand;

		public async Task ConnectAndReadData()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource = new CancellationTokenSource();
			//  await AddBandTileIfNotExist();
			//await ClearTilePages();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
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
						BandTile tile = new BandTile();
						tile.SetName("Alarm", null); //Localization.BandTileTitle
						tile.SetTileIcon = await LoadIcon("ms-appx:///Assets/BandShield48x48.png");
						tile.SmallIcon = await LoadIcon("ms-appx:///Assets/BandShield24x24.png");

						var button = new TextButton { ElementId = 1, Rect = new PageRect(10, 10, 200, 90) };
						var panel = new FilledPanel(button) { Rect = new PageRect(0, 0, 220, 150) };
						tile.PageLayouts.Add(new PageLayout(panel));

						// Create the Tile on the Band.
						await band.TileManager.AddTileAsync(tile, _cancellationTokenSource.Token);
						await band.TileManager.SetPagesAsync(tile.TileId, _cancellationTokenSource.Token,
							 new PageData(new Guid(""), 0, //Const.BandPageDataId), 0,
						new TextButtonData(1, ""))); //Localization.InitiateEmergencyCall)));
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
						var tiles = await band.TileManager.GetTilesTaskAsync();
						var tileToRemove = tiles.FirstOrDefault(x => x.TileId == new NSUuid(Constants.Band.BandTileId));

						// Create a Tile with a TextButton on it.
						//var tileId = new Guid(Constants.Band.BandTileId);
						//BandTile bla = new BandTile();

						// Remove the Tile from the Band, if present. An application won't need to do this everytime it runs. 
						// But in case you modify this sample code and run it again, let's make sure to start fresh.
						band.TileManager.RemoveTileAsync(tileToRemove, null);
					}
				}
			}
		}

		private async Task ClearTilePages()
		{
			var band = await GetConnectedBand();
			if (band != null)
			{
				var tileId = new Guid(""); //Const.BandTileId);
										   //await band.TileManager.RemovePagesAsync(tileId, _cancellationTokenSource.Token);
										   //await band.TileManager.SetPagesAsync(tileId, _cancellationTokenSource.Token,
										   //           new PageData(new Guid(Constants.Band.BandTileId), 0,
										   // new TextButtonData(1, ""))); //Localization.InitiateEmergencyCall)));
			}
		}

		//private async void HandleTileButtonPressed(object sender,
		//    BandTileEventArgs<IBandTileButtonPressedEvent> e)
		//{
		//    if (_emergencyClient == null)
		//        return;

		//    var result = await _emergencyClient.InitiateEmergencyCall(CancellationToken.None);

		//    // Send a notification.
		//    var callresult = result.StatusCode == HttpStatusCode.OK ? "succeeded" : "failed";
		//    await ((BandClient) sender).NotificationManager.SendMessageAsync(new Guid(Constants.Band.BandTileId), "Robin",
		//            $"Call {callresult}",
		//            DateTimeOffset.Now);
		//}

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
			var tiles = await band.TileManager.GetTilesAsync(_cancellationTokenSource.Token);
			var tileExits = tiles.Any(x => x.TileId == new Guid(Constants.Band.BandTileId));
			return tileExits;
		}

		private async Task<BandClient> GetConnectedBand()
		{
			// Get the list of Microsoft Bands paired to the phone.
			var pairedBands = BandClientManager.Instance.AttachedClients;
			if (pairedBands.Length < 1)
			{
				//no Bands paired with this device
				return null;
			}

			// Connect to Microsoft Band.
			if (_connectedBand == null)
			{
				var bandClient = pairedBands.First();
				await BandClientManager.Instance.ConnectTaskAsync(bandClient);
				if (bandClient.IsConnected)
					_connectedBand = bandClient;
				//OnConnectionStateChanged(new WearableConnectionChangedEventArgs(ConnectionState.Connected));
			}
			return _connectedBand;
		}

		//private static async Task<BandIcon> LoadIcon(string uri)
		//{
		//    var imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(uri));

		//    using (var fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
		//    {
		//        var bitmap = new WriteableBitmap(1, 1);
		//        await bitmap.SetSourceAsync(fileStream);
		//        return bitmap.ToBandIcon();
		//    }
		//}
	}
}