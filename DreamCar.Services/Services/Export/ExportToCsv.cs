using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DreamCar.Services.Services.Export
{
    public class ExportToCsv : FileResult
    {
        private readonly Dictionary<string, string> _dataToCsvFile;
        public ExportToCsv(Dictionary<string, string> dataToCsv, string fileDownloadName) : base("text/csv")
        {
            _dataToCsvFile = dataToCsv;
            FileDownloadName = fileDownloadName;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

            using var streamWriter = new StreamWriter(response.Body);
            foreach (var row in _dataToCsvFile)
            {
                await streamWriter.WriteLineAsync(
                    $"{row.Key}:\t{row.Value}"
                );
                await streamWriter.FlushAsync();
            }

        }
    }
}
