using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestTask.Core.Dto;
using TestTask.Core.Services.Interfaces;

namespace TestTask.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    public PatientsController(IPatientsService patientsService) => _patientsService = patientsService;

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Получить пациента по Id", Description = "Возвращает одного пациента по идентификатору.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPatientDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetPatientDto>> GetPatientAsync([FromRoute] Guid id)
    {
        var result = await _patientsService.GetPatientAsync(id);
        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Получить всех пациентов", Description = "Возвращает список всех пациентов с именами.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GetPatientDto>>> GetPatients()
    {
        var result = await _patientsService.GetAllPatientsIncludeNamesAsync();
        return Ok(result);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Поиск пациентов по дате рождения", Description = "Позволяет искать пациентов по одной или нескольким датам рождения.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GetPatientDto>>> SearchPatients([FromQuery(Name = "birthDate")] List<string> dates)
    {
        var result = await _patientsService.SearchPatientsByBirthDateAsync(dates);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Создать нового пациента", Description = "Создаёт одного пациента.")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> CreatePatient([FromBody] CreateOrUpdatePatientDto patient)
    {
        var result = await _patientsService.CreatePatientAsync(patient);
        return Created(new Uri(Request.Path, UriKind.Relative), result);
    }

    [HttpPost("batch")]
    [SwaggerOperation(Summary = "Массовое создание пациентов", Description = "Создаёт сразу несколько пациентов.")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<Guid>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Guid>>> CreatePatients([FromBody] IEnumerable<CreateOrUpdatePatientDto> patients)
    {
        var result = await _patientsService.CreatePatientsAsync(patients);
        return Created(new Uri(Request.Path, UriKind.Relative), result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Удалить пациента", Description = "Удаляет пациента по Id.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemovePatient([FromRoute] Guid id)
    {
        await _patientsService.DeletePatientAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Полное обновление пациента", Description = "Заменяет все поля пациента новыми данными.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdatePatient([FromRoute] Guid id, [FromBody] CreateOrUpdatePatientDto patient)
    {
        await _patientsService.UpdatePatientAsync(id, patient);
        return Ok();
    }

    [HttpPatch("{id}")]
    [SwaggerOperation(Summary = "Частичное обновление пациента", 
        Description = "Позволяет обновлять только изменённые поля пациента с использованием [JSON Patch](https://datatracker.ietf.org/doc/html/rfc6902).")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/json-patch+json")]
    public async Task<ActionResult> PatchPatient([FromRoute] Guid id, [FromBody] JsonPatchDocument<PatchPatientDto> patch)
    {
        //по-хорошему, тут бы медиатр какой-то, но ради пары строк кода еще абстракцию накидывать...
        var patient = await _patientsService.GetPatchPatientDtoAsync(id);

        patch.ApplyTo(patient);

        await _patientsService.PatchPatientAsync(id, patient);
        return Ok();
    }
}
