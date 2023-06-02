using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
  public class DocumentFormat
  {
    private string _templatePath = string.Empty;
    private string _outputFilePath = string.Empty;
    private string _boldRegularFontFilePath = string.Empty;
    private string _regularFontFilePath = string.Empty;
    private string _italicsFontFilePath = string.Empty;
    private string _boldItalicsFontFilePath = string.Empty;
    private bool _isAmended = false;

    public string TemplatePath
    {
      get { return _templatePath; }
      set { _templatePath = value; }
    }

    public string OutputFilePath
    {
      get { return _outputFilePath; }
      set { _outputFilePath = value; }
    }

    public string BoldRegularFontFilePath
    {
      get { return _boldRegularFontFilePath; }
      set { _boldRegularFontFilePath = value; }
    }

    public string RegularFontFilePath
    {
      get { return _regularFontFilePath; }
      set { _regularFontFilePath = value; }
    }

    public string ItalicsFontFilePath
    {
      get { return _italicsFontFilePath; }
      set { _italicsFontFilePath = value; }
    }

    public string BoldItalicsFontFilePath
    {
      get { return _boldItalicsFontFilePath; }
      set { _boldItalicsFontFilePath = value; }
    }

    public bool IsAmended
    {
      get { return _isAmended; }
      set { _isAmended = value; }
    }
  }
}
