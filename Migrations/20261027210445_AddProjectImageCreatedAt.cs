using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Al_Shami_ِAISS_.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectImageCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectImages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectImages");
        }
    }
}
