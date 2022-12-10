using Jacquemin.VoiceAsVote.API.Utils;

namespace Jacquemin.VoiceAsVote.API.Accounts;

class MailAccountValidator : IAccountValidator
{
    public IHash Hash { get; }

    public MailAccountValidator(IHash hash)
    {
        Hash = hash;
    }

    public bool IsIdentityValid(string identity)
    {
        return true; // TODO: Check identity is a correctly formatted mail address
    }

    public bool IsHashedPasswordValid(string hashedPassword) => Hash.IsHash(hashedPassword);
}