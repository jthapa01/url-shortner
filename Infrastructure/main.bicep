param location string = resourceGroup().location
var uniqueId = uniqueString(resourceGroup().id)

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'KeyVaultDeployment'
  params: {
    vaultName: 'kv-${uniqueId}'
    location: location
  }
}

module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'apiApp-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    location: location
  }
}
