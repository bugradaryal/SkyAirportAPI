using Entities.Enums;

namespace Utilitys.Logging
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class LogActionAttribute : Attribute
    {
        public Action_Type ActionType { get; }

        public LogActionAttribute(Action_Type actionType)
        {
            ActionType = actionType;
        }
    }
}