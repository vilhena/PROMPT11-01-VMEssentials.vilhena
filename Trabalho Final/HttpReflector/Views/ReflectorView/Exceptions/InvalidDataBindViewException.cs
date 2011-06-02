namespace HttpReflector.Views.Exceptions
{

    public class InvalidDataBindViewException : ViewException
    {
        public string ListProperies { get; set; }
        public string Property { get; set; }
        public string TypeName { get; set; }
        public string File { get; set; }

        public InvalidDataBindViewException(string property, string prop, string typeName, string file)
            :base(string.Format("For {0} type on {1} call cannot find {2} property, using file {3}",
            typeName,property,prop, file))
        {
            this.ListProperies = property;
            this.Property = prop;
            this.TypeName = typeName;
            this.File = file;
        }

    }
}
