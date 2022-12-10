using Azure.Data.Tables;
using Jacquemin.VoiceAsVote.API.VoiceTables.Models;

namespace Jacquemin.VoiceAsVote.API.VoiceTables;

public interface IVoiceTableStrategy
{
    string GetIdentitiesFilter(string hashedIdentity);
    ITableEntity Prepare(VoiceIdentity voiceIdentity);
    ITableEntity Prepare(VoiceAccount voiceAccount);
}