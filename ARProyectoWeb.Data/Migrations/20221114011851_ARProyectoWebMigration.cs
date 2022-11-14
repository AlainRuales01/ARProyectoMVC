using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ARProyectoWeb.Data.Migrations
{
    public partial class ARProyectoWebMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Task_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCourse",
                columns: table => new
                {
                    UsuarioCourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCourse", x => x.UsuarioCourseId);
                    table.ForeignKey(
                        name: "FK_UsuarioCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_UsuarioCourse_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "TaskCourse",
                columns: table => new
                {
                    TaskCourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    CalificacionProfesor = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCourse", x => x.TaskCourseId);
                    table.ForeignKey(
                        name: "FK_TaskCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_TaskCourse_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "TaskId");
                });

            migrationBuilder.CreateTable(
                name: "TaskRate",
                columns: table => new
                {
                    TaskRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    TaskCourseId = table.Column<int>(type: "int", nullable: false),
                    Calificacion = table.Column<double>(type: "float", nullable: false),
                    CalificacionUsuario = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskRate", x => x.TaskRateId);
                    table.ForeignKey(
                        name: "FK_TaskRate_TaskCourse_TaskCourseId",
                        column: x => x.TaskCourseId,
                        principalTable: "TaskCourse",
                        principalColumn: "TaskCourseId");
                    table.ForeignKey(
                        name: "FK_TaskRate_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_UsuarioId",
                table: "Task",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCourse_CourseId",
                table: "TaskCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskCourse_TaskId",
                table: "TaskCourse",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRate_TaskCourseId",
                table: "TaskRate",
                column: "TaskCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRate_UsuarioId",
                table: "TaskRate",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCourse_CourseId",
                table: "UsuarioCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCourse_UsuarioId",
                table: "UsuarioCourse",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskRate");

            migrationBuilder.DropTable(
                name: "UsuarioCourse");

            migrationBuilder.DropTable(
                name: "TaskCourse");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
