using ReactiveUI;

namespace evelina.ViewModels;

public class MainViewModel : ViewModelBase
{
    private WindowViewModelBase _activeVM;
    public WindowViewModelBase ActiveVM
    {
        get => _activeVM;
        set => this.RaiseAndSetIfChanged(ref _activeVM, value);
    }

    private bool _showMenu = true;
    public bool ShowMenu
    {
        get => _showMenu;
        set => this.RaiseAndSetIfChanged(ref _showMenu, value);
    }


    public MainViewModel()
    {
        var startVM = new StartViewModel(this);

        ActiveVM = startVM;
    }
}
