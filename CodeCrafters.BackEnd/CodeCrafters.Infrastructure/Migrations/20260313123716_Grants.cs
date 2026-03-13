using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeCrafters.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Grants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    FundingMinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FundingMaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectDurationMinMonths = table.Column<int>(type: "int", nullable: false),
                    ProjectDurationMaxMonths = table.Column<int>(type: "int", nullable: false),
                    EligibleApplicants = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    GeographicFocus = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AnnualCycle = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MaximumAwardsPerCycle = table.Column<int>(type: "int", nullable: false),
                    TotalProgrammeBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GrantTypes",
                columns: new[] { "Id", "AnnualCycle", "Code", "EligibleApplicants", "FundingMaxAmount", "FundingMinAmount", "GeographicFocus", "MaximumAwardsPerCycle", "Name", "ProjectDurationMaxMonths", "ProjectDurationMinMonths", "Purpose", "TotalProgrammeBudget" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "Applications open April 1 – June 30 each year", "CDG", "Registered NGOs, Trusts, Section 8 Companies with minimum 2 years of operation", 2000000m, 200000m, "Rural and semi-urban areas in India", 10, "Community Development Grant", 18, 6, "Fund community-level infrastructure and social service projects", 20000000m },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), "Rolling applications — reviewed quarterly (Jan, Apr, Jul, Oct)", "EIG", "NGOs, EdTech non-profits, Research institutions, Universities (public or private)", 5000000m, 500000m, "Any state in India; preference for aspirational districts", 5, "Education Innovation Grant", 24, 12, "Fund technology-enabled or pedagogy-innovation projects improving learning outcomes in government schools", 25000000m },
                    { new Guid("a3333333-3333-3333-3333-333333333333"), "Applications open twice yearly: Jan 1–Feb 28 and Jul 1–Aug 31", "ECAG", "NGOs, Farmer Producer Organisations (FPOs), Panchayat bodies, Research institutions", 3000000m, 300000m, "India — priority given to climate-vulnerable districts", 15, "Environment & Climate Action Grant", 24, 6, "Fund grassroots environmental conservation, climate resilience, and clean energy access projects", 30000000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrantTypes_Code",
                table: "GrantTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrantTypes");
        }
    }
}
