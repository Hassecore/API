namespace Hassecore.API.Business.DTOs.UserPairing
{
    public class PendingPairingResponseDto
    {
        public required string RequestingUserEmail { get; set; }
        public required bool AuthenticatedUserIsReceiver { get; set; }
    }
}
