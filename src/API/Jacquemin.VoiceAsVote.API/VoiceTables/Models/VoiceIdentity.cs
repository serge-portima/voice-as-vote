using System;
using Azure;
using Azure.Data.Tables;

namespace Jacquemin.VoiceAsVote.API.VoiceTables.Models;

public class VoiceIdentity : ITableEntity
{
    public string HashedIdentity { get; set; }
    public bool Confirmed { get; set; }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}