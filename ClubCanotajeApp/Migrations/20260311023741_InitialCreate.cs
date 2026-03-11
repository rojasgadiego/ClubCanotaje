using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubCanotajeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaRemador",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    edad_min = table.Column<int>(type: "int", nullable: true),
                    edad_max = table.Column<int>(type: "int", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaRemador", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "CodigoVerificacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    expira = table.Column<DateTime>(type: "datetime2", nullable: false),
                    usado = table.Column<bool>(type: "bit", nullable: false),
                    intentos = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigoVerificacion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCanoa",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCanoa", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "EstadoEvento",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoEvento", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "EstadoImplemento",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoImplemento", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "EstadoPago",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoPago", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "EstadoRemador",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoRemador", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "EstadoSalida",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoSalida", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "MetodoPago",
                columns: table => new
                {
                    id_metodo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodoPago", x => x.id_metodo);
                });

            migrationBuilder.CreateTable(
                name: "MotivoCancelacion",
                columns: table => new
                {
                    id_motivo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivoCancelacion", x => x.id_motivo);
                });

            migrationBuilder.CreateTable(
                name: "RolEnSalida",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolEnSalida", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "RolSistema",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolSistema", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "TipoCanoa",
                columns: table => new
                {
                    id_tipo_canoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    capacidad_max = table.Column<int>(type: "int", nullable: false),
                    capacidad_min = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCanoa", x => x.id_tipo_canoa);
                });

            migrationBuilder.CreateTable(
                name: "TipoEvento",
                columns: table => new
                {
                    id_tipo_evento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEvento", x => x.id_tipo_evento);
                });

            migrationBuilder.CreateTable(
                name: "TipoImplemento",
                columns: table => new
                {
                    id_tipo_implemento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoImplemento", x => x.id_tipo_implemento);
                });

            migrationBuilder.CreateTable(
                name: "TipoMembresia",
                columns: table => new
                {
                    id_tipo_membresia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duracion_dias = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMembresia", x => x.id_tipo_membresia);
                });

            migrationBuilder.CreateTable(
                name: "Remador",
                columns: table => new
                {
                    id_remador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rut = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido_paterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido_materno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    genero = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telefono_emergencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nombre_contacto_emergencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    foto_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cert_medica_vence = table.Column<DateOnly>(type: "date", nullable: true),
                    tiene_remo_propio = table.Column<bool>(type: "bit", nullable: false),
                    tiene_salvavidas_propio = table.Column<bool>(type: "bit", nullable: false),
                    id_categoria = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_ingreso = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_retiro = table.Column<DateOnly>(type: "date", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remador", x => x.id_remador);
                    table.ForeignKey(
                        name: "FK_Remador_CategoriaRemador_id_categoria",
                        column: x => x.id_categoria,
                        principalTable: "CategoriaRemador",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remador_EstadoRemador_id_estado",
                        column: x => x.id_estado,
                        principalTable: "EstadoRemador",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Canoa",
                columns: table => new
                {
                    id_canoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_tipo_canoa = table.Column<int>(type: "int", nullable: false),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    anio_fabricacion = table.Column<int>(type: "int", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero_serie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_adquisicion = table.Column<DateOnly>(type: "date", nullable: true),
                    ultima_revision = table.Column<DateOnly>(type: "date", nullable: true),
                    proxima_revision = table.Column<DateOnly>(type: "date", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canoa", x => x.id_canoa);
                    table.ForeignKey(
                        name: "FK_Canoa_EstadoCanoa_id_estado",
                        column: x => x.id_estado,
                        principalTable: "EstadoCanoa",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Canoa_TipoCanoa_id_tipo_canoa",
                        column: x => x.id_tipo_canoa,
                        principalTable: "TipoCanoa",
                        principalColumn: "id_tipo_canoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    id_evento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_tipo_evento = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    lugar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    organizador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url_info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_limite_inscripcion = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.id_evento);
                    table.ForeignKey(
                        name: "FK_Evento_EstadoEvento_id_estado",
                        column: x => x.id_estado,
                        principalTable: "EstadoEvento",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evento_TipoEvento_id_tipo_evento",
                        column: x => x.id_tipo_evento,
                        principalTable: "TipoEvento",
                        principalColumn: "id_tipo_evento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Implemento",
                columns: table => new
                {
                    id_implemento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_tipo_implemento = table.Column<int>(type: "int", nullable: false),
                    marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_adquisicion = table.Column<DateOnly>(type: "date", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Implemento", x => x.id_implemento);
                    table.ForeignKey(
                        name: "FK_Implemento_EstadoImplemento_id_estado",
                        column: x => x.id_estado,
                        principalTable: "EstadoImplemento",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Implemento_TipoImplemento_id_tipo_implemento",
                        column: x => x.id_tipo_implemento,
                        principalTable: "TipoImplemento",
                        principalColumn: "id_tipo_implemento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instructor",
                columns: table => new
                {
                    id_instructor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_remador = table.Column<int>(type: "int", nullable: true),
                    rut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido_paterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellido_materno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_ingreso = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_egreso = table.Column<DateOnly>(type: "date", nullable: true),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructor", x => x.id_instructor);
                    table.ForeignKey(
                        name: "FK_Instructor_Remador_id_remador",
                        column: x => x.id_remador,
                        principalTable: "Remador",
                        principalColumn: "id_remador");
                });

            migrationBuilder.CreateTable(
                name: "Membresia",
                columns: table => new
                {
                    id_membresia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_remador = table.Column<int>(type: "int", nullable: false),
                    id_tipo_membresia = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    activa = table.Column<bool>(type: "bit", nullable: false),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membresia", x => x.id_membresia);
                    table.ForeignKey(
                        name: "FK_Membresia_Remador_id_remador",
                        column: x => x.id_remador,
                        principalTable: "Remador",
                        principalColumn: "id_remador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Membresia_TipoMembresia_id_tipo_membresia",
                        column: x => x.id_tipo_membresia,
                        principalTable: "TipoMembresia",
                        principalColumn: "id_tipo_membresia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salida",
                columns: table => new
                {
                    id_salida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_canoa = table.Column<int>(type: "int", nullable: false),
                    fecha_hora_reserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_hora_programada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    duracion_estimada_min = table.Column<int>(type: "int", nullable: true),
                    fecha_hora_salida_real = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_hora_retorno_real = table.Column<DateTime>(type: "datetime2", nullable: true),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    id_responsable = table.Column<int>(type: "int", nullable: false),
                    zona_recorrido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    condicion_clima = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    es_entrenamiento = table.Column<bool>(type: "bit", nullable: false),
                    id_instructor_asignado = table.Column<int>(type: "int", nullable: true),
                    id_equipo = table.Column<int>(type: "int", nullable: true),
                    id_motivo_cancelacion = table.Column<int>(type: "int", nullable: true),
                    observacion_cancelacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cancelado_por = table.Column<int>(type: "int", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creado_por = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salida", x => x.id_salida);
                    table.ForeignKey(
                        name: "FK_Salida_Canoa_id_canoa",
                        column: x => x.id_canoa,
                        principalTable: "Canoa",
                        principalColumn: "id_canoa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salida_EstadoSalida_id_estado",
                        column: x => x.id_estado,
                        principalTable: "EstadoSalida",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salida_Remador_id_responsable",
                        column: x => x.id_responsable,
                        principalTable: "Remador",
                        principalColumn: "id_remador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipo",
                columns: table => new
                {
                    id_equipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_instructor = table.Column<int>(type: "int", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.id_equipo);
                    table.ForeignKey(
                        name: "FK_Equipo_Instructor_id_instructor",
                        column: x => x.id_instructor,
                        principalTable: "Instructor",
                        principalColumn: "id_instructor");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioSistema",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_remador = table.Column<int>(type: "int", nullable: true),
                    id_instructor = table.Column<int>(type: "int", nullable: true),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false),
                    ultimo_acceso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email_verificado = table.Column<bool>(type: "bit", nullable: false),
                    fecha_verificacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSistema", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_UsuarioSistema_Instructor_id_instructor",
                        column: x => x.id_instructor,
                        principalTable: "Instructor",
                        principalColumn: "id_instructor");
                    table.ForeignKey(
                        name: "FK_UsuarioSistema_Remador_id_remador",
                        column: x => x.id_remador,
                        principalTable: "Remador",
                        principalColumn: "id_remador");
                    table.ForeignKey(
                        name: "FK_UsuarioSistema_RolSistema_id_rol",
                        column: x => x.id_rol,
                        principalTable: "RolSistema",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuota",
                columns: table => new
                {
                    id_cuota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_membresia = table.Column<int>(type: "int", nullable: false),
                    periodo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuota", x => x.id_cuota);
                    table.ForeignKey(
                        name: "FK_Cuota_EstadoPago_id_estado",
                        column: x => x.id_estado,
                        principalTable: "EstadoPago",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cuota_Membresia_id_membresia",
                        column: x => x.id_membresia,
                        principalTable: "Membresia",
                        principalColumn: "id_membresia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalidaImplemento",
                columns: table => new
                {
                    id_salida_implemento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_salida = table.Column<int>(type: "int", nullable: false),
                    id_implemento = table.Column<int>(type: "int", nullable: false),
                    id_remador = table.Column<int>(type: "int", nullable: false),
                    devuelto = table.Column<bool>(type: "bit", nullable: false),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImplementoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidaImplemento", x => x.id_salida_implemento);
                    table.ForeignKey(
                        name: "FK_SalidaImplemento_Implemento_ImplementoId",
                        column: x => x.ImplementoId,
                        principalTable: "Implemento",
                        principalColumn: "id_implemento");
                    table.ForeignKey(
                        name: "FK_SalidaImplemento_Salida_id_salida",
                        column: x => x.id_salida,
                        principalTable: "Salida",
                        principalColumn: "id_salida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalidaParticipante",
                columns: table => new
                {
                    id_participante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_salida = table.Column<int>(type: "int", nullable: false),
                    id_remador = table.Column<int>(type: "int", nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: true),
                    usa_remo_club = table.Column<bool>(type: "bit", nullable: false),
                    usa_salvavidas_club = table.Column<bool>(type: "bit", nullable: false),
                    confirmo_asistencia = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalidaParticipante", x => x.id_participante);
                    table.ForeignKey(
                        name: "FK_SalidaParticipante_Remador_id_remador",
                        column: x => x.id_remador,
                        principalTable: "Remador",
                        principalColumn: "id_remador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalidaParticipante_RolEnSalida_id_rol",
                        column: x => x.id_rol,
                        principalTable: "RolEnSalida",
                        principalColumn: "id_rol");
                    table.ForeignKey(
                        name: "FK_SalidaParticipante_Salida_id_salida",
                        column: x => x.id_salida,
                        principalTable: "Salida",
                        principalColumn: "id_salida",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipoIntegrante",
                columns: table => new
                {
                    id_equipo_integrante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_equipo = table.Column<int>(type: "int", nullable: false),
                    id_remador = table.Column<int>(type: "int", nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: true),
                    fecha_ingreso = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_salida = table.Column<DateOnly>(type: "date", nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoIntegrante", x => x.id_equipo_integrante);
                    table.ForeignKey(
                        name: "FK_EquipoIntegrante_Equipo_id_equipo",
                        column: x => x.id_equipo,
                        principalTable: "Equipo",
                        principalColumn: "id_equipo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipoIntegrante_Remador_id_remador",
                        column: x => x.id_remador,
                        principalTable: "Remador",
                        principalColumn: "id_remador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventoInscripcion",
                columns: table => new
                {
                    id_inscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_evento = table.Column<int>(type: "int", nullable: false),
                    id_equipo = table.Column<int>(type: "int", nullable: true),
                    id_remador = table.Column<int>(type: "int", nullable: true),
                    id_canoa = table.Column<int>(type: "int", nullable: true),
                    categoria_competencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numero_largada = table.Column<int>(type: "int", nullable: true),
                    confirmada = table.Column<bool>(type: "bit", nullable: false),
                    fecha_inscripcion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoInscripcion", x => x.id_inscripcion);
                    table.CheckConstraint("CHK_EventoInsc_Part", "(id_equipo IS NOT NULL AND id_remador IS NULL) OR (id_equipo IS NULL AND id_remador IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_EventoInscripcion_Canoa_id_canoa",
                        column: x => x.id_canoa,
                        principalTable: "Canoa",
                        principalColumn: "id_canoa");
                    table.ForeignKey(
                        name: "FK_EventoInscripcion_Equipo_id_equipo",
                        column: x => x.id_equipo,
                        principalTable: "Equipo",
                        principalColumn: "id_equipo");
                    table.ForeignKey(
                        name: "FK_EventoInscripcion_Evento_id_evento",
                        column: x => x.id_evento,
                        principalTable: "Evento",
                        principalColumn: "id_evento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoInscripcion_Remador_id_remador",
                        column: x => x.id_remador,
                        principalTable: "Remador",
                        principalColumn: "id_remador");
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    id_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cuota = table.Column<int>(type: "int", nullable: false),
                    monto_pagado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_metodo_pago = table.Column<int>(type: "int", nullable: false),
                    comprobante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id_registrado_por = table.Column<int>(type: "int", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.id_pago);
                    table.ForeignKey(
                        name: "FK_Pago_Cuota_id_cuota",
                        column: x => x.id_cuota,
                        principalTable: "Cuota",
                        principalColumn: "id_cuota",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pago_MetodoPago_id_metodo_pago",
                        column: x => x.id_metodo_pago,
                        principalTable: "MetodoPago",
                        principalColumn: "id_metodo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventoResultado",
                columns: table => new
                {
                    id_resultado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_inscripcion = table.Column<int>(type: "int", nullable: false),
                    posicion_final = table.Column<int>(type: "int", nullable: true),
                    tiempo_oficial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    puntos = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    descalificado = table.Column<bool>(type: "bit", nullable: false),
                    motivo_desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoResultado", x => x.id_resultado);
                    table.ForeignKey(
                        name: "FK_EventoResultado_EventoInscripcion_id_inscripcion",
                        column: x => x.id_inscripcion,
                        principalTable: "EventoInscripcion",
                        principalColumn: "id_inscripcion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Canoa_codigo",
                table: "Canoa",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Canoa_id_estado",
                table: "Canoa",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Canoa_id_tipo_canoa",
                table: "Canoa",
                column: "id_tipo_canoa");

            migrationBuilder.CreateIndex(
                name: "IX_CodigoVerificacion_email_tipo_usado",
                table: "CodigoVerificacion",
                columns: new[] { "email", "tipo", "usado" });

            migrationBuilder.CreateIndex(
                name: "IX_Cuota_id_estado",
                table: "Cuota",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Cuota_id_membresia_periodo",
                table: "Cuota",
                columns: new[] { "id_membresia", "periodo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_id_instructor",
                table: "Equipo",
                column: "id_instructor");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoIntegrante_id_equipo",
                table: "EquipoIntegrante",
                column: "id_equipo");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoIntegrante_id_remador",
                table: "EquipoIntegrante",
                column: "id_remador");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_id_estado",
                table: "Evento",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_id_tipo_evento",
                table: "Evento",
                column: "id_tipo_evento");

            migrationBuilder.CreateIndex(
                name: "IX_EventoInscripcion_id_canoa",
                table: "EventoInscripcion",
                column: "id_canoa");

            migrationBuilder.CreateIndex(
                name: "IX_EventoInscripcion_id_equipo",
                table: "EventoInscripcion",
                column: "id_equipo");

            migrationBuilder.CreateIndex(
                name: "IX_EventoInscripcion_id_evento",
                table: "EventoInscripcion",
                column: "id_evento");

            migrationBuilder.CreateIndex(
                name: "IX_EventoInscripcion_id_remador",
                table: "EventoInscripcion",
                column: "id_remador");

            migrationBuilder.CreateIndex(
                name: "IX_EventoResultado_id_inscripcion",
                table: "EventoResultado",
                column: "id_inscripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Implemento_id_estado",
                table: "Implemento",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Implemento_id_tipo_implemento",
                table: "Implemento",
                column: "id_tipo_implemento");

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_id_remador",
                table: "Instructor",
                column: "id_remador");

            migrationBuilder.CreateIndex(
                name: "IX_Membresia_id_remador",
                table: "Membresia",
                column: "id_remador");

            migrationBuilder.CreateIndex(
                name: "IX_Membresia_id_tipo_membresia",
                table: "Membresia",
                column: "id_tipo_membresia");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_id_cuota",
                table: "Pago",
                column: "id_cuota");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_id_metodo_pago",
                table: "Pago",
                column: "id_metodo_pago");

            migrationBuilder.CreateIndex(
                name: "IX_Remador_email",
                table: "Remador",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Remador_id_categoria",
                table: "Remador",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Remador_id_estado",
                table: "Remador",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Remador_rut",
                table: "Remador",
                column: "rut",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Salida_id_canoa",
                table: "Salida",
                column: "id_canoa");

            migrationBuilder.CreateIndex(
                name: "IX_Salida_id_estado",
                table: "Salida",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Salida_id_responsable",
                table: "Salida",
                column: "id_responsable");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaImplemento_id_salida",
                table: "SalidaImplemento",
                column: "id_salida");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaImplemento_ImplementoId",
                table: "SalidaImplemento",
                column: "ImplementoId");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaParticipante_id_remador",
                table: "SalidaParticipante",
                column: "id_remador");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaParticipante_id_rol",
                table: "SalidaParticipante",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_SalidaParticipante_id_salida_id_remador",
                table: "SalidaParticipante",
                columns: new[] { "id_salida", "id_remador" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSistema_id_instructor",
                table: "UsuarioSistema",
                column: "id_instructor",
                unique: true,
                filter: "[id_instructor] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSistema_id_remador",
                table: "UsuarioSistema",
                column: "id_remador",
                unique: true,
                filter: "[id_remador] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSistema_id_rol",
                table: "UsuarioSistema",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSistema_username",
                table: "UsuarioSistema",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodigoVerificacion");

            migrationBuilder.DropTable(
                name: "EquipoIntegrante");

            migrationBuilder.DropTable(
                name: "EventoResultado");

            migrationBuilder.DropTable(
                name: "MotivoCancelacion");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "SalidaImplemento");

            migrationBuilder.DropTable(
                name: "SalidaParticipante");

            migrationBuilder.DropTable(
                name: "UsuarioSistema");

            migrationBuilder.DropTable(
                name: "EventoInscripcion");

            migrationBuilder.DropTable(
                name: "Cuota");

            migrationBuilder.DropTable(
                name: "MetodoPago");

            migrationBuilder.DropTable(
                name: "Implemento");

            migrationBuilder.DropTable(
                name: "RolEnSalida");

            migrationBuilder.DropTable(
                name: "Salida");

            migrationBuilder.DropTable(
                name: "RolSistema");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "EstadoPago");

            migrationBuilder.DropTable(
                name: "Membresia");

            migrationBuilder.DropTable(
                name: "EstadoImplemento");

            migrationBuilder.DropTable(
                name: "TipoImplemento");

            migrationBuilder.DropTable(
                name: "Canoa");

            migrationBuilder.DropTable(
                name: "EstadoSalida");

            migrationBuilder.DropTable(
                name: "Instructor");

            migrationBuilder.DropTable(
                name: "EstadoEvento");

            migrationBuilder.DropTable(
                name: "TipoEvento");

            migrationBuilder.DropTable(
                name: "TipoMembresia");

            migrationBuilder.DropTable(
                name: "EstadoCanoa");

            migrationBuilder.DropTable(
                name: "TipoCanoa");

            migrationBuilder.DropTable(
                name: "Remador");

            migrationBuilder.DropTable(
                name: "CategoriaRemador");

            migrationBuilder.DropTable(
                name: "EstadoRemador");
        }
    }
}
