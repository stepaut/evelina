using Avalonia.Controls;
using Avalonia.Interactivity;
using evelina.ViewModels;

namespace evelina.Views
{
    public partial class InputDialogView : UserControl
    {
        public InputDialogView()
        {
            InitializeComponent();
        }

        public void Ok_Click(object sender, RoutedEventArgs args)
        {
            //TODO
        }

        public void Cancel_Click(object sender, RoutedEventArgs args)
        {
            if (DataContext is InputDialogViewModel vm)
            {
                vm.Input = string.Empty;
            }

            //TODO
        }
    }
}
