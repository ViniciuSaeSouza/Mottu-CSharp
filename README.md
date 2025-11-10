# Mottu API

API RESTful em .NET 8 para gestão de motos, pátios (filiais) e usuários, com autenticação JWT, versionamento de API, Health Checks e endpoints de predição com ML.NET. Utiliza Oracle (EF Core) e segue uma arquitetura por camadas inspirada em Clean Architecture e DDD.

## Recursos principais
- CRUD para Motos, Pátios e Usuários
- Paginação e HATEOAS nas listagens de Motos
- Autenticação e autorização com JWT (Swagger integrado)
- Versionamento de API (v1, v2) via header ou query string
- Health Checks: geral, readiness e liveness
- Predições com ML.NET (manutenção de motos)
- Swagger/OpenAPI com exemplos e modelos

## Quickstart
Pré-requisitos: 
- .NET 8 SDK instalado
- Projeto clonado
- Terminal na Raiz do projeto `Mottu-CSharp\`

### 1) Restaurar dependências
```bash
dotnet restore
```

### 2) Configurar conexão com Oracle
Edite o arquivo `API/appsettings.json` e substitua a connection string Oracle:

```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=your-oracle-host:1521/your-service-name;User Id=your-username;Password=your-password;"
  }
}
```

**Exemplo para ambiente FIAP:**
```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=RM123456;Password=sua-senha-aqui;"
  }
}
```

### 3) Executar a API
```bash
dotnet run --project API --launch-profile "https"
```

### 4) Acessar a documentação
- Swagger UI: https://localhost:7018/swagger
- Health Check Geral: https://localhost:7018/health
- Health Check Live: https://localhost:7018/health/live
- Health Check Ready: https://localhost:7018/health/ready

## Configuração

### Configuração da Connection String Oracle:
Edite o arquivo `API/appsettings.json` e configure a seção ConnectionStrings:

```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=hostname:port/service_name;User Id=username;Password=password;"
  }
}
```

## Segurança
- ⚠️ Não commite credenciais reais no appsettings.json
- ✅ Para produção, use variáveis de ambiente ou Azure Key Vault
- ✅ Adicione appsettings.json ao .gitignore se contiver credenciais sensíveis

## Versionamento
A API suporta múltiplas versões (v1 e v2).
- Via Header: `X-Version: 2.0`
- Via Query String: `?version=2.0`

## Autenticação
- POST `/api/auth/login` — retorna um token JWT
  - Exemplo de payload:
    ```json
    { "email": "admin", "senha": "admin123" }
    ```
- Use o token nas chamadas protegidas: `Authorization: Bearer <token>`

## Endpoints principais

### Motos
- GET `/api/motos?pagina=1&tamanhoPagina=10` — lista paginada + HATEOAS
- GET `/api/motos/{id}` — obter por id
- POST `/api/motos` — criar
  - Exemplo:
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
- PUT `/api/motos/{id}` — atualizar
- DELETE `/api/motos/{id}` — remover

### Pátios
- GET `/api/patios`
- GET `/api/patios/{id}`
- POST `/api/patios`
- PATCH `/api/patios/{id}`
- DELETE `/api/patios/{id}`

### Usuários
- GET `/api/usuarios`
- GET `/api/usuarios/{id}`
- POST `/api/usuarios`
- PUT `/api/usuarios/{id}`
- DELETE `/api/usuarios/{id}`

### Predições (ML.NET)
- POST `/api/predicao/manutencao-moto` — predição individual (não requer JWT)
  - Exemplo:
    ```json
    { "kmRodados": 15000, "tempoUso": 12, "numeroViagens": 300, "idadeVeiculo": 2 }
    ```
- POST `/api/predicao/analise-frota` — análise de frota (requer JWT)

### Health Checks
- GET `/health` — status geral
- GET `/health/ready` — dependências (Oracle, repositórios)
- GET `/health/live` — liveness

## Testes
- Executar todos os testes
```bash
dotnet test
```
- Relatório de cobertura (opcional)
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Docker
- Buildar imagem
```bash
docker build -t mottu-api .
```
- Executar container
```bash
docker run -p 8080:80 -e Connection__String="<sua-connection-string>" mottu-api
```

## Arquitetura (resumo)
- Domínio: entidades, exceções e contratos (sem dependências externas)
- Aplicação: serviços, DTOs e validações (orquestra o domínio)
- Infraestrutura: EF Core, mapeamentos e repositórios
- API: controladores, versionamento, JWT, Health Checks e Swagger

## Contribuidores
- Prisma.Code
  - Laura de Oliveira Cintra — RM 558843
  - Maria Eduarda Alves da Paixão — RM 558832
  - Vinícius Saes de Souza — RM 554456
