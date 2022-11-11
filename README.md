# SwiftTestingFramework

[![Azure CI](https://github.com/xingyang-li/SwiftTestFramework/actions/workflows/deploy_resources.yml/badge.svg)](https://github.com/xingyang-li/SwiftTestFramework/actions/workflows/deploy_resources.yml)
[![Testing](https://github.com/xingyang-li/SwiftTestingFramework/actions/workflows/run_tests.yml/badge.svg)](https://github.com/xingyang-li/SwiftTestingFramework/actions/workflows/run_tests.yml)

<!-- GETTING STARTED -->
## Getting Started

This project is intended to provide testing coverage for Swift/Vnet features through the use of canary web apps across multiple regions.
To introduce changes to the framework and set up local testing, please follow the below requirements.

### Prerequisites

1. Clone the repo
   ```sh
   git clone https://github.com/xingyang-li/SwiftTestFramework.git
   ```
2. Make sure you have Azure SDKs and Tools downloaded for Visual Studio [https://azure.microsoft.com/en-us/downloads/](https://azure.microsoft.com/en-us/downloads/)
3. Install Azure Powershell in order to deploy resources to Azure locally [https://learn.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-9.1.0](https://learn.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-9.1.0)
4. Get access to the "WAWS Swift Test" Azure subscription. (Needed to test ARM template changes)


### Manipulating Azure Resources

There is a resource group deployed to every region defined in `location_matrix.json`. To extend this framework to another region, add the desired region name to this file. 

The ARM templates that define the resources are located in the `templates` folder. Each template file defines the resources in the region in which the file is named after. The templates are very similar to each other and differences are attributed to unique resource constraints found in each region. A new template file is required if another region is added to the framework.

To locally test ARM template changes, you can deploy resources to Azure using Azure Powershell.

1. Open Powershell and login to Azure with: `az login`
2. Set your subscription to the 'WAWS Swift Test': `Az-SetContext {WAWS Swift Test Subscription ID}`
3. Copy over ARM template JSON to `templates\stf-test.json` file.
4. Run: `.\deploy.ps1 -projectName stf-test -location {desired location}`
5. Navigate to the resource group "stf-test" in the desired location in the Azure portal to verify that the deployment succeeded.


### Adding API Changes

Each resource group contains canary web apps that will host an API app to connect/interact with other resources in the group. The code for these apps is located under `src\SwiftTestingFrameworkAPI`.

The app is an ASP.NET Core web API project, and you will need to make a new API endpoint for each new feature/test you would like to add to the framework. 

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

Under `.github\workflows`, there are yaml files that define the instructions for some GitHub Actions workflows set up for this repo that build app code and deploy resource to Azure. Workflows are automations that allow for hands-off deployment of this framework.




