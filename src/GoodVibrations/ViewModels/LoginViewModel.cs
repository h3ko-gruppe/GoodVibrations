using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using KeyChain.Net;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IKeyChainHelper _keyChainHelper;

        public LoginViewModel(IKeyChainHelper keyChainHelper)
        {
            _keyChainHelper = keyChainHelper;

            var canExecuteLogin = this.WhenAnyValue(x => x.Username, x => x.Password)
                                                   .Select(valueTuple => !string.IsNullOrWhiteSpace(valueTuple.Item1) &&
                                                           !string.IsNullOrWhiteSpace(valueTuple.Item2));
            Login = ReactiveCommand.CreateFromTask(OnLogin, canExecuteLogin);
            Register = ReactiveCommand.CreateFromTask(OnRegister);

            ShowMain = new Interaction<Unit, Unit>();
            ShowRegistration = new Interaction<Unit, Unit>();

            SetUITexts();

            CheckForCredentials();
        }

        [Reactive]
        public string Username { get; set; }

        [Reactive]
        public string Password { get; set; }

        [Reactive]
        public string UsernamePlaceholder { get; set; }

        [Reactive]
        public string PasswordPlaceholder { get; set; }

        [Reactive]
        public string LoginText { get; set; }
        
        [Reactive]
        public string RegisterText { get; set; }

        public ReactiveCommand Login { get; }
        public ReactiveCommand Register { get; }

        public Interaction<Unit, Unit> ShowMain { get; }
        public Interaction<Unit, Unit> ShowRegistration { get; }

        private void SetUITexts()
        {
            Title = "Login";
            UsernamePlaceholder = "Email";
            PasswordPlaceholder = "Password";
            LoginText = "Login";
            RegisterText = "Register";
        }

        private void CheckForCredentials()
        {
            Username = _keyChainHelper.GetKey(Constants.KeyChain.CommonKeyChainUsername);
            Password = _keyChainHelper.GetKey(Constants.KeyChain.CommonKeyChainPassword);
        }

        private void SaveCredentials()
        {
            _keyChainHelper.SetKey(Constants.KeyChain.CommonKeyChainUsername, Username);
            _keyChainHelper.SetKey(Constants.KeyChain.CommonKeyChainPassword, Password);
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
