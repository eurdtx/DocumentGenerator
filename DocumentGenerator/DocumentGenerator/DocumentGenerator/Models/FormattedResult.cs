using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
  public class FormattedResult
  {
    private bool _visible = false;
    private int _result = 0;
    private string _testName = string.Empty;
    private bool _hasTargetResult = false;
    private string _detectedString = string.Empty;
    private string _notDetectedString = string.Empty;
    private bool _centerTestNotPerformed = true;

    public FormattedResult() { }

    public FormattedResult(string testName, int result, bool hasTargetResult, string detectedString, string notDetectedString, bool visible, bool centerTestNotPerformed)
    {
      _visible = visible;
      _result = result;
      _testName = testName;
      _hasTargetResult = hasTargetResult;
      _detectedString = detectedString;
      _notDetectedString = notDetectedString;
      _centerTestNotPerformed = centerTestNotPerformed;
    }

    public bool Visible
    {
      get { return _visible; }
      set { _visible = value; }
    }

    public int Result
    {
      get { return _result; }
      set { _result = value; }
    }

    public string TestName
    {
      get { return _testName; }
      set { _testName = value; }
    }

    public bool HasTargetResult
    {
      get { return _hasTargetResult; }
      set { _hasTargetResult = value; }
    }

    public string DetectedString
    {
      get { return _detectedString; }
      set { _detectedString = value; }
    }

    public string NotDetectedString
    {
      get { return _notDetectedString; }
      set { _notDetectedString = value; }
    }

    public bool CenterTestNotPerformed
    {
      get { return _centerTestNotPerformed; }
      set { _centerTestNotPerformed = value; }
    }
  }
}
