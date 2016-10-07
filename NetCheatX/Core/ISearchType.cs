using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Search type plugin interface
    /// </summary>
    public interface ISearchType
    {
        /// <summary>
        /// Name of the ISearchType
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the ISearchType's purpose
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the ISearchType
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the ISearchType
        /// </summary>
        string Version { get; }

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
        /// Process the search parameter
        /// </summary>
        /// <param name="value">Value inputed. Type is dictated by ParamType</param>
        /// <returns>value converted to byte array</returns>
        byte[] ProcessParam(object value);

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

        /// <summary>
        /// Called when the search UI is loaded
        /// </summary>
        void Initialize(IPluginHost host);

        /// <summary>
        /// Called when search UI is unloading
        /// </summary>
        void Dispose(IPluginHost host);
    }
}
