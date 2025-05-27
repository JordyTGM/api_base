namespace WebApiBase.Configuration
{
    public class BaseSetting
    {
        public string codigoRespuestaExitoso { get; set; } = "00";
        public long tiempoEsperaCargaImagenDirectorioExternoMs { get; set; } = 3000;
        public string codigoRespuestaIngresoDatosIncorrecto { get; set; } = "SD02";
        public string codigoRespuestaNoContent { get; set; } = "204";
        public string ambiente { get; set; } = "local";

    }
}
