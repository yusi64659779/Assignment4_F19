using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment4_NASA.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "apod",
                columns: table => new
                {
                    date = table.Column<string>(nullable: false),
                    explanation = table.Column<string>(nullable: true),
                    hdurl = table.Column<string>(nullable: true),
                    media_type = table.Column<string>(nullable: true),
                    service_version = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apod", x => x.date);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apod");
        }
    }
}
