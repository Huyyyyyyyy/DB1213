using DBAdmin.DTOs.Requests;
using DBAdmin.DTOs.Responses;

namespace DBAdmin.Interfaces
{
    public interface IUser
    {
        Task<GetUserResponse> GetUserAsync(GetUserRequest request);
    }
}
