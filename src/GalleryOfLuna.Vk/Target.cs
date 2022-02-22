using Cronos;

using System;

namespace GalleryOfLuna.Vk
{
    public class Target : IEquatable<Target>
    {
        public string Name { get; }
        public string Description { get; }
        public string Query { get; }
        public CronExpression Schedule { get; }

        public Target(string query, CronExpression schedule, string? name = "", string? description = "")
        {
            Query = query;
            Schedule = schedule;
            Name = name ?? string.Empty;
            Description = description ?? string.Empty;
        }

        public static Target From(Configuration.Target configTarget)
        {
            var schedule = CronExpression.Parse(configTarget.Schedule, CronFormat.IncludeSeconds);

            var target = new Target(configTarget.Query, schedule, configTarget.Name, configTarget.Description);

            return target;
        }

        public Configuration.Target ToConfiguration() => new() 
            {
                Name = Name,
                Description = Description,
                Schedule = Schedule.ToString(),
                Query = Query
            };

        public bool Equals(Target? other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Name == other.Name && Description == other.Description && Query == other.Query && Schedule.Equals(other.Schedule);
        }

        public override bool Equals(
            object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Target)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, Query, Schedule);
        }
    }
}
