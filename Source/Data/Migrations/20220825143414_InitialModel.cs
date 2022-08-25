using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Source.Data.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DurationFactor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seconds = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurationFactor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapAppearance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapAppearance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LineAppearance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    Color_Red = table.Column<float>(type: "real", nullable: false),
                    Color_Green = table.Column<float>(type: "real", nullable: false),
                    Color_Blue = table.Column<float>(type: "real", nullable: false),
                    Color_Alpha = table.Column<float>(type: "real", nullable: false),
                    MapAppearanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineAppearance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineAppearance_MapAppearance_MapAppearanceId",
                        column: x => x.MapAppearanceId,
                        principalTable: "MapAppearance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RailwayAppearances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RailwayId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapAppearanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RailwayAppearances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RailwayAppearances_MapAppearance_MapAppearanceId",
                        column: x => x.MapAppearanceId,
                        principalTable: "MapAppearance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StationAppearance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    Position_Left = table.Column<float>(type: "real", nullable: false),
                    Position_Top = table.Column<float>(type: "real", nullable: false),
                    Caption_Offset_Left = table.Column<float>(type: "real", nullable: false),
                    Caption_Offset_Top = table.Column<float>(type: "real", nullable: false),
                    Caption_TextAligin = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    MapAppearanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationAppearance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StationAppearance_MapAppearance_MapAppearanceId",
                        column: x => x.MapAppearanceId,
                        principalTable: "MapAppearance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lines_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    MapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stations_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Railways",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    DurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Railways", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Railways_DurationFactor_DurationId",
                        column: x => x.DurationId,
                        principalTable: "DurationFactor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Railways_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Railways_Stations_FromId",
                        column: x => x.FromId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Railways_Stations_ToId",
                        column: x => x.ToId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transfer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    DurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfer_DurationFactor_DurationId",
                        column: x => x.DurationId,
                        principalTable: "DurationFactor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfer_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transfer_Stations_FromId",
                        column: x => x.FromId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfer_Stations_ToId",
                        column: x => x.ToId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineAppearance_LineId",
                table: "LineAppearance",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_LineAppearance_MapAppearanceId",
                table: "LineAppearance",
                column: "MapAppearanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_MapId",
                table: "Lines",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_MapAppearance_MapId",
                table: "MapAppearance",
                column: "MapId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RailwayAppearances_MapAppearanceId",
                table: "RailwayAppearances",
                column: "MapAppearanceId");

            migrationBuilder.CreateIndex(
                name: "IX_RailwayAppearances_RailwayId",
                table: "RailwayAppearances",
                column: "RailwayId");

            migrationBuilder.CreateIndex(
                name: "IX_Railways_DurationId",
                table: "Railways",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Railways_FromId",
                table: "Railways",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Railways_MapId",
                table: "Railways",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Railways_ToId",
                table: "Railways",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_StationAppearance_MapAppearanceId",
                table: "StationAppearance",
                column: "MapAppearanceId");

            migrationBuilder.CreateIndex(
                name: "IX_StationAppearance_StationId",
                table: "StationAppearance",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_LineId",
                table: "Stations",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_MapId",
                table: "Stations",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_DurationId",
                table: "Transfer",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_FromId",
                table: "Transfer",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_MapId",
                table: "Transfer",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_ToId",
                table: "Transfer",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineAppearance");

            migrationBuilder.DropTable(
                name: "RailwayAppearances");

            migrationBuilder.DropTable(
                name: "Railways");

            migrationBuilder.DropTable(
                name: "StationAppearance");

            migrationBuilder.DropTable(
                name: "Transfer");

            migrationBuilder.DropTable(
                name: "MapAppearance");

            migrationBuilder.DropTable(
                name: "DurationFactor");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "Maps");
        }
    }
}
