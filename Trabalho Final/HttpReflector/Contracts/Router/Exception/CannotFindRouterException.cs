using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpReflector.Contracts.Router.Exception
{
    public class CannotFindRouterException: RouterException
    {
        public string Path { get; set; }
        public CannotFindRouterException(string path)
            : base(string.Format("Route for {0} does not exists",
            path))
        {
            this.Path = path;
        }
    }

}

