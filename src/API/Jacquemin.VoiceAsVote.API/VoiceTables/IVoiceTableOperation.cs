using System.Threading.Tasks;
using Jacquemin.VoiceAsVote.API.VoiceTables.Models;

namespace Jacquemin.VoiceAsVote.API.VoiceTables;

public interface IVoiceTableOperation
{
    Task<VoiceIdentity> GetIdentityAsync(string hashedIdentity);
    Task CreateAccountAsync(string hashedIdentity, string hashedPassword);
}