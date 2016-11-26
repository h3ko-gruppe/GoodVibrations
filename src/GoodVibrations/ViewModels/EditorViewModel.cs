using System;
using System.Reactive;
using System.Threading.Tasks;
using GoodVibrations.Interfaces.Services;
using ReactiveUI;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;
using GoodVibrations.ViewModels.ItemViewModels;
using Plugin.Media.Abstractions;
using Plugin.Media;

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

        protected abstract Task OnTest();

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

        private async Task OnChooseImage()
        {
            var gallery = "Gallery";
            var camera = "Camera";
            var result = await App.Current.MainPage.DisplayActionSheet("Delete Template", "Cancel", null, gallery, camera);

            MediaFile pickerResult = null;
            if (result == gallery)
                pickerResult = await CrossMedia.Current.PickPhotoAsync();
            else if (result == camera)
                pickerResult = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { SaveToAlbum = true });
            
            if (pickerResult == null)
                return;

            var imagePath = pickerResult.Path;

            SetImagePath(imagePath);
        }

        protected abstract Task OnSaveRequested();
        protected abstract Task OnDeletionRequested();
        protected abstract void SetImagePath(string imagePath);
    }
}
