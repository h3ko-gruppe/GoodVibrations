using System;
using System.Reactive.Linq;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;

namespace GoodVibrations.Views.Cells
{
    public partial class PhoneCallTemplateCell : IViewFor<PhoneCallTemplateItemViewModel>
    {
        public PhoneCallTemplateCell()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (ViewModel == null)
                return;

            this.WhenActivated(dispose =>
            {
                dispose(ViewModel.WhenAnyValue(x => x.Name)
                        .Distinct()
                        .Subscribe(newValue => NameLabel.Text = newValue));
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
