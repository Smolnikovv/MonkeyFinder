﻿using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        MonkeyService monkeyService;
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        public MonkeysViewModel(MonkeyService monkeyService) 
        {
            Title = "Monkey Finder";
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
                await Shell.Current.DisplayAlert("Error", "Something went wrong", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
