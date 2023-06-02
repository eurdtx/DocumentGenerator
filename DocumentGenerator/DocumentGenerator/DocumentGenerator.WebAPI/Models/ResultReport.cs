using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentGenerator.Models;

namespace DocumentGenerator.WebAPI.Models
{
  public class ResultReport
  {
    private DocumentFormat _documentFormat = new DocumentFormat();
    private Requisition _requisition = new Requisition();
    private Results _results = new Results();

    public DocumentFormat DocumentFormat
    {
      get { return _documentFormat; }
      set { _documentFormat = value; }
    }

    public Requisition Requisition
    {
      get { return _requisition; }
      set { _requisition = value; }
    }

    public Results Results
    {
      get { return _results; }
      set { _results = value; }
    }
  }
}