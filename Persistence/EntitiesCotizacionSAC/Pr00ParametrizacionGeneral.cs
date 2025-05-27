using System;
using System.Collections.Generic;

namespace Persistence.EntitiesCotizacionSAC;

public partial class Pr00ParametrizacionGeneral
{
    public int IdParametro { get; set; }

    public string Ambiente { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string? Valor { get; set; }

    public string? Descripcion { get; set; }

    public string TipoDato { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public string? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? UsuarioActualizacion { get; set; }
}
