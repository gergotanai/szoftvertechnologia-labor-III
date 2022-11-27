using EntityFrameworkRecursiveCTE.Data;
using EntityFrameworkRecursiveCTE.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace EntityFrameworkRecursiveCTE
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new RecursiveCTEContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Database.Migrate();

            void AddBranch(Tree tree, TreeNode node, Employee employee)
            {
                var current = node == null
                    ? tree.AddNode($"[yellow]{employee.Name} ({employee.Title})[/]")
                    : node.AddNode($"[blue]{employee.Name}({employee.Title})[/]");

                if (employee.Reports == null)
                    return;

                foreach (var report in employee.Reports)
                {
                    AddBranch(tree, current, report);
                }
            }

            while (true)
            {
                AnsiConsole.Write("Please enter Employee #: ");
                var value = Console.ReadLine();

                if (!int.TryParse(value, out var id))
                {
                    Console.WriteLine($"Employee {value} not found.");
                    continue;
                }

                var org = db.AllReports(id)
                    // Example filtering
                    //.AsEnumerable()
                    //.Where(e => e.Name.Contains("Homer"))
                    .ToList();

                var tree = new Tree("Springfield Nuclear Power Plant");

                if (org.Any())
                {
                    AddBranch(tree, null, org.First());
                    AnsiConsole.Write(tree);
                }
                else
                {
                    Console.WriteLine($"Employee {value} not found.");
                }
            }
        }
    }
}