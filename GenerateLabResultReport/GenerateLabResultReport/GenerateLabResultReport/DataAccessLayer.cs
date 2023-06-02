using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateLabResult.Models;

namespace GenerateLabResultReport
{
  public class DataAccessLayer : IDataAccessLayer
  {
    public DocumentFormat GetSampleDocumentFormat(long sampleID, string outputFolder)
    {
      DocumentFormat documentFormat = new DocumentFormat();
      documentFormat.TemplatePath = Properties.Settings.Default.TEMPLATE_PATH;
      documentFormat.RegularFontFilePath = Properties.Settings.Default.REGULAR_FONT_FILE_PATH;
      documentFormat.ItalicsFontFilePath = Properties.Settings.Default.ITALICS_FONT_FILE_PATH;
      documentFormat.BoldRegularFontFilePath = Properties.Settings.Default.BOLD_REGULAR_FONT_FILE_PATH;
      documentFormat.BoldItalicsFontFilePath = Properties.Settings.Default.BOLD_ITALICS_FONT_FILE_PATH;

      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.DOCUMENT_VERSION_NO_AND_AMEND_QUERY;
          cmd.Parameters.AddWithValue("pSampleID", sampleID);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              if (!reader.IsDBNull(i)) documentFormat.OutputFilePath = Path.Combine(outputFolder, $"{sampleID}_{reader.GetInt32(i)}.pdf"); i++;
              if (!reader.IsDBNull(i)) documentFormat.IsAmended = reader.GetBoolean(i); i++;
            }
          }
        }
      }

      return documentFormat;
    }

    public Requisition GetSampleRequsition(long sampleID)
    {
      Requisition requisition = new Requisition();

      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.REQUISITION_INFO_QUERY;
          cmd.Parameters.AddWithValue("pSampleID", sampleID);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              if (!reader.IsDBNull(i)) requisition.PatientFirstName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PatientLastName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PatientMiddleName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.ClientPatientID = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PhysicianFirstName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PhysicianLastName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PhysicianMiddleName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PatientGender = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.PatientAgeInYears = reader.GetInt32(i); i++;
              if (!reader.IsDBNull(i)) requisition.PatientDOB = reader.GetDateTime(i); i++;
              if (!reader.IsDBNull(i)) requisition.PhysicianPhone = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.SpecimenSource = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.OtherSpecimenSourceSpecified = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.HasCollectionDateInd = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.HasCollectionTimeInd = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.SpecimenCollectionDate = reader.GetDateTime(i); i++;
              if (!reader.IsDBNull(i)) requisition.ClientName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.SpecimenReceiveDate = reader.GetDateTime(i); i++;
              if (!reader.IsDBNull(i)) requisition.TRLClientID = reader.GetInt64(i); i++;
              if (!reader.IsDBNull(i)) requisition.SpecimenID = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.AccessionID = reader.GetInt64(i); i++;
              if (!reader.IsDBNull(i)) requisition.ReleaseDate = reader.GetDateTime(i); i++;
              if (!reader.IsDBNull(i)) requisition.ClientAddress = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.ClientCityPlusState = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) requisition.ClientZIP = reader.GetString(i); i++;
            }
          }
        }
      }

      return requisition;
    }

    public Results GetSampleResults(long sampleID)
    {
      Results results = new Results();

      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.RESULTS_BASE_QUERY;
          cmd.Parameters.AddWithValue("pSampleID", sampleID);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              if (!reader.IsDBNull(i)) results.ReportPanelName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.IsPendingCultureAndSensitvityResults = reader.GetBoolean(i); i++;
              if (!reader.IsDBNull(i)) results.LaboratoryDevelopedTestDisclaimer = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.TechnologyUsedDisclaimer = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.ExtraTechnologyUsedDisclaimer = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.ShowTechnologyTrademark = reader.GetBoolean(i); i++;
              if (!reader.IsDBNull(i)) results.PatentOfTechnology = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.FooterPhrase = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.ShowFooterPhraseTrademark = reader.GetBoolean(i); i++;
              if (!reader.IsDBNull(i)) results.DocumentVersion = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) results.TestNotPerformed = reader.GetBoolean(i); i++;
              if (!reader.IsDBNull(i)) results.Comments = reader.GetString(i); i++;
            }
          }
        }
      }

      results.PanelTests = GetSamplePanelTests(sampleID);
      results.FormattedResults = GetSampleFormattedResults(sampleID);
      results.ManualResults = GetSampleManualResults(sampleID);

      return results;
    }

    public string GetSampleBlankReportFilePath(Results results, string outputFolder)
    {
      string blankReportFilePath = string.Empty;
      string panelName = string.Empty;
      long panelID = 0;
      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.PANEL_NAME_AND_PANEL_ID_QUERY;
          cmd.Parameters.AddWithValue("pReportPanelName", results.ReportPanelName);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              if (!reader.IsDBNull(i)) panelName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) panelID = reader.GetInt64(i); i++;
            }
          }
        }
      }

      blankReportFilePath = Path.Combine(outputFolder, $"{panelName}_{panelID}.pdf");

      return blankReportFilePath;
    }

    private PanelTest[] GetSamplePanelTests(long sampleID)
    {
      List<PanelTest> panelTests = new List<PanelTest>();

      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.PANEL_TESTS_QUERY;
          cmd.Parameters.AddWithValue("pSampleID", sampleID);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              PanelTest test = new PanelTest();
              if (!reader.IsDBNull(i)) test.TestName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) test.IndentInd = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) test.Visible = reader.GetBoolean(i); i++;
              panelTests.Add(test);
            }
          }
        }
      }

      return panelTests.ToArray();
    }

    private FormattedResult[] GetSampleFormattedResults(long sampleID)
    {
      List<FormattedResult> formattedResults = new List<FormattedResult>();

      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.FORMATTED_RESULTS_QUERY;
          cmd.Parameters.AddWithValue("pSampleID", sampleID);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              FormattedResult result = new FormattedResult();
              if (!reader.IsDBNull(i)) result.TestName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) result.Result = reader.GetInt32(i); i++;
              if (!reader.IsDBNull(i)) result.HasTargetResult = reader.GetBoolean(i); i++;
              if (!reader.IsDBNull(i)) result.Visible = reader.GetBoolean(i); i++;
              if (!reader.IsDBNull(i)) result.DetectedString = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) result.NotDetectedString = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) result.CenterTestNotPerformed = reader.GetBoolean(i); i++;
              formattedResults.Add(result);
            }
          }
        }
      }

      return formattedResults.ToArray();
    }

    private ManualResult[] GetSampleManualResults(long sampleID)
    {
      List<ManualResult> manualResults = new List<ManualResult>();

      using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.CONNECTION_STRING))
      {
        using (SqlCommand cmd = conn.CreateCommand())
        {
          cmd.Connection.Open();
          cmd.CommandText = Properties.Settings.Default.MANUAL_RESULTS_QUERY;
          cmd.Parameters.AddWithValue("pSampleID", sampleID);
          using (SqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int i = 0;
              ManualResult result = new ManualResult();
              if (!reader.IsDBNull(i)) result.TestName = reader.GetString(i); i++;
              if (!reader.IsDBNull(i)) result.ManualResultValue = reader.GetString(i); i++;
              manualResults.Add(result);
            }
          }
        }
      }

      return manualResults.ToArray();
    }

  }
}
