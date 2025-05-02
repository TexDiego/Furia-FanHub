using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Furia_FanHub.MVVM.ViewModels
{
    public class CadastroUsuarioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<InteresseModel> InteressesDisponiveis { get; set; }
        public ObservableCollection<string> Keys { get; } = new() { "DadosPessoais", "Endereco", "Interesses" };
        public ValidatableObject<string> Email { get; set; }

        private readonly Dictionary<string, bool> _visibilidades = new();
        public ICommand AlternarVisibilidadeCommand { get; }
        public ICommand ToggleSelectionCommand { get; }


        public bool Visibilidade_DadosPessoais => GetVisibilidade("DadosPessoais");
        public bool Visibilidade_Endereco => GetVisibilidade("Endereco");
        public bool Visibilidade_Interesses => GetVisibilidade("Interesses");

        public CadastroUsuarioViewModel()
        {
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

            Email = new ValidatableObject<string>();
            Email.Validations.Add(new RegexValidationRule
            {
                Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                ValidationMessage = "E-mail inválido"
            });

            _visibilidades.Add("DadosPessoais", true);
            _visibilidades.Add("Endereco", false);
            _visibilidades.Add("Interesses", false);

            AlternarVisibilidadeCommand = new Command<string>(AlternarVisibilidade);
        }

        public bool GetVisibilidade(string key) => _visibilidades.TryGetValue(key, out var v) && v;

        public void AlternarVisibilidade(string key)
        {
            if (!_visibilidades.ContainsKey(key)) return;
            _visibilidades[key] = !_visibilidades[key];
            OnPropertyChanged($"Visibilidade_{key}");
        }

        protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
