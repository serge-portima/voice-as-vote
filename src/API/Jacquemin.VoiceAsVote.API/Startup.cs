using Jacquemin.VoiceAsVote.API;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using Azure.Data.Tables;
using Azure.Identity;
using Jacquemin.VoiceAsVote.API.Accounts;
using Jacquemin.VoiceAsVote.API.Options;
using Jacquemin.VoiceAsVote.API.Utils;
using Jacquemin.VoiceAsVote.API.VoiceTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Jacquemin.VoiceAsVote.API;

internal class Startup : FunctionsStartup
{
    /// <summary>
    /// DefaultClientName of <see cref="AzureClientFactoryBuilder"/>
    /// </summary>
    private const string DefaultClientName = "Default";

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var optionNames = new[]
        {
            Microsoft.Extensions.Options.Options.DefaultName,
            DefaultClientName
        };

        foreach (var optionName in optionNames)
        {
            builder.Services.AddOptions<VavTableStorageOptions>(optionName)
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection(VavTableStorageOptions.Option).Bind(options);
                });
        }

        var credential = new ChainedTokenCredential(
            new VisualStudioCredential(),
            new ManagedIdentityCredential(),
            new VisualStudioCodeCredential()
        );

        builder.Services.AddAzureClients(factoryBuilder =>
        {
            TableServiceClient GetServiceClient(VavTableStorageOptions option)
            {
                var endpoint = new Uri(option.Endpoint);
                return new TableServiceClient(endpoint, credential);
            }
            factoryBuilder.AddClient<TableServiceClient, VavTableStorageOptions>(GetServiceClient);
        });

        builder.Services.AddSingleton(_ => SHA256.Create());
        builder.Services.AddSingleton<IHash, Hash>();
        builder.Services.AddSingleton<IAccountValidator, MailAccountValidator>();
        builder.Services.AddSingleton<IVoiceTableStrategy, VoiceTableStrategy>();
        builder.Services.AddSingleton<IVoiceTableOperation, VoiceTableOperation>();
    }
}