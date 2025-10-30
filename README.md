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

## 🏛️ Justificativa da Arquitetura

A solução foi estruturada seguindo os princípios da Clean Architecture e Domain-Driven Design (DDD), visando alta coesão, baixo acoplamento e facilidade de manutenção. A separação em camadas (Domínio, Aplicação, Infraestrutura e API) permite que regras de negócio fiquem isoladas de detalhes de implementação, como persistência e exposição via HTTP. O uso de DTOs garante segurança e clareza na comunicação entre camadas e com o cliente. O Entity Framework Core foi adotado para abstrair o acesso ao banco Oracle, facilitando testes e evolução futura. A API expõe endpoints RESTful com boas práticas, incluindo paginação, status codes adequados e documentação automática via Swagger/OpenAPI, promovendo interoperabilidade e facilidade de uso para integradores.

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
- **Health Checks para monitoramento de saúde da aplicação e banco de dados.**

---

## 🏥 Health Checks

A API implementa endpoints de health checks para monitoramento da saúde da aplicação, ideal para uso com Kubernetes, Docker, ou qualquer sistema de orquestração.

### Endpoints Disponíveis

#### 1. Health Check Completo
```
GET /health
```

Retorna o status geral da aplicação, incluindo:
- Conexão com banco de dados Oracle (DbContext)
- Verificação direta da conexão Oracle
- Status da API

**Resposta de Sucesso (200 OK):**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.1234567",
  "entries": {
    "oracle-db": {
      "status": "Healthy",
      "duration": "00:00:00.0567890",
      "tags": ["db", "oracle", "database"]
    },
    "oracle-connection": {
      "status": "Healthy",
      "duration": "00:00:00.0456789",
      "tags": ["db", "oracle", "sql"]
    },
    "api-health": {
      "status": "Healthy",
      "description": "API está funcionando corretamente",
      "duration": "00:00:00.0001234",
      "tags": ["api", "ready"]
    }
  }
}
```

**Resposta de Falha (503 Service Unavailable):**
```json
{
  "status": "Unhealthy",
  "totalDuration": "00:00:05.1234567",
  "entries": {
    "oracle-db": {
      "status": "Unhealthy",
      "description": "Cannot connect to database",
      "duration": "00:00:05.0567890",
      "exception": "Oracle.ManagedDataAccess.Client.OracleException: ...",
      "tags": ["db", "oracle", "database"]
    }
  }
}
```

#### 2. Readiness Check
```
GET /health/ready
```

Verifica se a aplicação está pronta para receber tráfego. Valida todas as dependências críticas (banco de dados, serviços externos, etc.).

**Uso:** Utilize em `readinessProbe` do Kubernetes ou health checks de load balancers.

**Status Codes:**
- `200 OK` — Aplicação pronta para receber requisições
- `503 Service Unavailable` — Aplicação não está pronta (banco de dados indisponível, etc.)

#### 3. Liveness Check
```
GET /health/live
```

Verifica se a aplicação está rodando (não verifica dependências externas). Retorna sempre `200 OK` se o processo estiver ativo.

**Status Codes:**
- `200 OK` — Aplicação está rodando

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
- `GET /api/patios` — Lista todos os patios (com até 10 motos e 10 usuários por pátio).
- `GET /api/patios/{id}` — Consulta um patio pelo ID (inclui motos e usuários do pátio).
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
2) Execute o container (Windows CMD):
```
docker run -d --name mottu-api -p 8080:8080 -e Connection__String="Data Source=HOST:1521/SERVICE;User ID=USUARIO;Password=SENHA" saesminerais/mottu:3.6.7
```
2.1) Execute o container (GitBash / Linux):
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

```yaml
version: "3.8"
services:
  mottu:
    image: saesminerais/mottu:latest
    ports:
      - "8080:8080"
    environment:
      - Connection__String=Data Source=host.docker.internal:1521/SERVICE;User ID=USUARIO;Password=SENHA
    restart: unless-stopped
```

---

## 🧪 Testes e Cobertura

Adicionei um projeto de testes xUnit na solução para a camada de aplicação em `TestsAplicacao.Tests`. Atualmente ele contém apenas o arquivo vazio `UnitTest1.cs` e ainda não possui testes para o serviço `UsuarioServico`.

Objetivo desta seção:
- Explicar como executar os testes localmente (Windows CMD e Git Bash).
- Mostrar como coletar cobertura (coverlet) e gerar relatório HTML (reportgenerator).
- Dar dicas rápidas de troubleshooting quando `dotnet` não for encontrado no terminal.

Pré-requisitos
- .NET SDK 8 instalado e disponível no PATH.
- (Opcional) `dotnet-reportgenerator-globaltool` para gerar HTML a partir do arquivo de cobertura.

Executando todos os testes (com coleta de cobertura)

Windows CMD:
```cmd
cd C:\caminho\para\seu\repo\CSharp
dotnet restore
dotnet test Tests\Aplicacao.Tests\Aplicacao.Tests.csproj --collect:"XPlat Code Coverage"
```

Git Bash (Linux-like):
```bash
cd /c/caminho/para/seu/repo/CSharp
dotnet restore
dotnet test Tests/Aplicacao.Tests/Aplicacao.Tests.csproj --collect:"XPlat Code Coverage"
```

Após a execução, o arquivo de cobertura normalmente é gerado em:
```
Tests/Aplicacao.Tests/TestResults/<GUID>/coverage.cobertura.xml
```

Gerando um relatório HTML legível (reportgenerator)
1) Instale a ferramenta (uma vez):
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
# certifique-se que ~/.dotnet/tools esteja no PATH do terminal
```
2) Gere o relatório (a partir da raiz do repositório):
```bash
reportgenerator -reports:"Tests/Aplicacao.Tests/TestResults/*/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```
3) Abra o relatório no Windows:
```bash
cmd.exe /c "start coverage-report\\index.htm"
# ou abra a pasta coverage-report no Explorer
explorer.exe coverage-report
```

Executando um único teste (filtro)
```cmd
dotnet test Tests\Aplicacao.Tests\Aplicacao.Tests.csproj --filter "FullyQualifiedName~Aplicacao.Tests.UsuarioServicoTests.AutenticarLogin_Sucesso_RetornaUsuarioDto"
```

Dicas de troubleshooting
- Se `dotnet` não for encontrado no Git Bash, verifique:
```bash
which dotnet || echo "dotnet not found by which"
cmd.exe /c "dotnet --info"
```
Se `cmd.exe /c "dotnet --info"` funcionar mas `which dotnet` não, adicione `C:\Program Files\dotnet` ao PATH do Git Bash (arquivo `~/.bashrc`):
```bash
export PATH="$PATH:/c/Program Files/dotnet"
export PATH="$PATH:$HOME/.dotnet/tools"  # se usar tools globais
```
- Limpar cache NuGet (se ocorrerem erros de restore):
```cmd
dotnet nuget locals all --clear
dotnet restore
```

Sugestão opcional: integração contínua (GitHub Actions)
- Recomendo criar um workflow que execute `dotnet test --collect:"XPlat Code Coverage"` e publique o relatório como artifact. Posso gerar um arquivo de workflow pronto se desejar.

Locais relevantes
- Projeto de testes: `Tests/Aplicacao.Tests/`
- Testes adicionados: `Tests/Aplicacao.Tests/UsuarioServicoTests.cs`

---

## 📑 Exemplos de Uso dos Endpoints

### Motos

- Criar moto — `POST /api/motos`
Request
```json
{
  "placa": "ABC1D23",
  "chassi": "9BWZZZ377VT004251",
  "idPatio": 1
}
```
Response 201
```json
{
  "id": 1,
  "placa": "ABC1D23",
  "modelo": "POP",
  "nomePatio": "Pátio Central",
  "chassi": "9BWZZZ377VT004251",
  "zona": 0,
  "idCarrapato": 3
}
```

- Atualizar moto (PUT) — `PUT /api/motos/1`
Request
```json
{
  "placa": "DEF4G56",
  "modelo": 2,
  "idPatio": 1,
  "idCarrapato": 3,
  "zona": 1
}
```
Response 200 (mesma forma do GET por id)
```json
{
  "id": 1,
  "placa": "DEF4G56",
  "modelo": "E",
  "nomePatio": "Pátio Central",
  "chassi": "9BWZZZ377VT004251",
  "zona": 1,
  "idCarrapato": 3
}
```
Observações:
- `modelo` no request é numérico (enum: 1=SPORT, 2=E, 3=POP). No response, vem como string UPPERCASE.
- `zona` é numérico (enum: 0=Saguao, 1=ManutencaoRapida, 2=DanosEstruturais, 3=SemPlaca, 4=BoletimOcorrencia, 5=Aluguel, 6=MotorDefeituoso).

- Atualizar parcialmente (PATCH) — `PATCH /api/motos/1`
Request
```json
{
  "placa": "DEF4G56"
}
```
Response 200 — corpo igual ao GET por id.

- Listar motos (paginação) — `GET /api/motos?pagina=1&tamanhoPagina=10`
Response 200
```json
{
  "temProximo": true,
  "temAnterior": false,
  "items": [
    {
      "id": 1,
      "placa": "ABC1D23",
      "modelo": "SPORT",
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
Request
```json
{
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000"
}
```
Response 201
```json
{
  "id": 1,
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000",
  "motos": [],
  "usuarios": []
}
```

- Obter patio — `GET /api/patios/1`
Response 200 (motos/usuarios conforme dados)
```json
{
  "id": 1,
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000",
  "motos": [
    {
      "id": 1,
      "placa": "ABC1D23",
      "modelo": "POP",
      "nomePatio": "Pátio Central",
      "chassi": "9BWZZZ377VT004251",
      "zona": 0,
      "idCarrapato": 3
    }
  ],
  "usuarios": [
    {
      "idUsuario": 10,
      "nome": "João Silva",
      "email": "joao@empresa.com",
      "senha": "Senha@123",
      "nomePatio": "Pátio Central",
      "idPatio": 1
    }
  ]
}
```

- Atualizar parcialmente — `PATCH /api/patios/1`
Request
```json
{
  "endereco": "Av. Brasil, 1500"
}
```
Response 200 — corpo igual ao GET por id.

### Usuários

- Criar usuário — `POST /api/usuarios`
Request
```json
{
  "nome": "João Silva",
  "email": "joao@empresa.com",
  "senha": "Senha@123",
  "idPatio": 1
}
```
Response 201
```json
{
  "idUsuario": 10,
  "nome": "João Silva",
  "email": "joao@empresa.com",
  "senha": "Senha@123",
  "nomePatio": "Pátio Central",
  "idPatio": 1
}
```

- Atualizar usuário — `PUT /api/usuarios/10`
Request
```json
{
  "nome": "João S. Silva",
  "email": "joaos@empresa.com",
  "senha": "NovaSenha@123"
}
```
Response 200 — mesmo formato do criar.

- Login — `POST /api/usuarios/login`
Request
```json
{
  "email": "joao@empresa.com",
  "senha": "Senha@123"
}
```
Response 200
```json
{
  "idUsuario": 10,
  "nome": "João Silva",
  "email": "joao@empresa.com",
  "senha": "Senha@123",
  "nomePatio": "Pátio Central",
  "idPatio": 1
}
```

### Carrapatos

- Criar carrapato — `POST /api/carrapatos`
Request
```json
{
  "codigoSerial": "CAR-0001-XYZ",
  "idPatio": 1
}
```
Response 201
```json
{
  "id": 3,
  "codigoSerial": "CAR-0001-XYZ",
  "statusBateria": "Alta",
  "statusDeUso": "Disponivel",
  "idPatio": 1
}
```

### Listas auxiliares

- Modelos de moto — `GET /api/modelos-moto`
Response 200
```json
[
  { "id": 1, "nome": "SPORT" },
  { "id": 2, "nome": "E" },
  { "id": 3, "nome": "POP" }
]
```

- Zonas — `GET /api/zonas`
Response 200
```json
[
  { "id": 0, "nome": "Saguao" },
  { "id": 1, "nome": "ManutencaoRapida" },
  { "id": 2, "nome": "DanosEstruturais" },
  { "id": 3, "nome": "SemPlaca" },
  { "id": 4, "nome": "BoletimOcorrencia" },
  { "id": 5, "nome": "Aluguel" },
  { "id": 6, "nome": "MotorDefeituoso" }
]
```

---

## 👥 Equipe - Prisma.Code
- Laura de Oliveira Cintra - RM 558843
- Maria Eduarda Alves da Paixão - RM 558832
- Vinícius Saes de Souza - RM 554456

> “Faça o teu melhor, na condição que você tem, enquanto você não tem condições melhores, para fazer melhor ainda.” — Mario Sergio Cortella
