# Copilot Training - .NET 8 + Entity Framework Core.
-------
Aplicacion Web API de demostración para implementaciones de Gihub Copilot enfocadas en el framework .NET 8.

- Crear y configurar un nuevo proyecto dotnet api.
- Configurar conexion a bases de datos.
- Aplicacion del patron repositorio.
- Configuracion de metodos CRUD y Controladores REST.

## Paso 1: Crear un proyecto .NET API utilizando Github Copilot CLI.
-------

1. Consultar a copilot CLI el comando para generar un nuevo proyecto .NET API
```powershell
ghcs "How to create a .NET API project"
```
_Respuesta Copilot CLI_:
```powershell
Suggestion:

  dotnet new webapi -n YourProjectName

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
dotnet new webapi -n Marena.API
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

