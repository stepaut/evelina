using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CTS.Import;
using Db;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using VisualTools;

namespace evelina.ViewModels
{
    public class PortfolioViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CreateAssetCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }


        public string Name => Model?.Name;
        public double? Volume => Model?.Stat.Volume;

        private AssetViewModel _selectedAsset = null;
        public AssetViewModel SelectedAsset
        {
            get => _selectedAsset;
            set => this.RaiseAndSetIfChanged(ref _selectedAsset, value);
        }

        public ObservableCollection<AssetViewModel> Assets { get; private set; }


        internal IPortfolio Model { get; private set; }


        public PortfolioViewModel(IPortfolio model, MainViewModel main) : base(main)
        {
            Model = model;
            Model.UpdateVisualStatEvent += Model_UpdateVisualStatEvent;

            Assets = new();
            foreach (IAsset existed in model.GetAssets())
            {
                AddAsset(existed);
            }
            RefreshAssets();
            SelectedAsset = Assets.FirstOrDefault();

            CloseCommand = ReactiveCommand.Create(Close);
            SaveCommand = ReactiveCommand.Create(Save);
            EditCommand = ReactiveCommand.Create(EditPortfoliInfo);
            CreateAssetCommand = ReactiveCommand.Create(CreateAsset);
            ImportCommand = ReactiveCommand.Create(Import);
        }


        public void Dispose()
        {
            foreach (AssetViewModel vm in Assets)
            {
                vm.DeleteMeEvent -= DeleteAsset;
                vm.Dispose();
            }
            Assets.Clear();

            SelectedAsset = null;

            Model.UpdateVisualStatEvent -= Model_UpdateVisualStatEvent;
            Model = null;
        }

        private void Model_UpdateVisualStatEvent()
        {
            OnPropertyChanged(nameof(Volume));

            foreach (AssetViewModel assetVM in Assets)
            {
                //TODO how rework? attrs?
                assetVM.OnPropertyChanged(nameof(AssetViewModel.Volume));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.SellPrice));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.Share));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.BuyedVolume));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.BuyedShare));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.Status));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.TargetVolume));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.TargetSellPrice));
                assetVM.OnPropertyChanged(nameof(AssetViewModel.TargetShare));
            }
        }

        private void Close()
        {
            Save();
            TurnBack();
        }

        private void Save()
        {
            Model.Save();
        }

        private void EditPortfoliInfo()
        {
            var editorVM = new PortfolioEditingViewModel(Model, _main);
            _main.ActiveVM = editorVM;
        }

        internal void AddAsset(IAsset asset)
        {
            if (Assets.Any(x => x.Model == asset))
            {
                return;
            }

            AssetViewModel vm = new AssetViewModel(asset, _main);
            vm.DeleteMeEvent += DeleteAsset;

            Assets.Add(vm);
        }

        private void CreateAsset()
        {
            AssetEditingViewModel editorVM = new AssetEditingViewModel(this, _main);
            _main.ActiveVM = editorVM;

            //var dialog = new InputDialogViewModel("Input name", "Input name of asset");
            //await DialogHost.Show(dialog);

            //string name = dialog.Input;
        }

        private async void DeleteAsset(AssetViewModel vm)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Deleting",
                $"Are you sure to delete {vm.Name}",
                ButtonEnum.YesNo);

            var res = await box.ShowAsync();

            if (res != ButtonResult.Yes)
            {
                return;
            }

            Model.DeleteAsset(vm.Model);

            vm.DeleteMeEvent -= DeleteAsset;
            Assets.Remove(vm);
        }

        private async void Import()
        {
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

            TopLevel topLevel = TopLevel.GetTopLevel(mainWindow);

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select CSV file with CTS",
                AllowMultiple = false,
                FileTypeFilter = new FilePickerFileType[] { Constants.CSVFileType },
            }); ;

            if (files.Count == 0)
            {
                return;
            }

            try
            {
                using (CTSImporter importer = new CTSImporter(files[0].Path.AbsolutePath))
                {
                    importer.Read();
                    importer.AddToPortfolio(Model);
                }
            }
            catch (Exception ex)
            {
                //TODO
                return;
            }

            foreach (IAsset existed in Model.GetAssets())
            {
                AddAsset(existed);
            }
            RefreshAssets();
            SelectedAsset ??= Assets.FirstOrDefault();
        }

        internal void RefreshAssets()
        {
            Assets = new ObservableCollection<AssetViewModel>(Assets.OrderBy(x => x.Name));
        }
    }
}
