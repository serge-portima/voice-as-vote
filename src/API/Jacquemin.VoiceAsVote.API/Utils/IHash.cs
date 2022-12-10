namespace Jacquemin.VoiceAsVote.API.Utils;

public interface IHash
{
    string GetHash(string firstToHash, params string[] extraToHashes);

    bool IsHash(string content);
}