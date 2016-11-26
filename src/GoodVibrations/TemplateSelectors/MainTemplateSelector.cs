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

        public MainTemplateSelector()
        {
            // Retain instances!
            _notificatorTemplate = new DataTemplate(typeof(NotificatorCell));
            _phoneCallTemplate = new DataTemplate(typeof(PhoneCallTemplateCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is PhoneCallTemplateItemViewModel)
                return _phoneCallTemplate;

            if (item is NotificationItemViewModel)
                return _notificatorTemplate;
                    
            return null;
        }
    }
}
