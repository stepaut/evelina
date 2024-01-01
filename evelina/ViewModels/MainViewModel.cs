using evelina.ViewModels;
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


    public MainViewModel()
    {
        var startVM = new StartViewModel(this);

        ActiveVM = startVM;
    }
}
