using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core
{
    /// <summary>
    /// Code editor plugin interface
    /// </summary>
    public interface ICodeEditor
    {
        /// <summary>
        /// Name of the ICodeEditor
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the ICodeEditor
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author(s) of the ICodeEditor
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Current version of the ICodeEditor
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Called on initialization
        /// </summary>
        void Initialize(ICodeEditorHost ceHost);

        /// <summary>
        /// Called when disposed
        /// </summary>
        void Dispose(ICodeEditorHost ceHost);
    }
}
