using System.Threading;

namespace HttpReflector.Contracts.UIBinder
{
    public interface IUIBinder
    {
        WaitCallback Callback { get; set; }
        void Start();
        void Stop();
    }
}
