namespace ProjetoLoginToken.Models.Response;

public class AuthenticatedUserResponse
{
	public AuthenticatedUserResponse() { }

	public AuthenticatedUserResponse(string acessToken)
	{
		AcessToken = acessToken;
	}

    public string AcessToken { get; set; }
}