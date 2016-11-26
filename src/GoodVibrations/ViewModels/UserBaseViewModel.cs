using System;
using KeyChain.Net;
using ReactiveUI.Fody.Helpers;

namespace GoodVibrations.ViewModels
{
    public abstract class UserBaseViewModel : BaseViewModel
    {
        protected readonly IKeyChainHelper KeyChainHelper;

        public UserBaseViewModel(IKeyChainHelper keyChainHelper)
        {
            KeyChainHelper = keyChainHelper;
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
        public string RegisterText { get; set; }

        protected void SaveCredentials()
        {
            KeyChainHelper.SetKey(Constants.KeyChain.CommonKeyChainUsername, Username);
            KeyChainHelper.SetKey(Constants.KeyChain.CommonKeyChainPassword, Password);
        }

        protected virtual void SetUiTexts()
        {
            UsernamePlaceholder = "Email";
            PasswordPlaceholder = "Password";
            RegisterText = "Register";
        }

        public override void Init(object parameters)
        {
            base.Init(parameters);

            SetUiTexts();
        }
   }
}
