namespace TDL.Services.Dto.User
{
    public class UserLoginResponseDto
    {
        public UserLoginResponseDto(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}
