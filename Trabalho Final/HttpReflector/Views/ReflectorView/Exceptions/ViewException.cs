namespace HttpReflector.Views.Exceptions
{
    public abstract class ViewException : System.Exception
    {
        protected ViewException(string msg) : base(msg) { }
    }
}
