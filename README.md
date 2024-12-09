# url-shortner
Url shortner service

## Infrastructure as a Code

### Download Azure CLI
https://learn.microsoft.com/en-us/cli/azure/

### Login to Azure
```bash
az login
```

### Create Resource Group

```bash
loca
az group create --name urlshortener --location eastus2
```

```bash
az ad sp create-for-rbac --name "Github-Actions-SP" `
                         --role contributor `
                         --scopes /subscriptions/21bbbb3d-189b-48e1-b499-6c74b9f9a598 `
                         --sdk-auth
```

#### Configure a federated identity credential on an app

https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-create-trust?pivots=identity-wif-apps-methods-azp
