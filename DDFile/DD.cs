using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDFile
{
    public class DD
    {
        public string Path { get; private set; }
        public List<Field> Fields { get; private set; }

        public DD(string path)
        {
            Path = path;
            Fields = new List<Field>();
        }

        #region write
        public void WriteFields()
        {
            File.WriteAllText(Path, string.Join(Environment.NewLine, Fields));
        }

        public void WriteDataLine()
        {
            File.AppendAllText(Path, Environment.NewLine + string.Join(",", Fields.Select(f => f.Data)));
        }
        #endregion

        #region read
        public void ReadFields()
        {
            Fields.Clear();
            using (StreamReader reader = new StreamReader(Path))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line?.StartsWith("@") == true)
                        Fields.Add(Field.FromString(line));
                    else
                        return;
                }
            }
        }

        private StreamReader _reader;
        public bool ReadDataLine()
        {
            string line;
            if (_reader == null)
            {
                _reader = new StreamReader(Path);
                do
                {
                    line = _reader.ReadLine();
                } while (line?.StartsWith("@") == true);
            }
            else
            {
                line = _reader.ReadLine();
            }
            if (string.IsNullOrWhiteSpace(line))
            {
                _reader.Dispose();
                return false;
            }
            string[] dl = line.Split(',');
            for (int i = 0; i < Fields.Count; i++)
                Fields[i].Data = long.Parse(dl[i]);
            return true;
        }
        #endregion
    }
}
