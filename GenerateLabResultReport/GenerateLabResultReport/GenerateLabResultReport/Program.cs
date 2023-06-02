using System;

namespace GenerateLabResultReport
{
  class Program
  {
    public static void Main(string[] args)
    {
      string errorMessage = string.Empty;
      ILogGenerator logGenerator = new LogGenerator();
      try
      {
        IDataAccessLayer dal = new DataAccessLayer();
        Processor processor = new Processor(dal, logGenerator);
        processor.Execute(args);
      }
      catch (Exception ex)
      {
        logGenerator.LogMessage(ex.Message, LogMessageTypeEnum.ERROR);
      }
      Console.Read();
    }

  }
}
