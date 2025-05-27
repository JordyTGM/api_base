using Domain.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Data
{
    public class RespuestaDataExportacionDto : ParametrosGenerales
    {
        public string Formato { get; set; } = "PDF";
        public List<ImagenDto> ListaImagenes { get; set; } = new List<ImagenDto>();

    }
}
