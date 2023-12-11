using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Company.Function.DAL;
using Company.Function.Models;

namespace Company.Function
{
    public static class SubscribeEmail
    {
        [FunctionName("SubscribeEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "email/subscribe")]
            HttpRequest req,
            ILogger log)
        {
            var emailValidator = new EmailAddressAttribute();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            EmailModel email = JsonConvert.DeserializeObject<EmailModel>(requestBody);

            log.LogInformation($"input {email.Email}");
            if (!emailValidator.IsValid(email.Email))
            {
                return new BadRequestObjectResult(new
                {
                    errorId = 0,
                    errorMessage = $"invalid email {email.Email}"
                });
            }

            var emailRepository = new EmailRepository();

            var insertedEmail = await emailRepository.Add(email);
            if (insertedEmail == null)
            {
                return new BadRequestObjectResult(new
                {
                    errorId = 1,
                    errorMessage = $"email is already subscribed {email.Email}"
                });
            }

            return new OkObjectResult($"email subscribed {email.Email}");
        }
    }
}