using System;

namespace CryptOnion.Observable.Scheduled
{
    public interface IInternalDelay
    {
        TimeSpan GetDelay();
    }
}
