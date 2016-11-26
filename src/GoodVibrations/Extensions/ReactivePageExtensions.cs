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
        public static void AutoWireViewModel<TViewModel>(this IViewFor<TViewModel> view, object parameters = null) where TViewModel : BaseViewModel
        {
            var viewModel = Locator.Current.GetService<TViewModel>();

            if (viewModel == null)
                throw new Exception($"Could not resolve {typeof(TViewModel).Name}. Please register in App.cs");

            view.ViewModel = viewModel;

            var bindable = view as BindableObject;

            if (bindable != null)
                bindable.BindingContext = viewModel;

            // init
            viewModel.Init(parameters);
        }

        public static IDisposable BindToTitle(this Page page, BaseViewModel viewModel)
        {
            return viewModel.WhenAnyValue(x => x.Title)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .Subscribe(newValue => page.Title = newValue);
        }
    }
}
