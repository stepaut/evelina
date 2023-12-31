using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Db;
using ReactiveUI;
using System.IO;
using System.Windows.Input;
using VisualTools;

namespace evelina.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ICommand CreatePortfolioCommand { get; }
    public ICommand OpenPortfolioCommand { get; }

    public string Greeting => "Welcome to Avalonia!";

    public MainViewModel()
    {
        CreatePortfolioCommand = ReactiveCommand.Create(() =>
        {
            CreatePortfolio();
        });

        OpenPortfolioCommand = ReactiveCommand.Create(() =>
        {
            OpenPortfolio();
        });
    }

    private async void CreatePortfolio()
    {
        IPortfolio portfolio = await PortfolioCreator.CreatePortfolio();
    }

    private async void OpenPortfolio()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

        TopLevel topLevel = TopLevel.GetTopLevel(mainWindow);

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Portfolio",
            AllowMultiple = false,
            FileTypeFilter = new FilePickerFileType[] { Constants.DbFileType },
        }); ;

        if (files.Count == 0)
        {
            return;
        }

        await using var stream = await files[0].OpenReadAsync();
        using var streamReader = new StreamReader(stream);
        var fileContent = await streamReader.ReadToEndAsync();

        IPortfolio portfolio = await PortfolioReader.ReadPortfolio(fileContent);
    }
}
