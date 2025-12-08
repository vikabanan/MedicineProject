using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineProject.Migrations
{
    /// <inheritdoc />
    public partial class AddReferralFieldsToVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreviousVisitId",
                table: "Visits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralType",
                table: "Visits",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousVisitId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "ReferralType",
                table: "Visits");
        }
    }
}
