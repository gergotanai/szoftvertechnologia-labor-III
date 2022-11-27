using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkRecursiveCTE.Migrations
{
    public partial class CreateAllReportsStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"CREATE PROCEDURE AllReports
                @Id INT
            AS
            WITH OrganizationalChart (Id, Name, Title, ManagerId, Below) AS (
                SELECT Id, Name, Title, ManagerId, 0
                FROM dbo.Employees
                WHERE Employees.Id = @Id
                UNION ALL
                SELECT e.Id, e.Name, e.Title, e.ManagerId, o.Below + 1
                FROM dbo.Employees AS e
                INNER JOIN OrganizationalChart AS o ON e.ManagerId = o.Id
            ) SELECT * from OrganizationalChart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
