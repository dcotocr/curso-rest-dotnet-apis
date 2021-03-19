## Lab: SQL Server (Route-to-Code)

1) Crear el lab desde el link: https://github.com/stvansolano/codebox-dotnet
2) Darle el nombre del lab (ejemplo): curso-rest-apis-lab4	
3) Copiar su link de estudiante del lab y clonarlo localmente con VS Code
4) Agregar un MapGet (route-to-code) al DotNetWebApi
5) Agregamos la configuracion para correr en modo=Development desde VS Code (Run & Debug)
6) Asegurarse de levantar el Docker Desktop
7) Desde VS Code abrir la extension de Docker y revisar los contenedores (1 contenedor)
8) Desde la terminal, correr el comando desde la ruta de carpeta: docker-compose up
9) Desde VS Code nos conectamos al SQL Server de Docker (localhost, 1433 AdventureWorks)
10) Desde la carpeta /db correr el stored procedure: sp_HelloWorld.sql
11) Agregar el paquete de System.Data.SqlClient (/DotNetWebApi): 
		dotnet add package System.Data.SqlClient --version 4.8.2

13) Correr el API en el endpoint http://localhost:5000/db/hello

## Codigo fuente a utilizar
using System.Threading.Tasks;
using System.Data.SqlClient;

// Program
        e.MapGet("db/hello", async context => {
            await Task.Delay(3000);
            
            var adventureWorks = "data source=localhost,1433;initial catalog=Adventureworks;persist security info=True;user id=sa;password=Password.123;MultipleActiveResultSets=True;";

            using (var connection = new SqlConnection(adventureWorks))
            {
                SqlCommand command = new SqlCommand("EXEC [dbo].[sp_HelloWorld]", connection);
                command.Connection.Open();
                var helloDb = command.ExecuteScalar() as string;

                context.Response.StatusCode = 200;

                await context.Response.WriteAsync(helloDb);
            }
        });
        
 -- Stored procedure -
DROP PROCEDURE IF EXISTS [dbo].[sp_HelloWorld]
GO
CREATE PROCEDURE sp_HelloWorld
AS
BEGIN
    declare @var1 nvarchar(max) = 'Hello, World! DB';

    SELECT @var1 as Result
    PRINT @var1;
END

--DECLARE @RC int
--EXECUTE @RC = [dbo].[sp_HelloWorld] 
--GO
