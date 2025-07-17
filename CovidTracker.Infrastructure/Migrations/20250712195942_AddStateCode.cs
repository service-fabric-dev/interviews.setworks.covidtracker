using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CovidTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStateCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                table: "StateStats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateCode",
                table: "StateStats");
        }
    }
}
