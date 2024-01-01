using ReactiveUI;

namespace evelina.ViewModels
{
    public class InputDialogViewModel : ViewModelBase
    {
        public string Title { get; }

        public string Text { get; }

        private string _input = string.Empty;
        public string Input
        {
            get => _input;
            set => this.RaiseAndSetIfChanged(ref _input, value);
        }

        public InputDialogViewModel(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public InputDialogViewModel()
        {
            Title = "Title";
            Text = "Text";
        }
    }
}
