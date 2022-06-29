namespace Domain.Entities.Posts
{
    public class PostApplication : Entity<Guid>
    {
        public Guid PostId { get; private init; }

        public string ApplicantId { get; private init; }

        public ApplicationStatus Status { get; private set; } = ApplicationStatus.Pending;

        public ApplicantInfo ApplicantInfo { get; private init; }

        public DateTime SubmittedAt { get; private init; }

        public DateTime? UpdatedAt { get; private set; }

        private PostApplication()
        {
        }

        public PostApplication(Guid postId,
            DateTime submittedAt,
            string applicantId,
            string applicantName,
            string applicantEmail,
            PhoneNumber? applicantPhoneNumber)
        {
            Id = Guid.NewGuid();
            PostId = postId;
            SubmittedAt = submittedAt;
            ApplicantId = applicantId;
            ApplicantInfo = new ApplicantInfo(Id, applicantName, applicantEmail, applicantPhoneNumber);
        }

        internal void Accept(DateTime acceptedAt)
        {
            if (Status != ApplicationStatus.Pending) throw new DomainException("Application must be pending to be accepted.");

            Status = ApplicationStatus.Accepted;
            UpdatedAt = acceptedAt;
        }

        internal void Reject(DateTime rejectedAt)
        {
            if (Status != ApplicationStatus.Pending) throw new DomainException("Application must be pending to be rejected.");

            Status = ApplicationStatus.Rejected;
            UpdatedAt = rejectedAt;
        }

        internal void Cancel(DateTime canceledAt)
        {
            if (Status != ApplicationStatus.Pending) throw new DomainException("Application must be pending to be canceled.");

            Status = ApplicationStatus.Canceled;
            UpdatedAt = canceledAt;
        }
    }
}