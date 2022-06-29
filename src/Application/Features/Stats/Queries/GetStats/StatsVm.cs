namespace Application.Features.Stats.Queries.GetStats
{
    public class StatsVm
    {
        public int ActiveMembersCount { get; set; }

        public int InactiveMembersCount { get; set; }

        public int FosterersCount { get; set; }

        public int TotalPostsCount { get; set; }

        public int CompletedPostsCount { get; set; }

        public int AdoptionsCount { get; set; }

        public int MissingCount { get; set; }

        public int FoundCount { get; set; }
    }
}