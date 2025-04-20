using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Data
{
    public class EventCorpContext : DbContext
    {
        public EventCorpContext(DbContextOptions options) : base(options) { }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<EventoModel> Eventos { get; set; }
        public DbSet<ErrorLog> Errores { get; set; }
        public DbSet<InscripcionModel> Inscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Relaciones

            // Usuario -> Categorías
            modelBuilder.Entity<CategoriaModel>()
                .HasOne(c => c.UsuarioRegistro)
                .WithMany(u => u.CategoriasRegistradas)
                .HasForeignKey(c => c.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario -> Eventos
            modelBuilder.Entity<EventoModel>()
                .HasOne(e => e.UsuarioRegistro)
                .WithMany(u => u.EventosRegistrados)
                .HasForeignKey(e => e.UsuarioRegistroId)
                .OnDelete(DeleteBehavior.Restrict);

            // Evento -> Categoría
            modelBuilder.Entity<EventoModel>()
                .HasOne(e => e.Categoria)
                .WithMany(c => c.Eventos)
                .HasForeignKey(e => e.CategoriaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario -> Errores (uno a muchos con navegación inversa)
            modelBuilder.Entity<ErrorLog>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Errores)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InscripcionModel>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Inscripciones) // Si tienes esta colección en UsuarioModel
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InscripcionModel>()
                .HasOne(i => i.Evento)
                .WithMany(e => e.Inscripciones) // Ya tienes esta colección en EventoModel
                .HasForeignKey(i => i.EventoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Datos Iniciales

            // Datos iniciales para la tabla de usuarios
            modelBuilder.Entity<UsuarioModel>()
                .HasData(
                    new UsuarioModel
                    {
                        Id = 1,
                        NombreUsuario = "admin",
                        NombreCompleto = "Administrador General",
                        Correo = "admin@eventcorp.com",
                        Telefono = "+50688888888",
                        Contrasena = "Admin123",
                        Rol = CoreLibrary.Auth.RolesEnum.Administrador
                    },
                    new UsuarioModel
                    {
                        Id = 2,
                        NombreUsuario = "organizador",
                        NombreCompleto = "Organizador de Eventos",
                        Correo = "organizador@eventcorp.com",
                        Telefono = "+50677777777",
                        Contrasena = "Organizador123",
                        Rol = CoreLibrary.Auth.RolesEnum.Organizador
                    },
                    new UsuarioModel
                    {
                        Id = 3,
                        NombreUsuario = "usuarioUNO",
                        NombreCompleto = "UsuarioUNO Regular",
                        Correo = "usuario1@eventcorp.com",
                        Telefono = "+50666666666",
                        Contrasena = "Usuario123",
                        Rol = CoreLibrary.Auth.RolesEnum.Usuario
                    },
                    new UsuarioModel
                    {
                        Id = 4,
                        NombreUsuario = "usuarioDOS",
                        NombreCompleto = "UsuarioDOS Regular",
                        Correo = "usuario2@eventcorp.com",
                        Telefono = "+50666666664",
                        Contrasena = "Usuario123",
                        Rol = CoreLibrary.Auth.RolesEnum.Usuario
                    }
                );

            // Datos iniciales para la tabla de categorías
            modelBuilder.Entity<CategoriaModel>()
                .HasData(
                    new CategoriaModel
                    {
                        IdCategoria = 1,
                        Nombre = "Conferencia",
                        Descripcion = "Eventos de tipo conferencia.",
                        Estado = true,
                        FechaRegistro = new DateTime(2025, 5, 20),
                        UsuarioRegistroId = 1
                    },
                    new CategoriaModel
                    {
                        IdCategoria = 2,
                        Nombre = "Taller",
                        Descripcion = "Eventos de tipo taller.",
                        Estado = true,
                        FechaRegistro = new DateTime(2025, 5, 10),
                        UsuarioRegistroId = 1
                    }
                );

            // Datos iniciales para la tabla de eventos
            modelBuilder.Entity<EventoModel>()
                .HasData(
                    new EventoModel
                    {
                        Id = 1,
                        Titulo = "Conferencia sobre Innovación Tecnológica",
                        Descripcion = "Evento que reúne a expertos en tecnología para discutir innovaciones recientes.",
                        CategoriaId = 1,
                        Fecha = new DateTime(2025, 5, 10),
                        Hora = new TimeSpan(14, 0, 0),
                        Duracion = 120,
                        Ubicacion = "Centro de Convenciones, San José",
                        CupoMaximo = 100,
                        FechaRegistro = new DateTime(2025, 4, 1),
                        UsuarioRegistroId = 3
                    },
                    new EventoModel
                    {
                        Id = 2,
                        Titulo = "Taller de Desarrollo Web",
                        Descripcion = "Taller práctico sobre tecnologías modernas para el desarrollo web.",
                        CategoriaId = 2, // Referencia a "Taller"
                        Fecha = new DateTime(2025, 6, 5),
                        Hora = new TimeSpan(9, 0, 0), // 9:00 AM
                        Duracion = 180, // 3 horas
                        Ubicacion = "Auditorio Universidad Fidélitas",
                        CupoMaximo = 50,
                        FechaRegistro = new DateTime(2025, 4, 1),
                        UsuarioRegistroId = 3
                    }
                );
            #endregion


            base.OnModelCreating(modelBuilder);
        }
    }
}
