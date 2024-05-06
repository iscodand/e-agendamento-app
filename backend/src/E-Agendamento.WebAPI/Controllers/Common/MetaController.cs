using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace E_Agendamento.WebAPI.Controllers.Common
{
    public class MetaController : BaseController
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(StartupBase).Assembly;
            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;

            return Ok($"Version {version}, Last Updated: {lastUpdate}");
        }
    }
}