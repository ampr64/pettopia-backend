namespace Domain.Entities
{
    public class Post : Entity<Guid>
    {
        public PostType PostType { get; set; } = null!;

        public PostStatus PostStatus { get; private set; } = PostStatus.Open;

        public string PetName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public PetGender PetGender { get; set; } = PetGender.Unknown;

        public NeuterStatus NeuterStatus { get; set; } = NeuterStatus.Unknown;

        public PetAge PetAge { get; set; } = null!;

        public PetSpecies PetSpecies { get; set; } = null!;

        public string CreatedBy { get; private init; } = null!;

        public DateTime CreatedAt { get; private init; }
                
        public ICollection<PostImage> Images { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
            PostStatus = PostStatus.Open;

            PetName = petName;
            Description = description;
            NeuterStatus = neuterStatus;
            PetGender = petGender;
            PetAge = petAge;
            PetSpecies = petSpecies;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            PostType = postType;

            AddDomainEvent(new PostCreatedEvent(this));
        }

        public void Close()
        {
            if (PostStatus != PostStatus.Open) throw new InvalidOperationException($"Post {Id} is '{PostStatus}' and it cannot be closed.");
            PostStatus = PostStatus.Closed;
        }

        public void Complete()
        {
            if (PostStatus != PostStatus.Open) throw new InvalidOperationException($"Post {Id} is '{PostStatus}' and it cannot be completed.");
            PostStatus = PostStatus.Completed;
        }
    }
}