param location string = resourceGroup().location
@secure()
param pgSqlPassword string

var uniqueId = uniqueString(resourceGroup().id)

module keyVault 'modules/secrets/key-vault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    vaultName: 'kv-${uniqueId}'
    location: location
  }
}

module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'api-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    location: location
    keyVaultName: keyVault.outputs.name
    appSettings: [
      {
        name: 'DatabaseName'
        value: 'urls'
      }
      {
        name: 'ContainerName'
        value: 'items'
      }
      {
        name: 'TokenRangeService__Endpoint'
        value: tokenRangeService.outputs.url
      }
    ]
  }
}

module tokenRangeService 'modules/compute/appservice.bicep' = {
  name: 'tokenRangeServiceDeployment'
  params: {
      appName: 'token-range-service-${uniqueId}'
      appServicePlanName: 'plan-token-range-${uniqueId}'
      location: location
      keyVaultName: keyVault.outputs.name
  }
}

module postgres 'modules/storage/postgresql.bicep' = {
  name: 'postgresDeployment'
  params: {
    name: 'postgresql-${uniqueString(resourceGroup().id)}'
    location: location
    administratorLogin: 'adminuser'
    administratorLoginPassword: pgSqlPassword
    keyVaultName: keyVault.outputs.name
  }
}

module cosmosDb 'modules/storage/cosmos-db.bicep' = {
  name: 'cosmosDbDeployment'
  params: {
    name: 'cosmos-db-${uniqueId}'
    location: location
    kind: 'GlobalDocumentDB'
    databaseName: 'urls'
    locationName: 'eastus2'
    keyVaultName: keyVault.outputs.name
  }
}

module keyVaultRoleAssignment 'modules/secrets/key-vault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssignmentDeployment'
  params: {
    keyVaultName: keyVault.outputs.name
    principalIds: [
      apiService.outputs.principalId
      tokenRangeService.outputs.principalId
      // Add more principal IDs as needed
    ]
  }
}

module entraApp 'modules/identity/entra-app.bicep' = {
  name: 'entraAppWebDeployment'
  params: {
    applicationName: 'web-${uniqueId}'
  }
}
