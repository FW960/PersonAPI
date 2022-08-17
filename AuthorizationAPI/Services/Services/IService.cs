using AuthorizationAPI.Entities;

namespace AuthorizationAPI.Services.Services;

public interface IService
{
    public bool Authenticate(MyAuthenticationRequest request, out TokenDTO token, HttpContext context, bool passIsEnc);
}