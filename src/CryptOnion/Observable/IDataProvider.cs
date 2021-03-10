using System.Threading;
using System.Threading.Tasks;

namespace CryptOnion.Observable
{
    public interface IDataProvider<T>
    {
        Task<T> ProvideAsync(CancellationToken cancellationToken);
    }
}
