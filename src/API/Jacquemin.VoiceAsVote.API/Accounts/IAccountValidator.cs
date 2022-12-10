namespace Jacquemin.VoiceAsVote.API.Accounts;

public interface IAccountValidator
{
    bool IsIdentityValid(string identity);
    bool IsHashedPasswordValid(string hashedPassword);
}