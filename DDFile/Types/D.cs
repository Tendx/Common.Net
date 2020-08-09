using System;

namespace DDFile
{
    public class D : Field
    {
        public override Type ValueType => typeof(double);
        private double _value;
        public override string Value
        {
            get => _value.ToString();
            set
            {
                _value = double.Parse(value);
                _data = (long)(_value * Math.Pow(10, Digit));
            }
        }

        public override string DataType => nameof(D) + Digit;
        private long _data;
        public override long Data
        {
            get => _data;
            set
            {
                _data = value;
                _value = _data / Math.Pow(10, Digit);
            }
        }

        public int Digit;
        public D(string name, int digit) : base(name)
        {
            Digit = digit;
        }
    }
}
