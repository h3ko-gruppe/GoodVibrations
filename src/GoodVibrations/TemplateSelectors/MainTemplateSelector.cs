using System;
using GoodVibrations.ViewModels.ItemViewModels;
using GoodVibrations.Views.Cells;
using Xamarin.Forms;

namespace GoodVibrations.TemplateSelectors
{
    public class MainTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _notificatorTemplate;
		private readonly DataTemplate _phoneCallTemplate;
        private readonly DataTemplate _actionItemViewModelTemplate;

        public MainTemplateSelector()
        {
            // Retain instances!
            _notificatorTemplate = new DataTemplate(typeof(NotificatorCell));
			_phoneCallTemplate = new DataTemplate(typeof(PhoneCallTemplateCell));
			_actionItemViewModelTemplate = new DataTemplate(() =>
			{
				var cell = new TextCell();
				cell.SetBinding(TextCell.TextProperty, "Title");
				cell.SetBinding(TextCell.CommandProperty, "SelectedCommand");
				cell.SetBinding(TextCell.CommandParameterProperty, new Binding("."));

				return cell;
			});
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is PhoneCallTemplateItemViewModel)
                return _phoneCallTemplate;

            if (item is NotificationItemViewModel)
                return _notificatorTemplate;

			if (item is ActionItemViewModel)
				return _actionItemViewModelTemplate;
                    
            return null;
        }
    }
}
