using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs.ViewModels
{
    public class InputDialogViewModel : ReactiveObject
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
    }
}
