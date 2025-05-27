using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.CoberturaPlan
{
    public class CoberturaImagenDto
    {

        public string NombreImagen { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string Base64 { get; set; } = string.Empty;
        public string Formato { get; set; } = "JPG";

    }
}
