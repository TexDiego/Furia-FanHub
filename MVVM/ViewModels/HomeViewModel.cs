using System.ComponentModel;
using System.Runtime.CompilerServices;
using Furia_FanHub.MVVM.Repositories;

namespace Furia_FanHub.MVVM.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");
        private readonly FanRepository _fanRepository;

        private string _nome = "Furioso";
        public string Nome
        {
            get => _nome;
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged();
                }
            }
        }

        public HomeViewModel(int id)
        {
            try
            {
                _fanRepository = new FanRepository(dbPath);

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await CarregarDados(id);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar o ViewModel: {ex.Message}");
            }
        }

        public async Task CarregarDados(int id)
        {
            try
            {
                Console.WriteLine(id);
                if (id == 0) return;

                var fan = await _fanRepository.GetFanByIdAsync(id);

                if (fan != null)
                {
                    var primeiroNome = fan.NomeCompleto.Split(" ")[0];
                    Nome = primeiroNome;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao carregar os dados do usuário.", "OK");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
