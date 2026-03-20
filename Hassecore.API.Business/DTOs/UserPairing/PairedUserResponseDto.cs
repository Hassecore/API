namespace Hassecore.API.Business.DTOs.UserPairing
{
    public class PairedUserResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
