using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core;

namespace BasicSearch
{
    public class Plugin : IPlugin
    {
        // Search methods
        SearchMethod.EqualTo _equalTo = new SearchMethod.EqualTo();
        SearchMethod.NotEqualTo _notEqualTo = new SearchMethod.NotEqualTo();
        SearchMethod.GreaterThan _greaterThan = new SearchMethod.GreaterThan();
        SearchMethod.GreaterThanOrEqualTo _greaterThanOrEqual = new SearchMethod.GreaterThanOrEqualTo();
        SearchMethod.LessThan _lessThan = new SearchMethod.LessThan();
        SearchMethod.LessThanOrEqualTo _lessThanOrEqual = new SearchMethod.LessThanOrEqualTo();
        SearchMethod.ValueBetween _valueBetween = new SearchMethod.ValueBetween();

        // Search types
        SearchType.OneByteS _oneByteS = new SearchType.OneByteS();
        SearchType.OneByteU _oneByteU = new SearchType.OneByteU();
        SearchType.TwoByteS _twoByteS = new SearchType.TwoByteS();
        SearchType.TwoByteU _twoByteU = new SearchType.TwoByteU();
        SearchType.FourByteS _fourByteS = new SearchType.FourByteS();
        SearchType.FourByteU _fourByteU = new SearchType.FourByteU();
        SearchType.EightByteS _eightByteS = new SearchType.EightByteS();
        SearchType.EightByteU _eightByteU = new SearchType.EightByteU();
        SearchType.XByteS _xByteS = new SearchType.XByteS();
        SearchType.XByteU _xByteU = new SearchType.XByteU();
        SearchType.SingleFP _singleFP = new SearchType.SingleFP();
        SearchType.DoubleFP _doubleFP = new SearchType.DoubleFP();
        SearchType.Text _text = new SearchType.Text();

        // Type editors
        SearchParamEditor.typeSByte _teSByte = new SearchParamEditor.typeSByte();
        SearchParamEditor.typeByte _teByte = new SearchParamEditor.typeByte();
        SearchParamEditor.typeShort _teShort = new SearchParamEditor.typeShort();
        SearchParamEditor.typeUShort _teUShort = new SearchParamEditor.typeUShort();
        SearchParamEditor.typeInt _teInt = new SearchParamEditor.typeInt();
        SearchParamEditor.typeUInt _teUInt = new SearchParamEditor.typeUInt();
        SearchParamEditor.typeLong _teLong = new SearchParamEditor.typeLong();
        SearchParamEditor.typeULong _teULong = new SearchParamEditor.typeULong();
        SearchParamEditor.typeFloat _teFloat = new SearchParamEditor.typeFloat();
        SearchParamEditor.typeDouble _teDouble = new SearchParamEditor.typeDouble();
        SearchParamEditor.typeString _teString = new SearchParamEditor.typeString();
        SearchParamEditor.typeByteArray _teByteArray = new SearchParamEditor.typeByteArray();

        public void Initialize(IPluginHost host)
        {
            // Register Search Methods
            host.RegisterSearchMethod(_equalTo);
            host.RegisterSearchMethod(_notEqualTo);
            host.RegisterSearchMethod(_greaterThan);
            host.RegisterSearchMethod(_greaterThanOrEqual);
            host.RegisterSearchMethod(_lessThan);
            host.RegisterSearchMethod(_lessThanOrEqual);
            host.RegisterSearchMethod(_valueBetween);

            // Register Search Types
            host.RegisterSearchType(_oneByteS);
            host.RegisterSearchType(_oneByteU);
            host.RegisterSearchType(_twoByteS);
            host.RegisterSearchType(_twoByteU);
            host.RegisterSearchType(_fourByteS);
            host.RegisterSearchType(_fourByteU);
            host.RegisterSearchType(_eightByteS);
            host.RegisterSearchType(_eightByteU);
            host.RegisterSearchType(_xByteS);
            host.RegisterSearchType(_xByteU);
            host.RegisterSearchType(_singleFP);
            host.RegisterSearchType(_doubleFP);
            host.RegisterSearchType(_text);

            // Register Type Editors
            host.RegisterTypeEditor(_teSByte);
            host.RegisterTypeEditor(_teByte);
            host.RegisterTypeEditor(_teShort);
            host.RegisterTypeEditor(_teUShort);
            host.RegisterTypeEditor(_teInt);
            host.RegisterTypeEditor(_teUInt);
            host.RegisterTypeEditor(_teLong);
            host.RegisterTypeEditor(_teULong);
            host.RegisterTypeEditor(_teFloat);
            host.RegisterTypeEditor(_teDouble);
            host.RegisterTypeEditor(_teString);
            host.RegisterTypeEditor(_teByteArray);
        }

        public void Dispose(IPluginHost host)
        {
            // Clean up
            _equalTo = null;
            _notEqualTo = null;
            _greaterThan = null;
            _greaterThanOrEqual = null;
            _lessThan = null;
            _lessThanOrEqual = null;
            _valueBetween = null;

            _oneByteS = null;
            _oneByteU = null;
            _twoByteS = null;
            _twoByteU = null;
            _fourByteS = null;
            _fourByteU = null;
            _eightByteS = null;
            _eightByteU = null;
            _xByteS = null;
            _xByteU = null;
            _singleFP = null;
            _doubleFP = null;
            _text = null;

            _teSByte = null;
            _teByte = null;
            _teShort = null;
            _teUShort = null;
            _teInt = null;
            _teUInt = null;
            _teLong = null;
            _teULong = null;
            _teFloat = null;
            _teDouble = null;
            _teString = null;
            _teByteArray = null;
        }
    }
}
