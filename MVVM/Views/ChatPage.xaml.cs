using System.Collections.ObjectModel;
using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Services;

namespace Furia_FanHub.MVVM.Views
{

    public partial class ChatPage : ContentPage
    {
        public ObservableCollection<ChatMessage> Messages { get; set; } = new ObservableCollection<ChatMessage>();

        private readonly Dictionary<string, string> FuriaPlayers = new()
        {
            ["KSCERATO"] = "Voc� � o KSCERATO da FURIA, calmo, confiante e sempre focado. Responda com intelig�ncia e toques de humor seco.",
            ["yuurih"] = "Voc� � o yuurih da FURIA, amig�vel e bem-humorado. Responda de forma descontra�da, como um brother falando com um f�.",
            ["chelo"] = "Voc� � o chelo da FURIA, sempre motivado e energ�tico. Use emojis e fale como quem est� animado com o jogo.",
            ["FalleN"] = "Voc� � o FalleN da FURIA, o Professor. Responda com calma, sabedoria e incentivo. Seja mentor e �dolo para o f�."
        };

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(); // Notifica o binding para atualizar a UI
            }
        }

        public string BotName { get => selectedPlayerName; }

        private string selectedPlayerName;
        private string selectedPrompt;

        public string SelectedPlayerDisplay => $"Falando com: {selectedPlayerName}";

        public ChatPage()
        {
            InitializeComponent();
            BindingContext = this;

            var rand = new Random();
            var randomPlayer = FuriaPlayers.ElementAt(rand.Next(FuriaPlayers.Count));
            selectedPlayerName = randomPlayer.Key;
            selectedPrompt = randomPlayer.Value;

            Messages.Add(new ChatMessage
            {
                Text = $"{selectedPlayerName} entrou no chat",
                IsSystemMessage = true
            });
        }

        private async void OnSendClicked(object sender, EventArgs e)
        {
            try
            {
                var userMessage = UserInput?.Trim();
                if (string.IsNullOrEmpty(userMessage)) return;

                Messages.Add(new ChatMessage { Text = userMessage, IsUser = true });
                UserEntry.Text = string.Empty;

                await BotMessage(userMessage, selectedPrompt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }

        private async Task BotMessage(string role, string input)
        {
            try
            {
                // Enviar o prompt e obter a resposta
                var response = await new ChatBotService().RequestConnection(input, role);

                if (response != null)
                {
                    // Adicionar a resposta do bot
                    Messages.Add(new ChatMessage { Text = response });
                }
                else
                {
                    Messages.Add(new ChatMessage { Text = "Erro ao tentar responder.", IsSystemMessage = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter resposta do bot: {ex.Message}");
                Messages.Add(new ChatMessage { Text = "Erro na resposta do bot.", IsSystemMessage = true });
            }
        }
    }
}

