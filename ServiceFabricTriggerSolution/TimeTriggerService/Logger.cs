using System.Diagnostics;

namespace TimeTriggerService
{
    public class Logger : ILogger
    {
        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
