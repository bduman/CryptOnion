namespace CryptOnion.Currency.Filter
{
    public interface IFilter<in T>
    {
        bool IsPassed(T obj);
    }
}
