﻿using Cronos;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk.Configuration
{
    public class Target
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
        public string Query { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;

        public sealed class Validator : AbstractValidator<Target>
        {
            public Validator()
            {
                RuleFor(target => target.Query)
                    .NotEmpty();
                RuleFor(target => target.Schedule)
                    .NotEmpty()
                    .Custom((cronExpression, context) =>
                    {
                        try
                        {
                            CronExpression.Parse(cronExpression);
                        }
                        catch (CronFormatException exception)
                        {
                            context.AddFailure(nameof(Target.Schedule), $"Invalid cron expression was presented - '{cronExpression}'. {exception.Message}");
                        }
                    });
            }
        }
    }
}