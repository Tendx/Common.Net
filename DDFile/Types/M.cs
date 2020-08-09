using System;
using System.Linq;
using System.Collections.Generic;

namespace DDFile
{
    public class M : Field
    {
        private Dictionary<long, string> _d2v;
        private Dictionary<string, long> _v2d;

        public override Type ValueType => typeof(string);
        private string _value;
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;
                _data = _v2d[value];
            }
        }

        public override string DataType => $"{nameof(M)}{{{string.Join(",", _d2v.Select(i => i.Key + ":" + i.Value))}}}";
        private long _data;
        public override long Data
        {
            get => _data;
            set
            {
                _data = value;
                _value = _d2v[value];
            }
        }

        public M(string name, Dictionary<int, string> map) : base(name)
        {
            _d2v = new Dictionary<long, string>();
            _v2d = new Dictionary<string, long>();
            foreach (var item in map)
            {
                _d2v.Add(item.Key, item.Value);
                _v2d.Add(item.Value, item.Key);
            }
        }
    }
}
