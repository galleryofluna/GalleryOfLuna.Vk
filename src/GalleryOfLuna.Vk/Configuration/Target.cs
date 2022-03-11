using Cronos;

using FluentValidation;

namespace GalleryOfLuna.Vk.Configuration
{
    public record Target
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Threshold { get; set; }

        public IEnumerable<string>? Tags { get; set; }

        public IEnumerable<string>? ExcludedTags { get; set; }

        public DateTime? Until { get; set; }

        public DateTime? After { get; set; }

        public string Schedule { get; set; } = string.Empty;

        public sealed class Validator : AbstractValidator<Target>
        {
            public Validator()
            {
                RuleFor(target => target.Schedule)
                    .NotEmpty()
                    .Custom((cronExpression, context) =>
                    {
                        try
                        {
                            CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
                        }
                        catch (CronFormatException exception)
                        {
                            context.AddFailure(nameof(Schedule),
                                $"Invalid cron expression was presented - '{cronExpression}'. {exception.Message}");
                        }
                    });
            }
        }
    }
}