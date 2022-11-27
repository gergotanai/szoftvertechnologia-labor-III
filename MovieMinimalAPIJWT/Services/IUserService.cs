using MovieMinimalAPIJWT.Models;

namespace MovieMinimalAPIJWT.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
