using System.ComponentModel.DataAnnotations;

namespace Jacquemin.VoiceAsVote.API.Accounts.Models;

public class AccountCreation
{
    [Required]
    public string Identity { get; init; }
    [Required]
    public string HashedPassword { get; init; }
}