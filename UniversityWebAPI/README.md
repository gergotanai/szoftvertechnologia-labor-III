## dotnet-aspnet-codegenerator

dotnet-aspnet-codegenerator is a global tool that must be installed. The following command installs the latest stable version of the dotnet-aspnet-codegenerator tool:

```
dotnet tool install -g dotnet-aspnet-codegenerator
```

<hr>

## Generating controllers

**Use namespaces for the input elements!**  
General exmaple:

```
dotnet-aspnet-codegenerator controller --no-build --force --restWithNoViews --model Namespace.ModelName --dataContext Namespace.DataContextName --controllerName ControllerName --relativeFolderPath Controllers
```

Concrete example for **ClassroomsController**:

```
dotnet-aspnet-codegenerator controller --no-build --force --restWithNoViews --model UniversityWebAPI.Models.Classroom --dataContext UniversityWebAPI.Data.UniversityContext --controllerName ClassroomsController --relativeFolderPath Controllers
```

- `--no-build` - Doesn't build the project before running. It also implicitly sets the `--no-restore` flag.
- `--force` or `-f` - Overwrite existing files.
- `--restWithNoViews` or `-api` - Generate a Controller with REST style API. noViews is assumed and any view related options are ignored.
- `--model` or `-m` - Model class to use.
- `--dataContext` or `-dc` - The DbContext class to use or the name of the class to generate.
- `--controllerName` or `-name` - Name of the controller.
- `--relativeFolderPath` or `-outDir` - Specify the relative output folder path from project where the file needs to be generated, if not specified, file will be generated in the project folder.

> `dotnet restore` - Restores the dependencies and tools of a project.
>
> Running the command with the `--restWithNoViews` argument it will use the `ApiControllerWithContext.cshtml` template file included in the nuget package.
>
> If you want to customize the template files just put them under the local `Templates\ControllerGenerator` folder and dotnet-aspnet-codegenerator will use them instead of the nuget package ones.
