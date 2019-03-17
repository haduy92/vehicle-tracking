using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleTracking.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    TableName = table.Column<string>(maxLength: 50, nullable: true),
                    KeyValues = table.Column<string>(nullable: true),
                    OldValues = table.Column<string>(nullable: true),
                    NewValues = table.Column<string>(nullable: true),
                    Action = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingRecordSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    VehicleId = table.Column<Guid>(nullable: false),
                    RecordedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingRecordSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(maxLength: 64, nullable: true),
                    PasswordSalt = table.Column<byte[]>(maxLength: 128, nullable: true),
                    Token = table.Column<string>(nullable: true),
                    StreetAddress1 = table.Column<string>(maxLength: 60, nullable: true),
                    StreetAddress2 = table.Column<string>(maxLength: 60, nullable: true),
                    StreetAddress3 = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 60, nullable: true),
                    State = table.Column<string>(maxLength: 60, nullable: true),
                    Country = table.Column<string>(maxLength: 60, nullable: true),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    VehicleCode = table.Column<string>(maxLength: 50, nullable: true),
                    DeviceCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ExtendedData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    VehicleCode = table.Column<string>(maxLength: 50, nullable: true),
                    DeviceCode = table.Column<string>(maxLength: 50, nullable: true),
                    Latitude = table.Column<string>(maxLength: 50, nullable: true),
                    Longitude = table.Column<string>(maxLength: 50, nullable: true),
                    RecordedDate = table.Column<DateTime>(nullable: false),
                    TrackingRecordSnapshotId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingRecords_TrackingRecordSnapshots_TrackingRecordSnaps~",
                        column: x => x.TrackingRecordSnapshotId,
                        principalTable: "TrackingRecordSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingRecords_TrackingRecordSnapshotId",
                table: "TrackingRecords",
                column: "TrackingRecordSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DeviceCode",
                table: "Vehicles",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleCode",
                table: "Vehicles",
                column: "VehicleCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "TrackingRecords");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "TrackingRecordSnapshots");
        }
    }
}
