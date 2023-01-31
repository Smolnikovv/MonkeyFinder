namespace MonkeyFinder.ViewModel
{
    [QueryProperty("Monkey", "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        public MonkeyDetailsViewModel()
        {

        }
        [ObservableProperty]
        Monkey monkey;
        [RelayCommand]
        async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        async Task OpenMapAsync()
        {
            try
            {
                await Map.OpenAsync(monkey.Latitude, monkey.Longitude,
                    new MapLaunchOptions
                    {
                        Name = monkey.Name,
                        NavigationMode = NavigationMode.None,
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error",
                    $"Unable to open map", "OK");
            }
        }
    }
}
