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