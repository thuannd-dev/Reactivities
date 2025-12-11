using System;
using Application.Activities.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Command : IRequest<string>
    {
        public required CreateActivityDto ActivityDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = mapper.Map<Activity>(request.ActivityDto);
            //Don't use AddAsync for this case, just use AddAsync when use SequenceHiLo ID(suitable for bulk insert)
            //Add just tracking entity in memory, don't work with database
            context.Activities.Add(activity);
            await context.SaveChangesAsync(cancellationToken);
            return activity.Id;
        }
    }
}
