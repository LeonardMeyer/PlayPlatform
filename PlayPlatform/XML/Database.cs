using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlayPlatform.XML
{
    [DataContract]
    public class Database
    {
        [DataMember]
        public string Hostname { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string DatabaseName { get; set; }

        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string Dbdriver { get; set; }

        [DataMember]
        public string Right { get; set; }

        [DataMember]
        public string Version { get; set; }

        public Database(string hostname, string password, string username, string databaseName, string right, string dbdriver, string connectionString, string version)
        {
            Hostname = hostname;
            Password = password;
            Username = username;
            DatabaseName = databaseName;
            Right = right;
            Dbdriver = dbdriver;
            ConnectionString = connectionString;
            Version = version;
        }
    }
}
