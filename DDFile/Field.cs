using System;
using System.Linq;

namespace DDFile
{
    public abstract class Field
    {
        public string Name { get; private set; }

        public abstract Type ValueType { get; }
        public abstract string Value { get; set; }

        public abstract string DataType { get; }
        public abstract long Data { get; set; }

        public Field(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"@{Name}:{DataType}";
        }

        public static Field FromString(string line)
        {
            string[] nt = line.Split(new char[] { ':' }, 2);
            string name = nt[0].Trim().Trim('@');
            char type = nt[1][0];
            if (type == 'A')
            {
                return new A(name, nt[1].Substring(2, nt[1].Length - 3).Split(','));
            }
            else if (type == 'B')
            {
                return new B(name);
            }
            else if (type == 'D')
            {
                return new D(name, int.Parse(nt[1].Substring(1)));
            }
            else if (type == 'L')
            {
                return new L(name, int.Parse(nt[1].Substring(1)));
            }
            else if (type == 'M')
            {
                return new M(name, nt[1].Substring(2, nt[1].Length - 3).Split(',').Select(p => p.Split(':')).ToDictionary(p => int.Parse(p[0]), p => p[1]));
            }
            else if (type == 'S')
            {
                return new S(name);
            }
            else
                return null;
        }
    }
}