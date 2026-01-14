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
        /// <summary>Get the current user's referral code.</summary>
        /// <remarks>
        /// **Authentication:** Required. Send header <c>X-Debug-UserId</c>.
        ///
        /// **Behavior:**
        /// - Returns the referral code and basic profile information for the authenticated user.
        /// **Request:**
        /// - No body
        ///
        /// **Response (200):**
        /// <code>
        /// {
        ///   "success": true,
        ///   "status": 200,
        ///   "data": {
        ///     "userId": "oburgosgo@gmail.com",
        ///     "referralCode": "ZaH234",
        ///     "fullName": "Oscar Burgos"
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        [Authorize]
        [HttpGet("referral-code")]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetReferralCode(CancellationToken ct)
        {
            return this.ApiOk(await _referralService.GetReferralCodeAsync(ct));
        }
        /// <summary>Get a referral invite by id.</summary>
        /// <remarks>
        /// **Authentication:** Not required (public).
        ///
        /// **Behavior:**
        /// - Returns invite details for the provided <c>inviteId</c>.
        /// **Request:**
        /// - No body
        ///
        /// **Response (200):**
        /// <code>
        /// {
        ///   "success": true,
        ///   "status": 200,
        ///   "data": {
        ///     "id": "11111111-1111-1111-1111-111111111111",
        ///     "referralCode": "ZaH234",
        ///     "referrerUserId": "oburgosgo@gmail.com",
        ///     "channel": "Sms",
        ///     "channelName": "Sms",
        ///     "deeplink": "https://example.link/abc",
        ///     "status": "Created",
        ///     "statusName": "Created",
        ///     "createdAt": "2026-01-13T12:00:00Z",
        ///     "expiresAt": "2026-01-15T12:00:00Z",
        ///     "referredUserName": null
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        [Authorize]
        [HttpGet("{inviteId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<InviteReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<ReferralInviteDto>>> GetInviteById(Guid inviteId, CancellationToken ct)
        {
            return this.ApiOk(await _referralService.GetInviteByIdAsync(inviteId,ct));
        }

        /// <summary>Get all referral invites created by the current user.</summary>
        /// <remarks>
        /// **Authentication:** Required. Send header <c>X-Debug-UserId</c>.
        ///
        /// **Behavior:**
        /// - Returns all invites where the authenticated user is the referrer.
        /// 
        /// **Request:**
        /// - No body
        ///
        /// **Response (200):**
        /// <code>
        /// {
        ///   "success": true,
        ///   "status": 200,
        ///   "data": [
        ///     {
        ///       "id": "11111111-1111-1111-1111-111111111111",
        ///       "referralCode": "ZaH234",
        ///       "referrerUserId": "oburgosgo@gmail.com",
        ///       "channel": "Sms",
        ///       "channelName": "Sms",
        ///       "deeplink": "https://example.link/abc",
        ///       "status": "Created",
        ///       "statusName": "Created",
        ///       "createdAt": "2026-01-13T12:00:00Z",
        ///       "expiresAt": "2026-01-15T12:00:00Z",
        ///       "referredUserName": null
        ///     }
        ///   ],
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<InviteReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ReferralInviteDto>>>> GetInvitesByUser(CancellationToken ct)
        {
            return this.ApiOk(await _referralService.GetInvitesByUserAsync(ct));
        }

        /// <summary>Create a referral invite.</summary>
        /// <remarks>
        /// **Authentication:** Required. Send header <c>X-Debug-UserId</c>.
        ///
        /// **Behavior:**
        /// - If an active invite already exists for the same channel, the API returns that invite (no new record is created).
        /// - Otherwise, the API validates invite limits (daily max + cooldown) and creates a new invite.
        ///
        /// **Business error codes (returned in response):**
        /// - <c>InviteCooldownNotMet</c>
        /// - <c>DailyInviteLimitReached</c>
        /// - <c>UserNotAuthenticated</c>
        /// 
        /// **Request body:**
        /// <code>
        /// { "channel": "Sms" }
        /// </code>
        ///
        /// **Response (200 - created or existing active invite):**
        /// <code>
        /// {
        ///   "success": true,
        ///   "status": 200,
        ///   "data": {
        ///     "inviteId": "11111111-1111-1111-1111-111111111111",
        ///     "referralId": "oburgosgo@gmail.com",
        ///     "deeplinkUrl": "https://example.link/abc",
        ///     "status": "Created",
        ///     "referralCode": "ZaH234",
        ///     "referralName": "Oscar Burgos",
        ///     "channel": "Sms",
        ///     "shareContent": {
        ///       "text": "Hi! Oscar Burgos here... https://example.link/abc"
        ///     },
        ///     "message": "Invite created successfully.",
        ///     "code": "Success",
        ///     "statusName": "Created",
        ///     "channelName": "Sms"
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<InviteReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status429TooManyRequests)]
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


        /// <summary>Resolve a referral invite.</summary>
        /// <remarks>
        /// **Authentication:** Not required (public). Used during onboarding to decide the next screen.
        ///
        /// **Behavior:**
        /// - If the invite is valid and active, returns <c>NextAction=ShowReferralAuthGate</c> and referrer information.
        /// - If the invite is invalid, returns <c>NextAction=ShowAuthGate</c> with a reason code.
        ///
        /// **Business error codes (returned in response):**
        /// - <c>InviteNotFound</c>
        /// - <c>InviteExpired</c> (the invite may be persisted as Expired)
        /// - <c>InviteAlreadyRedeemed</c>
        /// 
        /// **Request:**
        /// - No body
        ///
        /// **Response (200 - valid):**
        /// <code>
        /// {
        ///   "success": true,
        ///   "status": 200,
        ///   "data": {
        ///     "inviteId": "11111111-1111-1111-1111-111111111111",
        ///     "isValid": true,
        ///     "nextAction": "ShowReferralAuthGate",
        ///     "referrerName": "Oscar Burgos",
        ///     "expiresAt": "2026-01-15T12:00:00Z",
        ///     "referralCode": "ZaH234",
        ///     "code": "Success",
        ///     "message": "Invitation has been resolved succesfully."
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>

        [HttpPut("{inviteId:guid}/resolve")]
        [ProducesResponseType(typeof(ApiResponse<InviteReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status410Gone)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
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

        /// <summary>Redeem a referral invite.</summary>
        /// <remarks>
        /// **Authentication:** Required. Send header <c>X-Debug-UserId</c>.
        ///
        /// **Behavior:**
        /// - Validates the invite exists and is redeemable (not expired, not already redeemed).
        /// - If valid, marks the invite as <c>Redeemed</c> and stores the new user information.
        /// - If the invite is expired, it may be persisted as <c>Expired</c>.
        ///
        /// **Business error codes (returned in response):**
        /// - <c>InvalidUser</c>
        /// - <c>InviteNotFound</c>
        /// - <c>InviteExpired</c>
        /// - <c>InviteAlreadyRedeemed</c>
        /// - <c>InvalidReedemtion</c>
        /// 
        /// **Request body:**
        /// <code>
        /// { "newUserId": "new-user-1", "newUserName": "John Smith" }
        /// </code>
        ///
        /// **Response (200 - success):**
        /// <code>
        /// {
        ///   "success": true,
        ///   "status": 200,
        ///   "data": {
        ///     "inviteId": "11111111-1111-1111-1111-111111111111",
        ///     "success": true,
        ///     "code": "Redeemed",
        ///     "message": "Invite redeemed successfully.",
        ///     "redeemedAt": "2026-01-13T12:00:00Z"
        ///   },
        ///   "errors": []
        /// }
        /// </code>
        /// </remarks>
        [Authorize]
        [HttpPut("{inviteId:guid}/redeem")]
        [ProducesResponseType(typeof(ApiResponse<InviteReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status410Gone)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
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
