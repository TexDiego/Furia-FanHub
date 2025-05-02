using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Repositories;
using Furia_FanHub.MVVM.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Furia_FanHub.MVVM.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");
        private FanRepository _fanRepository;



        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userAge;
        public string UserAge
        {
            get => _userAge;
            set
            {
                if (_userAge != value)
                {
                    _userAge = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userPreferences;
        public string UserPreferences
        {
            get => _userPreferences;
            set
            {
                if (_userPreferences != value)
                {
                    _userPreferences = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userAddress;
        public string UserAddress
        {
            get => _userAddress;
            set
            {
                if (_userAddress != value)
                {
                    _userAddress = value;
                    OnPropertyChanged();
                }
            }
        }



        private bool _isGuest;
        public bool IsGuest
        {
            get => _isGuest;
            set
            {
                _isGuest = value;
                OnPropertyChanged();
            }
        }


        public ICommand Login { get; }
        public ICommand Register { get; }
        public ICommand Logoff { get; }


        public ProfileViewModel()
        {
            _fanRepository = new FanRepository(dbPath);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await LoadUserData();
            });

            Login = new Command(async () => await LoginCommand());
            Register = new Command(async () => await RegisterCommand());
            Logoff = new Command(async () => await LogoffCommand());
        }

        public async void RefreshData()
        {
            await LoadUserData();
        }

        private async Task LoginCommand()
        {
            await Shell.Current.GoToAsync("//login");
        }

        private async Task RegisterCommand()
        {
            await Shell.Current.GoToAsync("//register");
        }

        private async Task LogoffCommand()
        {
            var result = await App.Current.MainPage.DisplayAlert("Desconectar", "Tem certeza de que deseja sair da conta?", "Sim, sair", "Cancelar");
            if (result)
            {
                await Shell.Current.GoToAsync("//login");
            }
        }

        private async Task LoadUserData()
        {
            try
            {
                var id = GlobalProperties.FanId;

                if (id == 0)
                {
                    IsGuest = true;
                    UserName = string.Empty;
                    UserAge = string.Empty;
                    UserAddress = string.Empty;
                    UserPreferences = string.Empty;
                    return;
                }

                IsGuest = false;

                var fan = await _fanRepository.GetFanByIdAsync(id);

                if (fan == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Usuário não encontrado", "OK");
                    return;
                }

                UserName = fan.NomeCompleto;
                UserAge = GetUserAge(fan.DataNascimento);
                UserAddress = GetUserAddress(fan);
                UserPreferences = fan.Interesses.TrimEnd(',', ' ');
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading user data.", "OK");
            }
        }

        private string GetUserAge(DateTime birthDate)
        {
            var age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now < birthDate.AddYears(age)) age--;
            return $"{birthDate.ToString("dd/MM/yyyy")} ({age.ToString()} anos)";
        }

        private string GetUserAddress(FanProfile fan)
        {
            var address = $"{fan.Logradouro}, {fan.Cidade}, {fan.UF}, {fan.Pais}";
            return address;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
