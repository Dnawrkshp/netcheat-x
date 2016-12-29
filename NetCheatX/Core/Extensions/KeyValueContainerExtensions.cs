using NetCheatX.Core.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NetCheatX.Core.Extensions
{
    /// <summary>
    /// Extension functions for KeyValueContainer class instances.
    /// </summary>
    public static class KeyValueContainerExtensions
    {
        /// <summary>
        /// Attempts to save the <see cref="KeyValueContainer{TKey, TValue}"/> to a given path in XML format.
        /// </summary>
        /// <param name="kvc">The instance of <see cref="KeyValueContainer{TKey, TValue}"/> to save.</param>
        /// <param name="fileName">The file to which you want to write the XML-formatted <see cref="ICollection{T}"/> to.</param>
        public static void SaveXML(this Containers.KeyValueContainer<string, string> kvc, string fileName)
        {
            XElement element;
            XmlWriter xml = XmlWriter.Create(fileName, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true });

            xml.WriteStartElement("prefs");
            foreach (string key in kvc.Keys)
            {
                if (kvc[key] == null)
                    continue;

                element = new XElement(key.ToString(), kvc[key].ToString());
                element.WriteTo(xml);
            }
            xml.WriteEndElement();

            xml.Close();
        }

        /// <summary>
        /// Attempts to load the <see cref="KeyValueContainer{TKey, TValue}"/> from a given XML file.
        /// </summary>
        /// <param name="kvc">The instance of <see cref="KeyValueContainer{TKey, TValue}"/> to load into.</param>
        /// <param name="fileName">The file from which you want to read the XML-formatted <see cref="ICollection{T}"/> from.</param>
        public static bool LoadXML(this Containers.KeyValueContainer<string, string> kvc, string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return false;

            kvc.Clear();

            XmlReader xml = XmlReader.Create(fileName);

            while (xml.Read())
            {
                if (xml.IsStartElement())
                {
                    XElement x = (XElement)XNode.ReadFrom(xml);
                    foreach (XElement c in x.Elements())
                        kvc.Add(c.Name.LocalName, c.Value);
                }
            }

            xml.Close();
            return true;
        }
    }
}
