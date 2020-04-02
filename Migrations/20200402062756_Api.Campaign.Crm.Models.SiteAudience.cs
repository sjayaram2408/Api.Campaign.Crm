using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Campaign.Crm.Migrations
{
    public partial class ApiCampaignCrmModelsSiteAudience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteAudiences",
                columns: table => new
                {
                    SiteAudienceId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<int>(nullable: false),
                    AudienceId = table.Column<long>(nullable: false),
                    RetentionDays = table.Column<int>(nullable: false),
                    AudienceName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAudiences", x => x.SiteAudienceId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteAudiences");
        }
    }
}
