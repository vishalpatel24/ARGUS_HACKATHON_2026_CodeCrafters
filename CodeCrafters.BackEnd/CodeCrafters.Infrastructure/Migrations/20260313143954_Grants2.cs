using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodeCrafters.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Grants2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrantDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantDocuments_GrantTypes_GrantTypeId",
                        column: x => x.GrantTypeId,
                        principalTable: "GrantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrantWorkflowStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderIdx = table.Column<int>(type: "int", nullable: false),
                    AssigneeRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequiredReviewers = table.Column<int>(type: "int", nullable: false),
                    SlaDays = table.Column<int>(type: "int", nullable: true),
                    SlaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrantWorkflowStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrantWorkflowStages_GrantTypes_GrantTypeId",
                        column: x => x.GrantTypeId,
                        principalTable: "GrantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GrantTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubmissionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CurrentStageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageEnteredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StatusLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganisationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfEstablishment = table.Column<int>(type: "int", nullable: false),
                    StateOfRegistration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnnualOperatingBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonnelCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EquipmentAndMaterials = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TravelAndLogistics = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrainingAndWorkshops = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TechnologySoftware = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContentDevelopment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaplingsAndSeeds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommunityEngagementWages = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Overheads = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetJustification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorisedSignatoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProblemStatement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedSolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetBeneficiariesCount = table.Column<int>(type: "int", nullable: true),
                    BeneficiaryDemographics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyActivitiesList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedOutcomes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelevantPriorProjectsDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasReceivedGrantsBefore = table.Column<bool>(type: "bit", nullable: true),
                    PriorFunderNameAndAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnovationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProblemBeingSolved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnovationDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetSchoolsDistricts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfSchoolsTargeted = table.Column<int>(type: "int", nullable: true),
                    NumberOfStudentsToBenefit = table.Column<int>(type: "int", nullable: true),
                    GradeLevelsTargeted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnologyToolsUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvidenceBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectLeadNameAndQuals = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamSize = table.Column<int>(type: "int", nullable: true),
                    TeamExpertiseDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyPartners = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryLearningOutcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaselineAssessmentPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyMilestones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThematicArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnvironmentalProblemDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedIntervention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeographicCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectBeneficiariesCount = table.Column<int>(type: "int", nullable: true),
                    CommunityInvolvementPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnvironmentalIndicators = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClimateVulnerabilityContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskOfNotActing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectLeadNameAndExpertise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GovernmentPartnerships = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityPlanForFirst6Months = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_GrantTypes_GrantTypeId",
                        column: x => x.GrantTypeId,
                        principalTable: "GrantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_GrantWorkflowStages_CurrentStageId",
                        column: x => x.CurrentStageId,
                        principalTable: "GrantWorkflowStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AiSuggestedScore = table.Column<int>(type: "int", nullable: true),
                    FinalScore = table.Column<int>(type: "int", nullable: true),
                    ReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationReviews_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationReviews_Users_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationWorkflowHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransitionedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TriggeredByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAiTriggered = table.Column<bool>(type: "bit", nullable: false),
                    StatusLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationWorkflowHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowHistories_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowHistories_GrantWorkflowStages_StageId",
                        column: x => x.StageId,
                        principalTable: "GrantWorkflowStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationWorkflowHistories_Users_TriggeredByUserId",
                        column: x => x.TriggeredByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "GrantDocuments",
                columns: new[] { "Id", "Description", "DisplayOrder", "DocumentName", "GrantTypeId", "IsMandatory" },
                values: new object[,]
                {
                    { new Guid("b1111111-1111-1111-1111-111111111111"), null, 1, "Organization Registration Certificate", new Guid("a1111111-1111-1111-1111-111111111111"), true },
                    { new Guid("b1111111-1111-1111-1111-222222222222"), null, 2, "Latest Audited Financial Statements", new Guid("a1111111-1111-1111-1111-111111111111"), true },
                    { new Guid("b1111111-1111-1111-1111-333333333333"), null, 3, "Project Proposal Document", new Guid("a1111111-1111-1111-1111-111111111111"), true },
                    { new Guid("b1111111-1111-1111-1111-444444444444"), null, 4, "Implementation Plan", new Guid("a1111111-1111-1111-1111-111111111111"), true },
                    { new Guid("b1111111-1111-1111-1111-555555555555"), null, 5, "Budget Breakdown", new Guid("a1111111-1111-1111-1111-111111111111"), true },
                    { new Guid("b1111111-1111-1111-1111-666666666666"), null, 6, "Letters of Community Support", new Guid("a1111111-1111-1111-1111-111111111111"), false },
                    { new Guid("b2222222-2222-2222-2222-111111111111"), null, 1, "Organization Registration Certificate", new Guid("a2222222-2222-2222-2222-222222222222"), true },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), null, 2, "Project Concept Note", new Guid("a2222222-2222-2222-2222-222222222222"), true },
                    { new Guid("b2222222-2222-2222-2222-333333333333"), null, 3, "Detailed Project Proposal", new Guid("a2222222-2222-2222-2222-222222222222"), true },
                    { new Guid("b2222222-2222-2222-2222-444444444444"), null, 4, "Technology Solution Description", new Guid("a2222222-2222-2222-2222-222222222222"), true },
                    { new Guid("b2222222-2222-2222-2222-555555555555"), null, 5, "Budget Plan", new Guid("a2222222-2222-2222-2222-222222222222"), true },
                    { new Guid("b2222222-2222-2222-2222-666666666666"), null, 6, "Past Impact Evidence", new Guid("a2222222-2222-2222-2222-222222222222"), false },
                    { new Guid("b3333333-3333-3333-3333-111111111111"), null, 1, "Organization Registration Certificate", new Guid("a3333333-3333-3333-3333-333333333333"), true },
                    { new Guid("b3333333-3333-3333-3333-222222222222"), null, 2, "Environmental Impact Description", new Guid("a3333333-3333-3333-3333-333333333333"), true },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), null, 3, "Project Proposal", new Guid("a3333333-3333-3333-3333-333333333333"), true },
                    { new Guid("b3333333-3333-3333-3333-444444444444"), null, 4, "Implementation Timeline", new Guid("a3333333-3333-3333-3333-333333333333"), true },
                    { new Guid("b3333333-3333-3333-3333-555555555555"), null, 5, "Budget Plan", new Guid("a3333333-3333-3333-3333-333333333333"), true },
                    { new Guid("b3333333-3333-3333-3333-666666666666"), null, 6, "Community Partnership Letters", new Guid("a3333333-3333-3333-3333-333333333333"), false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationReviews_ApplicationId",
                table: "ApplicationReviews",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationReviews_ReviewerId",
                table: "ApplicationReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantId",
                table: "Applications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CurrentStageId",
                table: "Applications",
                column: "CurrentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_GrantTypeId",
                table: "Applications",
                column: "GrantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowHistories_ApplicationId",
                table: "ApplicationWorkflowHistories",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowHistories_StageId",
                table: "ApplicationWorkflowHistories",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationWorkflowHistories_TriggeredByUserId",
                table: "ApplicationWorkflowHistories",
                column: "TriggeredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantDocuments_GrantTypeId",
                table: "GrantDocuments",
                column: "GrantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrantWorkflowStages_GrantTypeId",
                table: "GrantWorkflowStages",
                column: "GrantTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationReviews");

            migrationBuilder.DropTable(
                name: "ApplicationWorkflowHistories");

            migrationBuilder.DropTable(
                name: "GrantDocuments");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "GrantWorkflowStages");
        }
    }
}
