using ReactiveUI;

namespace evelina.ViewModels;

public class ViewModelBase : ReactiveObject
{
    internal virtual void OnPropertyChanged(string propertyName)
    {
        this.RaisePropertyChanged(propertyName);
    }
}
