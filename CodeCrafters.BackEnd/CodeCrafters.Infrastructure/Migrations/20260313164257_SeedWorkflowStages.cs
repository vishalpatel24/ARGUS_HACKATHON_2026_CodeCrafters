using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeCrafters.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedWorkflowStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GrantWorkflowStages",
                columns: new[] { "Id", "AssigneeRole", "CreatedAt", "CreatedBy", "GrantTypeId", "IsDeleted", "Name", "OrderIdx", "RequiredReviewers", "SlaDays", "SlaType", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("c1111111-0001-0001-0001-111111111111"), "System", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Submitted", 1, 0, null, "Instant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0002-111111111111"), "AI Agent", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Eligibility Screening", 2, 0, 1, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0003-111111111111"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Under Review", 3, 1, 14, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0004-111111111111"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Award Decision", 4, 0, 7, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0005-111111111111"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Agreement Sent", 5, 0, 5, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0006-111111111111"), "Applicant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Active Grant", 6, 0, null, "N/A", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0007-111111111111"), "Applicant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Reporting", 7, 0, 30, "Calendar Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("c1111111-0001-0001-0008-111111111111"), "System", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a1111111-1111-1111-1111-111111111111"), false, "Closed", 8, 0, null, "Instant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0001-222222222222"), "System", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Submitted", 1, 0, null, "Instant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0002-222222222222"), "AI Agent", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Eligibility Screening", 2, 0, 1, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0003-222222222222"), "Grant Reviewer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Under Review", 3, 2, 21, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0004-222222222222"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Award Decision", 4, 0, 7, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0005-222222222222"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Agreement Sent", 5, 0, 5, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0006-222222222222"), "Applicant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Active Grant", 6, 0, null, "N/A", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0007-222222222222"), "Applicant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Reporting", 7, 0, 30, "Calendar Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("e2222222-0002-0002-0008-222222222222"), "System", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a2222222-2222-2222-2222-222222222222"), false, "Closed", 8, 0, null, "Instant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0001-333333333333"), "System", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Submitted", 1, 0, null, "Instant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0002-333333333333"), "AI Agent", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Eligibility Screening", 2, 0, 1, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0003-333333333333"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Under Review", 3, 1, 14, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0004-333333333333"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Award Decision", 4, 0, 7, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0005-333333333333"), "Program Officer", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Agreement Sent", 5, 0, 5, "Working Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0006-333333333333"), "Applicant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Active Grant", 6, 0, null, "N/A", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0007-333333333333"), "Applicant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Reporting", 7, 0, 30, "Calendar Days", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null },
                    { new Guid("f3333333-0003-0003-0008-333333333333"), "System", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("a3333333-3333-3333-3333-333333333333"), false, "Closed", 8, 0, null, "Instant", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0001-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0002-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0003-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0004-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0005-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0006-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0007-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-0001-0001-0008-111111111111"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0001-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0002-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0003-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0004-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0005-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0006-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0007-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-0002-0002-0008-222222222222"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0001-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0002-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0003-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0004-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0005-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0006-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0007-333333333333"));

            migrationBuilder.DeleteData(
                table: "GrantWorkflowStages",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-0003-0003-0008-333333333333"));
        }
    }
}
