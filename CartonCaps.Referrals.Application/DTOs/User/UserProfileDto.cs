using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.DTOs.User
{
    public sealed record UserProfileDto(

        string UserId,
        string ReferralCode,
        string FullName
    );
}
