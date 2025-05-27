using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Data
{
    public class SolicitudDataExportacionDto
    {
        public string Formato { get; set; } = "PNG";
        public string Base64 { get; set; } = string.Empty;
        public string Modulo { get; set; } = "COTIZACION";
        public string Documento { get; set; } = "PDF";
        
    }
}
