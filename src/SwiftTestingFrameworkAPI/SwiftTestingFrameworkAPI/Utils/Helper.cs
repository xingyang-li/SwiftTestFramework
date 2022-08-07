using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace SwiftTestingFrameworkAPI.Utils
{
    public class Helper
    {
        public class ProcessOutput 
        {
            public int ExitCode { get; set; }
            public string ErrorMessage { get; set; }

            public ProcessOutput(int exitCode, string errorMessage)
            {
                ExitCode = exitCode;
                ErrorMessage = errorMessage;
            }
        }

        public static ProcessOutput StartProcess(string exe, string arguments)
        {
            Process p = new Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.Start();
            string err = p.StandardError.ReadToEnd();
            p.WaitForExit();
            return new ProcessOutput(p.ExitCode, err);
        }

        public static PageBlobClient GetPageBlobClient(string blobName)
        {
            string connectionString = Environment.GetEnvironmentVariable(Constants.StorageConnectionStringName) ?? string.Empty;

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Constants.StorageContainerName);
            containerClient.CreateIfNotExists();

            PageBlobClient pageBlobClient = containerClient.GetPageBlobClient(blobName);
            return pageBlobClient;
        }

        public static string ExecuteSqlQuery()
        {
            string connectionString = Environment.GetEnvironmentVariable(Constants.SqlDatabaseConnectionStringName) ?? string.Empty;
            string queryResult = string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Connect to the database
                conn.Open();

                // Read rows
                SqlCommand selectCommand = new SqlCommand(@"IF NOT EXISTS( SELECT * FROM sys.tables WHERE name = 'Test')
                                                                BEGIN
                                                                CREATE Table Test(ID Datetime PRIMARY KEY);
                                                                END
                                                                GO
                                                                Declare @date Datetime;
                                                                SET @date = GetDate();
                                                                INSERT INTO dbo.Test Values(@date);
                                                                SELECT* FROM Test WHERE ID = @date; ", conn
                                                         );
                SqlDataReader results = selectCommand.ExecuteReader();

                // Enumerate over the rows
                while(results.Read())
                {
                    queryResult += results[0];
                }
            }

            return queryResult;
        }
    }
}
