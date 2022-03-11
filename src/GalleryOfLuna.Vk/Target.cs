using Cronos;

namespace GalleryOfLuna.Vk
{
    public class Target : IEquatable<Target>
    {
        public string Name { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public int Threshold { get; set; }

        public IEnumerable<string> Tags { get; init; } = Array.Empty<string>();

        public IEnumerable<string> ExcludedTags { get; init; } = Array.Empty<string>();

        public DateTime? Until { get; init; }

        public DateTime? After { get; init; }

        public CronExpression Schedule { get; }

        public Target(CronExpression schedule)
        {
            Schedule = schedule;
        }

        public static Target From(Configuration.Target configTarget)
        {
            var schedule = CronExpression.Parse(configTarget.Schedule, CronFormat.IncludeSeconds);

            var target = new Target(schedule)
            {
                Name = configTarget.Name ?? string.Empty,
                Description = configTarget.Description ?? string.Empty,

                Threshold = configTarget.Threshold ?? 0,

                Tags = configTarget.Tags ?? Array.Empty<string>(),
                ExcludedTags = configTarget.ExcludedTags ?? Array.Empty<string>(),
                After = configTarget.After,
                Until = configTarget.Until
            };

            return target;
        }

        public Configuration.Target ToConfiguration() => new()
        {
            Name = Name,
            Description = Description,
            Schedule = Schedule.ToString(),

            Threshold = Threshold,

            Tags = Tags,
            ExcludedTags = ExcludedTags,
            After = After,
            Until = After
        };

        public bool Equals(Target? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Name == other.Name
                   && Description == other.Description
                   && Equals(Tags, other.Tags)
                   && Equals(ExcludedTags, other.ExcludedTags)
                   && Nullable.Equals(Until, other.Until)
                   && Nullable.Equals(After, other.After)
                   && Schedule.Equals(other.Schedule);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((Target)obj);
        }

        public override int GetHashCode() =>
            HashCode.Combine(Name, Description, Tags, ExcludedTags, Until, After, Schedule);
    }
}