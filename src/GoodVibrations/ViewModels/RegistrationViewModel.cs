using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading.Tasks;
using KeyChain.Net;
using GoodVibrations.Interfaces.Services;

namespace GoodVibrations.ViewModels
{
    public class RegistrationViewModel : UserBaseViewModel
    {
        public RegistrationViewModel (IKeyChainHelper keyChainHelper, IAuthentificationSerivce authService) : base(keyChainHelper)
        {
            var canExecuteLogin = this.WhenAnyValue(x => x.Username, x => x.Password)
                                                   .Select(valueTuple => !string.IsNullOrWhiteSpace(valueTuple.Item1) &&
                                                           !string.IsNullOrWhiteSpace(valueTuple.Item2));
            Register = ReactiveCommand.CreateFromTask(OnRegister, canExecuteLogin);
            _authService = authService;
            ShowLogin = new Interaction<Unit, Unit>();
        }

        public ReactiveCommand Register { get; }

        public Interaction<Unit, Unit> ShowLogin { get; }

        private readonly IAuthentificationSerivce _authService;

       protected override void SetUiTexts()
        {
            base.SetUiTexts();

            Title = "Create Account";
        }


        private async Task OnRegister()
        {
            var successfull = await _authService.CreateAccount (Username, Password);

            if (successfull)
                await ShowLogin.Handle (Unit.Default);
            else {
                await App.Current.MainPage.DisplayAlert ("Error", "The account could not be created.", "OK");            
            }
        }
    }
}
