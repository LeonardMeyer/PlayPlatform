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
    public class Validation
    {

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }


        [IgnoreDataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string Date { get; set; }

        public Validation(string firstName, string lastName, string date)
        {
            FirstName = firstName;
            LastName = lastName;
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
