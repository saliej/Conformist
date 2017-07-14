using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Conformist.Configuration
{
    public class DisplayConfig
    {
        [XmlAttribute("fontName")]
        public string FontName { get; set; }

        [XmlAttribute("fontSize")]
        public int FontSize { get; set; }
    }
}
