namespace evelina.ViewModels
{
    /// <summary>
    /// VM, способные заменять активный UserControl основного окна
    /// </summary>
    public class WindowViewModelBase : ViewModelBase
    {
        internal delegate void ReturnBack();
        internal event ReturnBack ReturnBackEvent;


        protected MainViewModel _main;
        private WindowViewModelBase _previous;


        public WindowViewModelBase(MainViewModel main)
        {
            _main = main;
            _previous = _main.ActiveVM;
        }


        protected void TurnBack()
        {
            if (ReturnBackEvent != null)
            {
                // if needs not previous, it may be custom
                ReturnBackEvent.Invoke();
            }
            else
            {
                _main.ActiveVM = _previous;
            }
        }
    }
}
