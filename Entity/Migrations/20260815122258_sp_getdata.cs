using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class sp_getdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string data = "create procedure GetAllPerson as begin select*from Person end";
            migrationBuilder.Sql(data);
                             
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string data = "drop procedure GetAllPerson";
            migrationBuilder.Sql(data);
        }
    }
}
