using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartonCaps.Referrals.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferralInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferralCode = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    ReferrerUserId = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Channel = table.Column<short>(type: "INTEGER", maxLength: 3, nullable: false),
                    Deeplink = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Status = table.Column<short>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReferredUserId = table.Column<string>(type: "TEXT", nullable: true),
                    ReferredUserName = table.Column<string>(type: "TEXT", nullable: true),
                    RedeemedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralInvites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralInvites_ReferrerUserId",
                table: "ReferralInvites",
                column: "ReferrerUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralInvites");
        }
    }
}
