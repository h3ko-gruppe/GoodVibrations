using System;
using System.Reactive.Linq;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Views.Cells
{
    public class PhoneCallTemplateCell : ImageCell, IViewFor<PhoneCallTemplateItemViewModel>
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (ViewModel == null)
                return;

            this.WhenActivated(dispose =>
            {
                dispose(ViewModel.WhenAnyValue(x => x.Name)
                        .Distinct()
                        .Subscribe(newValue => Text = newValue));

                dispose(ViewModel.WhenAnyValue(x => x.PhoneNumber)
                        .Distinct()
                        .Subscribe(newValue => Detail = newValue));

                dispose(ViewModel.WhenAnyValue(x => x.ImagePath)
                        .Distinct()
                        .Subscribe(newValue => ImageSource = ImageSource.FromFile(newValue)));

                dispose(this.SubscribeToTap(ViewModel));
                dispose(this.SubscribeToDelete(ViewModel));
            });
        }

        #region IViewFor implementation

        public PhoneCallTemplateItemViewModel ViewModel
        {
            get { return BindingContext as PhoneCallTemplateItemViewModel; }
            set { BindingContext = value; }

        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as PhoneCallTemplateItemViewModel; }
        }

        #endregion
    }
}
