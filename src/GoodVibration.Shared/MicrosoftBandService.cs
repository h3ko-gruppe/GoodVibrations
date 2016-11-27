using System;
using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.Shared
{
    public class MicrosoftBandService : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

 //   public class MicrosoftBandService : IMicrosoftBandService, IDisposable
	//{
	//	private static CancellationToken _cancellationToken;
	//	public async Task ConnectAndReadData()
	//	{
	//		_cancellationToken = CancellationToken.None;
	//		await AddBandTileIfNotExist();
	//		await ClearTilePages();
	//	}

	//	public void Dispose()
	//	{
	//	}

	//	private void ErrorHandler(NSError error)
	//	{

	//	}

	//	public async Task AddBandTile()
	//	{
	//		try
	//		{
	//			var band = await GetConnectedBand();
	//			if (band != null)
	//			{
	//				// Connect to Microsoft Band.
	//				using (band)
	//				{
	//					NSError errorHandler;
	//					BandTile tile = new BandTile();
	//					tile.SetName("Alarm", out errorHandler); //Localization.BandTileTitle
	//					tile.SetTileIcon(await LoadIcon("ms-appx:///Assets/BandShield48x48.png"), out errorHandler);
	//					tile.SetSmallIcon(await LoadIcon("ms-appx:///Assets/BandShield24x24.png"), out errorHandler);

	//					var button = new PageRect(10, 10, 200, 90);
	//					var panel = new FilledPanel(button) { Rect = new PageRect(0, 0, 220, 150) };
	//					tile.PageLayouts.Add(new PageLayout(panel));

	//					// Create the Tile on the Band.
	//					await band.TileManager.AddTileTaskAsync(tile);
	//					//await band.TileManager.SetPagesTaskAsync(new PageData(),tile.TileId);
	//				}
	//			}
	//		}
	//		catch (Exception ex)
	//		{
	//			throw;
	//		}
	//	}

	//	public async Task GetBandTiles()
	//	{
	//		var band = await GetConnectedBand();
	//		if (band != null)
	//		{
	//			using (band)
	//			{
	//				BandTiles = await band.TileManager.GetTilesTaskAsync();
	//			}
	//		}
	//	}


	//	public async Task RemoveBandTile(string bandTileId)
	//	{
	//		var band = await GetConnectedBand();
	//		if (band != null)
	//		{
	//			using (band)
	//			{
	//				var tileExits = await CheckIfTileExists(band);
	//				if (tileExits)
	//				{
	//					var tileToRemove = BandTiles.FirstOrDefault(x => x.TileId == new NSUuid(bandTileId));

	//					// Remove the Tile from the Band, if present. An application won't need to do this everytime it runs. 
	//					// But in case you modify this sample code and run it again, let's make sure to start fresh.
	//					band.TileManager.RemoveTileAsync(tileToRemove, null);
	//				}
	//			}
	//		}
	//	}

	//	private async Task ClearTilePages()
	//	{
	//		var band = await GetConnectedBand();
	//		if (band != null)
	//		{
	//			var tileId = new Guid(""); //Const.BandTileId);
	//									   //await band.TileManager.RemovePagesAsync(tileId, _cancellationTokenSource.Token);
	//									   //await band.TileManager.SetPagesAsync(tileId, _cancellationTokenSource.Token,
	//									   //           new PageData(new Guid(Constants.Band.BandTileId), 0,
	//									   // new TextButtonData(1, ""))); //Localization.InitiateEmergencyCall)));
	//		}
	//	}

	//	//private async void HandleTileButtonPressed(object sender, BandTileEventArgs<IBandTileButtonPressedEvent> e)
	//	//{
	//	//	//if (_emergencyClient == null)
	//	//	//    return;

	//	//	//var result = await _emergencyClient.InitiateEmergencyCall(CancellationToken.None);

	//	//	//// Send a notification.
	//	//	//var callresult = result.StatusCode == HttpStatusCode.OK ? "succeeded" : "failed";
	//	//	//await ((BandClient) sender).NotificationManager.SendMessageAsync(new Guid(Constants.Band.BandTileId), "Robin",
	//	//	//        $"Call {callresult}",
	//	//	//        DateTimeOffset.Now);
	//	//}

	//	private async Task AddBandTileIfNotExist()
	//	{
	//		var band = await GetConnectedBand();
	//		if (band != null)
	//		{
	//			var tileExits = await CheckIfTileExists(band);
	//			if (!tileExits)
	//				await AddBandTile();
	//		}
	//	}

	//	private async Task<bool> CheckIfTileExists(BandClient band)
	//	{
	//		var tileExits = BandTiles.Any(x => x.TileId == new NSUuid(Constants.Band.BandTileId));
	//		return tileExits;
	//	}

	//	private async Task<BandClient> GetConnectedBand()
	//	{
	//		// Get the list of Microsoft Bands paired to the phone.
	//		var pairedBands = BandClientManager.Instance.AttachedClients;
	//		if (pairedBands.Length < 1)
	//		{
	//			//no Bands paired with this device
	//			return null;
	//		}

	//		// Connect to Microsoft Band.
	//		if (ConnectedBand == null)
	//		{
	//			var bandClient = pairedBands.First();
	//			await BandClientManager.Instance.ConnectTaskAsync(bandClient);
	//			if (bandClient.IsConnected)
	//				ConnectedBand = bandClient;
	//			//OnConnectionStateChanged(new WearableConnectionChangedEventArgs(ConnectionState.Connected));
	//		}
	//		return ConnectedBand;
	//	}

	//	private static async Task<BandIcon> LoadIcon(string uri)
	//	{
	//		NSError errorHandler = new NSError();
	//		var imageFile = await FileSystem.Current.GetFileFromPathAsync(uri, _cancellationToken);

	//		using (var fileStream = await imageFile.OpenAsync(FileAccess.Read))
	//		{
	//			//IBitmap profileImage = await BitmapLoader.Current.Load(fileStream, null, null); // null = orig width/height

	//			var bandIcon = BandIcon.FromUIImage(new UIKit.UIImage(uri), out errorHandler);
	//			return bandIcon;
	//		}
	//	}

	//	private BandTile[] _bandTiles;
	//	public BandTile[] BandTiles
	//	{
	//		get
	//		{
	//			return _bandTiles;
	//		}

	//		set
	//		{
	//			_bandTiles = value;
	//		}
	//	}

	//	private BandClient _connectedBand;
	//	public BandClient ConnectedBand
	//	{
	//		get
	//		{
	//			return _connectedBand;
	//		}

	//		set
	//		{
	//			_connectedBand = value;
	//		}
	//	}
	//}
}