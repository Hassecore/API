namespace Hassecore.API.Business.DTOs.UserPairing
{
    public class UserPairDto
    {
        public Guid CurrentUserId { get; set; }
        public Guid? PairedUserid { get; set; }
    }
}
