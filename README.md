# Copilot Training - .NET 8 + Entity Framework Core.

Aplicacion Web API de demostración para implementaciones de Gihub Copilot enfocadas en el framework .NET 8.
- Crear y configurar un nuevo proyecto dotnet api.
- Configurar conexion a bases de datos.
- Abstraccion de servicios
- Configuracion de metodos CRUD y Controladores REST.

## Paso 1: Crear un proyecto .NET API utilizando Github Copilot CLI.

1. Consultar a copilot CLI el comando para generar un nuevo proyecto .NET API
```powershell
ghcs "How to create a .NET API project"
```
_Respuesta Copilot CLI_:
```powershell
Suggestion:

  dotnet new webapi --use-controllers -n YourProjectName

? Select an option
> Copy command to clipboard
  Explain command
  Execute command
  Revise command
  Rate response
  exit
```

2. Seleccionamos la opcion `Copy command to clipboard` y cambiamos el nombre del proyecto por Marena.API
```powershell
dotnet new webapi --use-controllers -n Marena.API
```

3. Generamos un nuevo archivo de solucion llamado **Marena**
```powershell
dotnet new sln -n Marena
```

4. Usamos copilot CLI para agregar el proyecto Marena.API al archivo de solucion que hemos creado en el paso anterior.
```powershell
ghcs "How to add Marena.API project to Marena solution file"
```
_Respuesta Copilot CLI:_
```powershell
dotnet sln Marena.sln add Marena.API/Marena.API.csproj
```
5. Abrir archivo de solucion con **Visual Studio**.

_Estructura del proyecto obtenida:_

![VisualStudio_Project_Structure](assets/Project_Structure.JPG)

## Paso 2. Crear un contenedor Docker para ejecutar SQL Server.

1. Creamos un contenedor docker de SQL Server.
```powershell
ghcs "How can i run SQL Server in docker?"
```
- ` docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourPassword123' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest`
- Si no tienes Docker instalado en tu PC puedes instalarlo desde [aquí](https://www.docker.com/products/docker-desktop).
- Si Docker es una limitante, puedes instalar SQL Server Express desde [aquí](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).

2. Validar conexion con SQL Server.
> Puedes probar la conexion utilizando cualquier herramienta de gestion de bases de datos como SQL Server Management Studio, Azure Data Studio, DBeaver o Datagrip.

## Paso 3. Configurar Entity Framework Core.

1. Usando Copilot CLI, generamos los comandos a utilizar.
```powershell
ghcs "How to install and setup Entity Framework Core"
```
- `dotnet add package Microsoft.EntityFrameworkCore`
- `dotnet add package Microsoft.EntityFrameworkCore.Tools`
- `dotnet add package Microsoft.EntityFrameworkCore.Design`

## Paso 4. Configuramos el DBContext.

1. Usando Copilot Chat, solicitamos generar una clase que herede de DBContext.

- Create a class named `MarenaDBContext` taht inherits from `DBContext`
```C#
using Microsoft.EntityFrameworkCore;

public class MarenaDBContext : DbContext
{
    public MarenaDBContext(DbContextOptions<MarenaDBContext> options) : base(options)
    {
    }

    // Define DbSet properties for your entities here
    // public DbSet<YourEntity> YourEntities { get; set; }
}
```
2. Generamos una carpeta llamada `Persistence` y agregamos la clase `MarenaDBContext` dentro de esa carpeta.

3. Registramos la clase MarenaDBContext en los servicios cargados por el builder en el archivo Program.cs
- Register the `MarenaDBContext` in the `builder.services` method in the `Program.cs` file.
```C#
// Register MarenaDBContext with the dependency injection container
builder.Services.AddDbContext<MarenaDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```
4. Configuramos la cadena de conexion por defecto a la base de datos SQL Server.
- Add default connection string to `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=your_database_name;User Id=your_user_id;Password=your_password;"
  }
}
```
### Troubleshooting

#### UseSqlServer
Puede pasar que a estas alturas, se nos genere un error al momento de llamar al metodo `UseSqlServer`, utilizando Copilot podemos investigar la causa del error.
Para solventarlo realziamos lo siguiente:

- Seleccionamos el metodo que acabamos de agregar.
- Hacemos click derecho sobre la seleccion y seleccionamos la opcion "ask copilot"
- En la ventana de chat, usamos el comando `/explain`
- `/explain Why the UseSqlServer is highlighted as an error?`

Copilot nos menciona que hemos olvidado incorporar la referencia al paquete `Microsoft.EntityFrameworkCore.SqlServer`. Existen diversas formas de incorporar este paquete al proyecto, para esta ocasion usaremos la linea de comandos de `dotnet`
```powershell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

#### Trust Server Certificate.
- Add to connection string: TrustServerCertificate=True;

## Paso 5. Configurando Migraciones y Entidad Movies

1. Utilizando Copilot Chat preguntemos como generar una entidad llamada Movie, que contenga los siguientes atributos: Id, Name, Score, Genres, Year
- Generate an entity named Movie with the following attributes: `Id`, `Name`, `Score`, `Genres`, `Year`, use Entity Framework Core Schema validations

```C#
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Range(0, 10)]
    public double Score { get; set; }

    [Required]
    [MaxLength(200)]
    public string Genres { get; set; }

    [Range(1888, 2100)]
    public int Year { get; set; }
}
```
2. Agregamos manualmente el modelo dentro del DBContext que se ha definido anteriormente.
```C#
using Microsoft.EntityFrameworkCore;

public class MarenaDBContext : DbContext
{
    public MarenaDBContext(DbContextOptions<MarenaDBContext> options) : base(options)
    {
    }

    // Define DbSet properties for your entities here
    public DbSet<Movie> Movies { get; set; }
}
```

3. Con ayuda de Copilot CLI, solicitamos como generar las migraciones para crear la base de datos y la tabla Movie
```powershell
ghcs "What is the command to create migrations with Entity Framework Core?"
```
- `dotnet ef migrations add Initial_Migration`
- `dotnet ef database update`

## Paso 6. Agregamos el controlador para la entidad Movie

1. Empleando la herramienta de Copilot Chat, generaremos un controlador basado en la entidad `Movies` generada anteriormente.
- `Create a .NET API Controller for #Movie.Cs Entity` Notese, como acá usamos las referencias de archivos con la sintaxis "#[fileName]" en el chat.

```C#
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marena.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MarenaDBContext _context;

        public MoviesController(MarenaDBContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
```
- Agregamos este archivo con el nombre de `MoviesController.cs` bajo la carpeta de `Controllers`
- Ejecutamos la aplicacion directamente desde Visual Studio.
- Probamos el API mediante algun cliente como Postman, Insomnia o Swagger.
