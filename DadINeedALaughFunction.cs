using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace DadINeedALaughBot
{
    public static class DadINeedALaughFunction
    {
        [FunctionName("DadINeedALaughFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Welcome to DadINeedALaughBot!");

            var accountSID = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            var personalNumber = Environment.GetEnvironmentVariable("LC_NUMBER_WHATSAPP");

            TwilioClient.Init(accountSID, authToken);

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var message = MessageResource.Create(
            body: "This is a message that I want to send over WhatsApp with Twilio!",
            from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
            to: new Twilio.Types.PhoneNumber(personalNumber)
            );     

            return new OkObjectResult("Message SID: " + message.Sid + " , Status Code: " + message.Status);
        }
    }
}
