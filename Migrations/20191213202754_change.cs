using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamC_.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "acitivites",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Time",
                table: "acitivites",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
