param appServicePlanName string
param appName string
param location string = resourceGroup().location
param keyVaultName string
param appSettings array = []

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'B1'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      appSettings: concat(
        [
          {
            name: 'keyVaultName' 
            value: keyVaultName
          }
        ],
        appSettings
      )
    }
  }
}

resource webAppConfig 'Microsoft.Web/sites/config@2023-12-01' = {
  parent: webApp
  name: 'web'
  properties: {
    scmType: 'Github'
  }
}

output appServiceId string = webApp.id
output principalId string = webApp.identity.principalId
