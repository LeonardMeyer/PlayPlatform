using PlayLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlayPlatform.XML
{
    [DataContract]
    public class Application
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Icon { get; set; }

        [DataMember]
        public bool Mode { get; set; }

        [DataMember]
        public TechnologyType Technology { get; set; }

        [IgnoreDataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public CategoryType Category { get; set; }

        public Application(string name, string version, string description, string icon, bool mode, TechnologyType technology, CategoryType category, string date)
        {
            Name = name;
            Version = version;
            Description = description;
            Icon = icon;
            Mode = mode;
            Technology = technology;
            Category = category;
            Date = date;
        }
        
        [OnDeserialized]
        void OnDeserializing(StreamingContext context)
        {
            if (this.Date != null)
            {
                this.DateTime = DateTime.ParseExact(this.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

    }
}
