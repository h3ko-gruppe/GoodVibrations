using System;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using ReactiveUI;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;
using GoodVibrations.ViewModels.ItemViewModels;

namespace GoodVibrations.ViewModels
{
    public abstract class EditorViewModel : BaseViewModel
    {
        public EditorViewModel()
        {
            ChooseImage = ReactiveCommand.CreateFromTask(OnChooseImage);
            Save = ReactiveCommand.CreateFromTask(OnSave);
            Test = ReactiveCommand.CreateFromTask(OnTest);
            Delete = ReactiveCommand.CreateFromTask(OnDelete);

            Close = new Interaction<Unit, Unit>();
        }

        [Reactive]
        public string SaveText { get; set; }

        [Reactive]
        public string DeleteText { get; set; }

        [Reactive]
        public string ChooseImageText { get; set; }

        public ReactiveCommand ChooseImage { get; }
        public ReactiveCommand Save { get; }
        public ReactiveCommand Delete { get; }
        public ReactiveCommand Test { get; }

        public Interaction<Unit, Unit> Close { get; }

        protected override void CreateToolBarItems()
        {
            ToolBarItems.Clear();

            ToolBarItems.Add(new ActionItemViewModel()
            {
                Title = "Test",
                SelectedCommand = Test
            });
        }

        protected virtual void SetUiTexts()
        {
            SaveText = "Save";
            DeleteText = "Delete";
            ChooseImageText = "Choose Image";
        }

        protected virtual async Task OnTest()
        {
            await App.Current.MainPage.DisplayAlert("Test not implemented", $"{this.GetType().Name}.{nameof(OnTest)}", "Ok");
        }

        private async Task OnSave()
        {
            await OnSaveRequested();
            await Close.Handle(Unit.Default);
        }

        private async Task OnDelete()
        {
            var result = await App.Current.MainPage.DisplayActionSheet("Delete Template", "Cancel", DeleteText);

            if (result == DeleteText)
            {
                await OnDeletionRequested();
                await Close.Handle(Unit.Default);
            }
        }

        protected abstract Task OnSaveRequested();
        protected abstract Task OnDeletionRequested();
        protected abstract void SetImagePath(string imagePath);

        private Task OnChooseImage()
        {
            // TODO: show ActionSheet => Gallary or Camera
            // TODO: Start Camera or Gallery
            // TODO: set ImagePath

           var imagePath = "dummy.png";

            SetImagePath(imagePath);

            return Task.FromResult(true);
        }
    }
}
