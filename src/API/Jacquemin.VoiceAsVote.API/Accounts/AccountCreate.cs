using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using Jacquemin.VoiceAsVote.API.Accounts.Models;
using Jacquemin.VoiceAsVote.API.Utils;
using Jacquemin.VoiceAsVote.API.VoiceTables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Jacquemin.VoiceAsVote.API.Accounts
{
    public class AccountCreate
    {
        public IVoiceTableOperation VoiceTableOperation { get; }
        public IAccountValidator AccountValidator { get; }
        public IHash Hash { get; }

        public AccountCreate(IVoiceTableOperation voiceTableOperation, IAccountValidator accountValidator, IHash hash)
        {
            VoiceTableOperation = voiceTableOperation;
            AccountValidator = accountValidator;
            Hash = hash;
        }

        [FunctionName("AccountCreate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)][Required] AccountCreation accountCreation,
            ILogger log)
        {
            accountCreation.ValidateModel();

            var isIdentityValid = AccountValidator.IsIdentityValid(accountCreation.Identity);
            if (isIdentityValid is false)
            {
                return new BadRequestErrorMessageResult($"{nameof(accountCreation.Identity)} is invalid");
            }

            var isHashedPasswordValid = AccountValidator.IsHashedPasswordValid(accountCreation.HashedPassword);
            if (isHashedPasswordValid is false)
            {
                return new BadRequestErrorMessageResult($"{nameof(accountCreation.HashedPassword)} is invalid");
            }

            var hashedIdentity = Hash.GetHash(accountCreation.Identity);

            var existingAccount = await VoiceTableOperation.GetIdentityAsync(hashedIdentity);
            if (existingAccount is not null)
            {
                return new ConflictObjectResult("This account already exists");
            }

            await VoiceTableOperation.CreateAccountAsync(hashedIdentity, accountCreation.HashedPassword);

            return new OkResult();
        }
    }
}
