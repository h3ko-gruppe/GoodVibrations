using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GoodVibrations.Consts;
using KeyChain.Net;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class LoginViewModel : UserBaseViewModel
    {
        public LoginViewModel(IKeyChainHelper keyChainHelper) : base(keyChainHelper)
        {
            var canExecuteLogin = this.WhenAnyValue(x => x.Username, x => x.Password)
                                                   .Select(valueTuple => !string.IsNullOrWhiteSpace(valueTuple.Item1) &&
                                                           !string.IsNullOrWhiteSpace(valueTuple.Item2));
            Login = ReactiveCommand.CreateFromTask(OnLogin, canExecuteLogin);
            Register = ReactiveCommand.CreateFromTask(OnRegister);

            ShowMain = new Interaction<Unit, Unit>();
            ShowRegistration = new Interaction<Unit, Unit>();
        }

        [Reactive]
        public string LoginText { get; set; }

        public ReactiveCommand Login { get; }
        public ReactiveCommand Register { get; }

        public Interaction<Unit, Unit> ShowMain { get; }
        public Interaction<Unit, Unit> ShowRegistration { get; }

        protected override void SetUiTexts()
        {
            base.SetUiTexts();

            Title = "Login";
            LoginText = "Login";
        }

        public void CheckForCredentials()
        {
            Username = KeyChainHelper.GetKey(Constants.KeyChain.CommonKeyChainUsername);
            Password = KeyChainHelper.GetKey(Constants.KeyChain.CommonKeyChainPassword);
        }

        private async Task OnRegister()
        {
            await ShowRegistration.Handle(Unit.Default);
        }

        private async Task OnLogin()
        {
            // TODO: Login
            bool loginSuccessful = true;

            if (loginSuccessful)
                SaveCredentials();

            await ShowMain.Handle(Unit.Default);
        }
   }
}
