using Azure.Messaging.ServiceBus;
using QuickFix.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;
using MessageBus.Interface;

namespace QuickFix.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumerEmail : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string registerUserQueue;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ServiceBusProcessor _registerUserProcessor;

        public AzureServiceBusConsumerEmail(IConfiguration configuration, IEmailService emailService)
        {
            _emailService=emailService;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _registerUserProcessor = client.CreateProcessor(registerUserQueue);
        }

        public async Task Start()
        {
            _registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();
        }

       

        public async Task Stop()
        {
            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //TODO - try to log email
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch 
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

       
    }
}
