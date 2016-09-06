using System;
using System.Collections.Generic;
using System.Configuration;
using Amazon.SQS;
using Amazon.SQS.Model;
using GameStop.SupplyChain.AWSHelper;
using GameStop.SupplyChain.ThinkGeekDataContracts;


namespace GameStop.SupplyChain.AWSSQSBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string sqsQueueName = appSettings["AWSQueue" + args[0].ToString().ToUpper().Trim()].ToString();
            string sqsDLQueueName = appSettings["AWSDeadLetterQueue" + args[0].ToString().ToUpper().Trim()].ToString();
            int maxSQSRetries = Convert.ToInt32(appSettings["MaxSQSRetries"].ToString());

            IAmazonSQS sqsClient = new AmazonSQSClient();
            IAWSSQSWrapper awsSQSWrapper = new AWSSQSWrapper(sqsClient);

            //Display our current list of Amazon SQS queues
            Console.WriteLine("Printing list of Amazon SQS queues.\n");
            ListQueuesResponse lqr = awsSQSWrapper.LisQueues();

            bool queueExists = false;
            bool dlqueueExists = false;

            if (lqr.QueueUrls != null)
            {
                foreach (String queueUrl in lqr.QueueUrls)
                {
                    if(queueUrl.ToUpper().Trim().EndsWith(sqsQueueName.ToUpper().Trim()))
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
                awsSQSWrapper.CreateQueue(sqsDLQueueName, false);
            }
            
            if (!queueExists)
            {
                awsSQSWrapper.CreateQueue(sqsQueueName, false);
                awsSQSWrapper.AttachDeadLetterQueue(sqsQueueName, sqsDLQueueName, maxSQSRetries);
            }



            GetQueueUrlResponse awsGQUR = sqsClient.GetQueueUrl(sqsQueueName);

            LoadMessages(awsSQSWrapper, awsGQUR.QueueUrl.ToString(), 100);

            ReceiveMessageResponse awsRMR = awsSQSWrapper.ReceiveMessage(awsGQUR.QueueUrl.ToString());

            SKUUpsertMessageContract obj = new SKUUpsertMessageContract();

            ISKUUpsertMessageContract skuMC = (SKUUpsertMessageContract)awsSQSWrapper.JSONToObject(obj, awsRMR.Messages[0].Body.ToString());

            Console.WriteLine("Received AWS Message: {0}", awsRMR.Messages[0].MessageId.ToString());
            Console.WriteLine("  AWS Message Handle: {0}", awsRMR.Messages[0].ReceiptHandle.ToString());
            Console.WriteLine("Received TGK Message: {0}", skuMC.ID);

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

            
            DeleteMessageResponse awsDMR = awsSQSWrapper.DeleteMessage(awsGQUR.QueueUrl, awsRMR.Messages[0].ReceiptHandle.ToString());

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static void LoadMessages(IAWSSQSWrapper awsSQSWrapper, string queueURL, int messageCount)
        {
            //GetQueueUrlResponse gQUR = sqsClient.GetQueueUrl("MySQSQueue");

            ISKUUpsertMessageContract skuMessage = new SKUUpsertMessageContract();

            skuMessage.ID = Guid.NewGuid().ToString();
            skuMessage.Target = new MessageTarget();
            skuMessage.Target.Company = "TGK";
            skuMessage.Target.Warehouse = "SHP";
            skuMessage.Target.Brand = "TGK";
            skuMessage.MessageType = "sku_upsert";
            skuMessage.sku = "0EDD1H";
            skuMessage.product_id = "edd1";
            skuMessage.product_name = "Harry Potter House Flag";
            skuMessage.product_category = "Accessories";
            skuMessage.product_name = "Hufflepuff";
            skuMessage.Dimensions = new Dimensions(true);
            skuMessage.Identifiers = new Identifiers(true);

            int i = 0;
            string message = awsSQSWrapper.ObjectToJSON(skuMessage);

            for (i = 1; i <= messageCount; i++)
            {
                Console.WriteLine("Creating Message {0}", i);
                SendMessageResponse awsSMR = awsSQSWrapper.SendMessage(queueURL,message);
                Console.WriteLine("Successfully created Message {0}", awsSMR.MessageId.ToString());
            }
        }
    }
}
