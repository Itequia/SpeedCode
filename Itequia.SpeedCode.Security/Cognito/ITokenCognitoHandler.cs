using System.Threading.Tasks;

namespace Itequia.SpeedCode.Security.Cognito
{
    public interface ITokenCognitoHandler
    {
        Task<bool> Validate(string idToken, string region, string userPoolId, string clientId);
    }
}
