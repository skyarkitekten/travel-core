# Getting Setup

This document will guide you through setting up the projects on your local machine.

## Prerequisites

1. .NET 8 SDK
1. Visual Studio Code
1. Azure OpenAI API Key

## Configuration

1. Clone the repository
1. Open the solution in VS Code
1. Add User Secrets to the project

   - You must add configuration settings to the `appsettings.json` file in the root directory of the project or use .NET's secret manager. To use the secret manager, run the following commands:

   ```bash
   dotnet user-secrets init
   dotnet user-secrets set AzureOpenAI:Model <your-model> # default is "gpt-3.5-turbo"
   dotnet user-secrets set AzureOpenAI:ApiKey <your-api-url> # required, no default
   dotnet user-secrets set AzureOpenAI:ApiEndpoint <your-api-endpoint> # required, no default
   ```
