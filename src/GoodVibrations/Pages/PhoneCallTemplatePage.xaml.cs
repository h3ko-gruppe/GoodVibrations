using System.Reactive;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;

namespace GoodVibrations.Pages
{
    public partial class PhoneCallTemplatePage
    {
        public PhoneCallTemplatePage(PhoneCallTemplateItemViewModel template)
        {
            InitializeComponent();
            this.AutoWireViewModel(template);

            this.WhenActivated(dispose =>
            {
                dispose(this.BindToTitle(ViewModel));
                dispose(this.BindToToolBarItems(ViewModel));
                dispose(ViewModel.Close.RegisterHandler(async param =>
                {
                    await Navigation.PopAsync();
                    param.SetOutput(Unit.Default);
                }));
            });
        }
    }
}
