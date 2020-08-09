using System;

namespace DDFile
{
    public class L : Field
    {
        public override Type ValueType => typeof(long);
        private long _value;
        public override string Value
        {
            get => _value.ToString();
            set
            {
                _value = long.Parse(value);
                _data = _value / (long)Math.Pow(10, Digit);
            }
        }

        public override string DataType => nameof(L) + Digit;
        private long _data;
        public override long Data
        {
            get => _data;
            set
            {
                _data = value;
                _value = value * (long)Math.Pow(10, Digit);
            }
        }

        public int Digit { get; set; }
        public L(string name, int digit) : base(name)
        {
            Digit = digit;
        }
    }
}
