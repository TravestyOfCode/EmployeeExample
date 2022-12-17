using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeExample.Migrations
{
    public partial class AddsEmployeedatamodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    State = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    WorkEmail = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    PersonalPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    FilingStatus = table.Column<int>(type: "int", nullable: false),
                    HasTwoJobs = table.Column<bool>(type: "bit", nullable: false),
                    ClaimDependantAndOtherCreditsAmount = table.Column<int>(type: "int", nullable: false),
                    OtherIncomeAmount = table.Column<int>(type: "int", nullable: false),
                    DeductionAmount = table.Column<int>(type: "int", nullable: false),
                    ExtraWitholdingAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
