using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soft20181_starter.Migrations
{
    /// <inheritdoc />
    public partial class MyEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_userEventRegistrations",
                table: "userEventRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_userEventRegistrations_UserId",
                table: "userEventRegistrations");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "userEventRegistrations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userEventRegistrations",
                table: "userEventRegistrations",
                columns: new[] { "UserId", "EventId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_userEventRegistrations",
                table: "userEventRegistrations");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "userEventRegistrations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userEventRegistrations",
                table: "userEventRegistrations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_userEventRegistrations_UserId",
                table: "userEventRegistrations",
                column: "UserId");
        }
    }
}
