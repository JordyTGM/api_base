using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Base
{
    public class ParametrosGenerales
    {
        public string CodigoRespuesta { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
        public string? MensajeLog { get; set; }
        public string? Base64 { get; set; }
        public int TotalRegistros { get; set; }
    }
}
