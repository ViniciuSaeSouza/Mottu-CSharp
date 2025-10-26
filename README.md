# Mottu API - Gestão de Motos e Pátios

API RESTful desenvolvida em .NET 8 para gerenciamento de motos, pátios, usuários e dispositivos (carrapatos). A solução foi construída com foco em boas práticas REST, Clean Architecture, DDD e inclui documentação OpenAPI (Swagger) com implementação HATEOAS.

**Solução criada para o Challenge Mottu - 3ª Sprint**

---

## 👥 Integrantes - Equipe Prisma.Code

- **Laura de Oliveira Cintra** - RM 558843
- **Maria Eduarda Alves da Paixão** - RM 558832
- **Vinícius Saes de Souza** - RM 554456

---

## 🏛️ Justificativa da Arquitetura

A solução adota **Clean Architecture** e **Domain-Driven Design (DDD)** para manter as regras de negócio isoladas de detalhes de infraestrutura (banco de dados, frameworks web). Esta separação em camadas facilita:

- **Manutenibilidade**: Mudanças em uma camada não afetam as outras
- **Testabilidade**: Regras de negócio podem ser testadas independentemente
- **Evolução**: Possibilidade de trocar o mecanismo de persistência ou framework sem impactar o domínio
- **Clareza**: Responsabilidades bem definidas por camada

### Estrutura de Camadas:
- **Dominio**: Entidades, Value Objects, enums e exceções de negócio. Contém as regras fundamentais independentes de framework
- **Aplicacao**: DTOs, validações e serviços de aplicação. Orquestra operações entre domínio e infraestrutura
- **Infraestrutura**: Implementação de repositórios, contexto EF Core, mapeamentos e acesso a dados
- **API**: Controllers REST, configuração de rotas, Swagger e injeção de dependências

### Escolhas Técnicas:

**DTOs (Data Transfer Objects)**: Controlam a exposição de dados entre camadas e para consumidores externos, evitando vazamento de detalhes de implementação e permitindo contratos de API estáveis.

**HATEOAS (Hypermedia as the Engine of Application State)**: Implementado através do envelope `Recurso<T>` contendo `dados` e `links`. Cada resposta inclui links hipermídia (`rel`, `href`, `method`) que permitem aos clientes descobrir ações disponíveis sem hardcoding de URIs, melhorando a usabilidade e evolução da API.

**Entity Framework Core**: Abstrai o acesso ao banco Oracle, facilitando migrações, queries e evolução do modelo de dados.

**Swagger/OpenAPI**: Documenta automaticamente os endpoints com descrições, parâmetros e exemplos de payloads, reduzindo a curva de aprendizado para consumidores da API.

---

## ✅ Principais Funcionalidades

- CRUD completo para **Motos**, **Pátios**, **Usuários** e **Carrapatos** (dispositivos rastreadores)
- **HATEOAS** básico em todas as respostas (campo `links` com relações e URIs)
- **Paginação** para listagens grandes (motos e carrapatos)
- **Validações de domínio** com mensagens de erro apropriadas
- **Códigos HTTP adequados** (200, 201, 204, 400, 404, 500, 503)
- **Documentação Swagger** com descrições e exemplos de payloads XML

---

## 🔗 Como a API Retorna Recursos (HATEOAS)

A maioria dos endpoints retorna um envelope `Recurso<T>`:

```json
{
  "dados": { /* objeto DTO ou coleção */ },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/motos/1", "method": "GET" },
    { "rel": "update", "href": "https://localhost:5001/api/motos/1", "method": "PUT" },
    { "rel": "delete", "href": "https://localhost:5001/api/motos/1", "method": "DELETE" },
    { "rel": "collection", "href": "https://localhost:5001/api/motos", "method": "GET" }
  ]
}
```

Para endpoints paginados, a propriedade `dados` contém um objeto com `items`, `pagina`, `tamanhoPagina`, `totalPaginas`, `contagemTotal`.

---

## 📚 Endpoints Principais

### Motos (`/api/motos`)
- `GET /api/motos?pagina=1&tamanhoPagina=10` — Lista paginada de motos
- `GET /api/motos/{id}` — Obtém uma moto por ID
- `POST /api/motos` — Cria uma nova moto
- `PUT /api/motos/{id}` — Atualiza uma moto
- `DELETE /api/motos/{id}` — Remove uma moto

### Pátios (`/api/patios`)
- `GET /api/patios` — Lista todos os pátios
- `GET /api/patios/{id}` — Obtém um pátio por ID (com motos e usuários vinculados)
- `POST /api/patios` — Cria um novo pátio
- `PATCH /api/patios/{id}` — Atualiza parcialmente um pátio
- `DELETE /api/patios/{id}` — Remove um pátio

### Usuários (`/api/usuarios`)
- `GET /api/usuarios` — Lista todos os usuários
- `GET /api/usuarios/{id}` — Obtém um usuário por ID
- `POST /api/usuarios` — Cria um novo usuário
- `PUT /api/usuarios/{id}` — Atualiza um usuário
- `DELETE /api/usuarios/{id}` — Remove um usuário
- `POST /api/usuarios/login` — Autentica usuário (login)

### Carrapatos (`/api/carrapatos`)
- `GET /api/carrapatos?pagina=1&tamanhoPagina=10` — Lista paginada de carrapatos
- `GET /api/carrapatos/{id}` — Obtém um carrapato por ID
- `POST /api/carrapatos` — Cria um novo carrapato
- `PUT /api/carrapatos/{id}` — Atualiza um carrapato
- `DELETE /api/carrapatos/{id}` — Remove um carrapato

### Listas Auxiliares
- `GET /api/modelos-moto` — Lista modelos de moto disponíveis
- `GET /api/zonas` — Lista zonas disponíveis

---

## 🧩 Exemplos de Uso dos Endpoints

### Criar Moto — `POST /api/motos`

**Request body:**
```json
{
  "placa": "ABC1D23",
  "modelo": 4,
  "nomePatio": "Ipiranga",
  "chassi": "12345678901234567",
  "zona": 0,
  "idCarrapato": 1
}
```

**Response 201 Created:**
```json
{
  "dados": {
    "id": 1,
    "placa": "ABC1D23",
    "modelo": "POP",
    "nomePatio": "Ipiranga",
    "chassi": "12345678901234567",
    "zona": 0,
    "idCarrapato": 1
  },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/motos/1", "method": "GET" },
    { "rel": "update", "href": "https://localhost:5001/api/motos/1", "method": "PUT" },
    { "rel": "delete", "href": "https://localhost:5001/api/motos/1", "method": "DELETE" },
    { "rel": "collection", "href": "https://localhost:5001/api/motos", "method": "GET" }
  ]
}
```

### Obter Moto por ID — `GET /api/motos/1`

**Response 200 OK:**
```json
{
  "dados": {
    "id": 1,
    "placa": "ABC1D23",
    "modelo": "POP",
    "nomePatio": "Ipiranga",
    "chassi": "12345678901234567",
    "zona": 0,
    "idCarrapato": 1
  },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/motos/1", "method": "GET" },
    { "rel": "update", "href": "https://localhost:5001/api/motos/1", "method": "PUT" },
    { "rel": "delete", "href": "https://localhost:5001/api/motos/1", "method": "DELETE" },
    { "rel": "collection", "href": "https://localhost:5001/api/motos", "method": "GET" }
  ]
}
```

### Listar Motos (Paginado) — `GET /api/motos?pagina=1&tamanhoPagina=10`

**Response 200 OK:**
```json
{
  "dados": {
    "items": [
      {
        "id": 1,
        "placa": "ABC1D23",
        "modelo": "SPORT",
        "nomePatio": "Ipiranga",
        "chassi": "12345678901234567",
        "zona": 0,
        "idCarrapato": 3
      }
    ],
    "pagina": 1,
    "tamanhoPagina": 10,
    "contagemTotal": 25,
    "totalPaginas": 3,
    "temProximo": true,
    "temAnterior": false
  },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/motos?pagina=1&tamanhoPagina=10", "method": "GET" },
    { "rel": "first", "href": "https://localhost:5001/api/motos?pagina=1&tamanhoPagina=10", "method": "GET" },
    { "rel": "next", "href": "https://localhost:5001/api/motos?pagina=2&tamanhoPagina=10", "method": "GET" },
    { "rel": "last", "href": "https://localhost:5001/api/motos?pagina=3&tamanhoPagina=10", "method": "GET" }
  ]
}
```

### Criar Usuário — `POST /api/usuarios`

**Request body:**
```json
{
  "nome": "João Silva",
  "email": "joao@empresa.com",
  "senha": "Senha@123",
  "idPatio": 1
}
```

**Response 201 Created:**
```json
{
  "dados": {
    "idUsuario": 10,
    "nome": "João Silva",
    "email": "joao@empresa.com",
    "senha": "Senha@123",
    "nomePatio": "Pátio Central",
    "idPatio": 1
  },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/usuarios/10", "method": "GET" },
    { "rel": "update", "href": "https://localhost:5001/api/usuarios/10", "method": "PUT" },
    { "rel": "delete", "href": "https://localhost:5001/api/usuarios/10", "method": "DELETE" },
    { "rel": "collection", "href": "https://localhost:5001/api/usuarios", "method": "GET" }
  ]
}
```

### Criar Pátio — `POST /api/patios`

**Request body:**
```json
{
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000"
}
```

**Response 201 Created:**
```json
{
  "dados": {
    "id": 1,
    "nome": "Pátio Central",
    "endereco": "Av. Brasil, 1000",
    "motos": [],
    "usuarios": []
  },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/patios/1", "method": "GET" },
    { "rel": "update", "href": "https://localhost:5001/api/patios/1", "method": "PATCH" },
    { "rel": "delete", "href": "https://localhost:5001/api/patios/1", "method": "DELETE" },
    { "rel": "collection", "href": "https://localhost:5001/api/patios", "method": "GET" }
  ]
}
```

### Criar Carrapato — `POST /api/carrapatos`

**Request body:**
```json
{
  "codigoSerial": "CAR-0001-XYZ",
  "idPatio": 1
}
```

**Response 201 Created:**
```json
{
  "dados": {
    "id": 3,
    "codigoSerial": "CAR-0001-XYZ",
    "statusBateria": "Alta",
    "statusDeUso": "Disponivel",
    "idPatio": 1
  },
  "links": [
    { "rel": "self", "href": "https://localhost:5001/api/carrapatos/3", "method": "GET" },
    { "rel": "update", "href": "https://localhost:5001/api/carrapatos/3", "method": "PUT" },
    { "rel": "delete", "href": "https://localhost:5001/api/carrapatos/3", "method": "DELETE" },
    { "rel": "collection", "href": "https://localhost:5001/api/carrapatos", "method": "GET" }
  ]
}
```

---

## ⚙️ Tecnologias Utilizadas

- **.NET 8** / ASP.NET Core
- **C# 12**
- **Entity Framework Core 9**
- **Oracle Database** (via Oracle.EntityFrameworkCore)
- **Swashbuckle** (Swagger/OpenAPI)
- **Clean Architecture**
- **Domain-Driven Design (DDD)**

---

## 🏗️ Instruções de Execução da API

### Pré-requisitos
- .NET 8 SDK instalado ([Download aqui](https://dotnet.microsoft.com/download/dotnet/8.0))
- Acesso a um banco de dados Oracle (ou ajuste a connection string para outro provedor compatível)

### 1. Configurar a Connection String

Escolha uma das opções:

**Opção A — Arquivo `.env` na raiz da solução** (recomendado para desenvolvimento):
```env
Connection__String=Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA
```

**Opção B — `appsettings.json`** (em `API/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA"
  }
}
```

**Opção C — Variável de ambiente** (Windows CMD):
```cmd
set Connection__String=Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA
```

> **Nota**: A aplicação tenta ler `Connection__String` da variável de ambiente primeiro; se ausente, usa `ConnectionStrings:Oracle` do `appsettings.json`.

### 2. Restaurar Pacotes e Compilar

```bash
dotnet restore
dotnet build
```

### 3. (Opcional) Aplicar Migrations ao Banco

Se você possui o `dotnet-ef` instalado globalmente:

```bash
dotnet tool install --global dotnet-ef
cd Infraestrutura
dotnet ef database update --startup-project ..\API
cd ..
```

### 4. Executar a API

```bash
cd API
dotnet run
```

A API iniciará e exibirá as URLs de acesso no console (geralmente `https://localhost:7018` e `http://localhost:5157`).

### 5. Acessar o Swagger

Abra o navegador em:
- **HTTPS**: `https://localhost:7018/swagger`
- **HTTP**: `http://localhost:5157/swagger`

---

## 🐳 Executar via Docker (Imagem Pública)

A API está disponível como imagem pública no Docker Hub: **`saesminerais/mottu:3.6.7`**

### Pré-requisito
- Docker instalado ([Download aqui](https://www.docker.com/products/docker-desktop))
- Acesso à base Oracle

### Passo 1: Baixar a Imagem

```bash
docker pull saesminerais/mottu:3.6.7
```

### Passo 2: Executar o Container

**Windows CMD:**
```cmd
docker run -d --name mottu-api -p 8080:8080 -e Connection__String="Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA" saesminerais/mottu:3.6.7
```

**Git Bash / Linux / Mac:**
```bash
docker run -d \
  --name mottu-api \
  -p 8080:8080 \
  -e Connection__String="Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA" \
  saesminerais/mottu:3.6.7
```

### Passo 3: Acessar o Swagger

Abra o navegador em: `http://localhost:8080/swagger`

> **Dica**: Em ambientes Windows, se o Oracle estiver rodando no host (fora do container), use `host.docker.internal` no lugar de `localhost` no Data Source:
> ```
> Data Source=host.docker.internal:1521/SERVICE;...
> ```

---

## 🧪 Testes

### Como Rodar os Testes

Para executar os testes da solução, utilize o comando:

```bash
dotnet test
```

### Executar testes com relatório de cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Executar testes de um projeto específico:

```bash
dotnet test Mottu.Tests/Mottu.Tests.csproj
```

### Criar Projeto de Testes (Se Ainda Não Existe)

Caso deseje adicionar testes unitários e de integração à solução:

```bash
# Criar projeto de testes xUnit
dotnet new xunit -n Mottu.Tests

# Adicionar referências aos projetos
cd Mottu.Tests
dotnet add reference ../Dominio/Dominio.csproj
dotnet add reference ../Aplicacao/Aplicacao.csproj
dotnet add reference ../Infraestrutura/Infraestrutura.csproj
dotnet add reference ../API/API.csproj

# Adicionar o projeto à solução
cd ..
dotnet sln add Mottu.Tests/Mottu.Tests.csproj
```

### Pacotes Recomendados para Testes:

```bash
cd Mottu.Tests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.AspNetCore.Mvc.Testing
```

---

## 📘 Documentação Swagger/OpenAPI

O projeto inclui geração automática de documentação via **Swashbuckle**. A configuração está em `Program.cs` e a geração de arquivo XML de documentação está habilitada no `API.csproj`.

Ao executar a aplicação (`dotnet run`), acesse `/swagger` para visualizar:
- Lista de endpoints organizados por tags (Motos, Pátios, Usuários, Carrapatos)
- Descrições de cada operação
- Parâmetros com tipos e valores padrão
- Exemplos de request bodies (XML comments)
- Modelos de resposta (schemas)

---

## 📝 Observações e Melhorias Futuras

- As respostas usam o envelope `Recurso<T>` com `links` HATEOAS. Para outros formatos (HAL, JSON:API), podemos adaptar facilmente.
- Melhorias possíveis:
  - Adicionar testes unitários e de integração
  - Implementar autenticação/autorização (JWT)
  - Adicionar exemplos interativos no Swagger (Swashbuckle.AspNetCore.Filters)
  - Corrigir warnings de nullable para maior robustez
  - Implementar cache para endpoints de leitura
  - Adicionar logging estruturado (Serilog)

---

## 📞 Contato

**Equipe Prisma.Code**
- Email: prismacode3@gmail.com

---

> *"Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda."* — Mario Sergio Cortella
