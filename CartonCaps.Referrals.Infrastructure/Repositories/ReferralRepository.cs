using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;
using CartonCaps.Referrals.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Referrals.Infrastructure.Repositories
{
    public class ReferralRepository: IReferralRepository
    {
        private readonly ApplicationDbContext _context;
        public ReferralRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Guid> CreateInviteAsync(ReferralInvite invite, CancellationToken ct)
        {
            await _context.ReferralInvites.AddAsync(invite, ct);
            await _context.SaveChangesAsync(ct);

            return invite.Id;
        }

        public async Task<IEnumerable<ReferralInvite>> GetInvitesByUserAsync(string referralUserId,CancellationToken ct)
        {
            return await _context.ReferralInvites.AsNoTracking().Where(referral=> referral.ReferrerUserId == referralUserId).ToListAsync(ct);
        }

        public async Task<ReferralInvite?> GetInviteByIdAsync(Guid inviteId, CancellationToken ct)
        {
            return await _context.ReferralInvites.FindAsync(inviteId, ct);
        }

        public async Task UpdateInviteAsync(ReferralInvite referral, CancellationToken ct)
        {
            _context.ReferralInvites.Update(referral);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<DateTime?> GetLastInviteCreatedAtAsync(string userId,CancellationToken ct)
        {
            return await _context.ReferralInvites
            .AsNoTracking()
            .Where(x => x.ReferrerUserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => (DateTime?)x.CreatedAt)
            .FirstOrDefaultAsync(ct);
        }

        public Task<int> CountInvitesInRangeAsync(string userId, DateTime from, DateTime to, CancellationToken ct)
        {
            return _context.ReferralInvites.AsNoTracking().CountAsync(x =>
            x.ReferrerUserId == userId &&
            x.CreatedAt >= from &&
            x.CreatedAt < to, ct);
        }

        public Task<ReferralInvite?> GetActiveInviteAsync(string userId, ShareChannel channel, DateTime date, CancellationToken ct)
        {
            return _context.ReferralInvites
            .AsNoTracking().
            FirstOrDefaultAsync(x =>
            x.ReferrerUserId == userId &&
            x.Channel == channel &&
            x.ExpiresAt > date &&
            x.Status == ReferralInviteStatus.Created, ct);
        }
    }
}
