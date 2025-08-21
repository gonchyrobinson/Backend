namespace Backend.DTOs.AuthDtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserInfoDto User { get; set; } = new();
    }
}
