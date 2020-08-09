using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DDFile
{
    public class S : Field
    {
        public override Type ValueType => typeof(string);
        private string _value;
        public override string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _value = null;
                    _data = 0;
                }
                else
                {
                    string tmp = string.IsNullOrWhiteSpace(value) ? null : Regex.Replace(value, @"[^!-~]", string.Empty);
                    _value = tmp.Substring(0, Math.Min(tmp.Length, 8));
                    byte[] buffer = new byte[8]; 
                    Encoding.ASCII.GetBytes(_value).CopyTo(buffer, 0);
                    _data = BitConverter.ToInt64(buffer, 0);
                }
            }
        }

        public override string DataType => nameof(S);
        private long _data;
        public override long Data
        {
            get => _data;
            set
            {
                _data = value;
                _value = Regex.Replace(Encoding.ASCII.GetString(BitConverter.GetBytes(value)), @"[^!-~]", string.Empty);
            }
        }

        public S(string name) : base(name) { }
    }
}
