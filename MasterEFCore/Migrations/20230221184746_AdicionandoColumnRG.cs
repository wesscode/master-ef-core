using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterEFCore.Migrations
{
    public partial class AdicionandoColumnRG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RG",
                table: "employees");
        }
    }
}
