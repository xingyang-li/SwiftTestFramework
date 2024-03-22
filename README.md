# SwiftTestingFramework

[![Azure CI](https://github.com/xingyang-li/SwiftTestFramework/actions/workflows/deploy_resources.yml/badge.svg)](https://github.com/xingyang-li/SwiftTestFramework/actions/workflows/deploy_resources.yml)

<!-- GETTING STARTED -->
## Getting Started

This project is intended to provide testing coverage for Swift/Vnet features through the use of canary web apps hosted on Azure across multiple regions.
To introduce changes to the framework and set up local testing, please follow the below instructions.

### Prerequisites

1. Clone the repo
   ```sh
   git clone https://github.com/xingyang-li/SwiftTestFramework.git
   ```
2. Make sure you have Azure SDKs and Tools downloaded for Visual Studio [https://azure.microsoft.com/en-us/downloads/](https://azure.microsoft.com/en-us/downloads/)
3. Install Azure Powershell in order to deploy resources to Azure locally [https://learn.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-9.1.0](https://learn.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-9.1.0)
4. Get access to the "WAWS Swift Test" Azure subscription. (Needed to test ARM template changes)


### Creating Azure Resources

There is a resource group deployed to every region defined in `location_matrix.json`. To extend this framework to another region, add the desired region name to this file. 

The ARM templates that define the resources are located in the `templates` folder. Each template file defines the resources in the region in which the file is named after. The templates are very similar to each other and differences are attributed to unique resource constraints found in each region. A new template file is required if another region is added to the framework. To add resources to a region, add the corresponding ARM template resource to the json file for that region.

To locally test ARM template changes, you can deploy resources to Azure using Azure Powershell.

1. Open Powershell and login to Azure with: `az login`
2. Set your subscription to the 'WAWS Swift Test': `az account set --subscription {WAWS Swift Test Subscription ID}` (You may need to run `Az-SetContext {WAWS Swift Test Subscription ID}` instead if Azure Powershell is already set up for your Powershell instead of Azure CLI.)
3. Add ARM template changes to `templates\stf-test.json` file.
4. Run: `.\deploy.ps1 -projectName {resource group name} -location {desired location} -templateFile .\templates\stf-test.json`
5. Navigate to the specified resource group in the Azure portal to verify that the deployment succeeded.
6. Copy over all contents from stf-test.json to stf-prod.json once you have tested the deployment so that the Github Actions workflow will deploy your changes across all regions once the changes have been checked in.

### Adding API Controller

Each resource group contains canary web apps that will host an API app to connect/interact with other resources in the group. The code for these apps is located under `src\SwiftTestingFrameworkAPI`.

The app is an ASP.NET Core web API project, and you will need to make a new API controller for each new feature/test you would like to add to the framework. 

This is a good resource for developing ASP.NET Core API code: [https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0)

Once you are ready to test, open the project solution file with Visual Studio. You should be able to spin up the API site locally by clicking the green play button at center-top that says 'SwiftTestingFrameworkAPI'.

If you have Azure SDKs and tools downloaded for Visual Studio, you should be able to publish your code to a web app that is deployed on Azure: [https://learn.microsoft.com/en-us/visualstudio/deployment/quickstart-deploy-aspnet-web-app?view=vs-2022&tabs=azure](https://learn.microsoft.com/en-us/visualstudio/deployment/quickstart-deploy-aspnet-web-app?view=vs-2022&tabs=azure)

### Function App Changes

Each resource group also contains an Azure Function app that requests the endpoints on the API apps periodically with a timer trigger. The code for this is defined under `src\Function`. If you have added endpoints to the API app, you will need to edit the function app to request those endpoints.

This Function app also has the ability to write ETW events using AntaresEventProvider.

Like the API app, you will be able to run the Function project locally and publish the app to Azure using Visual Studio.

To run the Function app locally: You will need to add a file called `local.settings.json` to the `src\Function\` directory to set up local storage for the function app.

The file contents should contain the below:
```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    }
}
```


### Workflow Changes

Under `.github\workflows`, there are yaml files that define the instructions for GitHub Actions workflows set up for this repo that build app code and deploy resource to Azure. You can view these workflows under the `Actions` tab on GitHub webpage for the repo. Workflows are automations that allow for hands-off deployment of this framework, and editing these files will change how this framework is deployed.


### Final Steps

Once you are confident in your changes, merge the Pull Request with your changes to the main branch. This should automatically trigger the deployment, and you will see your changes in Azure soon. If the GitHub Actions job fails, you can also manually run the job if you navigate under the `Actions` tab on GitHub.




