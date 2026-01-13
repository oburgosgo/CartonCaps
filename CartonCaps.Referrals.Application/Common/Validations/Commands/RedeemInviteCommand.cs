
namespace CartonCaps.Referrals.Application.Common.Validations.Commands
{
    public sealed record RedeemInviteCommand(
        Guid InviteId, 
        string NewUserId, 
        string NewUserName
    );
}
