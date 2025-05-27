using Domain.DTO.Data;
using Domain.Utilitario;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Persistence;
using Persistence.Contexts;
using Persistence.EntitiesCotizacionSAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Application.Data;
using System.Runtime.Serialization;

namespace Application.Contratos
{
    public class ObtenerImagenesCotizacionQuery
    {
        public class Query : IRequest<RespuestaDataExportacionDto>
        {
            public SolicitudDataExportacionDto? SolicitudDataExportacionDto { get; set; }
        }

        public class Handler : IRequestHandler<Query, RespuestaDataExportacionDto>
        {

            public Handler()
            {
            }

            public async Task<RespuestaDataExportacionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request == null || request.SolicitudDataExportacionDto == null)
                {
                    return new RespuestaDataExportacionDto
                    {
                        TotalRegistros = 0,
                        Mensaje = "Información para tranformar pdf incompleta..",
                        CodigoRespuesta = ConstantesGlobales.CodigoRespuestaIngresoDatosIncorrecto
                    };
                }

                var dtoSolicitud = request.SolicitudDataExportacionDto;
                
                RespuestaDataExportacionDto dataRespuestaDto = await DataService.ExtraerImagenDePdf(dtoSolicitud, cancellationToken);

                return dataRespuestaDto;


            }
        }


    }
}
