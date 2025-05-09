param prefix string = 'load'
param location string = resourceGroup().location
param username string
param sshkey string
param allowedIP string
param defaultVmSize string = 'Standard_D4ds_v6'
param addressSpace string = '10.0.0.0/24'
param vms array = [
  {
    name: 'loadgen'
    public: true
  }
  {
    name: 'bff'
    public: false
  }
  {
    name: 'backend'
    public: false
  }
]

var uniqueNameFormat = '${prefix}-{0}-${uniqueString(resourceGroup().id, prefix)}'
var subnetName = 'vms'

resource vnet 'Microsoft.Network/virtualNetworks@2024-05-01' = {
  name: format(uniqueNameFormat, 'vnet')
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressSpace
      ]
    }
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: addressSpace
        }
      }
    ]
  }
  resource subnet 'subnets@2024-05-01' existing = {
    name: subnetName
  }
}

resource nsg 'Microsoft.Network/networkSecurityGroups@2024-05-01' = {
  name: format(uniqueNameFormat, 'nsg')
  location: location
  properties: {
    securityRules: empty(trim(allowedIP)) ? [] : [
      {
        name: 'allow-ssh'
        properties: {
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '22'
          sourceAddressPrefix: allowedIP
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 1000
          direction: 'Inbound'
        }
      }
    ]
  }
}

module vmsDeploys 'modules/vm.bicep' = [
  for vm in vms: {
    name: format(uniqueNameFormat, 'vm-${vm.name}')
    params: {
      name: format(uniqueNameFormat, 'vm-${vm.name}')
      hostname: vm.name
      location: location
      username: username
      sshPublicKey: sshkey
      public: vm.public
      subnetId: vnet::subnet.id
      nsgId: nsg.id
      vmSize: defaultVmSize
    }
  }
]
