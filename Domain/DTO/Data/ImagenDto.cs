using Domain.DTO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Data
{
    public class ImagenDto
    {
        public string Nombre { get; set; } = "IMAGEN";
        public int Weight { get; set; }
        public int Height { get; set; }
        public string Formato { get; set; } = "PNG";
        public string Base64 { get; set; } = "";

    }
}
