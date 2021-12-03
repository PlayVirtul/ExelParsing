using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory
{
    public class Threat : INotifyPropertyChanged
    {
        public List<object> values;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string Name { get; set; }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }
        private string source;
        public string Source
        {
            get => source;
            set
            {
                source = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
            }
        }
        private string target;
        public string Target
        {
            get => target;
            set
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Target)));
            }
        }
        public string confidentiality;
        public string Confidentiality
        {
            get => confidentiality;
            set
            {
                if(value == "1")
                {
                    confidentiality = "да"; 
                }
                else
                {
                    confidentiality = "нет";
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Confidentiality)));
            }
        }
        private string integrity;
        public string Integrity
        {
            get => integrity;
            set
            {
                if (value == "1")
                {
                    integrity = "да";
                }
                else
                {
                    integrity = "нет";
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Integrity)));
            }
        }
        private string availability;
        public string Availability
        {
            get => availability;
            set
            {
                if (value == "1")
                {
                    availability = "да";
                }
                else
                {
                    availability = "нет";
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Availability)));
            }
        }

        public Threat(string id, string name, string description, string source, string target, string сonfidentiality, string integrity, string availability)
        {
            Id = id;
            Name = name;
            Description = description;
            Source = source;
            Target = target;
            Confidentiality = сonfidentiality;
            Integrity = integrity;
            Availability = availability;

            values = new List<object>() { Id, Name, Description, Source, Target, Confidentiality, Integrity, Availability };
        }

    }
}
