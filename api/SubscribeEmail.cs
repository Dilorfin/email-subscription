using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Company.Function
{
    record EmailModel (string email);

    public static class SubscribeEmail
    {
        [FunctionName("SubscribeEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "email/subscribe")] HttpRequest req,
            ILogger log)
        {

            /*CosmosClient client = new CosmosClient(connectionString, clientOptions);
            Database database = await client.CreateDatabaseIfNotExistsAsync(databaseId);*/
            var emailValidator = new EmailAddressAttribute();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EmailModel data = JsonConvert.DeserializeObject<EmailModel>(requestBody);

            log.LogInformation($"input {data.email}");
            if (!emailValidator.IsValid(data.email))
            {
                return new BadRequestObjectResult($"invalid email {data.email}");
            }

            return new OkObjectResult($"email subscribed {data.email}");
        }
    }
}
