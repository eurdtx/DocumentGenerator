using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentGenerator.Models
{
  public class PanelTest
  {
    private bool _visible = false;
    private string _indentInd = "N";
    private string _testName = string.Empty;

    public PanelTest() { }

    public PanelTest(string testName, bool visible, string indentInd)
    {
      _visible = visible;
      _indentInd = indentInd;
      _testName = testName;
    }

    public bool Visible
    {
      get { return _visible; }
      set { _visible = value; }
    }

    public string IndentInd
    {
      get { return _indentInd; }
      set
      {
        if (value.Trim().ToUpper().Equals("Y") || value.Trim().ToUpper().Equals("1"))
          _indentInd = "Y";
        else if (value.Trim().ToUpper().Equals("2"))
          _indentInd = "2";
        else
          _indentInd = "N";
      }
    }

    public string TestName
    {
      get { return _testName; }
      set { _testName = value; }
    }
  }
}
