az group delete --resource-group $1 --yes 
az group create --resource-group $1 --location "$2"
az deployment group create --resource-group $1 --template-file $3 --parameters projectName=$1