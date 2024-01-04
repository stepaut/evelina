using Db;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace evelina.ViewModels
{
    public class AssetEditingViewModel : WindowViewModelBase, IDisposable
    {
        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private double? _targetVolume;
        public double? TargetVolume
        {
            get => _targetVolume;
            set => this.RaiseAndSetIfChanged(ref _targetVolume, value);
        }

        private double? _targetSellPrice;
        public double? TargetSellPrice
        {
            get => _targetSellPrice;
            set => this.RaiseAndSetIfChanged(ref _targetSellPrice, value);
        }

        private double? _targetShare;
        public double? TargetShare
        {
            get => _targetShare;
            set => this.RaiseAndSetIfChanged(ref _targetShare, value);
        }

        private PortfolioViewModel _portfolioVM;
        private IAsset _asset;


        public AssetEditingViewModel(IAsset asset, MainViewModel main) : this(main)
        {
            _asset = asset;
            _portfolioVM = null;

            Name = asset.Name;
            TargetVolume = asset.TargetVolume;
            TargetSellPrice = asset.TargetSellPrice;
            TargetShare = asset.TargetShare;
        }

        public AssetEditingViewModel(PortfolioViewModel vm, MainViewModel main) : this(main)
        {
            _asset = null;
            _portfolioVM = vm;
        }

        private AssetEditingViewModel(MainViewModel main) : base(main)
        {
            ApplyCommand = ReactiveCommand.Create(Apply);
            CancelCommand = ReactiveCommand.Create(Close);
        }


        public void Dispose()
        {
            _portfolioVM = null;
            _asset = null;
        }

        private async void Apply()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Warning", "Fill Name!", ButtonEnum.Ok);

                await box.ShowAsync();
                return;
            }

            if (_asset is null)
            {
                IAsset asset = _portfolioVM.Model.CreateAsset(Name);

                _portfolioVM.AddAsset(asset);
            }
            else
            {
                _asset.Name = Name;
            }

            Close();
        }

        private void Close()
        {
            TurnBack();
        }
    }
}
