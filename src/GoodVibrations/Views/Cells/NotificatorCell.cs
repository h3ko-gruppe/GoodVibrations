using System;
using System.Reactive.Linq;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Views.Cells
{
    public class NotificatorCell : TextCell, IViewFor<NotificationItemViewModel>
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (ViewModel == null)
                return;

            this.WhenActivated(dispose =>
            {
                dispose(ViewModel.WhenAnyValue(x => x.Notification.Name)
                        .Distinct()
                        .Subscribe(newValue => Text = newValue));

                dispose(this.SubscribeToTap(ViewModel));
                dispose(this.SubscribeToDelete(ViewModel));
            });
        }  

        #region IViewFor implementation

        public NotificationItemViewModel ViewModel
        {
            get { return BindingContext as NotificationItemViewModel; }
            set { BindingContext = value; }

        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as NotificationItemViewModel; }
        }

        #endregion
    }
}
