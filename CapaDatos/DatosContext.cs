using CapaModelos.BD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CapaDatos
{
    public class DatosContext : DbContext
    {
        public DatosContext(DbContextOptions<DatosContext> options) : base(options) { }

        public DbSet<Tecnico> Tecnico { get; set; }
        public DbSet<Sucursal> Sucursal { get; set; }
        public DbSet<RelacionTecnicoElemento> RelacionTecnicoElemento { get; set; }
        public DbSet<Elemento> Elementos { get; set; }

    }
}
