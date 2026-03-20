namespace Hassecore.API.Data.Context.CurrentUserContext
{
    public class CurrentUserContext
    {
        private bool _isInitialized = false;

        private Guid _userId { get; set; }
        private string? _externalId { get; set; }
        private string? _userName { get; set; }
        private string? _email { get; set; }
        private Guid? _pairedUserId { get; set; }

        public Guid UserId          => _isInitialized ? _userId! : throw new InvalidOperationException("CurrentUserContext has not been initialized.");
        public string ExternalId    => _isInitialized ? _externalId! : throw new InvalidOperationException("CurrentUserContext has not been initialized.");
        public string UserName      => _isInitialized ? _userName! : throw new InvalidOperationException("CurrentUserContext has not been initialized.");
        public string Email         => _isInitialized ? _email! : throw new InvalidOperationException("CurrentUserContext has not been initialized.");
        public Guid? PairedUserId   => _isInitialized ? _pairedUserId : throw new InvalidOperationException("CurrentUserContext has not been initialized.");

        public void Initialize(Guid id, string externalId, string username, string email, Guid? pairedUserId)
        {
            if (_isInitialized)
                throw new InvalidOperationException("CurrentUserContext has already been initialized.");

            _userId = id;
            _externalId = externalId;
            _userName = username;
            _email = email;
            _pairedUserId = pairedUserId;

            _isInitialized = true;
        }
    }
}
