namespace ISPRO.Helpers.Exceptions
{
    public class ModelException : FormatException
    {
        public string Key { get; private set; } 
        public ModelException(string key, string message) : base(message) 
        {
            Key = key;
        }
    }
}
