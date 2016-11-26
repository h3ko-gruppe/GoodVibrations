using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject
    {
        [Reactive]
        public string Title { get; set; }

        public virtual void Init(object parameters)
        {

        }
    }
}
