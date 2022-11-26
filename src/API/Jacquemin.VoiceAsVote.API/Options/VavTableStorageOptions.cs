namespace Jacquemin.VoiceAsVote.API.Options;

public class VavTableStorageOptions
{
    public static string Option => "TableStorageOptions";

    public string Endpoint { get; set; }
    public string TableName { get; set; }
}