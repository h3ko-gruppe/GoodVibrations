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
using System.Linq;
using System.IO;

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
                Title = "",
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

            var imagePath = await SaveFile(pickerResult);

            SetImagePath(imagePath);
        }

        private async Task<string> SaveFile(MediaFile file)
        {
            var folderPath = Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, "Images");

            var folder = await PCLStorage.FileSystem.Current.LocalStorage.CreateFolderAsync(folderPath, PCLStorage.CreationCollisionOption.OpenIfExists);
            var fileName = "Image.jpg";
            var savedFile = await folder.CreateFileAsync(fileName, PCLStorage.CreationCollisionOption.GenerateUniqueName);

            using (var fileStream = await savedFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite)) {
                using (var originalStream = file.GetStream())
                {
                    await originalStream.CopyToAsync(fileStream);
                }
            };

            return savedFile.Path;
        }

        protected abstract Task OnSaveRequested();
        protected abstract Task OnDeletionRequested();
        protected abstract void SetImagePath(string imagePath);
    }
}
