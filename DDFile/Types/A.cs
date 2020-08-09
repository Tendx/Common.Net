using System;
using System.Collections.Generic;

namespace DDFile
{
    public class A : Field
    {
        private List<string> _values;

        public override Type ValueType => typeof(string);
        private string _value;
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;
                _data = _values.IndexOf(_value);
            }
        }

        public override string DataType => $"{nameof(A)}[{string.Join(",", _values)}]";
        private long _data;
        public override long Data
        {
            get => _data;
            set
            {
                if (value < _values.Count)
                {
                    _data = value;
                    _value = _values[(int)_data];
                }
            }
        }

        public A(string name, IEnumerable<string> values) : base(name)
        {
            _values = new List<string>(values);
        }
    }
}
