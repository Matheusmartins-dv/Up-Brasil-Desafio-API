# 🚀 Projeto Cadastro de Produtos, Categorias e Usuários

Este é um projeto **boilerplate** que demonstra a implementação de padrões de arquitetura modernos e robustos, focado em **Clean Architecture**, **Vertical Slice** e **Multi-Tenancy** em um ambiente **.NET**.

---

## 🏗️ Arquitetura e Padrões de Design

O projeto adota uma arquitetura que combina vários padrões para garantir manutenibilidade, escalabilidade e separação de responsabilidades:

- **Clean Architecture:** Garante que as regras de negócio (`Domain` e `Application`) sejam independentes de detalhes de implementação (como `Infraestrutura` e `API`), facilitando a evolução e testes.
- **Vertical Slice Architecture:** As funcionalidades (features) são organizadas em "fatias" verticais, unindo todas as camadas (Domain, Application, Infra) necessárias para aquela funcionalidade específica. Isso melhora a coesão e diminui o acoplamento global.
- **CQRS (Command Query Responsibility Segregation):** Separa as responsabilidades de **escrita** (Commands) e **leitura** (Queries), otimizando o design e a performance para cada tipo de operação.

### Camadas do Projeto

O código está dividido em quatro camadas principais:

1.  **`Api`**: A camada de apresentação. Contém os _endpoints_ HTTP, _controllers_ e a configuração do _pipeline_ da aplicação. É a porta de entrada.
2.  **`Application`**: Contém a lógica de aplicação (use cases). Aqui ficam as implementações de **Commands** e **Queries** (CQRS), orquestrando as operações de domínio.
3.  **`Domain`**: O núcleo da aplicação. Contém as entidades, objetos de valor, agregados e as regras de negócio essenciais.
4.  **`Infra`**: Camada de infraestrutura. Contém a lógica de persistência de dados (Contexto do EF Core, Repositórios), configurações de banco de dados e serviços externos.

---

## 🏢 Multi-Tenancy e Segurança

Este projeto é baseado em um padrão de software **Multi-Tenant (Múltiplos Inquilinos)**.

- **Organização (Tenant):** Usuários que pertencem à mesma Organização (Tenant) conseguem realizar operações de escrita e leitura nos dados associados àquele tenant.
- **ABAC Simplificado:** É implementado um padrão simplificado de **Attribute-Based Access Control (ABAC)**. Toda requisição é validada para garantir que os dados lidos ou manipulados realmente pertencem ao **Tenant** ao qual o usuário está associado. Isso garante a segregação de dados entre diferentes inquilinos.

---

## ✨ Funcionalidades (Features)

O projeto implementa funcionalidades CRUD (Create, Read, Update, Delete) básicas para demonstrar a arquitetura:

- **Usuários (Users)**
- **Inquilinos (Tenants)**
- **Produtos (Products)**
- **Categorias de Produtos (Product Categories)**

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia         | Versão/Descrição                         |
| :----------------- | :--------------------------------------- |
| **Framework**      | **.NET 9**                               |
| **Banco de Dados** | **PostgreSQL**                           |
| **Padrão de API**  | CQRS, Clean Architecture, Vertical Slice |
| **Outros Padrões** | Multi-Tenancy, ABAC Simplificado         |

---

## 🚀 Como Executar Localmente

1.  **Pré-requisitos:** Certifique-se de ter o **.NET 9 SDK** instalado e uma instância do **PostgreSQL** em execução.
2.  **Configuração do BD:** Atualize a _string de conexão_ do PostgreSQL nos arquivos de configuração (`appsettings.json` ou variáveis de ambiente).
3.  **Migrações:** Execute as migrações do Entity Framework Core:
    ```bash
    dotnet ef database update --project SeuProjeto.Infra
    ```
4.  **Rodar o Projeto:** Inicie a API Apartir da pasta Api:
    ```bash
    dotnet run 
    ```
5.  O projeto estará acessível (por padrão) em `https://localhost:5001` (ou a porta configurada).
