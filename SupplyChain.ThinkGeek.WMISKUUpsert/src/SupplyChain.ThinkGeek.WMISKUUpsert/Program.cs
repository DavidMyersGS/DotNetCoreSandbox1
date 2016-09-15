using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using Amazon.SQS;
using Amazon.SQS.Model;
using GameStop.SupplyChain.AWSHelper;
using GameStop.SupplyChain.DataContracts.ThinkGeek.SKUUpsert;


namespace SupplyChain.ThinkGeek.WMISKUUpsert
{
    public class Program
    {
        static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //var appSettings = ConfigurationManager.AppSettings;
            
            string sqsQueueName = Configuration["AppSettings:AWSQueue" + args[0].ToString().ToUpper().Trim()].ToString();
            string sqsDLQueueName = Configuration["AppSettings:AWSDeadLetterQueue" + args[0].ToString().ToUpper().Trim()].ToString();
            int maxSQSRetries = Convert.ToInt32(Configuration["AppSettings:MaxSQSRetries"].ToString());
            string urlWMIService = Configuration["AppSettings:WMIServiceURL" + args[0].ToString().ToUpper().Trim()].ToString();
            int batchSize = Convert.ToInt32(Configuration["AppSettings:BatchSize"].ToString());

            IAmazonSQS sqsClient = new AmazonSQSClient();
            IAWSSQSWrapper awsSQSWrapper = new AWSSQSWrapper(sqsClient);

            //Display our current list of Amazon SQS queues
            Console.WriteLine("Checking to see if AWS environment is available...\n");
            ListQueuesResponse lqr = awsSQSWrapper.LisQueues();

            bool queueExists = false;
            bool dlqueueExists = false;

            if (lqr.QueueUrls != null)
            {
                foreach (String queueUrl in lqr.QueueUrls)
                {
                    if (queueUrl.ToUpper().Trim().EndsWith(sqsQueueName.ToUpper().Trim()))
                    {
                        queueExists = true;
                        Console.WriteLine("Queue Exists: {0}", queueUrl);

                        GetQueueAttributesResponse gQAR = awsSQSWrapper.GetQueueAttributes(queueUrl);
                        if (gQAR.Attributes != null)
                        {
                            foreach (KeyValuePair<string, string> tmp in gQAR.Attributes)
                            {
                                Console.WriteLine("     Attribute: {0}  Value:{1}", tmp.Key.ToString(), tmp.Value.ToString());
                            }
                        }
                    }

                    if (queueUrl.ToUpper().Trim().EndsWith(sqsDLQueueName.ToUpper().Trim()))
                    {
                        dlqueueExists = true;
                        Console.WriteLine("Queue Exists: {0}", queueUrl);

                        GetQueueAttributesResponse gQAR = awsSQSWrapper.GetQueueAttributes(queueUrl);
                        if (gQAR.Attributes != null)
                        {
                            foreach (KeyValuePair<string, string> tmp in gQAR.Attributes)
                            {
                                Console.WriteLine("     Attribute: {0}  Value:{1}", tmp.Key.ToString(), tmp.Value.ToString());
                            }
                        }
                    }
                }
            }

            if (!dlqueueExists)
            {
                Console.WriteLine("The dead letter queue was missing, let's build it...\n");
                awsSQSWrapper.CreateQueue(sqsDLQueueName, false);
                Console.WriteLine("Dead Letter Queue built successfuly.\n");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }

            if (!queueExists)
            {
                Console.WriteLine("Message queue was missing, let's build it...\n");
                awsSQSWrapper.CreateQueue(sqsQueueName, false);
                awsSQSWrapper.AttachDeadLetterQueue(sqsQueueName, sqsDLQueueName, maxSQSRetries);
                Console.WriteLine("Message queue built successfuly.\n");
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }

            Console.WriteLine("AWS environment ready.\n");
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            GetQueueUrlResponse awsGQUR = sqsClient.GetQueueUrl(sqsQueueName);

            Console.WriteLine("Creating test messages...");
            CreateMessages(awsSQSWrapper, awsGQUR.QueueUrl.ToString(), 10);
            Console.WriteLine("Creating test messages complete.");
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            string response = GetREST();

            Console.WriteLine("GET Received: {0}", response);
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            //string awsMessage = GetMessage(awsSQSWrapper, awsGQUR.QueueUrl);

            bool continueReceiving = true;
            while (continueReceiving)
            {
                ReceiveMessageResponse awsRMR = awsSQSWrapper.ReceiveMessage(awsGQUR.QueueUrl);

                if (awsRMR.Messages.Count > 0)
                {
                    response = PostREST(awsRMR.Messages[0].Body.ToString());
                    Console.WriteLine("POST Returned: {0}", response);
                    DeleteMessageResponse awsDMR = awsSQSWrapper.DeleteMessage(awsGQUR.QueueUrl, awsRMR.Messages[0].ReceiptHandle.ToString());
                    Console.WriteLine("Message Deleted: {0}", awsRMR.Messages[0].ReceiptHandle.ToString());
                }
                else
                {
                    continueReceiving = false;
                }
            }

            Console.WriteLine("\n\nAll messages sent.");
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static string GetREST()
        {
            var httpClient = new HttpClient();

            var response = httpClient.GetAsync("http://localhost:52476/api/sku").Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        static string PostREST(string json)
        {
            var client = new WebClient();

            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string result = client.UploadString("http://localhost:52476/api/sku", "POST", json);

            return result;
        }

        static void CreateMessages(IAWSSQSWrapper awsSQSWrapper, string queueURL, int messageCount)
        {
            //GetQueueUrlResponse gQUR = sqsClient.GetQueueUrl("MySQSQueue");

            SKUUpsertMessageContract skuMessage = new SKUUpsertMessageContract();

            skuMessage.GUID = Guid.NewGuid().ToString();
            skuMessage.MessageMetadata = new MessageMetadata();
            skuMessage.MessageMetadata.Company = "TGK";
            skuMessage.MessageMetadata.Warehouse = "SHP";
            skuMessage.MessageMetadata.Brand = "TGK";
            skuMessage.MessageType = "sku_upsert";
            skuMessage.SKU = "0EDD1H";
            skuMessage.ProductID = "edd1";
            skuMessage.ProductName = "Harry Potter House Flag";
            skuMessage.ProductCategory = "Accessories";
            skuMessage.ProductName = "Hufflepuff";
            skuMessage.dimensions = new Dimensions();
            skuMessage.dimensions.weight = ".45";
            skuMessage.dimensions.weight_override = "0";
            skuMessage.dimensions.length = "17";
            skuMessage.dimensions.height = ".5";
            skuMessage.dimensions.depth = "10";
            skuMessage.dimensions.volume = "85";
            skuMessage.dimensions.case_weight = "4.5";
            skuMessage.dimensions.case_length = "170";
            skuMessage.dimensions.case_height = "5";
            skuMessage.dimensions.case_depth = "100";
            skuMessage.dimensions.case_volume = "8500";
            skuMessage.identifiers = new Identifiers();
            skuMessage.identifiers.upc_1 = "618480236309";
            skuMessage.identifiers.upc_2 = "";
            skuMessage.identifiers.ean = "";
            skuMessage.identifiers.jan = "";
            skuMessage.identifiers.isbn = "";
            skuMessage.identifiers.warehoue_barcode = "0EDD1H";

            int i = 0;
            for (i = 1; i <= messageCount; i++)
            {
                skuMessage.GUID = Guid.NewGuid().ToString();
                string message = awsSQSWrapper.ObjectToJSON(skuMessage);

                Console.WriteLine("Creating Message {0}", i);
                SendMessageResponse awsSMR = awsSQSWrapper.SendMessage(queueURL, message);
                Console.WriteLine("Successfully created Message {0}", awsSMR.MessageId.ToString());
            }
        }

        static void ReadMessages(IAWSSQSWrapper awsSQSWrapper, string queueURL)
        {
            ReceiveMessageResponse awsRMR = awsSQSWrapper.ReceiveMessage(queueURL);

            SKUUpsertMessageContract obj = new SKUUpsertMessageContract();

            SKUUpsertMessageContract skuMC = (SKUUpsertMessageContract)awsSQSWrapper.JSONToObject(obj, awsRMR.Messages[0].Body.ToString());

            Console.WriteLine("Received AWS Message: {0}", awsRMR.Messages[0].MessageId.ToString());
            Console.WriteLine("  AWS Message Handle: {0}", awsRMR.Messages[0].ReceiptHandle.ToString());
            Console.WriteLine("Received TGK Message: {0}", skuMC.GUID);

            foreach (Message message in awsRMR.Messages)
            {
                Console.WriteLine("  Message");
                Console.WriteLine("    MessageId: {0}", message.MessageId);
                Console.WriteLine("    ReceiptHandle: {0}", message.ReceiptHandle);
                Console.WriteLine("    MD5OfBody: {0}", message.MD5OfBody);
                Console.WriteLine("    Body: {0}", message.Body);

                foreach (KeyValuePair<string, string> entry in message.Attributes)
                {
                    Console.WriteLine("  Attribute");
                    Console.WriteLine("    Name: {0}", entry.Key);
                    Console.WriteLine("    Value: {0}", entry.Value);
                }
            }
        }

        static string GetMessage(IAWSSQSWrapper awsSQSWrapper, string queueURL)
        {
            ReceiveMessageResponse awsRMR = awsSQSWrapper.ReceiveMessage(queueURL);

            return awsRMR.Messages[0].Body.ToString();

        }
    }
}
