namespace Domain.Entities.Posts
{
    public class Post : Entity<Guid>
    {
        public PostType Type { get; set; } = null!;

        public PostStatus Status { get; private set; } = PostStatus.Open;

        public string PetName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public PetGender PetGender { get; set; } = PetGender.Unknown;

        public NeuterStatus NeuterStatus { get; set; } = NeuterStatus.Unknown;

        public PetAge PetAge { get; set; } = null!;

        public PetSpecies PetSpecies { get; set; } = null!;

        public string CreatedBy { get; private init; } = null!;

        public DateTime CreatedAt { get; private init; }

        public Member Author { get; private set; }

        public DateTime? UpdatedAt { get; private set; } = null;

        private readonly List<PostImage> _images = new();
        public IReadOnlyList<PostImage> Images => _images.AsReadOnly();

        private readonly List<PostApplication> _applications = new();
        public IReadOnlyList<PostApplication> Applications => _applications.AsReadOnly();

        private bool IsOpen => Status == PostStatus.Open;

        private List<PostApplication> PendingApplications => _applications.Where(a => a.Status == ApplicationStatus.Pending).ToList();

        private Post() { }

        public Post(PostType postType,
            string petName,
            string description,
            PetGender petGender,
            NeuterStatus neuterStatus,
            PetAge petAge,
            PetSpecies petSpecies,
            string createdBy,
            DateTime createdAt)
        {
            Id = Guid.NewGuid();
            Status = PostStatus.Open;

            PetName = petName;
            Description = description;
            NeuterStatus = neuterStatus;
            PetGender = petGender;
            PetAge = petAge;
            PetSpecies = petSpecies;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            Type = postType;

            RaiseDomainEvent(new PostCreatedEvent(this));
        }

        public void SetImages(IEnumerable<PostImage> images, DateTime updatedAt)
        {
            if (!IsOpen) throw new DomainException($"Post must be open to set images.");
            if (!images.Any()) throw new DomainException($"Cannot set an empty list of images to a post.");

            UpdatedAt = updatedAt;
            _images.Clear();

            _images.AddRange(images);
        }

        public Guid AddApplication(DateTime now,
            string applicantId,
            string applicantName,
            string applicantEmail,
            PhoneNumber? applicantPhoneNumber)
        {
            if (!IsOpen) throw new DomainException("Post must be open to add an application.");
            if (CreatedBy == applicantId) throw new DomainException("Post author cannot apply to their own post.");
            if (_applications.Any(a => a.ApplicantId == applicantId && a.Status != ApplicationStatus.Canceled)) throw new DomainException("User has already applied for this post.");

            var application = new PostApplication(Id, now, applicantId, applicantName, applicantEmail, applicantPhoneNumber);

            _applications.Add(application);

            RaiseDomainEvent(new PostApplicationSubmittedEvent(application));

            return application.Id;
        }

        public void Close(DateTime updatedAt)
        {
            if (!IsOpen) throw new DomainException($"Post must be open to close it.");

            PendingApplications.ForEach(a => a.Cancel(updatedAt));

            UpdatedAt = updatedAt;
            Status = PostStatus.Closed;
        }

        public void Complete(PostApplication application, DateTime updatedAt)
        {
            if (!IsOpen) throw new DomainException($"Post must be open to complete it.");
            if (!_applications.Contains(application)) throw new DomainException($"The application does not belong to this post.");

            application.Accept(updatedAt);

            PendingApplications.ForEach(a => a.Reject(updatedAt));

            UpdatedAt = updatedAt;
            Status = PostStatus.Completed;

            RaiseDomainEvent(new PostCompletedEvent(this));
        }

        public void RejectApplication(PostApplication application, DateTime now)
        {
            if (!_applications.Contains(application)) throw new DomainException("Post cannot reject this application as it does not belong to it.");

            application.Reject(now);
        }

        public void CancelApplication(PostApplication application, DateTime now)
        {
            if (!_applications.Contains(application)) throw new DomainException("Post cannot cancel this application as it does not belong to it.");

            application.Cancel(now);
        }
    }
}