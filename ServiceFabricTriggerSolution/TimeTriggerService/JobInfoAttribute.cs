using System;

namespace TimeTriggerService
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JobInfoAttribute : Attribute
    {
        public string Name { get; set; }
        public string CronExpression { get; set; }
    }
}
