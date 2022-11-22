using Microsoft.AspNetCore.Mvc;

using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace dog_api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private const string _clientId = "1gogia6g34kkp7gr62vdud122i";
    private readonly RegionEndpoint _region = RegionEndpoint.USEast2;

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<string>> Register(User user)
    {
        var cognito = new AmazonCognitoIdentityProviderClient(_region);

        var request = new SignUpRequest
        {
            ClientId = _clientId,
            Password = user.Password,
            Username = user.Username
        };

        var emailAttribute = new AttributeType
        {
            Name = "email",
            Value = user.Email
        };
        request.UserAttributes.Add(emailAttribute);

        var response = await cognito.SignUpAsync(request);

        return Ok();
    }

    [HttpPost]
    [Route("signin")]
    public async Task<ActionResult<string>> SignIn(User user)
    {
        var cognito = new AmazonCognitoIdentityProviderClient(_region);

        var request = new AdminInitiateAuthRequest
        {
            UserPoolId = "us-east-2_yIxyJXCA6",
            ClientId = _clientId,
            AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
        };

        request.AuthParameters.Add("USERNAME", user.Username);
        request.AuthParameters.Add("PASSWORD", user.Password);

        var response = await cognito.AdminInitiateAuthAsync(request);

        return Ok(response.AuthenticationResult.IdToken);
    }
}