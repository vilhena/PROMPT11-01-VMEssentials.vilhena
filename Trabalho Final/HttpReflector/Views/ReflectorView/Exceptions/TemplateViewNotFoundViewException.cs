namespace HttpReflector.Views.Exceptions
{

    public class TemplateViewNotFoundViewException : ViewException
    {
        public string ViewName { get; set; }
        public string Path { get; set; }

        public TemplateViewNotFoundViewException(string viewname, string path)
            :base(string.Format("View {0} does not exists with path {1}",
            viewname, path))
        {
            this.ViewName = viewname;
            this.Path = path;
        }

    }
}
