using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
  public class ManualResult
  {
    private string _testName = string.Empty;
    private string _manualResultValue = string.Empty;

    public ManualResult() { }

    public ManualResult(string testName, string manualResultValue)
    {
      _testName = testName;
      _manualResultValue = manualResultValue;
    }

    public string TestName
    {
      get { return _testName; }
      set { _testName = value; }
    }

    public string ManualResultValue
    {
      get { return _manualResultValue; }
      set { _manualResultValue = value; }
    }
  }
}
