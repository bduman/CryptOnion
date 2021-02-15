namespace CryptOnion.Currency
{
    public abstract class CurrencyBase
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        protected CurrencyBase(string currencyCode, string name)
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
