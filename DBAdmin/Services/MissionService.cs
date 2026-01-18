using DBAdmin.DTOs.Requests;
using DBAdmin.DTOs.Responses;
using DBAdmin.Interfaces;
using DBDatabase;
using DBDatabase.Entities.Claim;
using DBDatabase.Entities.User;
using DBUtils;

namespace DBAdmin.Services
{
    public class MissionService : IMission
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;
        public MissionService(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        private static readonly int[] StreakRewards = { 10, 20, 30, 40, 50, 60, 70 };

        public async Task<DailyClaimResponse> DailyClaimAsync(DailyClaimRequest request)
        {
            ClaimRepository c_repo = new(_db, _logger);
            UserRepository u_repo = new(_db, _logger);
            UserRow? u = await u_repo.GetUserById(request.user_id);
            if (u == null) return new DailyClaimResponse()
            {
                code = 400,
                message = "user not found"
            };

            var today = DateTime.UtcNow.Date;
            if (u.usr_latest_claim_date?.Date == today)
            {
                return new DailyClaimResponse
                {
                    code = 200,
                    message = "already claimed"
                };
            }

            // miss one day -> reset
            if (u.usr_latest_claim_date?.Date == today.AddDays(-1)) u.usr_streak += 1;
            else u.usr_streak = 1;

            //full -> reset
            if (u.usr_streak > 7) u.usr_streak = 1;

            //decide rewards
            var reward = StreakRewards[u.usr_streak - 1];
            u.usr_points += reward;
            u.usr_latest_claim_date = today;

            //update
            bool update_success = await u_repo.Update(u);
            if (!update_success) return new DailyClaimResponse
            {
                code = 500,
                message = "failed to update user"
            };

            //claim
            var claim = new ClaimRow
            {
                clms_id = Guid.NewGuid(),
                clms_user_id = request.user_id,
                clms_created_at = DateTime.UtcNow,
                clms_points = reward,
                clms_streak = u.usr_streak,
                clms_bonus = u.usr_streak == 7 ? 1 : 0
            };
            await c_repo.Insert(claim);

            return new DailyClaimResponse
            {
                code = 200,
                message = "completed",
                data = claim
            };
        }
    }
}
