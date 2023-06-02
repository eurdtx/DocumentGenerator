using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLabResult.Models
{
  public class Results
  {
    private string _reportPanelName = string.Empty;
    bool _isPendingCultureAndSensitivityResults = false;
    private string _laboratoryDevelopedTestDisclaimer = string.Empty;
    private string _technologyUsedDisclaimer = string.Empty;
    private string _extraTechnologyUsedDisclaimer = string.Empty;
    private bool _showTechnologyTrademark = false;
    private string _patentOfTechnology = string.Empty;
    private string _footerPhrase = string.Empty;
    private bool _showFooterPhraseTrademark = false;
    private string _documentVersion = string.Empty;
    private PanelTest[] _panelTests = new PanelTest[0];
    private FormattedResult[] _formattedResults = new FormattedResult[0];
    private bool _testNotPerformed = false;
    private ManualResult[] _manualResults = new ManualResult[0];
    private string _comments = string.Empty;

    public string ReportPanelName
    {
      get { return _reportPanelName; }
      set { _reportPanelName = value; }
    }

    public bool IsPendingCultureAndSensitvityResults
    {
      get { return _isPendingCultureAndSensitivityResults; }
      set { _isPendingCultureAndSensitivityResults = value; }
    }

    public string LaboratoryDevelopedTestDisclaimer
    {
      get { return _laboratoryDevelopedTestDisclaimer; }
      set { _laboratoryDevelopedTestDisclaimer = value; }
    }

    public string TechnologyUsedDisclaimer
    {
      get { return _technologyUsedDisclaimer; }
      set { _technologyUsedDisclaimer = value; }
    }

    public string ExtraTechnologyUsedDisclaimer
    {
      get { return _extraTechnologyUsedDisclaimer; }
      set { _extraTechnologyUsedDisclaimer = value; }
    }

    public bool ShowTechnologyTrademark
    {
      get { return _showTechnologyTrademark; }
      set { _showTechnologyTrademark = value; }
    }

    public string PatentOfTechnology
    {
      get { return _patentOfTechnology; }
      set { _patentOfTechnology = value; }
    }

    public string FooterPhrase
    {
      get { return _footerPhrase; }
      set { _footerPhrase = value; }
    }
    
    public bool ShowFooterPhraseTrademark
    {
      get { return _showFooterPhraseTrademark; }
      set { _showFooterPhraseTrademark = value; }
    }

    public string DocumentVersion
    {
      get { return _documentVersion; }
      set { _documentVersion = value; }
    }

    public PanelTest[] PanelTests
    {
      get { return _panelTests; }
      set { _panelTests = value; }
    }

    public FormattedResult[] FormattedResults
    {
      get { return _formattedResults; }
      set { _formattedResults = value; }
    }

    public bool TestNotPerformed
    {
      get { return _testNotPerformed; }
      set { _testNotPerformed = value; }
    }

    public ManualResult[] ManualResults
    {
      get { return _manualResults; }
      set { _manualResults = value; }
    }

    public string Comments
    {
      get { return _comments; }
      set { _comments = value; }
    }
  }
}
