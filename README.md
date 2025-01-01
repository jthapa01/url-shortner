# url-shortner
Url shortner service

## Infrastructure as a Code

### Download Azure CLI
https://learn.microsoft.com/en-us/cli/azure/

### Dotnet User Secrets
#### Cd to Root of the project

```bash
dotnet user-secrets init
dotnet user-secrets set "AzureAdB2C:Instance" "https://login.microsoftonline.com/tfp/{0}/{1}/v2.0"
```

### Login to Azure
```bash
az login
```

### Create Resource Group

```bash
az group create --name urlshortener --location eastus2
```

### Create Service Principal
```bash
az ad sp create-for-rbac --name "Github-Actions-SP" `
                         --role contributor `
                         --scopes /subscriptions/21bbbb3d-189b-48e1-b499-6c74b9f9a598 `
                         --sdk-auth
```
### Apply Custom Contributor Role

```bash
az ad sp create-for-rbac --name "Github-Actions-SP" `
                         --role infra_deploy `
                         --scopes /subscriptions/21bbbb3d-189b-48e1-b499-6c74b9f9a598 `
                         --sdk-auth
```

### Run what-if
```bash
$rgName="[rg Name]"
az deployment group what-if --resource-group $rgName --template-file infrastructure/main.bicep
``` 

#### Configure a federated identity credential on an app

https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-create-trust?pivots=identity-wif-apps-methods-azp

### Get Publishing Profile
```bash
$webappName="[Web App here]"
$rgName="[rg Name]"
az webapp deployment list-publishing-profiles --name $webappName `
                    --resource-group $rgName --xml
```

### Get Static Web Apps Deployment Token
```bash
$webappName="[Web App here]"
$rgName="[rg Name]"
az staticwebapp secrets list --name $webappName --query "properties.apiKey"
                    --resource-group $rgName --xml
```

### Run Deployment 
```bash
$rgName="[rg Name]"
az deployment group create --resource-group $rgName --template-file ./Infrastructure/main.bicep
``` 

### Utilities
- Base62 converter url: https://math.tools/calculator/base/10-62