using Amazon.SQS;
using Amazon.SQS.Model;
using System.Runtime.Serialization.Json;

namespace GameStop.SupplyChain.AWSHelper
{
    public interface IAWSSQSWrapper
    {
        IAmazonSQS SQSClient { get; set; }

        ListQueuesResponse LisQueues();

        SetQueueAttributesResponse AttachDeadLetterQueue(string queueName, string queueNameDeadLetter, int maxRetries);

        string CreateQueue(string queueName, bool createDLQ = false);

        string DeleteQueue(string queueUrl);

        SendMessageResponse SendMessage(string queueUrl, string message);

        ReceiveMessageResponse ReceiveMessage(string queueUrl);

        DeleteMessageResponse DeleteMessage(string queueUrl, string messageHandle);

        GetQueueAttributesResponse GetQueueAttributes(string queueUrl);

        string ObjectToJSON(object obj);

        object JSONToObject(object obj, string json);
    }
}
