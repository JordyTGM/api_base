using Domain.DTO.CoberturaPlan;
using Domain.Utilitario;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CoberturaPlan
{
    public class CoberturaPlanService(Context context)
    {
        private readonly Context _context = context;
        private static readonly string[] clavesParametrosRutasArray = ["RutaBaseImagenes", "SeparadorRuta"];

        public async Task<List<CoberturaImagenDto>> AsignarRutasBaseAImagenes(List<CoberturaImagenDto> listaCoberturas, CancellationToken cancellationToken)
        {
            var rutasBaseImagenes = await (
                            from pr00 in _context.Pr00ParametrizacionGenerals
                            where pr00.Ambiente.Equals(ConstantesGlobales.Ambiente)
                            && pr00.Categoria.Equals("Rutas") && (clavesParametrosRutasArray).Contains(pr00.Clave)
                            select new
                            {
                                pr00.Ambiente,
                                pr00.Categoria,
                                pr00.Clave,
                                pr00.Valor,
                                pr00.Descripcion
                            }
                        ).ToListAsync(cancellationToken);


            var rutaBaseObj = rutasBaseImagenes.Find(x => x.Clave.Equals("RutaBaseImagenes"));
            var separadorObj = rutasBaseImagenes.Find(x => x.Clave.Equals("SeparadorRuta"));

            if (rutaBaseObj != null && separadorObj != null)
            {
                string rutaBaseImagenes = rutaBaseObj.Valor;
                string separadorRuta = separadorObj.Valor;
                foreach (CoberturaImagenDto imagen in listaCoberturas)
                {
                    imagen.URL = $"{rutaBaseImagenes}{imagen.URL}";
                    if (!separadorRuta.Equals("/"))
                    {
                        imagen.URL = imagen.URL.Replace("/", separadorRuta);
                    }
                }

            }
            else
            {
                foreach (CoberturaImagenDto imagen in listaCoberturas)
                {
                    imagen.URL = $"/app/SoportePlanesCotizador{imagen.URL}";
                }
            }
            return listaCoberturas;
        }
        public async Task<List<CoberturaImagenDto>> ObtenerListaUrlsImagenes(SolicitudCoberturasDto solicitudCoberturasDto, CancellationToken cancellationToken)
        {

            var listaPlanes = solicitudCoberturasDto.ListaPlanesCotizar.Select(plan => plan.Trim()).ToList();
            var imagenesRutas = await (
                from pr09 in _context.Pr09PlanCoberturaImagens
                where listaPlanes.Contains(pr09.CodigoPlan)
                group pr09 by new { pr09.CodigoPlan, pr09.Url, pr09.NombreImagen } into g
                select new
                {
                    g.Key.CodigoPlan,
                    Cobertura = new CoberturaImagenDto
                    {
                        URL = g.Key.Url,
                        NombreImagen = g.Key.NombreImagen,
                        Formato = "jpg"
                    }
                }
            ).ToListAsync(cancellationToken);

            var hashSetNombres = new HashSet<string>();
            var listaCoberturas = new List<CoberturaImagenDto>();

            listaCoberturas.AddRange(
                listaPlanes.SelectMany(codigoPlan =>
                    imagenesRutas
                        .Where(x => x.CodigoPlan == codigoPlan)
                        .Select(x => x.Cobertura)
                        .Where(coberturaImg => hashSetNombres.Add(coberturaImg.NombreImagen))
                        .OrderBy(x => x.NombreImagen)
                        
                )
            );

            return listaCoberturas;

        }

    }

}
