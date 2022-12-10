using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Jacquemin.VoiceAsVote.API.Utils;

class Hash : IHash
{
    public SHA256 HashAlgorithm { get; }
    public Regex IsHashRegex { get; set; }

    public Hash(SHA256 hashAlgorithm)
    {
        HashAlgorithm = hashAlgorithm;
        IsHashRegex = new Regex("^[A-Fa-f0-9]{64}$");
    }

    public string GetHash(string firstToHash, params string[] extraToHashes)
    {
        var toHash = $"{firstToHash}{string.Join("", extraToHashes)}";
        var bytes = Encoding.UTF8.GetBytes(toHash);
        var hash = HashAlgorithm.ComputeHash(bytes);
     
        return Encoding.UTF8.GetString(hash);
    }

    public bool IsHash(string content) => IsHashRegex.IsMatch(content);
}