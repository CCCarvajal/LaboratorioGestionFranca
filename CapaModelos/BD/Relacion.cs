using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelos.BD
{
    public partial class Relacion
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal SueldoBase { get; set; }
        public string CodigoSucursal { get; set; }
        public Sucursal Sucursal { get; set; }
        public List<RelacionTecnicoElemento> Relaciones { get; set; }
    }
}
