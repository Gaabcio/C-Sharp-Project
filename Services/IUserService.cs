using System;
using System.Threading.Tasks;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task UpdateLastLoginAsync(int userId);
    }
}