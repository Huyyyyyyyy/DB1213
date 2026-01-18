using DBAdmin.DTOs.Requests;
using DBAdmin.DTOs.Responses;

namespace DBAdmin.Interfaces
{
    public interface IMission
    {
        Task<DailyClaimResponse> DailyClaimAsync(DailyClaimRequest requets);
        //Task<UserPointsResponse> GetUserPointsAsync(Guid userId);
        //Task<ReferralLinkResponse> GetReferralLinkAsync(Guid userId);
        //Task<List<LeaderboardItemResponse>> GetLeaderboardAsync();
    }
}
