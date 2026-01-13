using CartonCaps.Referrals.Api.Common;
using CartonCaps.Referrals.Api.Contracts.Common;
using CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral;
using CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals;
using CartonCaps.Referrals.Application.DTOs.Referral.Redeem;
using CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral;
using CartonCaps.Referrals.Application.DTOs.User;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Referrals.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/invites")]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralsService _referralService;
        public ReferralsController(
            IReferralsService referralService
            ) {
            this._referralService = referralService;
        }
        [Authorize]
        [HttpGet("referral-code")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetReferralCode(CancellationToken ct)
        {
            return this.ApiOk(await _referralService.GetReferralCodeAsync(ct));
        }
        [Authorize]
        [HttpGet("{inviteId:guid}")]
        public async Task<ActionResult<ApiResponse<ReferralInviteDto>>> GetInviteById(Guid inviteId, CancellationToken ct)
        {
            return this.ApiOk(await _referralService.GetInviteByIdAsync(inviteId,ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReferralInviteDto>>>> GetInvitesByUser(CancellationToken ct)
        {
            return this.ApiOk(await _referralService.GetInvitesByUserAsync(ct));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<InviteReferralResponse>>> CreateInvite([FromBody] InviteReferralRequest request, CancellationToken ct)
        {
            var result = await _referralService.CreateInviteAsync(request, ct);

            if (!string.IsNullOrWhiteSpace(result.Code) && result.DeeplinkUrl is null)
            {
                var status = ErrorCodeMapper.ToHttpStatus(result.Code);
                return this.ApiError<InviteReferralResponse>(
                    status,
                    new ApiError { Code = result.Code, Message = result.Message ?? "Invite cannot be created." }
                );
            }
            return this.ApiOk(result);
        }

        [HttpPut("{inviteId:guid}/resolve")]
        public async Task<ActionResult<ApiResponse<ResolveReferralInviteResponse>>> ResolveInviteReferral(Guid inviteId, CancellationToken ct)
        {
            var result = await _referralService.ResolveInviteAsync(inviteId, ct);

            if (!result.IsValid)
            {
                var status = ErrorCodeMapper.ToHttpStatus(result.Code);
                return this.ApiError<ResolveReferralInviteResponse>(
                    status,
                    new ApiError { Code = result.Code, Message = result.Message ?? "Invite cannot be resolved." }
                );
            }

            return this.ApiOk(result);
        }

        [Authorize]
        [HttpPut("{inviteId:guid}/redeem")]
        public async Task<ActionResult<ApiResponse<RedeemReferralInviteResponse>>> RedeemInviteReferral([FromRoute] Guid inviteId, [FromBody] RedeemReferralInviteRequest request, CancellationToken ct)
        {
            var result = await _referralService.RedeemInviteAsync(inviteId,request, ct);

            if (!result.Success)
            {
                var status = ErrorCodeMapper.ToHttpStatus(result.Code);
                return this.ApiError<RedeemReferralInviteResponse>(
                    status,
                    new ApiError { Code = result.Code, Message = result.Message ?? "Invite cannot be resolved." }
                );
            }

            return this.ApiOk(result);
        }

    }
}
