using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TheCrawler.Lib.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectionSource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Host = table.Column<string>(nullable: true),
                    WhenAdded = table.Column<DateTime>(nullable: false),
                    WhenCollected = table.Column<DateTime>(nullable: false),
                    PagesCollectedCount = table.Column<int>(nullable: false),
                    EnabledForCollecting = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourcePages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(nullable: true),
                    WhenAdded = table.Column<DateTime>(nullable: false),
                    WhenCollected = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Html = table.Column<string>(nullable: true),
                    SourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourcePages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourcePages_CollectionSource_SourceId",
                        column: x => x.SourceId,
                        principalTable: "CollectionSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SourcePages_SourceId",
                table: "SourcePages",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourcePages");

            migrationBuilder.DropTable(
                name: "CollectionSource");
        }
    }
}
