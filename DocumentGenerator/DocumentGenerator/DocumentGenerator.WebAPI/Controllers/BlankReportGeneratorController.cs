using DocumentGenerator.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocumentGenerator.WebAPI.Controllers
{
  public class BlankReportGeneratorController : ApiController
  {
    [HttpGet]
    [Route("BlankReportGenerator")]
    public IEnumerable<string> Get()
    {
      return new string[] { "This is the Blank Report Generator." };
    }

    [HttpPost]
    [Route("BlankReportGenerator")]
    public IHttpActionResult Post([FromBody] ResultReport report)
    {
      bool completedNormally = false;
      Exception error = null;
      string errorMessage = string.Empty;
      try
      {
        string outputFilePath = string.Empty;
        ResultsReportGenerator generator = new ResultsReportGenerator(report.DocumentFormat, report.Requisition, report.Results);
        if (!generator.CreateBlankResultsReport(out outputFilePath, out errorMessage))
          throw new Exception(errorMessage);

        completedNormally = true;
      }
      catch (Exception ex)
      {
        error = ex;
      }

      if (!completedNormally)
        return InternalServerError(error);

      return Ok();
    }
  }
}