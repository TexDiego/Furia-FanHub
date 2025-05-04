using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Furia_FanHub.MVVM.ViewModels
{
    public class ConfigsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<InteresseModel> InteressesDisponiveis { get; set; }


        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");
        private FanRepository _fanRepository;


        public ICommand ToggleSelectionCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteAccountCommand { get; }





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


        private DateTime _userBirthDate;
        public DateTime UserBirthDate
        {
            get => _userBirthDate;
            set
            {
                if (_userBirthDate != value)
                {
                    _userBirthDate = value;
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


        private string _userCEP;
        public string UserCEP
        {
            get => _userCEP;
            set
            {
                if (_userCEP != value)
                {
                    _userCEP = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userRua;
        public string UserRua
        {
            get => _userRua;
            set
            {
                if (_userRua != value)
                {
                    _userRua = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userBairro;
        public string UserBairro
        {
            get => _userBairro;
            set
            {
                if (_userBairro != value)
                {
                    _userBairro = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userCidade;
        public string UserCidade
        {
            get => _userCidade;
            set
            {
                if (_userCidade != value)
                {
                    _userCidade = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userEstado;
        public string UserEstado
        {
            get => _userEstado;
            set
            {
                if (_userEstado != value)
                {
                    _userEstado = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _userPais;
        public string UserPais
        {
            get => _userPais;
            set
            {
                if (_userPais != value)
                {
                    _userPais = value;
                    OnPropertyChanged();
                }
            }
        }



        public ConfigsViewModel()
        {
            _fanRepository = new FanRepository(dbPath);

            InteressesDisponiveis = new ObservableCollection<InteresseModel>
            {
                new InteresseModel{ Name = "Counter-Strike" },
                new InteresseModel{ Name = "Valorant" },
                new InteresseModel{ Name = "League of Legends" },
                new InteresseModel{ Name = "Apex Legends" },
                new InteresseModel{ Name = "PUBG" },
                new InteresseModel{ Name = "Rainbow Six" },
                new InteresseModel{ Name = "Rocket League" },
                new InteresseModel{ Name = "Produtos" },
                new InteresseModel{ Name = "Torneios" },
                new InteresseModel{ Name = "Notícias" },
                new InteresseModel{ Name = "Eventos presenciais" },
                new InteresseModel{ Name = "Streamers da FURIA" }
            };

            ToggleSelectionCommand = new Command<InteresseModel>((item) =>
            {
                if (item != null)
                    item.IsSelected = !item.IsSelected;
            });
            SaveCommand = new Command(async () => await SaveChanges());
            DeleteAccountCommand = new Command(async () => await DeleteAccount());

            LoadUserData();
        }

        private async Task SaveChanges()
        {
            var fan = await _fanRepository.GetFanByIdAsync(GlobalProperties.FanId);
            if (fan == null)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Usuário não encontrado", "OK");
                return;
            }
            fan.NomeCompleto = UserName;
            fan.DataNascimento = UserBirthDate;
            fan.CEP = UserCEP;
            fan.Logradouro = UserRua;
            fan.Bairro = UserBairro;
            fan.Cidade = UserCidade;
            fan.UF = UserEstado;
            fan.Pais = UserPais;
            var interessesSelecionados = InteressesDisponiveis
                .Where(i => i.IsSelected)
                .Select(i => i.Name)
                .ToList();
            fan.Interesses = string.Join(", ", interessesSelecionados);
            await _fanRepository.UpdateFanAsync(fan);

            await App.Current.MainPage.DisplayAlert("Sucesso", "Alterações salvas com sucesso", "OK");
            await Shell.Current.GoToAsync("//profile");
        }

        private async Task DeleteAccount()
        {
            var confirm = await App.Current.MainPage.DisplayAlert("Confirmação", "Você tem certeza que deseja excluir sua conta?\n\nEssa ação é permanente e irreversível.", "Confirmar", "Cancelar");

            if (!confirm) return;

            var fan = await _fanRepository.GetFanByIdAsync(GlobalProperties.FanId);
            if (fan == null)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Usuário não encontrado", "OK");
                return;
            }

            await _fanRepository.DeleteFanAsync(fan);

            GlobalProperties.FanId = 0;

            await App.Current.MainPage.DisplayAlert("Sucesso", "Conta excluída com sucesso", "Voltar");

            await Shell.Current.GoToAsync("//login");
        }

        private async Task LoadUserData()
        {
            try
            {
                var id = GlobalProperties.FanId;

                if (id == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Usuário não encontrado", "OK");
                    return;
                }

                var fan = await _fanRepository.GetFanByIdAsync(id);

                if (fan == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Usuário não encontrado", "OK");
                    return;
                }

                UserName = fan.NomeCompleto;
                UserBirthDate = fan.DataNascimento;
                UserCEP = fan.CEP;
                UserRua = fan.Logradouro;
                UserBairro = fan.Bairro;
                UserCidade = fan.Cidade;
                UserEstado = fan.UF;
                UserPais = fan.Pais;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await CarregarInteressesUsuarioAsync();
                });

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading user data.", "OK");
            }
        }

        public async Task CarregarInteressesUsuarioAsync()
        {
            var fan = await _fanRepository.GetFanByIdAsync(GlobalProperties.FanId);
            if (fan == null || string.IsNullOrEmpty(fan.Interesses)) return;

            var interessesDoUsuario = fan.Interesses.Split(',')
                                                    .Select(i => i.Trim().ToLowerInvariant())
                                                    .ToHashSet();

            foreach (var interesse in InteressesDisponiveis)
            {
                interesse.IsSelected = interessesDoUsuario.Contains(interesse.Name.ToLowerInvariant());
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
