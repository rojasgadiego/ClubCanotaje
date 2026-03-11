using ClubCanotajeAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace ClubCanotajeAPI.Context
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            SeedCatalogos(db);
            SeedDatos(db);
        }

        // ══════════════════════════════════════════════════════════════
        //  CATÁLOGOS
        // ══════════════════════════════════════════════════════════════
        private static void SeedCatalogos(AppDbContext db)
        {
            if (!db.RolesSistema.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT RolSistema ON;
                    INSERT INTO RolSistema (id_rol, nombre, descripcion) VALUES
                    (1, 'Administrador', 'Acceso total al sistema'),
                    (2, 'Directiva',     'Gestión financiera, membresías y reportes'),
                    (3, 'Entrenador',    'Gestión de salidas, equipos y entrenamientos'),
                    (4, 'Remador',       'Acceso personal: perfil, reservas y estado de pago');
                    SET IDENTITY_INSERT RolSistema OFF;
                ");
            }

            if (!db.EstadosRemador.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT EstadoRemador ON;
                    INSERT INTO EstadoRemador (id_estado, nombre, descripcion) VALUES
                    (1, 'Activo',     'Remador con membresía vigente y habilitado'),
                    (2, 'Inactivo',   'Remador sin membresía activa'),
                    (3, 'Suspendido', 'Temporalmente inhabilitado por la directiva'),
                    (4, 'Retirado',   'Dejó el club definitivamente');
                    SET IDENTITY_INSERT EstadoRemador OFF;
                ");
            }

            if (!db.CategoriasRemador.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT CategoriaRemador ON;
                    INSERT INTO CategoriaRemador (id_categoria, nombre, edad_min, edad_max, descripcion) VALUES
                    (1, 'Junior',  12, 17,   'Remadores jóvenes entre 12 y 17 años'),
                    (2, 'Adulto',  18, 39,   'Categoría adulto general'),
                    (3, 'Máster',  40, NULL, 'Remadores de 40 años o más'),
                    (4, 'Elite',   18, NULL, 'Categoría de alto rendimiento');
                    SET IDENTITY_INSERT CategoriaRemador OFF;
                ");
            }

            if (!db.TiposCanoa.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT TipoCanoa ON;
                    INSERT INTO TipoCanoa (id_tipo_canoa, nombre, capacidad_max, capacidad_min, descripcion) VALUES
                    (1, 'V1', 1, 1, 'Canoa individual polinésica'),
                    (2, 'V6', 6, 4, 'Canoa grupal polinésica de 6 remadores');
                    SET IDENTITY_INSERT TipoCanoa OFF;
                ");
            }

            if (!db.EstadosCanoa.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT EstadoCanoa ON;
                    INSERT INTO EstadoCanoa (id_estado, nombre) VALUES
                    (1, 'Disponible'),
                    (2, 'En uso'),
                    (3, 'Reservada'),
                    (4, 'En mantenimiento'),
                    (5, 'Baja');
                    SET IDENTITY_INSERT EstadoCanoa OFF;
                ");
            }

            if (!db.EstadosSalida.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT EstadoSalida ON;
                    INSERT INTO EstadoSalida (id_estado, nombre, descripcion) VALUES
                    (1, 'Reservada',  'Salida reservada, pendiente de confirmación'),
                    (2, 'Confirmada', 'Salida confirmada, lista para partir'),
                    (3, 'En curso',   'Salida actualmente en el agua'),
                    (4, 'Finalizada', 'Salida completada exitosamente'),
                    (5, 'Cancelada',  'Salida cancelada');
                    SET IDENTITY_INSERT EstadoSalida OFF;
                ");
            }

            if (!db.RolesEnSalida.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT RolEnSalida ON;
                    INSERT INTO RolEnSalida (id_rol, nombre, descripcion) VALUES
                    (1, 'Timonel', 'Responsable de la dirección de la embarcación'),
                    (2, 'Remador', 'Participante remador'),
                    (3, 'Capitán', 'Líder del equipo en la salida');
                    SET IDENTITY_INSERT RolEnSalida OFF;
                ");
            }

            if (!db.TiposImplemento.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT TipoImplemento ON;
                    INSERT INTO TipoImplemento (id_tipo_implemento, nombre, descripcion) VALUES
                    (1, 'Remo',         'Remo para canoa individual o grupal'),
                    (2, 'Salvavidas',   'Chaleco salvavidas homologado'),
                    (3, 'Casco',        'Casco de protección'),
                    (4, 'Ropa de agua', 'Indumentaria impermeable'),
                    (5, 'Tabla',        'Tabla de apoyo');
                    SET IDENTITY_INSERT TipoImplemento OFF;
                ");
            }

            if (!db.EstadosImplemento.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT EstadoImplemento ON;
                    INSERT INTO EstadoImplemento (id_estado, nombre, descripcion) VALUES
                    (1, 'Disponible',       'Implemento disponible para uso'),
                    (2, 'En uso',           'Implemento actualmente en uso'),
                    (3, 'En mantenimiento', 'Implemento en reparación o revisión'),
                    (4, 'Baja',             'Implemento dado de baja');
                    SET IDENTITY_INSERT EstadoImplemento OFF;
                ");
            }

            if (!db.EstadosPago.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT EstadoPago ON;
                    INSERT INTO EstadoPago (id_estado, nombre) VALUES
                    (1, 'Pendiente'),
                    (2, 'Pagado'),
                    (3, 'Vencido'),
                    (4, 'Anulado');
                    SET IDENTITY_INSERT EstadoPago OFF;
                ");
            }

            if (!db.MetodosPago.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT MetodoPago ON;
                    INSERT INTO MetodoPago (id_metodo, nombre) VALUES
                    (1, 'Transferencia'),
                    (2, 'Efectivo'),
                    (3, 'Tarjeta débito'),
                    (4, 'Tarjeta crédito');
                    SET IDENTITY_INSERT MetodoPago OFF;
                ");
            }

            if (!db.TiposMembresia.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT TipoMembresia ON;
                    INSERT INTO TipoMembresia (id_tipo_membresia, nombre, duracion_dias, precio, descripcion, activo) VALUES
                    (1, 'Mensual',    30,  25000,  'Membresía mensual',    1),
                    (2, 'Trimestral', 90,  65000,  'Membresía trimestral', 1),
                    (3, 'Semestral',  180, 120000, 'Membresía semestral',  1),
                    (4, 'Anual',      365, 220000, 'Membresía anual',      1);
                    SET IDENTITY_INSERT TipoMembresia OFF;
                ");
            }

            if (!db.MotivosCancelacion.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT MotivoCancelacion ON;
                    INSERT INTO MotivoCancelacion (id_motivo, nombre) VALUES
                    (1, 'Clima adverso'),
                    (2, 'Falta de participantes'),
                    (3, 'Problemas con embarcación'),
                    (4, 'Decisión del responsable'),
                    (5, 'Otro');
                    SET IDENTITY_INSERT MotivoCancelacion OFF;
                ");
            }

            if (!db.TiposEvento.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT TipoEvento ON;
                    INSERT INTO TipoEvento (id_tipo_evento, nombre, descripcion) VALUES
                    (1, 'Regata',                 'Competencia de velocidad'),
                    (2, 'Travesía',               'Recorrido de larga distancia'),
                    (3, 'Campeonato',             'Competencia oficial federada'),
                    (4, 'Entrenamiento especial', 'Sesión de entrenamiento intensivo'),
                    (5, 'Amistoso',               'Encuentro informal entre clubes');
                    SET IDENTITY_INSERT TipoEvento OFF;
                ");
            }

            if (!db.EstadosEvento.Any())
            {
                db.Database.ExecuteSqlRaw(@"
                    SET IDENTITY_INSERT EstadoEvento ON;
                    INSERT INTO EstadoEvento (id_estado, nombre) VALUES
                    (1, 'Planificado'),
                    (2, 'Inscripciones abiertas'),
                    (3, 'En curso'),
                    (4, 'Finalizado'),
                    (5, 'Cancelado');
                    SET IDENTITY_INSERT EstadoEvento OFF;
                ");
            }
        }

        // ══════════════════════════════════════════════════════════════
        //  DATOS PRINCIPALES — solo si no hay remadores
        // ══════════════════════════════════════════════════════════════
        private static void SeedDatos(AppDbContext db)
        {
            if (db.Remadores.Any()) return;

            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;

                -- ── REMADORES ─────────────────────────────────────────────────
                INSERT INTO Remador (
                    rut, nombres, apellido_paterno, apellido_materno,
                    fecha_nacimiento, genero, email, telefono, telefono_emergencia,
                    nombre_contacto_emergencia, direccion,
                    cert_medica_vence, tiene_remo_propio, tiene_salvavidas_propio,
                    id_categoria, id_estado, fecha_ingreso, fecha_creacion
                ) VALUES
                ('12345678-9', 'Carlos Andrés', 'Muñoz',    'Pérez',   '19900315','M','carlos.munoz@email.com',     '+56912345678','+56987654321','Ana Pérez',      'Av. Costanera 100, Valparaíso',     '20251231',1,1,2,1,'20220110',GETDATE()),
                ('23456789-0', 'Valentina',     'Rojas',    'Soto',    '19950722','F','valentina.rojas@email.com',  '+56923456789','+56976543210','Luis Rojas',      'Pasaje Los Pinos 55, Viña del Mar', '20251130',0,1,2,1,'20220305',GETDATE()),
                ('34567890-1', 'Sebastián',     'Torres',   'Fuentes', '19881108','M','sebastian.torres@email.com', '+56934567890','+56965432109','María Fuentes',   'Calle Errazuriz 234, Valparaíso',   '20260228',1,0,2,1,'20210620',GETDATE()),
                ('45678901-2', 'Camila Paz',    'Vargas',   'Mora',    '19930430','F','camila.vargas@email.com',    '+56945678901','+56954321098','Pedro Vargas',    'Av. España 890, Viña del Mar',      '20250915',1,1,2,1,'20230214',GETDATE()),
                ('56789012-3', 'Roberto',       'González', 'Salinas', '19780912','M','roberto.gonzalez@email.com', '+56956789012','+56943210987','Carmen Salinas',  'Av. Almirante 45, Valparaíso',      '20260120',1,1,3,1,'20200801',GETDATE()),
                ('67890123-4', 'Patricia',      'Herrera',  'Lagos',   '19751203','F','patricia.herrera@email.com', '+56967890123','+56932109876','Jorge Lagos',     'Subida Ecuador 12, Valparaíso',     '20251010',0,1,3,1,'20210115',GETDATE()),
                ('78901234-5', 'Matías',        'Silva',    'Bravo',   '20080219','M','matias.silva@email.com',     '+56978901234','+56921098765','Laura Bravo',     'Calle San Martín 300, Viña del Mar','20251201',0,0,1,1,'20230901',GETDATE()),
                ('89012345-6', 'Isidora',       'Castro',   'Ortiz',   '20070625','F','isidora.castro@email.com',   '+56989012345','+56910987654','Francisco Ortiz', 'Av. Recreo 77, Viña del Mar',       '20260315',0,0,1,1,'20230901',GETDATE()),
                ('90123456-7', 'Felipe',        'Morales',  'Vega',    '19920814','M','felipe.morales@email.com',   '+56990123456','+56909876543','Rosa Vega',       'Av. Marina 500, Valparaíso',        '20260630',1,1,4,1,'20190510',GETDATE()),
                ('01234567-8', 'Javiera',       'Pinto',    'Díaz',    '19850128','F','javiera.pinto@email.com',    '+56901234567','+56998765432','Marcos Díaz',     'Calle Chacabuco 88, Valparaíso',    NULL,      0,0,2,2,'20200322',GETDATE());

                -- ── INSTRUCTORES ──────────────────────────────────────────────
                INSERT INTO Instructor (id_remador, rut, nombres, apellido_paterno, apellido_materno, email, telefono, activo, fecha_ingreso, bio, fecha_creacion) VALUES
                (1,    '12345678-9', 'Carlos Andrés', 'Muñoz',    'Pérez',   'carlos.munoz@email.com',        '+56912345678', 1, '20220110', 'Instructor certificado con 10 años de experiencia en canotaje polinésico.', GETDATE()),
                (5,    '56789012-3', 'Roberto',       'González', 'Salinas', 'roberto.gonzalez@email.com',    '+56956789012', 1, '20200801', 'Instructor Máster especialista en travesías y preparación física.',          GETDATE()),
                (NULL, '11111111-1', 'Jorge Ignacio', 'Saavedra', 'Riquelme','jorge.saavedra.inst@email.com', '+56911111111', 1, '20210310', 'Instructor externo especialista en arbitraje y reglamento.',                 GETDATE());

                -- ── CANOAS ───────────────────────────────────────────────────
                INSERT INTO Canoa (codigo, nombre, id_tipo_canoa, marca, modelo, anio_fabricacion, color, numero_serie, id_estado, fecha_adquisicion, ultima_revision, proxima_revision, fecha_creacion) VALUES
                ('V1-001','Relámpago', 1,'Pahoa',    'Sprint Pro', 2020,'Azul',    'PAH-2020-001',1,'20200115','20250110','20250710',GETDATE()),
                ('V1-002','Viento Sur',1,'Pahoa',    'Sprint Pro', 2021,'Rojo',    'PAH-2021-002',1,'20210320','20250205','20250805',GETDATE()),
                ('V1-003','Ola Azul',  1,'Kaimana',  'Classic',    2019,'Blanco',  'KAI-2019-003',1,'20190601','20241215','20250615',GETDATE()),
                ('V1-004','Tormenta',  1,'Kaimana',  'Classic',    2018,'Amarillo','KAI-2018-004',4,'20180910','20250301',NULL,      GETDATE()),
                ('V6-001','Polinesio', 2,'OC Racing','V6 Elite',   2022,'Negro',   'OCR-2022-001',1,'20220501','20250220','20250820',GETDATE()),
                ('V6-002','Maori',     2,'OC Racing','V6 Elite',   2021,'Verde',   'OCR-2021-002',1,'20211115','20250130','20250730',GETDATE()),
                ('V6-003','Pacifico',  2,'Tahiti',   'Club Series',2017,'Gris',    'TAH-2017-003',5,'20170420','20230601',NULL,      GETDATE());

                -- ── IMPLEMENTOS ───────────────────────────────────────────────
                INSERT INTO Implemento (codigo, id_tipo_implemento, marca, modelo, id_estado, fecha_adquisicion, fecha_creacion) VALUES
                ('REM-001',1,'Kialoa',    'Pueo Carbon',   1,'20210110',GETDATE()),
                ('REM-002',1,'Kialoa',    'Pueo Carbon',   1,'20210110',GETDATE()),
                ('REM-003',1,'Kialoa',    'Pueo Carbon',   1,'20210110',GETDATE()),
                ('REM-004',1,'Kai',       'Standard',      1,'20200315',GETDATE()),
                ('REM-005',1,'Kai',       'Standard',      1,'20200315',GETDATE()),
                ('REM-006',1,'Kai',       'Standard',      3,'20200315',GETDATE()),
                ('SAL-001',2,'Mustang',   'Inflatable Pro',1,'20220601',GETDATE()),
                ('SAL-002',2,'Mustang',   'Inflatable Pro',1,'20220601',GETDATE()),
                ('SAL-003',2,'Mustang',   'Inflatable Pro',1,'20220601',GETDATE()),
                ('SAL-004',2,'Hutchwilco','Classic',       1,'20210820',GETDATE()),
                ('SAL-005',2,'Hutchwilco','Classic',       1,'20210820',GETDATE()),
                ('SAL-006',2,'Hutchwilco','Classic',       1,'20210820',GETDATE());

                -- ── EQUIPOS ───────────────────────────────────────────────────
                INSERT INTO Equipo (nombre, descripcion, id_instructor, activo, fecha_creacion) VALUES
                ('Equipo Polinesio A','Equipo principal de competencia V6, categoría Elite/Adulto',1,1,GETDATE()),
                ('Equipo Máster Mar', 'Equipo de remadores Máster, entrena los sábados',           2,1,GETDATE()),
                ('Equipo Junior',     'Equipo de jóvenes talentos, categoría Junior',              1,1,GETDATE());

                INSERT INTO EquipoIntegrante (id_equipo, id_remador, id_rol, fecha_ingreso, activo) VALUES
                (1,9,3,'20190510',1),(1,1,1,'20220110',1),(1,2,2,'20220305',1),(1,3,2,'20210620',1),(1,4,2,'20230214',1),
                (2,5,3,'20200801',1),(2,6,2,'20210115',1),
                (3,7,2,'20230901',1),(3,8,2,'20230901',1);

                -- ── MEMBRESÍAS ────────────────────────────────────────────────
                INSERT INTO Membresia (id_remador, id_tipo_membresia, fecha_inicio, fecha_fin, activa, fecha_creacion) VALUES
                (1, 4,'20250101','20251231',1,GETDATE()),(2, 4,'20250101','20251231',1,GETDATE()),
                (3, 3,'20250101','20250629',1,GETDATE()),(4, 2,'20250401','20250629',1,GETDATE()),
                (5, 4,'20250101','20251231',1,GETDATE()),(6, 4,'20250101','20251231',1,GETDATE()),
                (7, 1,'20250501','20250531',1,GETDATE()),(8, 1,'20250501','20250531',1,GETDATE()),
                (9, 4,'20250101','20251231',1,GETDATE()),(10,1,'20240601','20240630',0,GETDATE());

                -- ── CUOTAS ───────────────────────────────────────────────────
                DECLARE @pm DECIMAL(10,2)=25000, @pa DECIMAL(10,2)=220000,
                        @pt DECIMAL(10,2)=65000, @ps DECIMAL(10,2)=120000;

                INSERT INTO Cuota (id_membresia,periodo,monto,fecha_vencimiento,id_estado,fecha_creacion) VALUES
                (1,'2025-01',@pa,'20250204',2,GETDATE()),(1,'2025-02',@pa,'20250304',2,GETDATE()),(1,'2025-03',@pa,'20250404',2,GETDATE()),(1,'2025-04',@pa,'20250504',2,GETDATE()),(1,'2025-05',@pa,'20250604',1,GETDATE()),
                (2,'2025-01',@pa,'20250204',2,GETDATE()),(2,'2025-02',@pa,'20250304',2,GETDATE()),(2,'2025-03',@pa,'20250404',2,GETDATE()),(2,'2025-04',@pa,'20250504',3,GETDATE()),(2,'2025-05',@pa,'20250604',1,GETDATE()),
                (3,'2025-01',@ps,'20250204',2,GETDATE()),(3,'2025-02',@ps,'20250304',2,GETDATE()),(3,'2025-03',@ps,'20250404',1,GETDATE()),
                (4,'2025-04',@pt,'20250504',2,GETDATE()),(4,'2025-05',@pt,'20250604',1,GETDATE()),
                (5,'2025-01',@pa,'20250204',2,GETDATE()),(5,'2025-02',@pa,'20250304',2,GETDATE()),(5,'2025-03',@pa,'20250404',2,GETDATE()),(5,'2025-04',@pa,'20250504',2,GETDATE()),(5,'2025-05',@pa,'20250604',1,GETDATE()),
                (9,'2025-01',@pa,'20250204',2,GETDATE()),(9,'2025-02',@pa,'20250304',2,GETDATE()),(9,'2025-03',@pa,'20250404',2,GETDATE()),(9,'2025-04',@pa,'20250504',2,GETDATE()),(9,'2025-05',@pa,'20250604',1,GETDATE()),
                (7,'2025-05',@pm,'20250604',1,GETDATE()),(8,'2025-05',@pm,'20250604',1,GETDATE()),
                (10,'2024-06',@pm,'20240704',3,GETDATE());

                -- ── PAGOS ─────────────────────────────────────────────────────
                INSERT INTO Pago (id_cuota,monto_pagado,fecha_pago,id_metodo_pago,comprobante,id_registrado_por) VALUES
                (1, 220000,'20250105',1,'TRF-20250105-001',1),(2, 220000,'20250203',1,'TRF-20250203-002',1),
                (3, 220000,'20250304',1,'TRF-20250304-003',1),(4, 220000,'20250402',3,'TRF-20250402-004',1),
                (6, 220000,'20250106',1,'TRF-20250106-005',NULL),(7, 220000,'20250205',1,'TRF-20250205-006',NULL),
                (8, 220000,'20250305',2,NULL,NULL),(11,120000,'20250108',1,'TRF-20250108-007',NULL),
                (12,120000,'20250207',1,'TRF-20250207-008',NULL),(14, 65000,'20250403',3,'TBD-20250403-009',NULL),
                (15,220000,'20250104',1,'TRF-20250104-010',NULL),(16,220000,'20250204',1,'TRF-20250204-011',NULL),
                (17,220000,'20250303',1,'TRF-20250303-012',NULL),(18,220000,'20250401',1,'TRF-20250401-013',NULL),
                (21,220000,'20250103',1,'TRF-20250103-014',NULL),(22,220000,'20250202',1,'TRF-20250202-015',NULL),
                (23,220000,'20250302',1,'TRF-20250302-016',NULL),(24,220000,'20250401',1,'TRF-20250401-017',NULL);

                -- ── USUARIOS DEL SISTEMA (password: admin123) ─────────────────
                INSERT INTO UsuarioSistema (id_remador,id_instructor,id_rol,username,password_hash,activo,email_verificado,fecha_verificacion,fecha_creacion) VALUES
                (NULL,NULL,1,'admin',           '$2a$12$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LkdybiiuT6i',1,1,GETDATE(),GETDATE()),
                (1,   NULL,3,'carlos.munoz',    '$2a$12$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LkdybiiuT6i',1,1,GETDATE(),GETDATE()),
                (2,   NULL,4,'valentina.rojas', '$2a$12$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LkdybiiuT6i',1,1,GETDATE(),GETDATE()),
                (9,   NULL,4,'felipe.morales',  '$2a$12$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LkdybiiuT6i',1,1,GETDATE(),GETDATE()),
                (5,   NULL,3,'roberto.gonzalez','$2a$12$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LkdybiiuT6i',1,1,GETDATE(),GETDATE()),
                (10,  NULL,4,'javiera.pinto',   '$2a$12$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LkdybiiuT6i',0,1,GETDATE(),GETDATE());
            ");

            SeedSalidas(db);
            SeedEventos(db);
        }

        // ══════════════════════════════════════════════════════════════
        //  SALIDAS — cada INSERT separado para capturar el ID correcto
        // ══════════════════════════════════════════════════════════════
        private static void SeedSalidas(AppDbContext db)
        {
            // Salida 1 — Finalizada, V1 Carlos
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Salida (id_canoa,fecha_hora_reserva,fecha_hora_programada,duracion_estimada_min,fecha_hora_salida_real,fecha_hora_retorno_real,id_estado,id_responsable,zona_recorrido,condicion_clima,es_entrenamiento,id_instructor_asignado,id_equipo,creado_por,fecha_creacion)
                VALUES (1,'20250410 08:00:00','20250410 09:00:00',90,'20250410 09:05:00','20250410 10:35:00',4,1,'Bahía de Valparaíso','Soleado',0,NULL,NULL,1,GETDATE());
            ");
            int s1 = db.Salidas.OrderByDescending(s => s.Id).Select(s => s.Id).First();

            // Salida 2 — Finalizada, entrenamiento V6 Equipo Polinesio A
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Salida (id_canoa,fecha_hora_reserva,fecha_hora_programada,duracion_estimada_min,fecha_hora_salida_real,fecha_hora_retorno_real,id_estado,id_responsable,zona_recorrido,condicion_clima,es_entrenamiento,id_instructor_asignado,id_equipo,creado_por,fecha_creacion)
                VALUES (5,'20250415 07:30:00','20250415 08:00:00',120,'20250415 08:05:00','20250415 10:10:00',4,9,'Canal Beagle zona norte','Parcialmente nublado',1,1,1,9,GETDATE());
            ");
            int s2 = db.Salidas.OrderByDescending(s => s.Id).Select(s => s.Id).First();

            // Salida 3 — Finalizada, travesía Máster
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Salida (id_canoa,fecha_hora_reserva,fecha_hora_programada,duracion_estimada_min,fecha_hora_salida_real,fecha_hora_retorno_real,id_estado,id_responsable,zona_recorrido,condicion_clima,es_entrenamiento,id_instructor_asignado,id_equipo,creado_por,fecha_creacion)
                VALUES (6,'20250420 09:00:00','20250420 10:00:00',180,'20250420 10:08:00','20250420 13:05:00',4,5,'Ruta costera norte','Viento moderado',0,2,2,5,GETDATE());
            ");
            int s3 = db.Salidas.OrderByDescending(s => s.Id).Select(s => s.Id).First();

            // Salida 4 — Confirmada, próximo entrenamiento V6
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Salida (id_canoa,fecha_hora_reserva,fecha_hora_programada,duracion_estimada_min,fecha_hora_salida_real,fecha_hora_retorno_real,id_estado,id_responsable,zona_recorrido,condicion_clima,es_entrenamiento,id_instructor_asignado,id_equipo,creado_por,fecha_creacion)
                VALUES (5,'20250520 07:00:00','20250524 08:00:00',120,NULL,NULL,2,9,'Bahía de Valparaíso',NULL,1,1,1,9,GETDATE());
            ");
            int s4 = db.Salidas.OrderByDescending(s => s.Id).Select(s => s.Id).First();

            // Salida 5 — Reservada, V1 Valentina
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Salida (id_canoa,fecha_hora_reserva,fecha_hora_programada,duracion_estimada_min,fecha_hora_salida_real,fecha_hora_retorno_real,id_estado,id_responsable,zona_recorrido,condicion_clima,es_entrenamiento,id_instructor_asignado,id_equipo,creado_por,fecha_creacion)
                VALUES (2,'20250521 10:00:00','20250525 09:00:00',60,NULL,NULL,1,2,'Zona sur puerto',NULL,0,NULL,NULL,2,GETDATE());
            ");
            int s5 = db.Salidas.OrderByDescending(s => s.Id).Select(s => s.Id).First();

            // Salida 6 — Cancelada por tormenta
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Salida (id_canoa,fecha_hora_reserva,fecha_hora_programada,duracion_estimada_min,fecha_hora_salida_real,fecha_hora_retorno_real,id_estado,id_responsable,zona_recorrido,condicion_clima,es_entrenamiento,id_instructor_asignado,id_equipo,id_motivo_cancelacion,creado_por,fecha_creacion)
                VALUES (5,'20250405 06:00:00','20250405 08:00:00',90,NULL,NULL,5,9,'Bahía de Valparaíso','Tormenta',0,1,1,1,9,GETDATE());
            ");
            int s6 = db.Salidas.OrderByDescending(s => s.Id).Select(s => s.Id).First();

            // ── PARTICIPANTES ─────────────────────────────────────────
            db.Database.ExecuteSqlRaw($@"
                INSERT INTO SalidaParticipante (id_salida,id_remador,id_rol,usa_remo_club,usa_salvavidas_club,confirmo_asistencia) VALUES
                ({s1},1,2,0,0,1),
                ({s2},9,3,0,0,1),({s2},1,1,0,0,1),({s2},2,2,1,0,1),({s2},3,2,0,1,1),({s2},4,2,0,1,1),
                ({s3},5,3,0,0,1),({s3},6,2,1,0,1),
                ({s4},9,3,0,0,1),({s4},1,1,0,0,1),({s4},2,2,1,0,1),({s4},3,2,0,1,1),({s4},4,2,0,1,0),
                ({s5},2,2,1,1,0),
                ({s6},9,3,0,0,1),({s6},1,1,0,0,1),({s6},2,2,1,0,1),({s6},3,2,0,1,1),({s6},4,2,0,1,1);
            ");

            // ── IMPLEMENTOS EN SALIDAS ────────────────────────────────
            db.Database.ExecuteSqlRaw($@"
                INSERT INTO SalidaImplemento (id_salida,id_implemento,id_remador,devuelto) VALUES
                ({s2},1,2,1),({s2},7,2,1),({s2},2,3,1),({s2},8,4,1),
                ({s4},3,2,0),({s4},9,4,0);
            ");
        }

        // ══════════════════════════════════════════════════════════════
        //  EVENTOS — cada INSERT separado para capturar IDs correctos
        // ══════════════════════════════════════════════════════════════
        private static void SeedEventos(AppDbContext db)
        {
            // ── Eventos ───────────────────────────────────────────────
            db.Database.ExecuteSqlRaw(@"
                SET DATEFORMAT ymd;
                INSERT INTO Evento (nombre,id_tipo_evento,id_estado,fecha_inicio,fecha_fin,lugar,organizador,descripcion,fecha_limite_inscripcion,fecha_creacion) VALUES
                ('Regata Regional Valparaíso 2025',1,4,'20250315','20250315','Bahía de Valparaíso', 'Federación Chilena de Canotaje','Regata regional clasificatoria. Participaron 12 clubes.','20250301',GETDATE()),
                ('Travesía Costera Norte 2025',    2,4,'20250420','20250420','Valparaíso - Concón', 'Club de Canotaje Polinésico',   'Travesía anual de 18 km por la costa norte.',            '20250410',GETDATE()),
                ('Campeonato Nacional 2025',       3,2,'20250810','20250812','Lago Rapel',          'Federación Chilena de Canotaje','Campeonato nacional, todas las categorías.',             '20250720',GETDATE()),
                ('Encuentro Amistoso Inter-Clubes',5,1,'20250607','20250607','Viña del Mar',        'Club de Canotaje Polinésico',   'Encuentro amistoso entre clubes de la región.',          '20250530',GETDATE());
            ");

            int e1 = db.Eventos.OrderBy(e => e.Id).Skip(0).Select(e => e.Id).First();
            int e2 = db.Eventos.OrderBy(e => e.Id).Skip(1).Select(e => e.Id).First();
            int e3 = db.Eventos.OrderBy(e => e.Id).Skip(2).Select(e => e.Id).First();

            // ── Inscripciones Evento 1 (Regata) ──────────────────────
            db.Database.ExecuteSqlRaw($@"
                INSERT INTO EventoInscripcion (id_evento,id_equipo,id_remador,id_canoa,categoria_competencia,numero_largada,confirmada,fecha_inscripcion) VALUES
                ({e1},1,   NULL,5,'V6 Abierto',1,1,GETDATE()),
                ({e1},NULL,9,   1,'V1 Elite',  5,1,GETDATE());
            ");

            int i1 = db.EventoInscripciones.OrderBy(i => i.Id).Skip(0).Select(i => i.Id).First();
            int i2 = db.EventoInscripciones.OrderBy(i => i.Id).Skip(1).Select(i => i.Id).First();

            db.Database.ExecuteSqlRaw($@"
                INSERT INTO EventoResultado (id_inscripcion,posicion_final,tiempo_oficial,puntos,descalificado) VALUES
                ({i1},2,'00:28:45', 80.00,0),
                ({i2},1,'00:24:12',100.00,0);
            ");

            // ── Inscripciones Evento 2 (Travesía) ────────────────────
            db.Database.ExecuteSqlRaw($@"
                INSERT INTO EventoInscripcion (id_evento,id_equipo,id_remador,id_canoa,categoria_competencia,numero_largada,confirmada,fecha_inscripcion) VALUES
                ({e2},2,   NULL,6,'V6 Máster',3,1,GETDATE()),
                ({e2},NULL,9,   1,'V1 Elite', 7,1,GETDATE());
            ");

            int i3 = db.EventoInscripciones.OrderBy(i => i.Id).Skip(2).Select(i => i.Id).First();
            int i4 = db.EventoInscripciones.OrderBy(i => i.Id).Skip(3).Select(i => i.Id).First();

            db.Database.ExecuteSqlRaw($@"
                INSERT INTO EventoResultado (id_inscripcion,posicion_final,tiempo_oficial,puntos,descalificado) VALUES
                ({i3},1,'02:15:30',100.00,0),
                ({i4},2,'02:10:05', 90.00,0);
            ");

            // ── Inscripciones Evento 3 (Campeonato) — sin resultado aún
            db.Database.ExecuteSqlRaw($@"
                INSERT INTO EventoInscripcion (id_evento,id_equipo,id_remador,id_canoa,categoria_competencia,confirmada,fecha_inscripcion) VALUES
                ({e3},1,   NULL,5,'V6 Abierto',1,GETDATE()),
                ({e3},NULL,9,   1,'V1 Elite',  1,GETDATE());
            ");
        }
    }
}