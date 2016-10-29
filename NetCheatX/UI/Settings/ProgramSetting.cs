using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace NetCheatX.UI.Settings
{
    [Serializable, XmlRoot("Config"), XmlType("Config")]
    public class ProgramSetting
    {
        private string _path = null;

        [XmlArray("PluginDirectories")]
        public List<string> PluginPaths { get; set; }

        [XmlElement("Run64Bit")]
        public bool Run64 { get; set; }

        public static ProgramSetting Load(string path)
        {
            ProgramSetting con;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProgramSetting));
                FileStream xmlFile = new FileStream(path, FileMode.Open);
                con = (ProgramSetting)serializer.Deserialize(xmlFile);

                con._path = path;

                if (con.PluginPaths == null)
                {
                    con.PluginPaths = new List<string>();
                    con.PluginPaths.Add("Plugins");
                    con.Save();
                }
                return con;
            }
            catch (Exception e) { Program.logger.LogException(e); }

            con = new ProgramSetting();
            con._path = path;
            con.PluginPaths = new List<string>();
            con.PluginPaths.Add("Plugins");
            con.Run64 = IntPtr.Size == 8;
            con.Save();

            return con;
        }

        public void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ProgramSetting));
                using (Stream s = File.OpenWrite(_path))
                {
                    serializer.Serialize(s, this);
                }
            }
            catch (Exception e) { Program.logger.LogException(e); }
        }

        public void Close()
        {
            Save();

            // Clean up
            PluginPaths.Clear();
            PluginPaths = null;

            _path = null;
        }
    }
}
