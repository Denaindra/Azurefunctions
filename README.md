# Azure Functions - connectWithGPT

A modern Azure Functions application built with .NET 8.0 and the isolated worker model, featuring HTTP triggers with built-in OpenTelemetry observability and Application Insights integration.

## 📋 Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Installation](#installation)
- [Development](#development)
- [Deployment](#deployment)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Troubleshooting](#troubleshooting)
- [License](#license)

## 📖 Overview

This project implements Azure Functions using the latest isolated worker model with:

- **Runtime**: .NET 8.0
- **Azure Functions Version**: v4
- **Model**: Isolated Worker (Exe)
- **Observability**: OpenTelemetry with Application Insights
- **HTTP Support**: ASP.NET Core integration

### Key Features

✅ Modern .NET 8.0 with latest security patches  
✅ OpenTelemetry for distributed tracing  
✅ Application Insights integration  
✅ HTTP trigger with GET/POST support  
✅ Nullable reference types enabled  
✅ Implicit using statements  

## 🔧 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local) (v4.x)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli) (for deployment)
- [Git](https://git-scm.com/)
- [Visual Studio Code](https://code.visualstudio.com/) (optional, but recommended)

### Verify Installation

```bash
dotnet --version
func --version
az --version
```

## 📁 Project Structure

```
functions/
├── HttpTriggerGPT.cs           # HTTP trigger function
├── Program.cs                  # Application startup and configuration
├── functions.csproj            # Project file with dependencies
├── host.json                   # Azure Functions host configuration
├── local.settings.json         # Local development settings
├── .vscode/                    # VS Code configuration
│   ├── launch.json            # Debug configuration
│   ├── tasks.json             # Task definitions
│   ├── settings.json          # Editor settings
│   └── extensions.json        # Recommended extensions
├── Properties/
│   └── launchSettings.json    # Launch profile settings
├── bin/                        # Build output
└── obj/                        # Build intermediate files
```

## 🚀 Installation

### 1. Clone the Repository

```bash
git clone <repository-url>
cd functions
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

## 💻 Development

### Running Locally

Start the Azure Functions runtime locally:

```bash
func start
```

The function will be available at:
- `http://localhost:7071/api/HttpTriggerGPT`

### Making Requests

**GET Request:**
```bash
curl http://localhost:7071/api/HttpTriggerGPT
```

**POST Request:**
```bash
curl -X POST http://localhost:7071/api/HttpTriggerGPT
```

### Debugging

In VS Code, press `F5` to start debugging. Breakpoints will work in your function code.

### Monitoring Local Execution

Logs are printed to the terminal where `func start` is running. You'll see:
- Request information
- Function execution logs
- Any exceptions or errors

## 🌐 Deployment

### Prerequisites for Deployment

1. An Azure subscription
2. A resource group
3. A storage account
4. Azure CLI installed and logged in

### Step-by-Step Deployment

#### 1. Login to Azure

```bash
az login
```

#### 2. Create Azure Resources (if not already created)

```powershell
# Set variables
$resourceGroup = "pay as you go"
$storageAccount = "connectwithgptstorage"
$functionApp = "connectWithGPT"
$region = "centralus"

# Create storage account
az storage account create `
  --resource-group $resourceGroup `
  --name $storageAccount `
  --location $region `
  --sku Standard_LRS

# Create Function App
az functionapp create `
  --resource-group $resourceGroup `
  --consumption-plan-location $region `
  --runtime dotnet-isolated `
  --runtime-version 8.0 `
  --functions-version 4 `
  --name $functionApp `
  --storage-account $storageAccount
```

#### 3. Deploy the Function

```bash
func azure functionapp publish connectWithGPT --build remote
```

Alternatively, use dotnet publish:

```bash
dotnet publish -c Release -o ./publish
func azure functionapp publish connectWithGPT --from-package publish
```

#### 4. Verify Deployment

```bash
# Get the function URL
az functionapp function show `
  --resource-group "pay as you go" `
  --name connectWithGPT `
  --function-name HttpTriggerGPT `
  --query "invokeUrlTemplate"

# Test the function
$url = "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT"
Invoke-WebRequest -Uri $url -Method Get
```

### Enable Application Insights

Set the Application Insights connection string:

```bash
az functionapp config appsettings set `
  --name connectWithGPT `
  --resource-group "pay as you go" `
  --settings APPLICATIONINSIGHTS_CONNECTION_STRING="<your-connection-string>"
```

## 📡 API Endpoints

### HttpTriggerGPT

**Endpoint**: `/api/HttpTriggerGPT`

**Methods**: GET, POST

**Authorization**: Function-level (requires function key)

**Response**:
```json
"Welcome to Azure Functions!"
```

**Example**:
```bash
curl https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?code=<function-key>
```

## ⚙️ Configuration

### Local Development (local.settings.json)

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "APPLICATIONINSIGHTS_CONNECTION_STRING": ""
  }
}
```

### Azure Configuration

**Environment Variables** (via `az functionapp config appsettings`):

- `APPLICATIONINSIGHTS_CONNECTION_STRING` - Application Insights connection string
- `AzureWebJobsStorage` - Storage account connection string (auto-configured)

### Application Insights

Application Insights is configured in `Program.cs` and automatically enabled when the `APPLICATIONINSIGHTS_CONNECTION_STRING` environment variable is set.

**Telemetry Features**:
- Request/response tracking
- Exception telemetry
- Performance metrics
- Custom events and logs

## 🔍 Monitoring and Logging

### Local Logging

Logs are sent to console output:

```bash
func start
```

### Azure Application Insights

1. Go to Azure Portal
2. Navigate to your Function App
3. Select "Application Insights" 
4. View:
   - Live metrics
   - Request traces
   - Failed requests
   - Performance counters
   - Custom events

### View Logs via Azure CLI

```bash
az functionapp log tail --name connectWithGPT --resource-group "pay as you go"
```

## 🐛 Troubleshooting

### Issue: Function not running locally

**Solution**: Ensure Azure Functions Core Tools v4 is installed:
```bash
func --version
npm install -g azure-functions-core-tools@4
```

### Issue: "local.settings.json not found"

**Solution**: The file is auto-created, but if missing, the Azure Functions runtime requires it.

### Issue: Storage connection string error

**Solution**: For local development, use:
```json
"AzureWebJobsStorage": "UseDevelopmentStorage=true"
```

### Issue: Application Insights not showing data

**Solution**: 
1. Verify `APPLICATIONINSIGHTS_CONNECTION_STRING` is set in Azure
2. Restart the function app: `az functionapp restart --name connectWithGPT --resource-group "pay as you go"`
3. Make a request to trigger telemetry

### Issue: "Unauthorized" when calling function

**Solution**: Include the function key in the query string:
```bash
curl "https://connectWithGPT.azurewebsites.net/api/HttpTriggerGPT?code=<your-function-key>"
```

Get your function key:
```bash
az functionapp keys list --name connectWithGPT --resource-group "pay as you go"
```

## 📚 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.Azure.Functions.Worker | 2.52.0 | Core Azure Functions runtime |
| Microsoft.Azure.Functions.Worker.Sdk | 2.0.7 | Functions SDK |
| Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore | 2.1.1 | HTTP and ASP.NET Core support |
| Microsoft.Azure.Functions.Worker.OpenTelemetry | 1.2.0 | OpenTelemetry integration |
| OpenTelemetry.Extensions.Hosting | 1.15.3 | Hosting extensions |
| Azure.Monitor.OpenTelemetry.Exporter | 1.7.0 | Application Insights exporter |

## 🔐 Security Notes

- Function triggers use `AuthorizationLevel.Function` - requires a function key
- Local settings with storage connection strings should NOT be committed to Git
- `local.settings.json` is in `.gitignore`
- For production, use managed identities instead of connection strings when possible

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Contributing

1. Create a feature branch (`git checkout -b feature/AmazingFeature`)
2. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
3. Push to the branch (`git push origin feature/AmazingFeature`)
4. Open a Pull Request

## 📞 Support

For issues and questions:
1. Check the [Troubleshooting](#troubleshooting) section
2. Review [Azure Functions documentation](https://learn.microsoft.com/en-us/azure/azure-functions/)
3. Check Azure Functions GitHub issues
4. Contact your team or support

---

**Last Updated**: 2026-09-01  
**Framework**: .NET 8.0  
**Azure Functions Version**: v4
