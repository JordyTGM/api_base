using Azure.Core;
using Domain.DTO.CoberturaPlan;
using Domain.Utilitario;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static iText.IO.Image.Jpeg2000ImageData;
using static System.Net.WebRequestMethods;

namespace Application.CoberturaPlan
{
    public class CoberturaPlanQuery
    {
        public class Query : IRequest<RespuestaCoberturasDto>
        {
            public required SolicitudCoberturasDto SolicitudCoberturasDto { get; set; }
        }

        public static RespuestaCoberturasDto ValidarRequest(Query request)
        {
            string mensaje = "";
            if (request.SolicitudCoberturasDto == null ||
                request.SolicitudCoberturasDto.ListaPlanesCotizar.Count == 0
               )
            {
                mensaje = "Campos incompletos para solicitud.";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return new RespuestaCoberturasDto
                {
                    Mensaje = mensaje,
                    CodigoRespuesta = ConstantesGlobales.CodigoRespuestaExitoso
                };
            }
            else
            {
                return new RespuestaCoberturasDto
                {
                    Mensaje = mensaje,
                    CodigoRespuesta = ConstantesGlobales.CodigoRespuestaIngresoDatosIncorrecto
                };
            }
        }
        public class Handler(CoberturaPlanService coberturaPlanService, ILogger<Handler> logger, IMemoryCache memoryCache) : IRequestHandler<Query, RespuestaCoberturasDto>
        {
            private readonly CoberturaPlanService _coberturaPlanService = coberturaPlanService;
            private readonly ILogger<Handler> _logger = logger;
            private readonly IMemoryCache _memoryCache = memoryCache;

            /// <summary>
            /// Metodo que me permite obtener las imagenes de coberturas de los planes cotizados.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<RespuestaCoberturasDto> Handle(Query request, CancellationToken cancellationToken)
            {
                RespuestaCoberturasDto respuesta = ValidarRequest(request);
                if (!respuesta.CodigoRespuesta.Equals(ConstantesGlobales.CodigoRespuestaExitoso))
                {
                    return respuesta;
                }

                respuesta.ListaPlanesCotizar = await _coberturaPlanService.ObtenerListaUrlsImagenes(request.SolicitudCoberturasDto, cancellationToken);
                if (respuesta.ListaPlanesCotizar.Count == 0)
                {
                    respuesta.Mensaje = "No se encontraron rutas de las coberturas.";
                    respuesta.CodigoRespuesta = ConstantesGlobales.CodigoRespuestaNoContent;
                }
                else
                {
                    respuesta.ListaPlanesCotizar = await _coberturaPlanService.AsignarRutasBaseAImagenes(respuesta.ListaPlanesCotizar, cancellationToken);
                    try
                    {
                        respuesta.ListaPlanesCotizar = await CoberturaPlanOperaciones.ObtenerImagenBase64(respuesta.ListaPlanesCotizar, _logger, _memoryCache);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Problemas al obtener las imágenes del directorio");
                    }
                }
                return respuesta;
            }

        }
    }
}
