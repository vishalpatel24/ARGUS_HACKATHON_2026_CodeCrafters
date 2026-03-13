using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeCrafters.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddYearOfEstablishmentAndStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOfEstablishment",
                table: "Organisations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DocumentVaultItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StoredFilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVaultItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentVaultItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverallResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HardChecksPassed = table.Column<int>(type: "int", nullable: false),
                    HardChecksFailed = table.Column<int>(type: "int", nullable: false),
                    SoftFlags = table.Column<int>(type: "int", nullable: false),
                    OfficerAction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OfficerActionReason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReviewedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreeningReports_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreeningReports_Users_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScreeningChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScreeningReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CheckName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CheckType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AiScore = table.Column<int>(type: "int", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreeningChecks_ScreeningReports_ScreeningReportId",
                        column: x => x.ScreeningReportId,
                        principalTable: "ScreeningReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GrantWorkflowStages",
                columns: new[] { "Id", "AssigneeRole", "CreatedAt", "CreatedBy", "GrantTypeId", "IsDeleted", "Name", "OrderIdx", "RequiredReviewers", "SlaDays", "SlaType", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("eeeee111-1111-1111-1111-000000000100"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Submitted", 1, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000101"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Eligibility Screening", 2, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000102"), "Grant Reviewer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Under Review", 3, 2, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000103"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Award Decision", 4, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000104"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Agreement Sent", 5, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000105"), "Finance Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Active Grant", 6, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000106"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Reporting", 7, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000107"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Closed", 8, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000200"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Submitted", 1, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000201"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Eligibility Screening", 2, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000202"), "Grant Reviewer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Under Review", 3, 2, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000203"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Award Decision", 4, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000204"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Agreement Sent", 5, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000205"), "Finance Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Active Grant", 6, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000206"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Reporting", 7, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000207"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Closed", 8, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000300"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Submitted", 1, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000301"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Eligibility Screening", 2, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000302"), "Grant Reviewer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Under Review", 3, 2, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000303"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Award Decision", 4, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000304"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Agreement Sent", 5, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000305"), "Finance Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Active Grant", 6, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000306"), "Program Officer", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Reporting", 7, 0, 7, "Working Days", null, null },
                    { new Guid("eeeee111-1111-1111-1111-000000000307"), "Grant Admin", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Closed", 8, 0, 7, "Working Days", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVaultItems_UserId_DocumentType",
                table: "DocumentVaultItems",
                columns: new[] { "UserId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningChecks_ScreeningReportId",
                table: "ScreeningChecks",
                column: "ScreeningReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningReports_ApplicationId",
                table: "ScreeningReports",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningReports_ReviewedByUserId",
                table: "ScreeningReports",
                column: "ReviewedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentVaultItems");

            migrationBuilder.DropTable(
                name: "ScreeningChecks");

            migrationBuilder.DropTable(
                name: "ScreeningReports");

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000100"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000101"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000102"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000103"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000104"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000105"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000106"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000107"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000200"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000201"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000202"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000203"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000204"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000205"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000206"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000207"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000300"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000301"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000302"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000303"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000304"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000305"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000306"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("eeeee111-1111-1111-1111-000000000307"));

            migrationBuilder.DropColumn(
                name: "YearOfEstablishment",
                table: "Organisations");
        }
    }
}
