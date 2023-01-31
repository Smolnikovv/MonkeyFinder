using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        MonkeyService monkeyService;
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        IConnectivity connectivity;
        IGeolocation geolocation;
        public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            Title = "Monkey Finder";
            this.monkeyService = monkeyService;
            this.connectivity = connectivity;
            this.geolocation = geolocation;
        }
        [RelayCommand]
        async Task GetClosestMonkey()
        {
            if (IsBusy || Monkeys.Count== 0) 
                return;

            try
            {
                var location = await geolocation.GetLastKnownLocationAsync();
                if (location is null)
                {
                    location = await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30)

                        });
                }
                if (location is null)
                    return;

                var first = Monkeys.OrderBy(x => location
                .CalculateDistance(x.Latitude, x.Longitude, DistanceUnits.Kilometers))
                    .FirstOrDefault();

                if (first is null)
                    return;

                await Shell.Current.DisplayAlert("Closest Monkey",
                    $"{first.Name} in {first.Location}", "Ok");
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error",
                    $"Unable to get monkeys", "OK");
            }
        }
        [RelayCommand]
        async Task GoToDetailsAsync(Monkey monkey)
        {
            if (monkey is null)
                return;
            await Shell.Current.GoToAsync($"{nameof(DetailPage)}", true, new Dictionary<string, object>
            {
                {"Monkey", monkey }
            });

        }
        [RelayCommand]
        async Task GetMonkeyAsync()
        {
            if(IsBusy)
            {
                return;
            }
            try
            {
                if(connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet issue", $"No interneto", "Ok");
                }
                IsBusy = true;
                var monkeys = await monkeyService.GetMonkeys();
                if (monkeys.Count != 0)
                {
                    Monkeys.Clear();
                }
                foreach (var monkey in monkeys)
                {
                    Monkeys.Add(monkey);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Something went wrong {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
