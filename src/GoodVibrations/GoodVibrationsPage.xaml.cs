using Xamarin.Forms;

namespace GoodVibrations
{
    public partial class GoodVibrationsPage : ContentPage
    {
        public GoodVibrationsPage ()
        {
            InitializeComponent ();

            var service = new Services.PersistenceService ();
            var y = service.Load<Models.Sample> (x => x.Test == 3);
        }
    }
}
