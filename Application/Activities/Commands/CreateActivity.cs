using System;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Commands;

public class CreateActivity
{
    public class Command : IRequest<string>
    {
        public required Activity Activity { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            //Don't use AddAsync for this case, just use AddAsync when use SequenceHiLo ID(suitable for bulk insert)
            //Add just tracking entity in memory, don't work with database
            context.Activities.Add(request.Activity);
            await context.SaveChangesAsync(cancellationToken);
            return request.Activity.Id;
        }
    }
}
