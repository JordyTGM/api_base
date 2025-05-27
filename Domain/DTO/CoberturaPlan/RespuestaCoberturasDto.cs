using Domain.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.CoberturaPlan
{
    public class RespuestaCoberturasDto : ParametrosGenerales
    {
        public List<CoberturaImagenDto> ListaPlanesCotizar { get; set; } = new List<CoberturaImagenDto>();

    }
}
