using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLabResultReport
{
  public enum LogMessageTypeEnum { INFO, ERROR };

  public interface ILogGenerator
  {
    string Output { get; }
    void LogMessage(object message, LogMessageTypeEnum messageType);
  }
}
