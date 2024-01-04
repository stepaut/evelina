using Db;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class PortfolioViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand CloseCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CreateAssetCommand { get; }


        public string Name => Model?.Name;

        private AssetViewModel _selectedAsset = null;
        public AssetViewModel SelectedAsset
        {
            get => _selectedAsset;
            set => this.RaiseAndSetIfChanged(ref _selectedAsset, value);
        }

        public ObservableCollection<AssetViewModel> Assets { get; } = new();


        internal IPortfolio Model { get; private set; }


        public PortfolioViewModel(IPortfolio model, MainViewModel main) : base(main)
        {
            Model = model;

            foreach (IAsset existed in model.GetAssets())
            {
                AddAsset(existed);
            }
            SelectedAsset = Assets.FirstOrDefault();

            CloseCommand = ReactiveCommand.Create(Close);
            EditCommand = ReactiveCommand.Create(EditPortfoliInfo);
            CreateAssetCommand = ReactiveCommand.Create(CreateAsset);
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
            Model = null;
        }

        private void Close()
        {
            Model.Save();
            TurnBack();
        }

        private void EditPortfoliInfo()
        {
            var editorVM = new PortfolioEditingViewModel(Model, _main);
            _main.ActiveVM = editorVM;
        }

        internal void AddAsset(IAsset asset)
        {
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
    }
}
