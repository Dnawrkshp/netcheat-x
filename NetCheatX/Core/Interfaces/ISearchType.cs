using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Interfaces
{
    /// <summary>
    /// Search type plugin interface
    /// </summary>
    public interface ISearchType : IPluginBase
    {
        /// <summary>
        /// List of all supported platforms in abbreviated format (PS3, PC, iOS, Android)
        /// Leave null if all are supported
        /// </summary>
        string[] SupportedPlatforms { get; }

        /// <summary>
        /// Names of each column
        /// </summary>
        string[] Columns { get; }

        /// <summary>
        /// This is the data type that you expect to be passed to ProcessParam
        /// In most cases use typeof(string)
        /// There MUST be an ITypeEditor that can edit your type
        /// </summary>
        Type ParamType { get; }

        /// <summary>
        /// Whether the data type the ISearchType represents is signed or unsigned
        /// </summary>
        bool Signed { get; }

        /// <summary>
        /// Alignment of data type the ISearchType represents
        /// </summary>
        int Alignment { get; }

        /// <summary>
        /// Process ISearchResult
        /// </summary>
        /// <param name="columnValues">Array of column values. Populated by method</param>
        /// <param name="result">ISearchResult to be processed</param>
        void ProcessResult(out string[] columnValues, ISearchResult result);

        /// <summary>
        /// Process ISearchResult into NetCheat PS3 legacy code
        /// </summary>
        /// <param name="code">Uninitialized string</param>
        /// <param name="result">ISearchResult to be processed</param>
        void ResultToLegacyCode(out string code, ISearchResult result);
    }
}
