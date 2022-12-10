using System;
using Azure;
using Azure.Data.Tables;

namespace Jacquemin.VoiceAsVote.API.VoiceTables.Models;

public class VoiceAccount : ITableEntity
{
    public string HashedKey { get; set; }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}