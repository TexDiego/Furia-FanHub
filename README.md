# 🎮 Furia FanHub

**Furia FanHub** é um aplicativo multiplataforma desenvolvido com **.NET MAUI**, criado exclusivamente como parte de um **processo seletivo da FURIA Esports**. Ele tem como objetivo centralizar recursos simples para fãs da equipe, como chatbot e perfil de usuário, além de facilitar o acesso às redes sociais da organização.

> ⚠️ Este projeto é apenas uma prova de conceito para avaliação técnica. Ele **não é um app oficial da FURIA** nem está publicado em nenhuma loja.

---

## ✨ Funcionalidades

- 🤖 **Chatbot FURIA (via OpenAI)**
  - Interação com um bot inteligente que incorpora personalidades de jogadores de CS da Furia
- 👤 **Perfil de Usuário**
  - Criação de um perfil local com nome, CPF, data de nascimento, endereço e interesses
  - Busca de endereço por CEP através da API pública ViaCEP
- 🏠 **Página Inicial com atalhos**
  - Redirecionamentos para:
    - [Instagram da FURIA](https://www.instagram.com/furiagg)
    - [Twitter/X da FURIA](https://x.com/FURIA)
    - Link direto para contato no WhatsApp

---

## 🧱 Estrutura do Projeto

```
Furia-FanHub/
├── .idea/                 # Configurações do ambiente de desenvolvimento
├── MVVM/                  # Implementações do padrão MVVM
│   ├── Models/            # Modelos de dados (representações de informações utilizadas pelo app)
│   │   └── Interfaces/    # Interfaces que definem contratos para serviços e modelos
│   ├── ViewModels/        # Lógica de apresentação (controle de dados e interações com as Views)
│   ├── Views/             # Interfaces de usuário (definição das telas e layout)
│   ├── Helpers/           # Classes auxiliares para funcionalidades compartilhadas
│   ├── Services/          # Lógica de negócios complexa (comunicação com APIs e persistência)
│   ├── Resources/         # Recursos utilizados no app, como imagens, fontes e estilos
│   └── Extensions/        # Métodos de extensão para tipos existentes
├── Platforms/             # Configurações específicas por plataforma (Android, iOS, etc)
├── Properties/            # Metadados e configurações do projeto
├── Resources/             # Arquivos de recursos globais, como imagens e fontes
├── App.xaml               # Definições globais de estilos e recursos
├── App.xaml.cs            # Lógica de inicialização do aplicativo
├── AppShell.xaml          # Estrutura de navegação do aplicativo
├── AppShell.xaml.cs       # Lógica associada à navegação
├── MauiProgram.cs         # Configuração e inicialização do MAUI
├── Furia FanHub.csproj    # Arquivo de projeto do .NET
└── Furia FanHub.sln       # Solução do Visual Studio
```

---

## ⚠️ Arquivos Ausentes

Para proteger informações sensíveis, dois arquivos **não estão incluídos** no repositório público:

### `Secrets.cs`

Contém chaves de API, como:

```csharp
namespace Furia_FanHub.MVVM.Models
{
  public class Secrets
  {
      public string OPENAI_API_KEY = "sua-chave-aqui";
  }
}
```

> Sem esse arquivo, o chatbot não funcionará corretamente.

### `debug.keystore`

Arquivo necessário para assinatura do aplicativo em **modo debug** no Android. Sem ele, o build Android local pode falhar.

---

## 🚀 Como Executar

### Pré-requisitos

- Visual Studio 2022+ com suporte a MAUI
- .NET SDK 8.0 ou superior
- Emulador ou dispositivo Android

### Passos

1. Clone o repositório:

```bash
git clone https://github.com/TexDiego/Furia-FanHub.git
```

2. Abra a solução `Furia FanHub.sln` no Visual Studio.

3. Adicione (caso tenha acesso):
   - `MVVM/Helpers/Secrets.cs`
   - `debug.keystore` (para Android)

4. Compile e execute.

---

## 🧪 Objetivo

Este projeto tem como propósito demonstrar:

- Uso de .NET MAUI para apps
- Integração com APIs externas (OpenAI e ViaCEP)
- Organização com padrão MVVM
- Boas práticas de separação de responsabilidades

---

## 📄 Licença

Este projeto é fornecido **exclusivamente para fins de avaliação técnica** no processo seletivo da FURIA Esports. Não há licença de uso comercial associada.

---

Desenvolvido com 💻 por [@TexDiego](https://github.com/TexDiego)
