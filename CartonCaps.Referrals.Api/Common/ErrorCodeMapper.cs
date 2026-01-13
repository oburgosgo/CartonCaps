using System.Reflection.Metadata.Ecma335;

namespace CartonCaps.Referrals.Api.Common
{
    public static class ErrorCodeMapper
    {
        public static int ToHttpStatus(string code)
        {
            switch (code)
            {
                case "InviteNotFound":
                    return StatusCodes.Status404NotFound;
                case "InviteExpired":
                    return StatusCodes.Status410Gone;
                case "InviteAlreadyRedeemed":
                    return StatusCodes.Status409Conflict;
                case "SelfReferralNotAllowed":
                    return StatusCodes.Status409Conflict;
                case "InviteCooldownNotMet":
                    return StatusCodes.Status429TooManyRequests;
                case "DailyInviteLimitReached":
                    return StatusCodes.Status429TooManyRequests;
                case "InvalidRequest":
                    return StatusCodes.Status400BadRequest;
                default:
                    return StatusCodes.Status400BadRequest;
            }
        }
    }
}
