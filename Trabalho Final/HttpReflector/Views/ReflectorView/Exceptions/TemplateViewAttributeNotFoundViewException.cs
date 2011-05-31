namespace HttpReflector.Views.Exceptions
{

    public class TemplateViewAttributeNotFoundViewException : ViewException
    {
        public string ViewName { get; set; }

        public TemplateViewAttributeNotFoundViewException(string viewname)
            :base(string.Format("View {0} does not exists",
            viewname))
        {
            this.ViewName = viewname;
        }

    }
}
