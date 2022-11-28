using AutoMapper;
using ProjetoLoginToken.Models;
using ProjetoLoginToken.Models.Requests;

namespace ProjetoLoginToken.Profiles;

public class UserProfiles : Profile
{
	public UserProfiles()
	{
		CreateMap<RegisterRequest, User>();
    }
}