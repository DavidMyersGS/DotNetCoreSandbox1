using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Runtime.Serialization.Json;

namespace GameStop.SupplyChain.AWSHelper
{
    public class AWSSQSWrapper : IAWSSQSWrapper
    {
        public IAmazonSQS SQSClient { get; set; }


        public AWSSQSWrapper(IAmazonSQS client)
        {
            SQSClient = client;
        }

        public string CreateQueue(string queueName, bool createDLQ = false)
        {
            string queueUrl = SQSClient.CreateQueue(new CreateQueueRequest()
            {
                QueueName = queueName
            }).QueueUrl;

            return queueUrl;
        }

        public SetQueueAttributesResponse AttachDeadLetterQueue(string queueName, string queueNameDeadLetter, int maxRetries)
        {
            GetQueueUrlResponse queueURL = SQSClient.GetQueueUrl(queueName);
            GetQueueUrlResponse queueURLDeadLetter = SQSClient.GetQueueUrl(queueNameDeadLetter);

            // Next, we need to get the the ARN (Amazon Resource Name) of our dead
            // letter queue so we can configure our main queue to deliver messages to it.
            IDictionary attributes = SQSClient.GetQueueAttributes(new GetQueueAttributesRequest()
            {
                QueueUrl = queueURLDeadLetter.QueueUrl,
                AttributeNames = new List<string>() { "QueueArn" }
            }).Attributes;

            string dlqArn = (string)attributes["QueueArn"];

            // The last step is setting a RedrivePolicy on our main queue to configure
            // it to deliver messages to our dead letter queue if they haven't been
            // successfully processed after five attempts.
            string redrivePolicy = string.Format(
                "{{\"maxReceiveCount\":\"{0}\", \"deadLetterTargetArn\":\"{1}\"}}",
                maxRetries, dlqArn);

            SetQueueAttributesResponse awsSQAR = SQSClient.SetQueueAttributes(new SetQueueAttributesRequest()
            {
                QueueUrl = queueURL.QueueUrl,
                Attributes = new Dictionary<string, string>()
                    {
                        {"RedrivePolicy", redrivePolicy}
                    }
            });

            return awsSQAR;
        }

        public DeleteMessageResponse DeleteMessage(string queueUrl, string messageHandle)
        {
            var deleteRequest = new DeleteMessageRequest { QueueUrl=queueUrl, ReceiptHandle=messageHandle};
            DeleteMessageResponse deleteResponse = SQSClient.DeleteMessage(deleteRequest);

            return deleteResponse;
        }

        public string DeleteQueue(string queueUrl)
        {
            throw new NotImplementedException();
        }

        public ListQueuesResponse LisQueues()
        {
            ListQueuesRequest listQueuesRequest = new ListQueuesRequest();
            ListQueuesResponse listQueuesResponse = SQSClient.ListQueues(listQueuesRequest);

            return listQueuesResponse;
        }

        public ReceiveMessageResponse ReceiveMessage(string queueUrl)
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = queueUrl };
            ReceiveMessageResponse receiveMessageResponse = SQSClient.ReceiveMessage(receiveMessageRequest);

            return receiveMessageResponse;
        }

        public SendMessageResponse SendMessage(string queueUrl, string message)
        {
            var sendMessageRequest = new SendMessageRequest{QueueUrl = queueUrl, MessageBody=message};
            SendMessageResponse sendMessageResponse = SQSClient.SendMessage(sendMessageRequest);

            return sendMessageResponse;
        }

        public GetQueueAttributesResponse GetQueueAttributes(string queueUrl)
        {
            List<string> list = new List<string>();
            list.Add("All");

            GetQueueAttributesRequest getQueueAttributesRequest = new GetQueueAttributesRequest { QueueUrl = queueUrl, AttributeNames=list };
            GetQueueAttributesResponse getQueueAttributesResponse = SQSClient.GetQueueAttributes(getQueueAttributesRequest);

            return getQueueAttributesResponse;
        }

        public string ObjectToJSON (object obj)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, obj);
            ms.Position = 0;

            StreamReader sr = new StreamReader(ms);

            return sr.ReadToEnd();
        }

        public object JSONToObject(object obj, string json)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(json));
            ms.Position = 0;

            var p2 = Convert.ChangeType(js.ReadObject(ms), obj.GetType());

            return p2;
        }
    }
}
