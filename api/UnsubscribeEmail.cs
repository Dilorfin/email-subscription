using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Company.Function.Models;

namespace Company.Function
{
    public static class UnsubscribeEmail
    {
        [FunctionName("UnsubscribeEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "email/unsubscribe")] HttpRequest req,
            ILogger log)
        {
            var emailValidator = new EmailAddressAttribute();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EmailModel data = JsonConvert.DeserializeObject<EmailModel>(requestBody);

            log.LogInformation($"input {data.Email}");
            if (!emailValidator.IsValid(data.Email))
            {
                return new BadRequestObjectResult($"invalid email {data.Email}");
            }

            return new OkObjectResult($"email unsubscribed {data.Email}");
        }
    }
}
