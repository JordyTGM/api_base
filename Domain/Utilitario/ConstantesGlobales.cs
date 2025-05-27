using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilitario
{ 
    public static class ConstantesGlobales
    {
        
        public static string CodigoRespuestaExitoso { get; set; } = "00";
        public static long TiempoEsperaCargaImagenDirectorioExternoMs { get; set; } = 3000;
        public static string CodigoRespuestaIngresoDatosIncorrecto { get; set; } = "SD02";
        public static string CodigoRespuestaNoContent { get; set; } = "204";
        public static string Ambiente { get; set; } = "local";

    }
    
}
