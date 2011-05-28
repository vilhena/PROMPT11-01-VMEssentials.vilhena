namespace HttpReflector.Controllers.Exception
{
    
    public class InvalidPathAssemblyModelException : ModelException
    {
        public string Path { get; set; }

        public InvalidPathAssemblyModelException(string path)
            :base(string.Format("Path {0} does not exists",
            path))
        {
            this.Path = path;
        }

    }
}
