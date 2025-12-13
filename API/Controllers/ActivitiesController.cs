using System;
using Application.Activities.Queries;
using Application.Activities.Commands;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Activities.DTOs;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities()
    {
        return await Mediator.Send(new GetActivityList.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetail(string id)
    {
        throw new Exception("Server test error");
        return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
    {
        return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
    }

    [HttpPut]
    public async Task<ActionResult> EditActivity(Activity activity)
    {
        //await Mediator.Send(new EditActivity.Command { Activity = activity }) return value type Result Unit
        //You can also leave with type of controller is Task<ActionResult<Unit>> but user will get a body with {} -empty object
        //This can't give valuable for user. => just return Task<ActionResult>
        return HandleResult(await Mediator.Send(new EditActivity.Command { Activity = activity }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActivity(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
    }

}
