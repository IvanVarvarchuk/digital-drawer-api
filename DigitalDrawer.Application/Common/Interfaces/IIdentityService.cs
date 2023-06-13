using DigitalDrawer.Application.Common.Models;

namespace DigitalDrawer.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<bool> ValidateUser(LoginUserViewModel userModel);

    Task<User> FindUserByEmail(string email);

    Task<(Result Result, User User)> CreateUserAsync(RegisterUserModel model);

    Task<Result> DeleteUserAsync(string userId);
}
