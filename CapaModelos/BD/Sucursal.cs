using System;
using System.Collections.Generic;

namespace CapaModelos.BD;

public partial class Sucursal
{
    public string Codigo { get; set; } = null!;

    public string? Nombre { get; set; }

    public virtual ICollection<Tecnico> Tecnicos { get; set; } = new List<Tecnico>();
}
