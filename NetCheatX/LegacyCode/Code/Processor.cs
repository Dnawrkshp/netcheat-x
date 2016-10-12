using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace LegacyCode.Code
{
    public class Processor
    {
        private List<ILegacyType> _codes = null;
        private IPluginHost _host = null;

        private static Dictionary<char, Type> LegacyCodeTypes = null;

        // Code information
        private string _codeText = "";
        public string CodeText
        {
            get { return _codeText; }
            set
            {
                _codeText = value;
                SetCodes(_codeText);
            }
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public bool Constant { get; set; }

        #region Public Static Functions

        public static ILegacyType GetILegacyType(char c)
        {
            // Unsupprted code type
            if (!LegacyCodeTypes.Keys.Contains(c))
                return null;

            Type t = LegacyCodeTypes[c];

            // Create new instance from type
            return (ILegacyType)Activator.CreateInstance(t);
        }

        public static void AddLegacyCode(IPluginHost host, ref List<ILegacyType> codes, ref int index, string[] lines)
        {
            ILegacyType legacyCode = GetILegacyType(lines[index][0]);

            // Unsupported code type
            if (legacyCode == null)
                return;

            // Load code from string
            if (!legacyCode.FromString(host, ref index, lines))
            {
                legacyCode.Dispose();
                legacyCode = null;
                return;
            }

            codes.Add(legacyCode);
        }

        public static bool IsValidHex(string word)
        {
            foreach (char c in word)
            {
                if ((c < 0x30 || c > 0x39) && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
                    return false;
            }
            return true;
        }

        #endregion

        public Processor(IPluginHost host)
        {
            _host = host;

            if (LegacyCodeTypes == null)
            {
                LegacyCodeTypes = new Dictionary<char, Type>();

                LegacyCodeTypes.Add('0', typeof(LegacyWrite));
                LegacyCodeTypes.Add('1', typeof(LegacyTextWrite));
                LegacyCodeTypes.Add('2', typeof(LegacyFloatWrite));
                LegacyCodeTypes.Add('4', typeof(LegacyCondensedWrite));
                LegacyCodeTypes.Add('6', typeof(LegacyPointerWrite));
                LegacyCodeTypes.Add('A', typeof(LegacyCopyPaste));
                LegacyCodeTypes.Add('D', typeof(LegacyComparisonEq));
                LegacyCodeTypes.Add('E', typeof(LegacyComparisonMaskUnset));
                LegacyCodeTypes.Add('F', typeof(LegacyCopyWrite));
            }
        }

        public bool Execute()
        {
            if (_codes == null || _host == null)
                return false;

            foreach (ILegacyType code in _codes)
                code.Execute(_host);

            return true;
        }

        // Clean up
        public void Dispose()
        {
            _host = null;

            CleanupCodes();
        }


        private void SetCodes(string codes)
        {
            int index = 0;

            if (codes == null || codes.Length == 0)
                return;

            CleanupCodes();
            _codes = new List<ILegacyType>();

            string[] lines = RemoveComments(codes).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (index = 0; index < lines.Length; index++)
                AddLegacyCode(_host, ref _codes, ref index, lines);
        }

        private void CleanupCodes()
        {
            if (_codes != null)
            {
                foreach (ILegacyType type in _codes)
                    type.Dispose();
                _codes.Clear();
                _codes = null;
            }
        }

        private string RemoveComments(string codes)
        {
            // Remove comments
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            return System.Text.RegularExpressions.Regex.Replace(codes, re, "$1");
        }
    }
}
