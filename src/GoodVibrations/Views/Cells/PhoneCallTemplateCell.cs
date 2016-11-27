﻿using System;
using System.Reactive.Linq;
using GoodVibrations.Extensions;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Views.Cells
{
    public class PhoneCallTemplateCell : ImageCell, IViewFor<PhoneCallTemplateItemViewModel>
    {
        public PhoneCallTemplateCell()
        {
            this.WhenActivated(dispose =>
            {
                if (ViewModel == null)
                    return;

                dispose(ViewModel.WhenAnyValue(x => x.PhoneCall.Name)
                        .Distinct()
                        .Subscribe(newValue => Text = newValue));

                dispose(ViewModel.WhenAnyValue(x => x.PhoneCall.DestinationNumber)
                        .Distinct()
                        .Subscribe(newValue => Detail = newValue));

                dispose(ViewModel.WhenAnyValue(x => x.PhoneCall.Icon)
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
