using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DreamCar.Services.Services.Export
{
    public class ExportToCsvService : FileResult
    {
        private readonly Dictionary<string, string> _dataToCsvFile;
        public ExportToCsvService(Dictionary<string, string> dataToCsv, string fileDownloadName) : base("text/csv")
        {
            _dataToCsvFile = dataToCsv;
            FileDownloadName = fileDownloadName;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

            await using var streamWriter = new StreamWriter(response.Body);
            foreach (var (key, value) in _dataToCsvFile)
            {
                await streamWriter.WriteLineAsync(
                    $"{key}:\t{value}"
                );
                await streamWriter.FlushAsync();
            }

        }
    }
}
