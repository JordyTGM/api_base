using Domain.DTO.CoberturaPlan;
using Domain.Utilitario;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CoberturaPlan
{
    public static class CoberturaPlanOperaciones
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async static Task<List<CoberturaImagenDto>> ObtenerImagenBase64(
            List<CoberturaImagenDto> listaImagenesPlanesCotizar,
            ILogger logger,
            IMemoryCache memoryCache)
        {
            foreach (CoberturaImagenDto cobertura in listaImagenesPlanesCotizar)
            {
                string cacheKey = $"ImagenBase64_{cobertura.NombreImagen}";
                string rutaImagen = $"{cobertura.URL}{cobertura.NombreImagen}";
                if (!File.Exists(rutaImagen))
                {
                    logger.LogWarning("No se encontró la imagen: {RutaImagen}", rutaImagen);
                    continue;
                }


                DateTime lastModified = File.GetLastWriteTimeUtc(rutaImagen);

                if (!memoryCache.TryGetValue(cacheKey, out (string Base64, DateTime LastModified) cachedData)
                    || cachedData.LastModified < lastModified)
                {
                    await _semaphore.WaitAsync();

                    try
                    {
                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                        byte[] imageBytes = await File.ReadAllBytesAsync(rutaImagen);
                        string base64 = Convert.ToBase64String(imageBytes);
                        cobertura.Base64 = base64;
                        memoryCache.Set(cacheKey, (base64, lastModified), TimeSpan.FromMinutes(10));

                        stopwatch.Stop();
                        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                        if (elapsedMilliseconds > ConstantesGlobales.TiempoEsperaCargaImagenDirectorioExternoMs)
                        {
                            long fileSize = new FileInfo(rutaImagen).Length;
                            logger.LogInformation(
                                "Demora al obtener la imagen del directorio: {ElapsedMilliseconds} ms para la ruta {RutaImagen}, Tamaño: {FileSize} bytes",
                                elapsedMilliseconds, rutaImagen, fileSize
                            );
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error al procesar la imagen: {RutaImagen}", rutaImagen);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                else
                {
                    cobertura.Base64 = cachedData.Base64;
                }
            }

            return listaImagenesPlanesCotizar;
        }

    }
}
