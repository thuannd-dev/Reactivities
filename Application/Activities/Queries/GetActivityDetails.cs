using System;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Queries;

public class GetActivityDetails
{
    public class Query : IRequest<Activity>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Query, Activity>
    {
        public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
        {
            //?? is called null-coalescing operator use to check null
            //if the value of variable in the left operator is null RETURN the value of the right operator.
            var activity = await context.Activities
                .FindAsync([request.Id], cancellationToken) ?? throw new Exception("Activity Not Found.");
            return activity;
        }
    }

}
