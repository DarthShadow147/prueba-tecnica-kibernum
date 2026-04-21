# PROBLEMAS DE CODIGO LEGADO

## 1. Credenciales "Hardcodeadas"
```C#
string connStr = "Server=PROD-SERVER;Database=IntergalaxyDB;User Id=admin;Password=admin123;";
```

### Problemas
- Exposicion de credenciales
- No configurable

## 2. Riesgo de SQL Injection
```C#
string sql = "INSERT INTO Solicitudes (...) VALUES (" +
ddlPersonaje.SelectedValue + ", '" +
txtSolicitante.Text + "', '" +
txtEvento.Text + "', '" +
txtFechaEvento.Text + "', 0, '" +
DateTime.Now + "')";
```

### Problemas
- Vulnerable a ataques

## 3. Logica mezclada
Todo esta mezclado en una sola capa

- Acceso a datos
- Validaciones
- Logica de negocio
- UI

## 4. No hay manejo de errores
Si falla se rompe toda la ejecucion

# SOLUCION EN .NET 8

## CONTROLLER
```C#
[ApiController]
[Route("api/solicitudes")]
public class HiringController : ControllerBase
{
    private readonly IHiringService _Service;

    public HiringController(IHiringService Service)
    {
        _Service = Service;
    }

    /// <summary>
    /// Crea una nueva solicitud de contratación.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHiringRequestDto pRequest)
    {
        var Id = await _Service.CreateAsync(pRequest);
        return Ok(Id);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var lResult = await _Service.GetByIdAsync(id);
        return Ok(lResult);
    }
}
```

## SERVICE (LOGICA DE NEGOCIO)
```C#
public async Task<int> CreateAsync(CreateHiringRequestDto pRequest)
{
    _logger.LogInformation("Creating hiring request");

    var lExists = await _characterRepository.ExistsByExternalIdAsync(pRequest.CharacterId);

    if (!lExists)
        throw new ArgumentException("Character does not exist");

    var lEntity = new HiringRequest
    {
        CharacterId = pRequest.CharacterId,
        Applicant = pRequest.Applicant,
        Event = pRequest.Event,
        EventDate = pRequest.EventDate,
        Status = RequestStatus.PENDING,
        CreatedAt = DateTime.UtcNow
    };

    await _Repository.AddAsync(lEntity);
    await _Repository.SaveChangesAsync();

    return lEntity.Id;
}
```

## REPOSITORY (EF CORE)
```C#
public async Task AddAsync(HiringRequest pRequest)
{
    await _Context.HiringRequest.AddAsync(pRequest);
}
```

## VALIDATOR (FLUENT VALIDATION)
```C#
public class CreateHiringRequestValidator : AbstractValidator<CreateHiringRequestDto>
{
    public CreateHiringRequestValidator()
    {
        RuleFor(x => x.CharacterId).GreaterThan(0);
        RuleFor(x => x.Applicant).NotEmpty();
        RuleFor(x => x.Event).NotEmpty();
        RuleFor(x => x.EventDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Event date must be in the future");
    }
}
```


## EXPLICACION
Se han separado responsabilidades usando Clean Architecture, se elimino SQL Manual usando EF Core, se implementan validaciones con FluentValidation y se centralizo el manejo de errores con Middleware.