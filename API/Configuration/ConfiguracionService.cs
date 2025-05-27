using Microsoft.Extensions.Options;
using Domain.Utilitario;


namespace WebApiRetencionClientes.Configuration
{
    public interface IConfigurationService
    {
        void CargarConfiguraciones();
    }
    public class ConfiguracionService(IOptions<BaseSetting> cotizadorSettings) : IConfigurationService
    {
        private readonly BaseSetting _baseSettings = cotizadorSettings.Value;

        public void CargarConfiguraciones()
        {

            ConstantesGlobales.CodigoRespuestaExitoso = _baseSettings.codigoRespuestaExitoso;
            ConstantesGlobales.TiempoEsperaCargaImagenDirectorioExternoMs = _baseSettings.tiempoEsperaCargaImagenDirectorioExternoMs;
            ConstantesGlobales.CodigoRespuestaIngresoDatosIncorrecto = _baseSettings.codigoRespuestaIngresoDatosIncorrecto;
            ConstantesGlobales.CodigoRespuestaNoContent = _baseSettings.codigoRespuestaNoContent;
            ConstantesGlobales.Ambiente = _baseSettings.ambiente;

        }
    }
}
