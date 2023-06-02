using GenerateLabResult.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLabResultReport
{
  public interface IDataAccessLayer
  {
    DocumentFormat GetSampleDocumentFormat(long sampleID, string outputFolder);
    Requisition GetSampleRequsition(long sampleID);
    Results GetSampleResults(long sampleID);
    string GetSampleBlankReportFilePath(Results results, string outputFolder);
  }
}
