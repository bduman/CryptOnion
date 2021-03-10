using System;

namespace CryptOnion.Observable.Scheduled
{
    public class DefaultInternalDelay : IInternalDelay
    {
        private readonly TimeSpan _delay;

        public DefaultInternalDelay()
        {
            this._delay = TimeSpan.FromSeconds(3);
        }

        public TimeSpan GetDelay()
        {
            return this._delay;
        }
    }
}
