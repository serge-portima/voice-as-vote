using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Jacquemin.VoiceAsVote.API.Options;
using Jacquemin.VoiceAsVote.API.Utils;
using Jacquemin.VoiceAsVote.API.VoiceTables.Models;
using Microsoft.Extensions.Options;

namespace Jacquemin.VoiceAsVote.API.VoiceTables;

public class VoiceTableOperation : IVoiceTableOperation
{
    public TableServiceClient TableServiceClient { get; }
    public VavTableStorageOptions Options { get; }
    public IVoiceTableStrategy VoiceTableStrategy { get; }
    public IHash Hash { get; }

    public VoiceTableOperation(
        TableServiceClient tableServiceClient,
        IOptions<VavTableStorageOptions> options,
        IVoiceTableStrategy voiceTableStrategy,
        IHash hash
    )
    {
        TableServiceClient = tableServiceClient;
        VoiceTableStrategy = voiceTableStrategy;
        Hash = hash;
        Options = options.Value;
    }

    protected virtual TableClient GetTableClient() => TableServiceClient.GetTableClient(Options.TableName);
    protected virtual string GetAccountKey(string hashedIdentity, string hashedPassword) => Hash.GetHash(hashedIdentity + hashedPassword);

    public Task<VoiceIdentity> GetIdentityAsync(string hashedIdentity)
    {
        var tableClient = GetTableClient();
        var filter = VoiceTableStrategy.GetIdentitiesFilter(hashedIdentity);
        
        return tableClient.QueryAsync<VoiceIdentity>(filter, 1)
            .AsPages()
            .SelectMany(page => page.Values.ToAsyncEnumerable())
            .FirstOrDefaultAsync()
            .AsTask();
    }

    public Task CreateAccountAsync(string hashedIdentity, string hashedPassword)
    {
        var tableClient = GetTableClient();

        var accountKey = GetAccountKey(hashedIdentity, hashedPassword);

        var voiceIdentity = new VoiceIdentity
        {
            HashedIdentity = hashedIdentity,
            Confirmed = true // TODO: Auto confirmed for now
        };
        var voiceAccount = new VoiceAccount
        {
            HashedKey = accountKey
        };
        IEnumerable<TableTransactionAction> transactionActions = new[]
        {
            new TableTransactionAction(TableTransactionActionType.Add, VoiceTableStrategy.Prepare(voiceIdentity)),
            new TableTransactionAction(TableTransactionActionType.Add, VoiceTableStrategy.Prepare(voiceAccount))
        };
        return tableClient.SubmitTransactionAsync(transactionActions);
    }
}