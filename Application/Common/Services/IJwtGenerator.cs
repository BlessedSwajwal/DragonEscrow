using Domain.User;

namespace Application.Common.Services;

public interface IJwtGenerator
{
    string GenerateJwt(UserBase User);
}
