using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PlayPlatform.XML
{
    public static class XMLParser
    {
        //XML to domain objects
        public static Manifest FromXML(string pathToXml){
            DataContractSerializer dcs = new DataContractSerializer(typeof(Manifest));
            FileStream fs = new FileStream(pathToXml, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            Manifest deserializedManifest = (Manifest)dcs.ReadObject(reader);
            reader.Close();
            fs.Close();
            return deserializedManifest;
        }

        //Domain objects to XML
        public static string toXML(Manifest manifest)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(manifest.GetType());
                serializer.WriteObject(memoryStream, manifest);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
