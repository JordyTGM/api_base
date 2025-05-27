using Domain.DTO.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Domain.Utilitario;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using System.Drawing;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas;
using ImageMagick;


namespace Application.Data
{
    public class DataService
    {

        protected DataService()
        {

        }

        public static async Task<RespuestaDataExportacionDto> ExtraerImagenDePdf(SolicitudDataExportacionDto solicitudDataDto, CancellationToken cancellationToken)
        {
            RespuestaDataExportacionDto respuestaDataDto = new RespuestaDataExportacionDto();

            byte[] pdfBytes = Convert.FromBase64String(solicitudDataDto.Base64);

            await using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
            {
                using (MagickImage image = new MagickImage(pdfStream))
                {
                    await using (MemoryStream imageStream = new MemoryStream())
                    {
                        byte[] imageBytes = imageStream.ToArray();
                        respuestaDataDto.Base64 = Convert.ToBase64String(imageBytes);
                    }
                }
            }

            respuestaDataDto.CodigoRespuesta = ConstantesGlobales.CodigoRespuestaExitoso;
            return respuestaDataDto;

        }
    }
}
