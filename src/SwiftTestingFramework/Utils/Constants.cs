using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftTestingFramework.Utils
{
    public class Constants
    {
#if southcentralus
        public const string WindowsAppUrl = "https://stf-southcentralus-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-southcentralus-linuxapp.azurewebsites.net";
#elif westcentralus
        public const string WindowsAppUrl = "https://stf-westcentralus-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-westcentralus-linuxapp.azurewebsites.net";
#elif westus2
        public const string WindowsAppUrl = "https://stf-westus2-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-westus2-linuxapp.azurewebsites.net";
#elif northcentralus
        public const string WindowsAppUrl = "https://stf-northcentralus-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-northcentralus-linuxapp.azurewebsites.net";
#elif westeurope
        public const string WindowsAppUrl = "https://stf-westeurope-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-westeurope-linuxapp.azurewebsites.net";
#elif eastus2euap
        public const string WindowsAppUrl = "https://stf-eastus2euap-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-eastus2euap-linuxapp.azurewebsites.net";
#elif centraluseuap
        public const string WindowsAppUrl = "https://stf-centraluseuap-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-centraluseuap-linuxapp.azurewebsites.net";
#endif

        public const int MaxRetries = 3;
    }
}
