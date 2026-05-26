# 🎬 Lumix Play

Sistema completo de streaming desenvolvido em **ASP.NET Core** com autenticação, sistema de favoritos, busca dinâmica de filmes, integração com API externa, sessões persistentes e documentação interativa via Swagger.

---

## 📖 Descrição do Projeto

O **Lumix Play** é uma plataforma de streaming inspirada nos serviços modernos mais populares do mercado. O ecossistema foi projetado dividindo-se entre uma aplicação web tradicional utilizando Razor Views e uma API REST estruturada para o gerenciamento de recursos.

### Funcionalidades Core:
* **Autenticação Segura:** Cadastro de usuários e login com senhas criptografadas.
* **Recuperação de Senha:** Fluxo estruturado para redefinição de credenciais (em desenvolvimento).
* **Catálogo Dinâmico:** Listagem e busca de títulos com base em dados em tempo real.
* **Área do Usuário:** Gerenciamento de perfil (edição de dados e foto) e listagem personalizada de favoritos.
* **Arquitetura Escalável:** Divisão clara de responsabilidades no backend (Controllers, Services, Repositories, DTOs e Data).

---

## 🚀 Tecnologias Utilizadas

### Backend & Banco de Dados
* **ASP.NET Core 8.0**
* **Entity Framework Core** (OR/M para persistência)
* **MySQL** (Banco de dados relacional via Pomelo Provider)
* **Swagger UI** (Documentação e testes de rotas)

### Frontend
* **HTML5** | **CSS3** | **JavaScript (ES6+)**
* **Razor Views (.cshtml)** para renderização dinâmica no servidor

### Segurança
* **BCrypt.Net-Next** para hashing seguro de senhas
* **Autenticação via Cookies/Sessões** para a área de Views
* **JWT (JSON Web Tokens)** estruturado para proteção dos Endpoints da API
* Cookies configurados como **HttpOnly** contra ataques XSS

### Integrações Externas
* **The Movie Database (TMDB) API** para sincronização de dados de filmes

---

## 📂 Estrutura do Projeto

```text
Lumix/
├── Controllers/
│   ├── AuthController.cs
│   ├── FavoritesController.cs
│   ├── HomeController.cs
│   ├── MovieController.cs
│   ├── ProfileController.cs
│   └── SwaggerController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
├── Migrations/
├── Models/
│   ├── Conteudo.cs
│   ├── Favorito.cs
│   ├── FavoritoRequest.cs
│   ├── ResetSenhaModel.cs
│   └── Usuario.cs
├── Repositories/
├── Services/
│   └── TmdbService.cs
├── Views/
│   ├── Auth/
│   │   ├── ForgotPassword.cshtml
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── ResetPassword.cshtml
│   ├── Home/
│   │   └── Index.cshtml
│   ├── Movie/
│   │   └── Details.cshtml
│   ├── Profile/
│   │   └── Index.cshtml
│   └── Shared/
├── wwwroot/
│   ├── css/
│   │   ├── login.css
│   │   └── style.css
│   ├── images/
│   │   ├── logo.png
│   │   └── perfil-default.png
│   ├── uploads/
│   └── index.html
├── appsettings.json
├── Lumix.http
├── Program.cs
└── README.md

```

---

## ⚙️ Funcionalidades Detalhadas

* **🔐 Autenticação:** Fluxos de login, registro e logout protegidos por middlewares específicos que validam o estado da sessão.
* **🎞️ Vitrine de Filmes:** Consumo assíncrono da API do TMDB segmentado por categorias como Ação, Romance e Comédia, além de busca por texto.
* **❤️ Sincronização de Favoritos:** Persistência no banco de dados local associando o ID do usuário ao ID do filme retornado pelo TMDB.
* **👤 Customização do Perfil:** Upload de foto de exibição salvo no diretório local e atualização de dados cadastrais.
* **📱 Interface Responsiva:** Estilização moderna baseada em conceitos de *Glassmorphism*, gradientes neon, cards animados e sliders horizontais adaptados para Mobile, Tablet e Desktop.

---

## 🛠️ Configuração e Execução

### 1. Pré-requisitos

* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* Instância do [MySQL Server](https://www.google.com/search?q=https://dev.mysql.com/downloads/) ativa.

### 2. Configuração do Banco de Dados

Abra o arquivo `appsettings.json` na raiz do projeto e ajuste a string de conexão de acordo com o seu ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=lumix;user=root;password=SUA_SENHA_AQUI"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

### 3. Execução via Terminal

Siga os passos abaixo na ordem indicada para clonar, preparar o ambiente e rodar a aplicação:

1. **Clonar o repositório** para a sua máquina local.
2. **Navegar até a pasta raiz** onde o projeto principal do ASP.NET reside.
3. **Restaurar os pacotes** e as dependências NuGet do ecossistema .NET.
4. **Atualizar o Banco de Dados** executando as Migrations estruturadas no Entity Framework.
5. **Iniciar o servidor** local da aplicação.

```bash
git clone [https://github.com/DevJSilva5/LumixPlay.git](https://github.com/DevJSilva5/LumixPlay.git)
cd LumixPlay/Lumix
dotnet restore
dotnet ef database update
dotnet run

```

---

## 🌐 Mapeamento de Rotas

### Aplicação Web (MVC/Razor)

| Rota | Método | Descrição | Protegida? |
| --- | --- | --- | --- |
| `/Auth/Login` | GET/POST | Tela e processamento de login | Não |
| `/Auth/Register` | GET/POST | Tela e processamento de cadastro de usuário | Não |
| `/Home/Index` | GET | Dashboard principal com a vitrine de filmes | Sim |
| `/Profile/Index` | GET/POST | Visualização e edição do perfil do usuário | Sim |
| `/Movie/Details/{id}` | GET | Exibição de detalhes de um filme específico | Sim |


---

## 👨‍💻 Autor

Projeto desenvolvido com foco acadêmico para consolidação de conhecimentos práticos em:

* Arquitetura de Software com ASP.NET Core
* Modelagem de dados com Entity Framework Core e MySQL
* Padrões de Segurança Web (Hashing, Cookies HttpOnly e Bearer Tokens)
* Consumo assíncrono de APIs de terceiros (HttpClient / JSON Serialization)

---

## 📄 Licença

Este projeto foi desenvolvido estritamente para fins educacionais e de portfólio, estando livre para clonagem, estudos e modificações.

```

```