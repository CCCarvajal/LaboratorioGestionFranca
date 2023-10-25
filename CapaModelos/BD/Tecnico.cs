using System;
using System.Collections.Generic;

namespace CapaModelos.BD;

public class Tecnico
{
    public string Codigo { get; set; } = null!;

    public string? Nombre { get; set; }

    public decimal? SueldoBase { get; set; }

    public string? CodigoSucursal { get; set; }

    public virtual Sucursal? CodigoSucursalNavigation { get; set; }
}
