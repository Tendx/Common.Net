using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDFile
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;
            string input = args[0];
            string output = Path.Combine(Path.GetDirectoryName(input), Path.GetFileNameWithoutExtension(input) + ".csv");

            DD dd = new DD(input);
            dd.ReadFields();
            File.WriteAllText(output, string.Join(",", dd.Fields.Select(p => p.Name)));
            while (dd.ReadDataLine())
            {
                File.AppendAllText(output, Environment.NewLine + string.Join(",", dd.Fields.Select(p => p.Value.ToString())));
            }

            //DD dd = new DD("temp.dd");
            //dd.Fields.Add(new B("Active"));
            //dd.Fields.Add(new L("Date", 0));
            //dd.Fields.Add(new D("Price", 2));
            //dd.Fields.Add(new S("Info"));
            //dd.Fields.Add(new M("Type", new Dictionary<int, string> { { 1, "T1" }, { 2, "T2" } }));
            //dd.Fields.Add(new A("Symbol", new List<string> { "S00001", "S00002" }));
            //dd.WriteFields();
            //dd.Fields[0].Value = true.ToString();
            //dd.Fields[1].Value = "20201120";
            //dd.Fields[2].Value = "123.321";
            //dd.Fields[3].Value = "nihao";
            //dd.Fields[4].Value = "T2";
            //dd.Fields[5].Value = "S00001";

            //dd.WriteDataLine();
        }
    }
}
