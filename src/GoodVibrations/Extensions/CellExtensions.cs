using System;
using System.Reactive.Linq;
using GoodVibrations.ViewModels.ItemViewModels;
using ReactiveUI;
using Xamarin.Forms;

namespace GoodVibrations.Extensions
{
    public static class CellExtensions
    {
        public static IDisposable SubscribeToTap(this Cell cell, SelectableItemViewModel viewModel)
        {
            return cell.Events()
                       .Tapped
                       .Select(x => viewModel)
                       .InvokeCommand(viewModel.SelectedCommand);
        }

        public static IDisposable SubscribeToDelete(this Cell cell, SelectableItemViewModel viewModel)
        {
            return viewModel.WhenAnyValue(x => x.DeleteCommand).Subscribe(newValue =>
            {
                if (cell == null)
                    return;

                cell.ContextActions.Clear();

                if (newValue != null)
                {
                    cell.ContextActions.Add(new MenuItem()
                    {
                        Text = "Delete",
                        Command = newValue,
                        IsDestructive = true
                    });
                }
            });
        }
    }
}
