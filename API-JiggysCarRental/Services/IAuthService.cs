using API_JiggysCarRental.MODELS;

namespace API_JiggysCarRental.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<bool> Login(User user);
        Task<bool> RegisterUser(User user);
    }
}