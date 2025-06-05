// using System;
// using System.Linq;
// using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingManagementSystem.Data;
using ParkingManagementSystem.Models;

namespace ParkingManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ParkingDbContext _context;  // Dostęp do bazy danych

        public UserService(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            // Znajdź aktywnego użytkownika po nazwie
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            // Sprawdź hasło używając BCrypt
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                await UpdateLastLoginAsync(user.Id); // Aktualizuj ostatnie logowanie
                return user;
            }

            return null;
        }

        // public async Task<User?> GetUserByIdAsync(int userId)
        // {
        //     return await _context.Users
        //         .Include(u => u.Vehicles)
        //             .ThenInclude(v => v.VehicleType)
        //         .FirstOrDefaultAsync(u => u.Id == userId);
        // }

        // public async Task<User?> GetUserByUsernameAsync(string username)
        // {
        //     return await _context.Users
        //         .FirstOrDefaultAsync(u => u.Username == username);
        // }

        // public async Task<bool> CreateUserAsync(User user, string password)
        // {
        //     try
        //     {
        //         user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        //         user.CreatedAt = DateTime.Now;

        //         _context.Users.Add(user);
        //         await _context.SaveChangesAsync();
        //         return true;
        //     }
        //     catch
        //     {
        //         return false;
        //     }
        // }

        // public async Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null)
        // {
        //     var query = _context.Users.Where(u => u.Username == username);

        //     if (excludeUserId.HasValue)
        //         query = query.Where(u => u.Id != excludeUserId.Value);

        //     return !await query.AnyAsync();
        // }

        // public async Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        // {
        //     if (string.IsNullOrEmpty(email)) return true;

        //     var query = _context.Users.Where(u => u.Email == email);

        //     if (excludeUserId.HasValue)
        //         query = query.Where(u => u.Id != excludeUserId.Value);

        //     return !await query.AnyAsync();
        // }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}