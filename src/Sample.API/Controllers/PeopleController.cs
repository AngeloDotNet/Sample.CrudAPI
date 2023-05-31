namespace Sample.API.Controllers;

public class PeopleController : BaseController
{
    private readonly ILoggerService logger;
    private readonly IPeopleService peopleService;
    private readonly IValidation validation;

    public PeopleController(ILoggerService logger, IPeopleService peopleService, IValidation validation)
    {
        this.logger = logger;
        this.peopleService = peopleService;
        this.validation = validation;
    }

    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        var result = await peopleService.GetListItemAsync();

        return result.Count > 0
            ? Ok(new DefaultResponse(true, result))
            : throw new ExceptionResponse(HttpStatusCode.NotFound, 0, "NotFound", $"The people list is empty");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var result = await GetSinglePerson(id);

        return result != null
            ? Ok(new DefaultResponse(true, result))
            : throw new ExceptionResponse(HttpStatusCode.NotFound, 0, "NotFound", $"Person with id {id} not found");
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] PersonEntity person, [FromServices] IValidator<PersonEntity> validator)
    {
        await ValidationEntity(person, validator);

        if (await GetSinglePerson(person.Id) != null)
        {
            logger.SaveLogWarning($"Person with id {person.Id} already exists");
            throw new ExceptionResponse(HttpStatusCode.Conflict, 0, "Conflict", $"Person with id {person.Id} already exists");
        }

        await peopleService.CreateItemAsync(person);

        logger.SaveLogInformation($"Create person with id {person.Id}");
        return Ok(new DefaultResponse(true, person));
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonEntity person, [FromServices] IValidator<PersonEntity> validator)
    {
        await ValidationEntity(person, validator);

        await peopleService.UpdateItemAsync(person);

        logger.SaveLogInformation($"Update person with id {person.Id}");
        return Ok(new DefaultResponse(true, $"Update person with id {person.Id}"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var person = await GetSinglePerson(id);

        if (person == null)
        {
            logger.SaveLogWarning($"Person with id {id} not found");
            throw new ExceptionResponse(HttpStatusCode.NotFound, 0, "NotFound", $"Person with id {id} not found");
        }

        await peopleService.DeleteItemAsync(person);

        logger.SaveLogInformation($"Delete person with id {person.Id}");
        return Ok(new DefaultResponse(true, $"Delete person with id {person.Id}"));
    }

    private async Task<PersonEntity> GetSinglePerson(Guid id)
    {
        return await peopleService.GetItemAsync(id);
    }

    private async Task ValidationEntity<T>(T person, IValidator<T> validator) where T : class
    {
        var validationResult = await validator.ValidateAsync(person);

        if (!validationResult.IsValid)
        {
            logger.SaveLogError($"Validation not valid");
            throw new ExceptionResponse(HttpStatusCode.UnprocessableEntity, 0, "UnprocessableEntity", $"Validation not valid", validation.ProcessErrorList(validationResult));
        }
    }
}