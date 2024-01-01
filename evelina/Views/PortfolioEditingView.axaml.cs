using Avalonia.Controls;
using Avalonia.Interactivity;
using evelina.ViewModels;

namespace evelina.Views
{
    public partial class PortfolioEditingView : UserControl
    {
        public PortfolioEditingView()
        {
            InitializeComponent();
        }

        public void Apply_Click(object sender, RoutedEventArgs args)
        {
            if (DataContext is PortfolioEditingViewModel vm)
            {
            }
        }

        public void Cancel_Click(object sender, RoutedEventArgs args)
        {
        }
    }
}
