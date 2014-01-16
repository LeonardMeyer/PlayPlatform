using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlayPlatform.XML
{
    [DataContract]
    public class Manifest
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public Application Application { get; set; }

        [DataMember]
        public Database Database { get; set; }

        [DataMember]
        public Server Server { get; set; }

        [DataMember]
        public Validation Validation { get; set; }

        public Manifest(string version, Application application, Database database, Server server, Validation validation)
        {
            Version = version;
            Application = application;
            Database = database;
            Server = server;
            Validation = validation;
        }
    }
}
