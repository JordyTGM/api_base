using System;
using System.Collections.Generic;

namespace Persistence.EntitiesCotizacionSAC;

public partial class Pr09PlanCoberturaImagen
{
    public int CodigoCobertura { get; set; }

    public string CodigoPlan { get; set; } = null!;

    public string NombreImagen { get; set; } = null!;

    public string Url { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
