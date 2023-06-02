using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentGenerator.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DocumentGenerator
{
  public class ResultsReportGenerator
  {
    private readonly DocumentFormat _documentFormat;
    private readonly Requisition _req;
    private readonly Results _results;

    private enum ResultsReportEnum { Pending = 0, NotDetected = 1, Detected = 2, IndividualTNP = 3 }

    public ResultsReportGenerator(DocumentFormat documentFormat, Requisition requisition, Results results)
    {
      _documentFormat = documentFormat;
      _req = requisition;
      _results = results;
    }

    public bool CreatePatientResultsReport(out string outputFilePath, out string errorMessage)
    {
      bool completedNormally = false;
      outputFilePath = string.Empty;
      errorMessage = string.Empty;
      try
      {
        bool isBlankReport = false;
        using (PdfReader pdfReader = new PdfReader(_documentFormat.TemplatePath))
        {
          CreateDocument(pdfReader, isBlankReport);
        }
        outputFilePath = _documentFormat.OutputFilePath;
        completedNormally = true;
      }
      catch (Exception ex)
      {
        errorMessage = $"CreatePatientResultsReport(): {ex.Message}";
      }
      return completedNormally;
    }

    public bool CreateBlankResultsReport(out string outputFilePath, out string errorMessage)
    {
      bool completedNormally = false;
      outputFilePath = string.Empty;
      errorMessage = string.Empty;
      try
      {
        bool isBlankReport = true;
        using (PdfReader pdfReader = new PdfReader(_documentFormat.TemplatePath))
        {
          CreateDocument(pdfReader, isBlankReport);
        }
        outputFilePath = _documentFormat.OutputFilePath;
        completedNormally = true;
      }
      catch (Exception ex)
      {
        errorMessage = $"CreateBlankResultsReport(): {ex.Message}";
      }
      return completedNormally;
    }

    private void CreateDocument(PdfReader pdfReader, bool isBlankReport)
    {
      using (Document doc = new Document())
      {
        CreateFile(pdfReader, doc, isBlankReport);
      }
    }

    private void CreateFile(PdfReader pdfReader, Document doc, bool isBlankReport)
    {
      using (FileStream fs = new FileStream(_documentFormat.OutputFilePath, FileMode.Create, FileAccess.Write))
      {
        CreatePDF(pdfReader, doc, fs, isBlankReport);
      }
    }

    private void CreatePDF(PdfReader pdfReader, Document doc, FileStream fs, bool isBlankReport)
    {
      using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
      {
        writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
        writer.SetFullCompression();

        doc.Open();
        PdfContentByte cb = writer.DirectContent;

        CreateReport(pdfReader, doc, fs, writer, cb, isBlankReport);

        if(writer.PageNumber == 1)
        {
          PdfImportedPage page = writer.GetImportedPage(pdfReader, 1);
          cb.AddTemplate(page, -15, 55);
        }

        cb.SetFlatness(0);

        doc.Close();
        fs.Close();
        writer.Close();
        pdfReader.Close();
      }
    }

    private void CreateReport(PdfReader pdfReader, Document doc, FileStream fs, PdfWriter writer, PdfContentByte cb, bool isBlankReport)
    {
      CreateTestLayer(writer, cb, isBlankReport);
    }

    private void CreateTestLayer(PdfWriter writer, PdfContentByte cb, bool isBlankReport)
    {
      PdfLayer layer = new PdfLayer("testLayer", writer);
      cb.BeginLayer(layer);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetCharacterSpacing(-.50f);
      DrawHeader(cb, isBlankReport);
      DrawFooter(cb);
      DrawTests(writer, cb, isBlankReport);
      DrawComments(cb);
      DrawGreyLineSeparators(cb);
      cb.SetCharacterSpacing(0);
      cb.EndLayer();
    }

    private void DrawHeader(PdfContentByte cb, bool isBlankReport)
    {
      if (_documentFormat.IsAmended && !isBlankReport)
        AddAmendedReportText(cb);
      DrawHeaderBoxes(cb);
      AddTextToHeaderBoxes(cb, isBlankReport);
    }

    private void DrawHeaderBoxes(PdfContentByte cb)
    {
      DrawBox(cb, 14f, 750, 14f, 763, 292, 763, 292, 750, BaseColor.BLACK, BaseColor.BLACK);
      DrawBox(cb, 14f, 707, 14f, 745, 292, 745, 292, 707, BaseColor.LIGHT_GRAY, BaseColor.WHITE);
      DrawBox(cb, 303, 750, 303, 763, 580, 763, 580, 750, BaseColor.BLACK, BaseColor.BLACK);
      DrawBox(cb, 303, 707, 303, 745, 580, 745, 580, 707, BaseColor.LIGHT_GRAY, BaseColor.WHITE);
      DrawBox(cb, 14f, 690, 14f, 703, 292, 703, 292, 690, BaseColor.BLACK, BaseColor.BLACK);
      DrawBox(cb, 14f, 620, 14f, 685, 292, 685, 292, 620, BaseColor.LIGHT_GRAY, BaseColor.WHITE);
      DrawBox(cb, 303, 690, 303, 703, 580, 703, 580, 690, BaseColor.BLACK, BaseColor.BLACK);
      DrawBox(cb, 303, 620, 303, 685, 580, 685, 580, 620, BaseColor.LIGHT_GRAY, BaseColor.WHITE);
      DrawBox(cb, 14f, 595, 14f, 609, 295, 609, 295, 595, BaseColor.BLACK, BaseColor.BLACK);
      DrawBox(cb, 427, 595, 427, 609, 580, 609, 580, 595, BaseColor.BLACK, BaseColor.BLACK);

      DrawLine(cb, 300, 609, 350, 609, BaseColor.BLACK);
      DrawLine(cb, 355, 595, 420, 595, BaseColor.BLACK);
      DrawLine(cb, 300, 595, 350, 595, BaseColor.BLACK);
      DrawLine(cb, 355, 609, 420, 609, BaseColor.BLACK);

      if (_results.IsPendingCultureAndSensitvityResults)
        DrawBox(cb, 14f, 200, 14f, 213, 292, 213, 292, 200, BaseColor.BLACK, BaseColor.BLACK);
    }

    private void DrawBox(PdfContentByte cb, float xPoint1, float yPoint1, float xPoint2, float yPoint2, float xPoint3, float yPoint3, float xPoint4, float yPoint4, BaseColor strokeColor, BaseColor fillColor)
    {
      cb.MoveTo(xPoint1, yPoint1);
      cb.LineTo(xPoint2, yPoint2);
      cb.LineTo(xPoint3, yPoint3);
      cb.LineTo(xPoint4, yPoint4);
      cb.SetColorStroke(strokeColor);
      cb.SetColorFill(fillColor);
      cb.ClosePathFillStroke();
    }

    private void DrawLine(PdfContentByte cb, float xPoint1, float yPoint1, float xPoint2, float yPoint2, BaseColor strokeColor)
    {
      cb.MoveTo(xPoint1, yPoint1);
      cb.LineTo(xPoint2, yPoint2);
      cb.SetColorStroke(strokeColor);
      cb.Stroke();
    }

    private void AddTextToHeaderBoxes(PdfContentByte cb, bool isBlankReport)
    {
      AddTitleTextToHeader(cb);
      AddLabelTextToHeader(cb);
      if (!isBlankReport) 
        AddValueTextToHeader(cb);
    }

    private void AddTitleTextToHeader(PdfContentByte cb)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      float fSize = GetTitleTextFontSize(_results.ReportPanelName);
      DrawRequisitionLabels(cb, boldFont, fSize);
      DrawDetectionLabels(cb, boldFont);

      if (_results.IsPendingCultureAndSensitvityResults)
      {
        DrawPendingCultureAndSensitivityLabel(cb, boldFont, fSize);
        DrawPendingCultureAndSensitivityValue(cb);
      }
    }

    private void DrawRequisitionLabels(PdfContentByte cb, BaseFont font, float size)
    {
      cb.SetColorFill(BaseColor.WHITE);
      cb.SetFontAndSize(font, size);
      cb.BeginText();
      DrawText(cb, 20f, 753, "PATIENT:");
      DrawText(cb, 308, 753, "ORDERING PHYSICIAN:");
      DrawText(cb, 20f, 693, "SPECIMEN:");
      DrawText(cb, 308, 693, "CLIENT:");
      DrawText(cb, 20f, 598, _results.ReportPanelName + ":");
      DrawText(cb, 430, 598, "COMMENTS:");
      cb.EndText();
    }

    private void DrawDetectionLabels(PdfContentByte cb, BaseFont font)
    {
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(font, 9);
      cb.BeginText();
      DrawText(cb, 302, 599, "DETECTED");
      DrawText(cb, 355, 599, "NOT DETECTED");
      cb.EndText();
    }

    private void DrawPendingCultureAndSensitivityLabel(PdfContentByte cb, BaseFont font, float size)
    {
      cb.SetColorFill(BaseColor.WHITE);
      cb.SetFontAndSize(font, size);
      cb.BeginText();
      DrawText(cb, 20f, 203, "Culture and Sensitivity:");
      cb.EndText();
    }

    private void DrawPendingCultureAndSensitivityValue(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 10);
      cb.BeginText();
      DrawText(cb, 21.5f, 190, "Pending");
      cb.EndText();
    }

    private BaseFont GetFont(string filePath)
    {
      return BaseFont.CreateFont(filePath, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    }

    private float GetTitleTextFontSize(string reportPanelName)
    {
      float fSize = 10;

      if (reportPanelName.Length >= 40)
        fSize = 9;

      if (reportPanelName.Length >= 50)
      {
        System.Drawing.Image tempImage = new System.Drawing.Bitmap(1, 1);
        System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(tempImage);
        float maxWidth = 425;

        PrivateFontCollection collection = new PrivateFontCollection();
        collection.AddFontFile(_documentFormat.RegularFontFilePath);
        System.Drawing.FontFamily fontFamily = collection.Families[0];
        System.Drawing.Font changeFont = new System.Drawing.Font(fontFamily, 10, System.Drawing.FontStyle.Bold);
        System.Drawing.Font outputFont;
        ResizeFont(graphic, maxWidth, reportPanelName, changeFont, out outputFont);
        fSize = outputFont.Size;
      }

      return fSize;
    }

    private bool ResizeFont(System.Drawing.Graphics e, float maxWidth, string text, System.Drawing.Font inputFont, out System.Drawing.Font outputFont)
    {
      System.Drawing.SizeF size;
      outputFont = inputFont;
      const float DECREMENT_SIZE = 0.5f;
      bool fits = false;
      do
      {
        size = e.MeasureString(text, outputFont);
        if (size.Width <= DECREMENT_SIZE)
          break;
        else if (size.Width > maxWidth)
          outputFont = new System.Drawing.Font(outputFont.FontFamily, (outputFont.Size - DECREMENT_SIZE), outputFont.Style);
        else
          fits = true;
      } while (!fits);

      return fits;
    }

    private void DrawText(PdfContentByte cb, float x, float y, string text)
    {
      cb.SetTextMatrix(x, y);
      cb.ShowText(text);
    }

    private void AddLabelTextToHeader(PdfContentByte cb)
    {
      DrawPatientLabels(cb);
      DrawOrderingPhysicianLabels(cb);
      DrawSpecimenLabels(cb);
      DrawClientLabels(cb);
    }

    private void DrawPatientLabels(PdfContentByte cb)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(boldFont, 9);
      cb.BeginText();
      DrawText(cb, 177, 728, "ID:");
      DrawText(cb, 20f, 714, "Gender:");
      DrawText(cb, 70f, 714, "Age:");
      DrawText(cb, 110, 714, "DOB:");
      cb.EndText();
    }

    private void DrawOrderingPhysicianLabels(PdfContentByte cb)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(boldFont, 9);
      cb.BeginText();
      DrawText(cb, 308, 728, "Name:");
      DrawText(cb, 308, 714, "Phone:");
      cb.EndText();
    }

    private void DrawSpecimenLabels(PdfContentByte cb)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(boldFont, 9);
      cb.BeginText();
      DrawText(cb, 20f, 670, "Source:");
      DrawText(cb, 166, 670, "Collected:");
      DrawText(cb, 166, 656, "Received:");
      DrawText(cb, 20f, 656, "Specimen ID:");
      DrawText(cb, 20f, 642, "Accession ID:");
      DrawText(cb, 166, 642, "Reported:");
      cb.EndText();
    }

    private void DrawClientLabels(PdfContentByte cb)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(boldFont, 9);
      cb.BeginText();
      DrawText(cb, 308, 670, "Name:");
      DrawText(cb, 308, 656, "Code:");
      DrawText(cb, 308, 642, "Address:");
      cb.EndText();
    }

    private void AddValueTextToHeader(PdfContentByte cb)
    {
      FillPatientHeader(cb);
      FillOrderingPhysicianHeader(cb);
      FillSpecimenHeader(cb);
      FillClientHeader(cb);
    }

    private void DrawFooter(PdfContentByte cb)
    {
      DrawDisclaimers(cb);

      if (_results.ShowTechnologyTrademark) 
        DrawTechnologyTrademark(cb);

      DrawFooterPhrase(cb);
      DrawDocumentVersion(cb);
    }

    private void AddAmendedReportText(PdfContentByte cb)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(boldFont, 16);
      cb.BeginText();
      DrawText(cb, 200, 775, "*AMENDED REPORT*");
      cb.EndText();
    }

    private string PascalCase(string sentence)
    {
      string[] words = sentence.Split(new char[] { ' ' });
      string output = string.Empty;
      foreach (string word in words)
      {
        string temp = string.Empty;
        if (word.Length == 1)
          temp = word.ToUpper();
        else if (word.Length > 1)
          temp = word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        if (temp.Length > 0)
          output += temp + " ";
      }
      output = output.Trim();
      return output;
    }

    private void FillPatientHeader(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 9);
      cb.BeginText();

      string patientName = PascalCase(_req.PatientFirstName + " " + _req.PatientLastName);
      if (_req.PatientMiddleName.Length > 0)
        patientName = PascalCase(_req.PatientFirstName + " " + _req.PatientMiddleName + " " + _req.PatientLastName);

      string patientDOB = string.Empty;
      if (_req.PatientDOB.Year > 1819)
        patientDOB = _req.PatientDOB.ToString("MM/dd/yy");

      DrawText(cb, 20f, 728, patientName);
      DrawText(cb, 193, 728, _req.ClientPatientID);
      DrawText(cb, 55f, 714, _req.PatientGenderInd);
      DrawText(cb, 93f, 714, _req.PatientAgeInYears.ToString());
      DrawText(cb, 136, 714, patientDOB);

      cb.EndText();
    }

    private void FillOrderingPhysicianHeader(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 9);
      cb.BeginText();

      string providerName = PascalCase(_req.PhysicianLastName + ", " + _req.PhysicianFirstName);
      if (_req.PhysicianMiddleName.Length > 0)
        providerName += " " + _req.PhysicianMiddleName.Substring(0, 1).ToUpper();

      DrawText(cb, 335, 728, providerName);
      DrawText(cb, 338, 714, _req.PhysicianPhone);

      cb.EndText();
    }

    private void FillSpecimenHeader(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 9);
      cb.BeginText();

      string specimenSource = _req.SpecimenSource;
      if (specimenSource.Trim().ToUpper().Contains("OTHER") && _req.OtherSpecimenSourceSpecified.Length > 0)
        specimenSource += ": " + _req.OtherSpecimenSourceSpecified;

      string collectionDate = "NOT GIVEN";
      if (_req.HasCollectionDateInd.Equals("Y") && _req.HasCollectionTimeInd.Equals("Y") && _req.SpecimenCollectionDate.Year > 1819)
        collectionDate = _req.SpecimenCollectionDate.ToString("MM/dd/yy, hh:mm tt");
      else if (_req.HasCollectionDateInd.Equals("Y") && _req.SpecimenCollectionDate.Year > 1819)
        collectionDate = _req.SpecimenCollectionDate.ToString("MM/dd/yy");

      string releaseDate = string.Empty;
      if (_req.ReleaseDate.Year > 1819)
        releaseDate = _req.ReleaseDate.ToString("MM/dd/yy, hh:mm tt");

      DrawText(cb, 54, 670, PascalCase(specimenSource));
      DrawText(cb, 75, 656, _req.SpecimenID);
      DrawText(cb, 75, 642, _req.AccessionID.ToString());
      DrawText(cb, 210, 670, collectionDate);
      DrawText(cb, 210, 656, _req.SpecimenReceiveDate.ToString("MM/dd/yy"));
      DrawText(cb, 210, 642, releaseDate);

      cb.EndText();
    }

    private void FillClientHeader(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 9);
      cb.BeginText();

      string csz = _req.ClientCityPlusState + " " + _req.ClientZIP;
      DrawText(cb, 344, 670, _req.ClientName);
      DrawText(cb, 337, 656, _req.TRLClientID.ToString());
      DrawText(cb, 351, 642, _req.ClientAddress);
      DrawText(cb, 351, 632, csz);

      cb.EndText();
    }

    private void DrawTechnologyTrademark(PdfContentByte cb)
    {
      DrawMarkForTrademark(cb, 66.5f, 22);
      DrawPatentOfTechnology(cb);
    }

    private void DrawMarkForTrademark(PdfContentByte cb, float x, float y)
    {
      BaseFont boldFont = GetFont(_documentFormat.BoldRegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(boldFont, 3);
      cb.BeginText();
      DrawText(cb, x, y, "TM");
      cb.EndText();
    }

    private void DrawPatentOfTechnology(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetFontAndSize(regularFont, 7.5f);
      cb.BeginText();
      DrawText(cb, 298, 27, _results.PatentOfTechnology);
      cb.EndText();
    }

    private void DrawDisclaimers(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 7.5f);
      cb.BeginText();
      DrawText(cb, 14f, 27, _results.LaboratoryDevelopedTestDisclaimer);
      DrawText(cb, 14f, 18, _results.TechnologyUsedDisclaimer);
      DrawText(cb, 14f, 9f, _results.ExtraTechnologyUsedDisclaimer);
      cb.EndText();
    }

    private void DrawFooterPhrase(PdfContentByte cb)
    {
      DrawFooterText(cb);

      if (_results.ShowFooterPhraseTrademark) 
        DrawMarkForTrademark(cb, 580, 31);
    }

    private void DrawFooterText(PdfContentByte cb)
    {
      BaseFont italicsBoldFont = GetFont(_documentFormat.BoldItalicsFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(italicsBoldFont, 11);
      cb.SetCharacterSpacing(-0.50f);
      cb.BeginText();
      DrawText(cb, 406, 24, _results.FooterPhrase);
      cb.EndText();
    }

    private void DrawDocumentVersion(PdfContentByte cb)
    {
      BaseFont regularFont = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFont, 6.5f);
      cb.BeginText();
      DrawText(cb, 479, 5, _results.DocumentVersion);
      cb.EndText();
    }

    private void DrawGreyLineSeparators(PdfContentByte cb)
    {
      DrawLine(cb, 352.5f, 609, 352.5f, 40, BaseColor.LIGHT_GRAY);
      DrawLine(cb, 423.5f, 609, 423.5f, 40, BaseColor.LIGHT_GRAY);
      DrawLine(cb, 14f, 37, 580, 37, BaseColor.LIGHT_GRAY);
    }

    private void DrawTests(PdfWriter writer, PdfContentByte cb, bool isBlankReport)
    {
      float y = 0;
      if (!isBlankReport)
        y = DrawPatientTests(writer, cb);
      else
        y = DrawBlankTests(writer, cb);

      if (_results.ManualResults.Length > 0)
        DrawManualTests(cb, y);
    }

    private float DrawBlankTests(PdfWriter writer, PdfContentByte cb)
    {
      float y = 580;
      float x = 21.5f;
      float v = 12;

      foreach (PanelTest test in _results.PanelTests)
      {
        DrawBlankTestLine(test, writer, cb, x, y);
        y -= v;
      }

      return y;
    }

    private void DrawBlankTestLine(PanelTest test, PdfWriter writer, PdfContentByte cb, float x, float y)
    {
      if (!test.Visible)
        return;

      PdfContentByte dl = writer.DirectContent;
      dl.SetLineWidth(1);

      string indent = string.Empty;
      if (test.IndentInd.Equals("Y"))
        indent = "  ";
      else if (test.IndentInd.Equals("2"))
        indent = "    ";

      BaseFont regularFontBase = GetFont(_documentFormat.RegularFontFilePath);
      BaseFont italicsFontBase = GetFont(_documentFormat.ItalicsFontFilePath);
      Font regularFont = new Font(regularFontBase, 10);
      Font italicsFont = new Font(italicsFontBase, 10);

      if (test.TestName.Contains(@"<i>"))
        DrawParsedItalicizedTest(cb, test.TestName, indent, x, y, regularFont, italicsFont);
      else
        DrawRegularTest(cb, test.TestName, indent, x, y, regularFontBase);
    }

    private void DrawParsedItalicizedTest(PdfContentByte cb, string testName, string indent, float x, float y, Font regularFont, Font italicsFont)
    {
      bool needsItalics = false;
      Phrase formattedText = new Phrase();
      formattedText.Add(new Chunk(indent));
      int countTokens = 1;
      string[] testNameSplit = testName.Split(' ');
      foreach (string token in testNameSplit)
      {
        if (token.Contains(@"<i>") && token.Contains(@"</i>"))
        {
          string filteredToken = token.Remove(token.IndexOf('<'), 3);
          string filteredTokenSecond = filteredToken.Remove(filteredToken.IndexOf('<'), 4);
          if (countTokens < testNameSplit.Length)
            filteredTokenSecond = filteredTokenSecond + " ";
          formattedText.Add(new Chunk(filteredTokenSecond, italicsFont));
          needsItalics = false;
        }
        else if (token.Contains(@"<i>"))
        {
          string filteredToken = token.Remove(token.IndexOf('<'), 3);
          if (countTokens < testNameSplit.Length)
            filteredToken = filteredToken + " ";
          formattedText.Add(new Chunk(filteredToken, italicsFont));
          needsItalics = true;
        }
        else if (token.Contains("</i>"))
        {
          string filteredToken = token.Remove(token.IndexOf('<'), 4);
          if (countTokens < testNameSplit.Length)
            filteredToken = filteredToken + " ";
          formattedText.Add(new Chunk(filteredToken, italicsFont));
          needsItalics = false;
        }
        else
        {
          string newToken = token;
          if (countTokens < testNameSplit.Length)
            newToken = token + " ";
          if (needsItalics)
            formattedText.Add(new Chunk(newToken, italicsFont));
          else
            formattedText.Add(new Chunk(newToken, regularFont));
        }
        countTokens += 1;
      }
      ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, formattedText, x, y, 0);
    }

    private void DrawRegularTest(PdfContentByte cb, string testName, string indent, float x, float y, BaseFont font)
    {
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(font, 10);
      cb.BeginText();
      cb.SetTextMatrix(x, y);
      cb.ShowText(indent + testName);
      cb.EndText();
    }

    private float DrawPatientTests(PdfWriter writer, PdfContentByte cb)
    {
      float y = 580;
      float x = 21.5f;
      float v = 12;
      float detectionX = 325;
      float nonDetectionX = 380;

      foreach (FormattedResult result in _results.FormattedResults)
      {
        DrawPatientTestLine(result, writer, cb, x, y, detectionX, nonDetectionX);
        y -= v;
      }

      return y;
    }

    private void DrawPatientTestLine(FormattedResult result, PdfWriter writer, PdfContentByte cb, float x, float y, float detectionX, float nonDetectionX)
    {
      if (!result.Visible)
        return;

      BaseFont regularFontBase = GetFont(_documentFormat.RegularFontFilePath);
      BaseFont boldFontBase = GetFont(_documentFormat.BoldRegularFontFilePath);
      BaseFont italicsFontBase = GetFont(_documentFormat.ItalicsFontFilePath);
      BaseFont italicsBoldFont = GetFont(_documentFormat.BoldItalicsFontFilePath);
      DrawRegularOrItalicizedPatientTests(result, cb, x, y, regularFontBase, boldFontBase, italicsFontBase, italicsBoldFont);

      if (result.HasTargetResult)
      {
        if ((ResultsReportEnum)result.Result == ResultsReportEnum.Detected)
          DrawDetectedString(result, cb, y, detectionX, boldFontBase);
        else
          DrawDetectedString(result, cb, y, detectionX, regularFontBase);

        DrawNotDetectedString(result, cb, y, detectionX, nonDetectionX, regularFontBase);
      }

      if ((ResultsReportEnum)result.Result == ResultsReportEnum.Detected) 
        DrawLineUnderDetected(result, writer, x, y, detectionX);
    }

    private static void DrawLineUnderDetected(FormattedResult result, PdfWriter writer, float x, float y, float detectionX)
    {
      PdfContentByte dl = writer.DirectContent;
      dl.SetLineWidth(1);

      dl.MoveTo(x, y - 3);
      float tempX = detectionX + 10;

      if (result.DetectedString.StartsWith("X-SEE NOTE"))
        tempX += 15;

      dl.LineTo(tempX, y - 3);
      dl.Stroke();
    }

    private void DrawRegularOrItalicizedPatientTests(FormattedResult result, PdfContentByte cb, float x, float y, BaseFont regularFontBase, BaseFont boldFontBase, BaseFont italicsFontBase, BaseFont italicsBoldFont)
    {
      if (result.TestName.Contains(@"<i>"))
      {
        Font regularFont = new Font(regularFontBase, 10);
        Font italicsFont = new Font(italicsFontBase, 10);

        if ((ResultsReportEnum)result.Result == ResultsReportEnum.Detected)
        {
          regularFont = new Font(boldFontBase, 10);
          italicsFont = new Font(italicsBoldFont, 10);
        }

        string indent = "";
        int countSpaces = result.TestName.TakeWhile(Char.IsWhiteSpace).Count();
        for (int i = 0; i < countSpaces; i++)
          indent += " ";

        DrawParsedItalicizedTest(cb, result.TestName, indent, x, y, regularFont, italicsFont);
      }
      else
      {
        string indent = "";
        if ((ResultsReportEnum)result.Result == ResultsReportEnum.Detected)
          DrawRegularTest(cb, result.TestName, indent, x, y, boldFontBase);
        else
          DrawRegularTest(cb, result.TestName, indent, x, y, regularFontBase);
      }
    }

    private static void DrawDetectedString(FormattedResult result, PdfContentByte cb, float y, float detectionX, BaseFont font)
    {
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(font, 10);
      cb.BeginText();
      cb.SetTextMatrix(detectionX, y);
      string detectedString = result.DetectedString;
      if (detectedString.StartsWith("X-SEE NOTE"))
      {
        detectedString = "X" + detectedString.Substring(10);
        cb.SetTextMatrix(detectionX - 5, y);
      }
      cb.ShowText(detectedString);
      cb.EndText();
    }

    private void DrawNotDetectedString(FormattedResult result, PdfContentByte cb, float y, float detectionX, float nonDetectionX, BaseFont font)
    {
      bool seeNote = false;
      string notDetectedString = result.NotDetectedString;
      if (notDetectedString.StartsWith("X-SEE NOTE"))
      {
        notDetectedString = "X" + notDetectedString.Substring(10);
        seeNote = true;
      }

      cb.SetColorFill(BaseColor.BLACK);
      cb.BeginText();

      if (_results.TestNotPerformed || seeNote || (ResultsReportEnum)result.Result == ResultsReportEnum.IndividualTNP)
      {
        cb.SetFontAndSize(font, 9);
        cb.SetTextMatrix(nonDetectionX - 5, y);
      }
      else
      {
        cb.SetFontAndSize(font, 10);
        cb.SetTextMatrix(nonDetectionX, y);
      }

      if (notDetectedString.ToUpper().Contains("TNP") && result.CenterTestNotPerformed)
      {
        notDetectedString = "Test Not Performed";
        cb.SetFontAndSize(font, 10);
        cb.SetTextMatrix(detectionX - 8, y);
      }

      if (notDetectedString.ToUpper().Contains("INCONCLUSIVE") || notDetectedString.ToUpper().Contains("INVALID"))
      {
        cb.SetFontAndSize(font, 9);
        cb.SetTextMatrix(nonDetectionX - 25, y);
      }
      
      cb.ShowText(notDetectedString);
      cb.EndText();
    }

    private void DrawManualTests(PdfContentByte cb, float yInitial)
    {
      float x = 21.5f;
      float v = 12;
      float detectionX = 325;
      BaseFont regularFontBase = GetFont(_documentFormat.RegularFontFilePath);
      float y = yInitial - (2 * v);
      foreach (ManualResult result in _results.ManualResults)
      {
        cb.SetColorFill(BaseColor.BLACK);
        cb.BeginText();
        cb.SetFontAndSize(regularFontBase, 10);
        cb.SetTextMatrix(x, y);
        cb.ShowText(result.TestName);
        cb.SetTextMatrix(detectionX - 70, y);
        cb.ShowText(result.ManualResultValue);
        cb.EndText();
        y -= v;
      }
    }

    private void DrawComments(PdfContentByte cb)
    {
      float y = 580;
      float x = 430;
      float v = 12;
      char[] k = { '\n', '\r' };
      const string CLIENT_COMMENTS_TAG = "CLIENT COMMENTS:";
      const string LAB_NOTES_TAG = "LAB NOTES:";
      int length = 0;
      int max = 28;
      string writeString = string.Empty;

      string comments2 = _results.Comments;
      if (comments2.Contains(CLIENT_COMMENTS_TAG))
      {
        int idx = comments2.IndexOf(CLIENT_COMMENTS_TAG);
        comments2 = comments2.Insert(idx + CLIENT_COMMENTS_TAG.Length, "\n");
      }
      string[] comments = comments2.Split(k);

      BaseFont regularFontBase = GetFont(_documentFormat.RegularFontFilePath);
      cb.SetColorFill(BaseColor.BLACK);
      cb.SetFontAndSize(regularFontBase, 10);

      cb.BeginText();
      for (int i = 0; i < comments.Length; i++)
      {
        char[] q = { ' ' };
        string[] words = comments[i].Split(q);

        if (comments2.Contains(CLIENT_COMMENTS_TAG) && comments[i].Contains(LAB_NOTES_TAG)) //move now an extra line to separate
          y -= v;

        for (int j = 0; j < words.Length; j++)
        {
          if (!words[j].Equals(string.Empty))
          {
            if (writeString.Length + words[j].Length > max)
            {
              cb.SetTextMatrix(x, y);
              cb.ShowText(writeString);
              writeString = words[j].Trim() + " ";
              length = writeString.Length;
              y -= v;
            }
            else
            {
              writeString += words[j].Trim() + " ";
              length = writeString.Length;
            }
          }
        }
        cb.SetTextMatrix(x, y);
        cb.ShowText(writeString.Trim());
        y -= v;
        length = 0;
        writeString = string.Empty;
      }
      cb.EndText();
    }
  }
}
