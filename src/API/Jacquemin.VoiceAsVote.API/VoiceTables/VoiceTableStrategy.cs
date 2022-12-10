using System;
using System.Runtime.CompilerServices;
using System.Text;
using Azure.Data.Tables;
using Jacquemin.VoiceAsVote.API.VoiceTables.Models;

namespace Jacquemin.VoiceAsVote.API.VoiceTables;

class VoiceTableStrategy : IVoiceTableStrategy
{
    private const string LowerBound = "0";
    private const string UpperBound = "1";

    private const string IdentityPrefix = "ID";
    private const string AccountPrefix = "AC";

    protected virtual string GetPartitionKeyForIdentities() => "Identity";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string PaddedValue(int i) => $"{i:D10}";

    public string GetIdentitiesFilter(string hashedIdentity)
    {
        var partitionKey = GetPartitionKeyForIdentities();

        var rowKey = GetRowKey(new VoiceIdentity
        {
            HashedIdentity = hashedIdentity
        });
        return $"PartitionKey eq '{partitionKey}' and RowKey eq '{rowKey}'";
    }

    public ITableEntity Prepare(VoiceIdentity voiceIdentity)
    {
        var partitionKey = GetPartitionKeyForIdentities();
        var rowKey = GetRowKey(voiceIdentity);

        voiceIdentity.PartitionKey = partitionKey;
        voiceIdentity.RowKey = rowKey;

        return voiceIdentity;
    }

    public ITableEntity Prepare(VoiceAccount voiceAccount)
    {
        var partitionKey = GetPartitionKeyForIdentities();
        var rowKey = GetRowKey(voiceAccount);

        voiceAccount.PartitionKey = partitionKey;
        voiceAccount.RowKey = rowKey;

        return voiceAccount;
    }

    private static string UTF8ToHex(string utf8) => Convert.ToHexString(Encoding.UTF8.GetBytes(utf8));
    protected virtual string GetRowKey(VoiceAccount voiceIdentity) => $"{IdentityPrefix}{UTF8ToHex(voiceIdentity.HashedKey)}";
    protected virtual string GetRowKey(VoiceIdentity voiceIdentity) => $"{AccountPrefix}{UTF8ToHex(voiceIdentity.HashedIdentity)}";
}