using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace MQTest
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://maximiz.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8YEJQvOQx460YlgikPYnb+zNT+lQKqjmS/kc9vdWMek=";
        const string QueueName = "webevent";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send Messages
            await SendMessagesAsync(numberOfMessages);

            //Console.ReadKey();

            await queueClient.CloseAsync();
        }

        static async Task Test()
        {
            var entity = new Maximiz.Model.Entity.Account
            {
                Id = Guid.NewGuid(),
                Currency = "USD",
                Name = "henk",
                Publisher = "taboola",
                SecondaryId = "817327",
            };

            var message = new Maximiz.Model.Protocol.CreateOrUpdateObjectsMessage(entity, Maximiz.Model.CrudAction.Syncback);

            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new System.IO.MemoryStream())
            //using (var stream = System.IO.File.OpenWrite("mydump.bin"))
            {
                bf.Serialize(stream, message);
                var rr = new Message(stream.ToArray());
                await queueClient.SendAsync(rr);
                //await queueClient.SendAsync(new Message(System.Text.Encoding.UTF8.GetBytes("kaas is lekker")));
            }

            //await Task.CompletedTask;
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    await Test();

                    // Create a new message to send to the queue
                    //string messageBody = $"Message {i}";
                    //var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    //using (var stream = File.Open(@"D:\Projects\MaxiMiz\Poller\Poller.Host\bin\Debug\netcoreapp2.2\mydump.bin", FileMode.Open))
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    stream.CopyTo(ms);
                    //    var message = new Message(ms.ToArray());
                    //    //var message = new Message(Encoding.UTF8.GetBytes("hi there!"));
                    //    // Write the body of the message to the console
                    //    //Console.WriteLine($"Sending message: {messageBody}");


                    //    // Send the message to the queue
                    //    await queueClient.SendAsync(message);

                    //    stream.Close();
                    //}
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
