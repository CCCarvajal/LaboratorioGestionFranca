using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapaModelos.BD;

public partial class RelacionTecnicoElemento
{
    public int Id { get; set; }
    [Required]
    public string? CodigoTecnico { get; set; }
    [Required]
    public string? CodigoElemento { get; set; }
    public int? Cantidad { get; set; }


    public virtual Elemento? CodigoElementoNavigation { get; set; }

    public virtual Tecnico? CodigoTecnicoNavigation { get; set; }
}
