using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.News.Database.Migrations.LogsApiNews
{
    /// <inheritdoc />
    public partial class Add_StatusCode_In_Log_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status_code",
                schema: "insania_logs_api_news",
                table: "r_logs_api_news",
                type: "integer",
                nullable: true,
                comment: "Код статуса");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_code",
                schema: "insania_logs_api_news",
                table: "r_logs_api_news");
        }
    }
}
