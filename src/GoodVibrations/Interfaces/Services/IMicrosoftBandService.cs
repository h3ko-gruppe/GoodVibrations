using System.Threading.Tasks;

namespace GoodVibrations.Interfaces.Services
{
    public interface IMicrosoftBandService
    {
        //static Task<BandIcon> LoadIcon(string uri);
        //Task<IBandClient> GetConnectedBand();
        //Task<bool> CheckIfTileExists(IBandClient band);
        //Task AddBandTileIfNotExist();
        //Task StartReadingFromBand();
        //void HandleTileButtonPressed(object sender, BandTileEventArgs<IBandTileButtonPressedEvent> e);
        //Task ClearTilePages();
        //void OnConnectionStateChanged(WearableConnectionChangedEventArgs e);
        //Task RemoveBandTile();
        //Task AddBandTile();
        //void CancelBandConnection();
        //void Dispose();
        //Task ConnectAndReadData();

        Task ConnectAndReadData ()
        Task Notify ();

    }
}