using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AircraftStatuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "text", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftStatuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", maxLength: 128, nullable: false),
                    Location = table.Column<string>(type: "text", maxLength: 512, nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", maxLength: 16, nullable: false),
                    MailAdress = table.Column<string>(type: "text", maxLength: 96, nullable: false, defaultValue: "Undefined"),
                    Description = table.Column<string>(type: "text", maxLength: 1024, nullable: false, defaultValue: "No Description"),
                    Status = table.Column<string>(type: "text", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Surname = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Gender = table.Column<char>(type: "char(1)", nullable: false, defaultValue: 'U'),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    IsSuspended = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Created_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2025, 4, 3, 3, 45, 57, 780, DateTimeKind.Local).AddTicks(6659)),
                    Uptaded_at = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2025, 4, 3, 3, 45, 57, 780, DateTimeKind.Local).AddTicks(7069)),
                    CountryCode = table.Column<string>(type: "text", maxLength: 5, nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Surname = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Role = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<char>(type: "char(1)", nullable: false, defaultValue: 'U')
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LogLevels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<string>(type: "text", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogLevels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Last_Maintenance_Date = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Fuel_Capacity = table.Column<double>(type: "numeric(7,1)", nullable: false),
                    Max_Altitude = table.Column<double>(type: "numeric(8,1)", nullable: false),
                    Engine_Power = table.Column<int>(type: "integer", nullable: false),
                    Carry_Capacity = table.Column<double>(type: "numeric(7,2)", nullable: false),
                    aircraftStatus_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Aircrafts_AircraftStatuses_aircraftStatus_id",
                        column: x => x.aircraftStatus_id,
                        principalTable: "AircraftStatuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "text", maxLength: 1024, nullable: false, defaultValue: "No Description"),
                    WebAdress = table.Column<string>(type: "text", maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", maxLength: 16, nullable: false),
                    Country = table.Column<string>(type: "text", maxLength: 60, nullable: false),
                    airport_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.id);
                    table.ForeignKey(
                        name: "FK_Airlines_Airports_airport_id",
                        column: x => x.airport_id,
                        principalTable: "Airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Surname = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Role = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<char>(type: "char(1)", nullable: false, defaultValue: 'U'),
                    PhoneNumber = table.Column<string>(type: "text", maxLength: 16, nullable: false, defaultValue: "Undefined"),
                    Start_Date = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2025, 4, 3, 3, 45, 57, 781, DateTimeKind.Local).AddTicks(3082)),
                    airport_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personals", x => x.id);
                    table.ForeignKey(
                        name: "FK_Personals_Airports_airport_id",
                        column: x => x.airport_id,
                        principalTable: "Airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminOperations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Operation_type = table.Column<string>(type: "text", nullable: false),
                    Target_table = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Target_id = table.Column<int>(type: "integer", nullable: false),
                    Operation_Date = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2025, 4, 3, 3, 45, 57, 782, DateTimeKind.Local).AddTicks(4746)),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminOperations", x => x.id);
                    table.ForeignKey(
                        name: "FK_AdminOperations_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogEntrys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2025, 4, 3, 3, 45, 57, 782, DateTimeKind.Local).AddTicks(1999)),
                    Message = table.Column<string>(type: "text", maxLength: 5096, nullable: false),
                    Action_type = table.Column<string>(type: "text", nullable: false),
                    Target_table = table.Column<string>(type: "text", maxLength: 64, nullable: false),
                    Record_id = table.Column<int>(type: "integer", nullable: false),
                    AdditionalData = table.Column<string>(type: "jsonb", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    loglevel_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntrys", x => x.id);
                    table.ForeignKey(
                        name: "FK_LogEntrys_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogEntrys_LogLevels_loglevel_id",
                        column: x => x.loglevel_id,
                        principalTable: "LogLevels",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Crew_Aircrafts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    crew_id = table.Column<int>(type: "integer", nullable: false),
                    aircraft_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crew_Aircrafts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Crew_Aircrafts_Aircrafts_aircraft_id",
                        column: x => x.aircraft_id,
                        principalTable: "Aircrafts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Crew_Aircrafts_Crews_crew_id",
                        column: x => x.crew_id,
                        principalTable: "Crews",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "text", maxLength: 1024, nullable: false, defaultValue: "No Description"),
                    Arrival_Date = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Deperture_Date = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Status = table.Column<string>(type: "text", maxLength: 32, nullable: false),
                    airline_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.id);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_airline_id",
                        column: x => x.airline_id,
                        principalTable: "Airlines",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Flight_Aircrafts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    flight_id = table.Column<int>(type: "integer", nullable: false),
                    aircraft_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight_Aircrafts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Flight_Aircrafts_Aircrafts_flight_id",
                        column: x => x.flight_id,
                        principalTable: "Aircrafts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Flight_Aircrafts_Flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OperationalDelay",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Delay_Reason = table.Column<string>(type: "text", maxLength: 1024, nullable: false),
                    Delay_Duration = table.Column<string>(type: "text", maxLength: 12, nullable: false),
                    Delay_Time = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    flight_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalDelay", x => x.id);
                    table.ForeignKey(
                        name: "FK_OperationalDelay_Flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Seat_number = table.Column<int>(type: "integer", nullable: false),
                    Seat_Class = table.Column<string>(type: "text", maxLength: 32, nullable: false),
                    Location = table.Column<string>(type: "text", maxLength: 512, nullable: false, defaultValue: "Undefined"),
                    Is_Available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    flight_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.id);
                    table.ForeignKey(
                        name: "FK_Seats_Flights_flight_id",
                        column: x => x.flight_id,
                        principalTable: "Flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    Puchase_date = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValue: new DateTime(2025, 4, 3, 3, 45, 57, 780, DateTimeKind.Local).AddTicks(8080)),
                    Baggage_weight = table.Column<double>(type: "numeric(8,2)", nullable: false, defaultValue: 0.0),
                    seet_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Seats_seet_id",
                        column: x => x.seet_id,
                        principalTable: "Seats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AircraftStatuses",
                columns: new[] { "id", "Status" },
                values: new object[,]
                {
                    { 1, "Available" },
                    { 2, "InMaintenance" },
                    { 3, "OutOfService" },
                    { 4, "NotOperational" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8b832739-21d6-4e2a-801f-2b1b322aef8b", null, "Administrator", "ADMİNİSTRATOR" },
                    { "be6ef88e-e798-493f-846a-97812152d846", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "LogLevels",
                columns: new[] { "id", "Level" },
                values: new object[,]
                {
                    { 1, "Trace" },
                    { 2, "Debug" },
                    { 3, "Info" },
                    { 4, "Warn" },
                    { 5, "Error" },
                    { 6, "Fatal" },
                    { 7, "Alert" },
                    { 8, "Emergency" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminOperations_user_id",
                table: "AdminOperations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_aircraftStatus_id",
                table: "Aircrafts",
                column: "aircraftStatus_id");

            migrationBuilder.CreateIndex(
                name: "IX_AircraftStatuses_Status",
                table: "AircraftStatuses",
                column: "Status",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_airport_id",
                table: "Airlines",
                column: "airport_id");

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_PhoneNumber",
                table: "Airlines",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_MailAdress",
                table: "Airports",
                column: "MailAdress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airports_PhoneNumber",
                table: "Airports",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crew_Aircrafts_aircraft_id",
                table: "Crew_Aircrafts",
                column: "aircraft_id");

            migrationBuilder.CreateIndex(
                name: "IX_Crew_Aircrafts_crew_id",
                table: "Crew_Aircrafts",
                column: "crew_id");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Aircrafts_flight_id",
                table: "Flight_Aircrafts",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_airline_id",
                table: "Flights",
                column: "airline_id");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntrys_loglevel_id",
                table: "LogEntrys",
                column: "loglevel_id");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntrys_user_id",
                table: "LogEntrys",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_LogLevels_Level",
                table: "LogLevels",
                column: "Level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationalDelay_flight_id",
                table: "OperationalDelay",
                column: "flight_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personals_airport_id",
                table: "Personals",
                column: "airport_id");

            migrationBuilder.CreateIndex(
                name: "IX_Personals_PhoneNumber",
                table: "Personals",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_flight_id",
                table: "Seats",
                column: "flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_seet_id",
                table: "Tickets",
                column: "seet_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_user_id",
                table: "Tickets",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminOperations");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Crew_Aircrafts");

            migrationBuilder.DropTable(
                name: "Flight_Aircrafts");

            migrationBuilder.DropTable(
                name: "LogEntrys");

            migrationBuilder.DropTable(
                name: "OperationalDelay");

            migrationBuilder.DropTable(
                name: "Personals");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "LogLevels");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "AircraftStatuses");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}
