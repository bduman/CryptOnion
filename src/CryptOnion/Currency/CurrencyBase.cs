namespace CryptOnion.Currency
{
    public class CurrencyBase
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public CurrencyBase(string currencyCode, string name)
        {
            this.Code = currencyCode;
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Code;
        }
    }
}
