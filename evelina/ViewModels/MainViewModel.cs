using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Db;
using DialogHostAvalonia;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using VisualTools;

namespace evelina.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ICommand CreatePortfolioCommand { get; }
    public ICommand OpenPortfolioCommand { get; }


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
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

        TopLevel topLevel = TopLevel.GetTopLevel(mainWindow);

        IReadOnlyList<IStorageFolder> folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select place for Portfolio",
            AllowMultiple = false,
        });

        if (folders.Count == 0)
        {
            return;
        }

        var dialog = new InputDialogViewModel("Input name", "Input name of Portfolio");
        await DialogHost.Show(dialog);

        string name = dialog.Input;

        IPortfolio portfolio = PortfolioFactory.CreatePortfolio(name);

        string path = Path.Combine(folders[0].Path.ToString(), $"{name}.{Constants.DB_EXTENSION}");

        bool success = await portfolio.SaveAs(path);
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

        IPortfolio portfolio = PortfolioFactory.ReadPortfolio(files[0].Path.ToString());
    }
}
