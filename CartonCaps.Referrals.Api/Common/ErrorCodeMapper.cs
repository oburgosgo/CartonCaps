using CartonCaps.Referrals.Application.Common.Errors;
using System.Reflection.Metadata.Ecma335;

namespace CartonCaps.Referrals.Api.Common
{
    public static class ErrorCodeMapper
    {
        public static int ToHttpStatus(string code)
        {
            switch (code)
            {
                case ReferralErrorCodes.InviteNotFound:
                    return StatusCodes.Status404NotFound;
                case ReferralErrorCodes.InviteExpired:
                    return StatusCodes.Status410Gone;
                case ReferralErrorCodes.InviteAlreadyRedeemed:
                    return StatusCodes.Status409Conflict;
                case ReferralErrorCodes.InvalidReedemtion:
                    return StatusCodes.Status409Conflict;
                case ReferralErrorCodes.InviteCooldownNotMet:
                    return StatusCodes.Status429TooManyRequests;
                case ReferralErrorCodes.DailyInviteLimitReached:
                    return StatusCodes.Status429TooManyRequests;
                case ReferralErrorCodes.UserNotAuthenticated:
                    return StatusCodes.Status401Unauthorized;
                case ReferralErrorCodes.InvalidUser:
                    return StatusCodes.Status409Conflict;

                default:
                    return StatusCodes.Status400BadRequest;
            }
        }
    }
}
