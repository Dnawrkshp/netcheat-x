using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Contains relevant data types and delegates.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// CPU Endianess.
        /// </summary>
        public enum Endian
        {
            ///<summary>Little Endian.</summary>
            LittleEndian,
            ///<summary>Big Endian.</summary>
            BigEndian
        }

        /// <summary>
        /// CPU 32/64 bit architecture.
        /// </summary>
        public enum BitArchitecture
        {
            ///<summary>32-bit architecture. Memory ranges from 0 to 0xFFFFFFFF.</summary>
            bit32,
            ///<summary>64-bit architecture. Memory ranges from 0 to 0xFFFFFFFFFFFFFFFF.</summary>
            bit64
        }

        /// <summary>
        /// Process State.
        /// </summary>
        public enum ProcessState
        {
            ///<summary>The process is actively running.</summary>
            Running,
            ///<summary>The process is paused.</summary>
            Paused,
            ///<summary>The process is terminated.</summary>
            Killed
        }

        /// <summary>
        /// Search Mode.
        /// </summary>
        public enum SearchMode
        {
            ///<summary>An ISearchMethod that can only be used for the first scan.</summary>
            First,
            ///<summary>An ISearchMethod that can only be used after the first scan.</summary>
            Next,
            ///<summary>An ISearchMethod that can be used for both the first and later scans.</summary>
            Both
        }

        /// <summary>
        /// PluginBaseContainer Plugin changed EventArgs.
        /// </summary>
        public struct PluginBaseChangedEventArgs
        {
            ///<summary>Plugin extension that added Plugin.</summary>
            public Interfaces.IPluginBase ParentPlugin;
            ///<summary>Plugin added.</summary>
            public Interfaces.IPluginBase Plugin;
        }

        /// <summary>
        /// Search parameter.
        /// </summary>
        public struct SearchParam
        {
            ///<summary>Name of parameter</summary>
            public string name;
            ///<summary>Type of parameter. Ignore if process is set true.</summary>
            public Type type;
            ///<summary>If the parameter needs to be processed by an ISearchType first set this true. It wll be passed to your method as a byte array.</summary>
            public bool process;
            /// <summary>Information on what the parameter is used for.</summary>
            public string description;
        }

        /// <summary>
        /// Memory range.
        /// </summary>
        public struct MemoryRange
        {
            ///<summary>Start of memory range.</summary>
            public ulong start;
            ///<summary>Stop of memory range.</summary>
            public ulong stop;
        }

        /// <summary>
        /// Callback function blueprint for plugins.
        /// </summary>
        /// <param name="host">UI application IPluginHost instance.</param>
        public delegate bool PluginFunctionCallback(Interfaces.IPluginHost host);

        /// <summary>
        /// Callback function blueprint for adding new XForm.
        /// </summary>
        /// <param name="xForm">An uninitialized XForm.</param>
        /// <param name="host">UI application IPluginHost instance.</param>
        public delegate bool PluginWindowCallback(out UI.XForm xForm, Interfaces.IPluginHost host);

        /// <summary>
        /// Callback function blueprint for methods incorporating progress.
        /// </summary>
        /// <param name="pluginBase">Plugin base making call.</param>
        /// <param name="value">Value of progress.</param>
        /// <param name="max">Max value of progress.</param>
        /// <param name="description">Description of current progress.</param>
        public delegate void SetProgressCallback(Interfaces.IPluginBase pluginBase, int value, int max, string description);
    }

    /// <summary>
    /// Represents the version
    /// </summary>
    public class ObjectVersion
    {
        private uint[] _values = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetCheatX.Core.ObjectVersion"/> class that uses the specified <see cref="System.UInt32"/> values.
        /// </summary>
        /// <param name="levels">The values of each level of the version.</param>
        public ObjectVersion(params uint[] levels)
        {
            _values = levels;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object obj)
        {
            if (obj is ObjectVersion && this == (obj as ObjectVersion))
                return true;

            return false;
        }

        /// <summary>
        /// Returns the hash of the object.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation of this <see cref="ObjectVersion"/>.
        /// </summary>
        public override string ToString()
        {
            if (_values != null && _values.Length > 0)
                return String.Join(".", _values);
            else
                return "0";
        }

        private static bool Compare(ObjectVersion v1, ObjectVersion v2, byte mode)
        {
            int max = v1._values.Length < v2._values.Length ? v2._values.Length : v1._values.Length;
            uint vl1, vl2;

            for (int x = 0; x < max; x++)
            {
                vl1 = x >= v1._values.Length ? 0 : v1._values[x];
                vl2 = x >= v2._values.Length ? 0 : v2._values[x];

                switch (mode)
                {
                    case 0: // Equal To
                        if (vl1 != vl2)
                            return false;
                        break;
                    case 1: // Not Equal To
                        if (vl1 != vl2)
                            return true;
                        break;
                    case 2: // Less Than
                        if (vl1 < vl2)
                            return true;
                        if (vl1 > vl2)
                            return false;
                        break;
                    case 3: // Greater Than
                        if (vl1 > vl2)
                            return true;
                        if (vl1 < vl2)
                            return false;
                        break;
                }
            }

            switch (mode)
            {
                case 0: // Equal To
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>Determines whether the two instances of <see cref="NetCheatX.Core.ObjectVersion"/> are equal.</summary>
        public static bool operator ==(ObjectVersion v1, ObjectVersion v2)
        {
            return Compare(v1, v2, 0);
        }

        /// <summary>Determines whether the two instances of <see cref="NetCheatX.Core.ObjectVersion"/> are not equal.</summary>
        public static bool operator !=(ObjectVersion v1, ObjectVersion v2)
        {
            return Compare(v1, v2, 1);
        }

        /// <summary>Determines whether one instance of <see cref="NetCheatX.Core.ObjectVersion"/> is less than the other.</summary>
        public static bool operator <(ObjectVersion v1, ObjectVersion v2)
        {
            return Compare(v1, v2, 2);
        }

        /// <summary>Determines whether one instance of <see cref="NetCheatX.Core.ObjectVersion"/> is greater than the other.</summary>
        public static bool operator >(ObjectVersion v1, ObjectVersion v2)
        {
            return Compare(v1, v2, 3);
        }

        /// <summary>Determines whether one instance of <see cref="NetCheatX.Core.ObjectVersion"/> is less than or equal to the other.</summary>
        public static bool operator <=(ObjectVersion v1, ObjectVersion v2)
        {
            return !Compare(v1, v2, 3);
        }

        /// <summary>Determines whether one instance of <see cref="NetCheatX.Core.ObjectVersion"/> is greater than or equal to the other.</summary>
        public static bool operator >=(ObjectVersion v1, ObjectVersion v2)
        {
            return !Compare(v1, v2, 2);
        }
    }
}
