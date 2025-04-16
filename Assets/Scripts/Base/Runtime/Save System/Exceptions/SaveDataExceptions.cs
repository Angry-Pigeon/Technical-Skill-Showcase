using System;
namespace Tool_Development.SerializableScriptableObject.Scripts.Exceptions
{
    public class SaveDataExceptions
    {
        public class SaveDidNotFound : Exception
        {
            public SaveDidNotFound() { }
            public SaveDidNotFound(string message) : base(message) { }
            public SaveDidNotFound(string message, Exception inner) : base(message, inner) { }
        }
    }
}