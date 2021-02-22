namespace PlayCaddy.Core.Infrastructure
{
    public class EnumKeyValue
    {
        private string _Name = "";
        private string _Value = "";

        public EnumKeyValue(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public string Key
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}