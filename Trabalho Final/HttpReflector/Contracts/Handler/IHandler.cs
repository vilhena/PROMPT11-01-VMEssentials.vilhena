using System;
using HttpReflector.Contracts.View;

namespace HttpReflector.Contracts.Handler
{
    public interface IHandler
    {
        IView Run();
    }

}
