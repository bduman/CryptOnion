namespace CryptOnion.Currency
{
    public abstract class AbstractCurrency
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        protected AbstractCurrency(string currencyCode, string name)
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
