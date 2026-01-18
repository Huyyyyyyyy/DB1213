using DBAdmin.DTOs.Requests;
using DBAdmin.DTOs.Responses;
using DBAdmin.Interfaces;
using DBDatabase;
using DBDatabase.Entities.User;
using DBUtils;

namespace DBAdmin.Services
{
    public class UserService : IUser
    {
        private readonly DBExecutor _db;
        private readonly SimpleLogger _logger;
        public UserService(DBExecutor db, SimpleLogger logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task<GetUserResponse> GetUserAsync(GetUserRequest request)
        {
            UserRepository u_repo = new(_db, _logger);
            UserRow? u = await u_repo.GetUserById(request.user_id);
            if (u == null) return new GetUserResponse()
            {
                code = 400,
                message = "user not found"
            };

            return new GetUserResponse()
            {
                code = 200,
                message = "success",
                data = u
            };
        }
    }
}
