using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Conformist.Configuration
{
    public class XmlConfigurationSection<T> : ConfigurationSection where T : class, new()
    {
        private T _configurationItem;
        private XmlSerializer _serializer;

        protected override void Init()
        {
            base.Init();

            var root = new XmlRootAttribute(SectionInformation.Name);
            _serializer = new XmlSerializer(typeof(T), root);
        }

        public static T GetSection(string sectionName)
        {
            var section = (XmlConfigurationSection<T>)ConfigurationManager.GetSection(sectionName);
            return section?._configurationItem;
        }

        public static T GetSection()
        {
            var section = (XmlConfigurationSection<T>)ConfigurationManager.GetSection(typeof(T).Name);
            return section?._configurationItem;
        }

        public static T GetSection(System.Configuration.Configuration configuration, string sectionName)
        {
            var section = (XmlConfigurationSection<T>)configuration.GetSection(sectionName);
            return section?._configurationItem;
        }

        public static T GetSection(System.Configuration.Configuration configuration)
        {
            var section = (XmlConfigurationSection<T>)configuration.GetSection(typeof(T).Name);
            return section?._configurationItem;
        }

        protected override void DeserializeSection(XmlReader reader)
        {
            _configurationItem = (T)_serializer.Deserialize(reader);
        }
    }
}
