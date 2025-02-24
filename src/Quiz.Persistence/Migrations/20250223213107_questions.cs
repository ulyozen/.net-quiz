using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class questions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Templates_TemplateId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropIndex(
                name: "IX_QuestionTypeId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "QuestionTypeId",
                table: "Questions",
                newName: "QuestionType");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateId",
                table: "Questions",
                newName: "IX_Questions_TemplateId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswers",
                table: "Questions",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Options",
                table: "Questions",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Templates_TemplateId",
                table: "Questions",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Templates_TemplateId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Options",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "Questions",
                newName: "QuestionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_TemplateId",
                table: "Questions",
                newName: "IX_TemplateId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ValueType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Templates_TemplateId",
                table: "Questions",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
