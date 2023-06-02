using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using GenerateLabResult.Models;
using Newtonsoft.Json;

namespace GenerateLabResultReport
{
  public class Processor
  {
    IDataAccessLayer _dal;
    ILogGenerator _logGenerator;

    public Processor(IDataAccessLayer dal, ILogGenerator logGenerator)
    {
      _dal = dal;
      _logGenerator = logGenerator;
    }

    public string GetLogOutput()
    {
      return _logGenerator.Output;
    }

    public void Execute(string[] args)
    {
      if (!ValidateArguments(args))
      {
        ShowSyntax();
        return;
      }

      List<long> sampleIDs = GetSampleIDsFromArgument(args[0]);
      string outputFolder = args[1];
      ProcessSampleIDs(sampleIDs, outputFolder);
    }

    private List<long> GetSampleIDsFromArgument(string argument)
    {
      List<long> sampleIDs = new List<long>();
      if (File.Exists(argument))
        sampleIDs = File.ReadAllLines(argument).ToList().Select(ParseStringToLong).ToList();
      else
        sampleIDs.Add(Int64.Parse(argument));
      return sampleIDs;
    }

    private long ParseStringToLong(string text)
    {
      long defaultNumber = -1;
      long number;
      if (Int64.TryParse(text, out number))
        return number;
      return defaultNumber;
    }

    private void ProcessSampleIDs(List<long> sampleIDs, string outputFolder)
    {
      List<string> generatedFiles = new List<string>();
      foreach (long sampleID in sampleIDs)
      {
        ResultReport model = GenerateReportModel(sampleID, outputFolder);
        string blankReportFilePath = _dal.GetSampleBlankReportFilePath(model.Results, outputFolder);

        if (!generatedFiles.Contains(model.DocumentFormat.OutputFilePath))
        {
          PostRequest("PatientReportGenerator", model);
          generatedFiles.Add(model.DocumentFormat.OutputFilePath);
        }

        model.DocumentFormat.OutputFilePath = blankReportFilePath;
        if (!generatedFiles.Contains(model.DocumentFormat.OutputFilePath))
        {
          PostRequest("BlankReportGenerator", model);
          generatedFiles.Add(model.DocumentFormat.OutputFilePath);
        }
      }
    }

    private ResultReport GenerateReportModel(long sampleID, string outputFolder)
    {
      DocumentFormat documentFormat = _dal.GetSampleDocumentFormat(sampleID, outputFolder);
      Requisition requisition = _dal.GetSampleRequsition(sampleID);
      Results results = _dal.GetSampleResults(sampleID);

      ResultReport model = new ResultReport();
      model.DocumentFormat = documentFormat;
      model.Requisition = requisition;
      model.Results = results;
      return model;
    }

    private bool ValidateArguments(string[] args)
    {
      bool validArgs = true;

      if (args.Length != 2)
      {
        validArgs = false;
        return validArgs;
      }

      long sampleID;
      if (!Int64.TryParse(args[0], out sampleID) && !File.Exists(args[0]))
      {
        validArgs = false;
        return validArgs;
      }

      if (!Directory.Exists(args[1]))
      {
        validArgs = false;
        return validArgs;
      }

      return validArgs;
    }

    private void ShowSyntax()
    {
      string syntax = "SYNTAX: GenerateLabResultReport.exe [Sample IDs Text File Path OR Single Sample ID] [Output Folder Name]";
      _logGenerator.LogMessage(syntax, LogMessageTypeEnum.ERROR);
    }

    private void PostRequest(string controller, ResultReport model)
    {
      using (HttpClient client = new HttpClient())
      {
        client.BaseAddress = new Uri(Properties.Settings.Default.DOCUMENTGENERATOR_BASE_URI);
        string json = JsonConvert.SerializeObject(model);
        var post = client.PostAsync(controller, new StringContent(json, Encoding.UTF8, "application/json"));
        post.Wait();
        if (post.Result.IsSuccessStatusCode)
          _logGenerator.LogMessage($"Created {model.DocumentFormat.OutputFilePath} with Status Code: {post.Result.StatusCode}", LogMessageTypeEnum.INFO);
        else
          _logGenerator.LogMessage($"Failed to create {model.DocumentFormat.OutputFilePath} with Status Code: {post.Result.StatusCode}", LogMessageTypeEnum.ERROR);
      }
    }

  }
}
