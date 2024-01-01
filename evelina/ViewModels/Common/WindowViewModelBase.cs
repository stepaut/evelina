namespace evelina.ViewModels
{
    public class WindowViewModelBase : ViewModelBase
    {
        internal delegate void ReturnBack();
        internal event ReturnBack ReturnBackEvent;


        protected MainViewModel _main;


        public WindowViewModelBase(MainViewModel main)
        {
            _main = main;
        }


        protected void TurnBack()
        {
            ReturnBackEvent?.Invoke();
        }
    }
}
