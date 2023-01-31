﻿using Microsoft.AspNetCore.Mvc;
using Sample.API.Entity;
using Sample.API.Service;
using System.Net.Mime;

namespace Sample.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PeopleController : ControllerBase
{
    private readonly ILogger<PeopleController> logger;
    private readonly IPeopleService peopleService;

    public PeopleController(ILogger<PeopleController> logger, IPeopleService peopleService)
    {
        this.logger = logger;
        this.peopleService = peopleService;
    }

    [HttpGet("people")]
    public async Task<IActionResult> GetPeople()
    {
        var people = await peopleService.GetPeopleAsync();
        return Ok(people);
    }

    [HttpGet("person/{id}")]
    public async Task<IActionResult> GetPerson(int id)
    {
        var person = await peopleService.GetPersonAsync(id);
        return Ok(person);
    }

    [HttpPost("person")]
    public async Task<IActionResult> CreatePerson([FromBody] PersonEntity person)
    {
        await peopleService.CreatePersonAsync(person);
        return CreatedAtAction("GetPerson", new { id = person.Id }, person);
    }

    [HttpPut("person")]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonEntity person)
    {
        await peopleService.UpdatePersonAsync(person);
        return Ok();
    }

    [HttpDelete("person/{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var person = await peopleService.GetPersonAsync(id);

        if (person == null)
        {
            return NotFound();
        }

        await peopleService.DeletePersonAsync(person);
        return Ok();
    }
}