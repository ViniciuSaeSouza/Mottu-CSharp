# Mottu API - Gestão de Motos e Patios

API RESTful desenvolvida em .NET 8 para gerenciamento de motos e patios, utilizando Oracle, Entity Framework Core, Clean Architecture e princípios de DDD.

*solução criada para o **Challenge Mottu***
---

## 📦 Estrutura da Solução

- **Dominio**: Entidades de domínio, enums e exceções.
- **Aplicacao**: DTOs, validações e mapeamentos.
- **Infraestrutura**: Contexto do EF Core, configurações de banco e mapeamentos Fluent API.
- **API**: Controladores, endpoints, configuração de rotas e Swagger.

---

## 🚀 Funcionalidades

- Cadastro, consulta, atualização e remoção de motos.
- Cadastro, consulta, atualização e remoção de patios.
- Cadastro, consulta, atualização e remoção de usuários.
- Cadastro, consulta, atualização e remoção de carrapatos (rastreador).
- Listagens auxiliares de modelos de moto e zonas.
- Paginação no endpoint de listagem de motos.
- Validações de domínio e unicidade de placa.
- Documentação automática via Swagger/OpenAPI.
- Respostas HTTP padronizadas (200, 201, 204, 400, 401, 404, 500, 503).
- Uso de DTOs para entrada e saída de dados.
- Injeção de dependência e separação por camadas.

---

## 🔗 Endpoints Principais

### Motos (`api/motos`)
- `GET /api/motos?pagina=1&tamanhoPagina=10` — Lista paginada de motos.
- `GET /api/motos/{id}` — Consulta uma moto pelo ID.
- `POST /api/motos` — Cadastra uma nova moto.
- `PUT /api/motos/{id}` — Atualiza totalmente uma moto.
- `PATCH /api/motos/{id}` — Atualiza parcialmente uma moto.
- `DELETE /api/motos/{id}` — Remove uma moto.

### Patios (`api/patios`)
- `GET /api/patios` — Lista todos os patios (sem motos associadas).
- `GET /api/patios/{id}` — Consulta um patio pelo ID (pode incluir dados relacionados conforme DTO).
- `POST /api/patios` — Cadastra um novo patio.
- `PATCH /api/patios/{id}` — Atualiza parcialmente um patio.
- `DELETE /api/patios/{id}` — Remove um patio.

### Usuários (`api/usuarios`)
- `GET /api/usuarios` — Lista todos os usuários.
- `GET /api/usuarios/{id}` — Consulta um usuário pelo ID.
- `POST /api/usuarios` — Cadastra um novo usuário.
- `PUT /api/usuarios/{id}` — Atualiza um usuário.
- `DELETE /api/usuarios/{id}` — Remove um usuário.
- `POST /api/usuarios/login` — Autentica usuário (login).

### Carrapatos (`api/carrapatos`)
- `GET /api/carrapatos` — Lista todos os carrapatos.
- `GET /api/carrapatos/{id}` — Consulta um carrapato pelo ID.
- `POST /api/carrapatos` — Cadastra um novo carrapato.
- `PUT /api/carrapatos/{id}` — Atualiza um carrapato.
- `DELETE /api/carrapatos/{id}` — Remove um carrapato.

### Listas auxiliares
- `GET /api/modelos-moto` — Modelos de moto disponíveis.
- `GET /api/zonas` — Zonas disponíveis.

---

## ⚙️ Tecnologias Utilizadas

- .NET 8 / ASP.NET Core
- C# 12
- Entity Framework Core 9
- Oracle (Oracle.EntityFrameworkCore / ODP.NET Core)
- Swagger (Swashbuckle)
- Clean Architecture
- Domain-Driven Design (DDD)

---

## 🏗️ Como Executar

Pré-requisitos: .NET SDK 8 instalado. Banco Oracle acessível e string de conexão válida.

1) Configure a string de conexão
- Opção A — arquivo `.env` na raiz da solução:
```
Connection__String=Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA
```
- Opção B — `appsettings.json` (API/appsettings.json):
```
{
  "ConnectionStrings": {
    "Oracle": "Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA"
  }
}
```
- Opção C — variável de ambiente (sessão atual do Windows CMD):
```
set Connection__String=Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA
```
Observação: a aplicação lê `Connection__String` via variável de ambiente; se ausente, usa `ConnectionStrings:Oracle` do appsettings.

2) Restaurar e compilar
```
dotnet restore
dotnet build
```

3) (Opcional) Aplicar migrations no banco
- Requer o `dotnet-ef` instalado globalmente e usa o projeto API como startup.
```
dotnet tool update --global dotnet-ef
cd Infraestrutura
dotnet ef database update --startup-project ..\API
cd ..
```

4) Executar a API (perfil https)
```
cd API
dotnet run
```

Acesse o Swagger em:
- HTTP:  http://localhost:5157/swagger
- HTTPS: https://localhost:7018/swagger

---

## 🐳 Executar via Docker

A API está disponível como imagem pública no Docker Hub: `saesminerais/mottu:3.6.7`.

- Pré-requisito: ter o Docker instalado e acesso à base Oracle.
- A imagem escuta na porta interna 8080.

Passos:
1) Baixe a imagem
```
docker pull saesminerais/mottu:3.6.7
```
2) Execute o container (GitBash / Linux):
```
docker run -d \
--name mottu-api \
-p 8080:8080 \
-e Connection__String="Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA" \
saesminerais/mottu:3.6.7
```
Notas:
- Se preferir usar o appsettings, você pode fornecer `-e ConnectionStrings__Oracle="..."` (a aplicação tenta `Connection__String` e, se ausente, usa `ConnectionStrings:Oracle`).
- Em alguns ambientes, para acessar um Oracle no host a partir do container, use `host.docker.internal` no Data Source (ex.: `Data Source=host.docker.internal:1521/SERVICE;...`).

Acesse o Swagger: http://localhost:8080/swagger

---

## 📑 Exemplos de Uso dos Endpoints

### Motos

- Criar moto — `POST /api/motos`
```json
{
  "placa": "ABC1D23",
  "chassi": "9BWZZZ377VT004251",
  "idPatio": 1
}
```
- Atualizar moto (PUT) — `PUT /api/motos/1`
```json
{
  "placa": "DEF4G56",
  "modelo": 2,
  "idPatio": 1,
  "idCarrapato": 3,
  "zona": 1
}
```
- Atualizar parcialmente (PATCH) — `PATCH /api/motos/1`
```json
{
  "placa": "DEF4G56"
}
```
- Listar motos (paginação) — `GET /api/motos?pagina=1&tamanhoPagina=10`
```json
{
  "temProximo": true,
  "temAnterior": false,
  "items": [
    {
      "id": 1,
      "placa": "ABC1D23",
      "modelo": "HondaBiz",
      "nomePatio": "Pátio Central",
      "chassi": "9BWZZZ377VT004251",
      "zona": 0,
      "idCarrapato": 3
    }
  ],
  "pagina": 1,
  "tamanhoPagina": 10,
  "contagemTotal": 25,
  "totalPaginas": 3
}
```

### Patios
- Criar patio — `POST /api/patios`
```json
{
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000"
}
```
- Atualizar parcialmente — `PATCH /api/patios/1`
```json
{
  "endereco": "Av. Brasil, 1500"
}
```

### Usuários
- Criar usuário — `POST /api/usuarios`
```json
{
  "nome": "João Silva",
  "email": "joao@empresa.com",
  "senha": "Senha@123",
  "idPatio": 1
}
```
- Login — `POST /api/usuarios/login`
```json
{
  "email": "joao@empresa.com",
  "senha": "Senha@123"
}
```

### Carrapatos
- Criar carrapato — `POST /api/carrapatos`
```json
{
  "codigoSerial": "CAR-0001-XYZ",
  "idPatio": 1
}
```

### Listas auxiliares
- Modelos de moto — `GET /api/modelos-moto`
- Zonas — `GET /api/zonas`

---

## 👥 Equipe - Prisma.Code
- Laura de Oliveira Cintra - RM 558843
- Maria Eduarda Alves da Paixão - RM 558832
- Vinícius Saes de Souza - RM 554456

> “Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda.” — Mario Sergio Cortella