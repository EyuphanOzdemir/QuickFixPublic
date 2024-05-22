using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "EmailLogs",
                newName: "Sender");

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "EmailLogs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "EmailLogs",
                newName: "Email");
        }
    }
}
