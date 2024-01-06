using FluentIcons.Avalonia;
using ReactiveUI;
using System.Windows.Input;

namespace evelina.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ICommand TriggerPaneCommand { get; }

    private WindowViewModelBase _activeVM;
    public WindowViewModelBase ActiveVM
    {
        get => _activeVM;
        set
        {
            this.RaiseAndSetIfChanged(ref _activeVM, value);
            OnPropertyChanged(nameof(ShowMenu));

            if (value is PortfolioViewModel p)
            {
                CurrentPortfolio = p;
            }
        }
    }

    private bool _isPaneOpen = true;
    public bool IsPaneOpen
    {
        get => _isPaneOpen;
        set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
    }

    public bool ShowMenu => ActiveVM is IMenuCompatible;

    private PortfolioViewModel _currentPortfolio;
    public PortfolioViewModel CurrentPortfolio
    {
        get => _currentPortfolio;
        set => this.RaiseAndSetIfChanged(ref _currentPortfolio, value);
    }


    public MainViewModel()
    {
        var startVM = new StartViewModel(this);

        ActiveVM = startVM;
        TriggerPaneCommand = ReactiveCommand.Create(TriggerPane);
    }


    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}
