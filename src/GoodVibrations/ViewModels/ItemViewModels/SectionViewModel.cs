using System;
using System.Collections;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels.ItemViewModels
{
    public abstract class SectionViewModel : ReactiveObject
    {
        [Reactive]
        public string Title { get; set; }

        public IEnumerable Items { get; set; }
    }

    public class SectionViewModel<T> : SectionViewModel where T : BaseItemViewModel
    {
        public SectionViewModel()
        {
            Items = new ReactiveList<T>();
        }

        public new ReactiveList<T> Items
        {
            get
            {
                return base.Items as ReactiveList<T>;
            }
            private set
            {
                base.Items = value;
            }
        }
    }
}
