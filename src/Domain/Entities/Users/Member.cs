namespace Domain.Entities.Users
{
    public abstract class Member : Entity<string>
    {
        public string FirstName { get; private set; } = null!;

        public string LastName { get; private set; } = null!;

        public string Email { get; private init; }

        public DateTime BirthDate { get; private set; }

        public Address? Address { get; private set; }

        public ProfilePicture? ProfilePicture { get; private set; }

        public Role Role { get; private set; } = null!;

        public AccountStatus Status { get; private set; } = AccountStatus.Active;

        public string? AboutMe { get; private set; }

        public DateTime RegisteredAt { get; private init; }

        public DateTime? UpdatedAt { get; protected set; }

        public PhoneNumber? PhoneNumber { get; private set; }

        public string? InstagramProfileUrl { get; private set; }

        public string? FacebookProfileUrl { get; private set; }

        public bool IsComplete { get; private set; }

        public IReadOnlyList<Post> Posts { get; private set; } = new List<Post>();

        protected Member()
        {
        }

        protected Member(string id,
            string firstName,
            string lastName,
            string email,
            DateTime birthDate,
            DateTime registeredAt)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            RegisteredAt = registeredAt;

            AddDomainEvent(new UserRegisteredEvent(this));
        }

        public void UpdateDetails(DateTime updatedAt, string firstName, string lastName, DateTime birthDate)
        {
            UpdatedAt = updatedAt;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public void UpdateContactInfo(DateTime updatedAt,
            PhoneNumber? phoneNumber,
            string? instagramProfileUrl,
            string? facebookProfileUrl)
        {
            UpdatedAt = updatedAt;
            PhoneNumber = phoneNumber;
            InstagramProfileUrl = instagramProfileUrl;
            FacebookProfileUrl = facebookProfileUrl;
        }

        public void SetAddress(DateTime updatedAt, Address address)
        {
            UpdatedAt = updatedAt;
            Address = address;
        }

        public void SetAboutMe(string aboutMe)
        {
            AboutMe = aboutMe;
        }

        public void ChangeProfilePicture(DateTime updatedAt, ProfilePicture profilePicture)
        {
            UpdatedAt = updatedAt;
            ProfilePicture = profilePicture;
        }

        public void SetAsComplete(DateTime updatedAt)
        {
            IsComplete = true;
            UpdatedAt = updatedAt;
        }

        public void MarkAsDeactivated(DateTime deactivatedAt)
        {
            foreach (var post in Posts)
            {
                post.Close(deactivatedAt);
            }

            UpdatedAt = deactivatedAt;
            Status = AccountStatus.Deactivated;
        }

        public void MarkAsBanned(DateTime bannedAt)
        {
            foreach (var post in Posts)
            {
                post.Close(bannedAt);
            }

            UpdatedAt = bannedAt;
            Status = AccountStatus.Banned;
        }
    }
}