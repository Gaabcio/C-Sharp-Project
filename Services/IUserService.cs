using System;
using System.Threading.Tasks;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        // Task<User?> GetUserByIdAsync(int userId);
        // Task<User?> GetUserByUsernameAsync(string username);
        // Task<bool> CreateUserAsync(User user, string password);
        // Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null);
        // Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task UpdateLastLoginAsync(int userId);
    }
}