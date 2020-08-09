using System;

namespace DDFile
{
    public class B : Field
    {
        public override Type ValueType => typeof(bool);
        private bool _value;
        public override string Value
        {
            get => _value.ToString();
            set
            {
                _value = bool.Parse(value);
                _data = _value ? 1 : 0;
            }
        }

        public override string DataType => nameof(B);
        private long _data;
        public override long Data
        {
            get => _data;
            set
            {
                _data = value;
                _value = _data > 0;
            }
        }

        public B(string name) : base(name) { }
    }
}
