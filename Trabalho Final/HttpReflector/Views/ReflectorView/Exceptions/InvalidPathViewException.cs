namespace HttpReflector.Views.Exceptions
{

    public class InvalidPathViewException : ViewException
    {
        public string Path { get; set; }

        public InvalidPathViewException(string path)
            :base(string.Format("Path {0} does not exists",
            path))
        {
            this.Path = path;
        }

    }
}
