using EntityFrameworkRecursiveCTE.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkRecursiveCTE.Migrations
{
    [DbContext(typeof(RecursiveCTEContext))]
    [Migration("00000000000000_CreateAllReportsStoredProcedure")]
    partial class CreateAllReportsStoredProcedure
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {

        }
    }
}
