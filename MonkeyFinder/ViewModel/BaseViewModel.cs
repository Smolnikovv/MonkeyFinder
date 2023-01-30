namespace MonkeyFinder.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        public BaseViewModel() 
        {
            
        }
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool _isBusy;
        [ObservableProperty]
        string _title;
        public bool IsNotBusy => !IsBusy;
    }
}
