using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        [Reactive]
        public string Title { get; set; }
    }
}
