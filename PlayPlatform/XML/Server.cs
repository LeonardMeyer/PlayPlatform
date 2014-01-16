using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlayPlatform.XML
{
    [DataContract]
    public class Server
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Type { get; set; }

        public Server(string version, string type)
        {
            Version = version;
            Type = type;
        }
    }
}
