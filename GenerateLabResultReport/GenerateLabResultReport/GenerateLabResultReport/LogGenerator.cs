using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLabResultReport
{
  public class LogGenerator : ILogGenerator
  {
    private string _output = string.Empty;

    public string Output
    {
      get { return _output; }
    }

    public void LogMessage(object message, LogMessageTypeEnum messageType)
    {
      string formattedMessage = $"[{messageType}: {DateTime.Now.ToString("hh:mm tt")}] {message}";
      _output += formattedMessage + Environment.NewLine;
      Console.WriteLine(formattedMessage);
    }
  }
}
