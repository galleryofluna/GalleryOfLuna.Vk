using FluentValidation;

namespace GalleryOfLuna.Vk.Configuration
{
    public class TargetsConfiguration : List<Target>
    {
        public sealed class Validator : AbstractValidator<TargetsConfiguration>
        {
            public Validator()
            {
                RuleForEach(x => x)
                    .SetValidator(new Target.Validator());
            }
        }
    }
}