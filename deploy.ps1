param([string]$projectName, [string]$location, [string]$templateFile)

az group delete --resource-group $projectName --yes 
az group create --resource-group $projectName --location $location
az deployment group create --resource-group $projectName --template-file $templateFile --parameters projectName=$projectName