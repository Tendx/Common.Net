using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Zen
{
    public static class Serialization
    {
        public static T DeepCopy<T>(this T src) where T : ModelBase
        {
            if (src is null) return null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, src);
                ms.Position = 0;
                return formatter.Deserialize(ms) as T;
            }
        }

        public static void SaveXml<T>(T config, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, config);
            writer.Close();
        }

        public static T LoadXml<T>(string path)
        {
            if (!File.Exists(path)) return default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(path);
            try
            {
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
