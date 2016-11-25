using System;
using System.Reactive.Linq;
using GoodVibrations.ViewModels;
using ReactiveUI;
using Splat;
using Xamarin.Forms;

namespace GoodVibrations.Extensions
{
    public static class ReactivePageExtensions
    {
        public static void AutoWireViewModel<TViewModel>(this IViewFor<TViewModel> view) where TViewModel : class
        {
            var viewModel = Locator.Current.GetService<TViewModel>();

            if (viewModel == null)
                throw new Exception($"Could not resolve {typeof(TViewModel).Name}. Please register in App.cs");

            view.ViewModel = viewModel;
        }

        public static IDisposable BindToTitle(this Page page, BaseViewModel viewModel)
        {
            return viewModel.WhenAnyValue(x => x.Title)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(newValue => page.Title = newValue);
        }
    }
}
