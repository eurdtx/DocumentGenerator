using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DocumentGenerator.WebAPI.Models;
using DocumentGenerator;

namespace DocumentGenerator.WebAPI.Controllers
{
  public class PatientReportGeneratorController : ApiController
  {
    [HttpGet]
    [Route("PatientReportGenerator")]
    public IEnumerable<string> Get()
    {
      return new string[] { "This is the Patient Report Generator." };
    }

    [HttpPost]
    [Route("PatientReportGenerator")]
    public IHttpActionResult Post([FromBody] ResultReport report)
    {
      bool completedNormally = false;
      Exception error = null;
      string errorMessage = string.Empty;
      try
      {
        string outputFilePath = string.Empty;
        ResultsReportGenerator generator = new ResultsReportGenerator(report.DocumentFormat, report.Requisition, report.Results);
        if (!generator.CreatePatientResultsReport(out outputFilePath, out errorMessage))
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