using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading.Tasks;
using KeyChain.Net;

namespace GoodVibrations.ViewModels
{
    public class RegistrationViewModel : UserBaseViewModel
    {
        public RegistrationViewModel (IKeyChainHelper keyChainHelper) : base(keyChainHelper)
        {
            var canExecuteLogin = this.WhenAnyValue(x => x.Username, x => x.Password)
                                                   .Select(valueTuple => !string.IsNullOrWhiteSpace(valueTuple.Item1) &&
                                                           !string.IsNullOrWhiteSpace(valueTuple.Item2));
            Register = ReactiveCommand.CreateFromTask(OnRegister, canExecuteLogin);

            ShowLogin = new Interaction<Unit, Unit>();
        }

        public ReactiveCommand Register { get; }

        public Interaction<Unit, Unit> ShowLogin { get; }


       protected override void SetUiTexts()
        {
            base.SetUiTexts();

            Title = "Register";
        }


        private async Task OnRegister()
        {
            // TODO: Register user
            await ShowLogin.Handle(Unit.Default);
        }
    }
}
