namespace DigMap.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public string[] Errors { get; set; }
    }
}