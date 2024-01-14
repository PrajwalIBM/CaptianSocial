using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaptianSocial.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingtheReactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReactionType",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReactionType",
                table: "Reactions");
        }
    }
}
