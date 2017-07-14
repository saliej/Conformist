using System.Collections.Generic;
using System.Xml.Serialization;

namespace Conformist.Configuration
{
    public class ProcessConfigSection : XmlConfigurationSection<List<ProcessConfig>>
    {
    }

    public class ProcessConfig
    {
        [XmlAttribute("autoStart")]
        public bool AutoStart { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Path { get; set; }

        [XmlElement]
        public string Arguments { get; set; }

        [XmlElement]
        public DisplayConfig Display { get; set; }
    }
}
