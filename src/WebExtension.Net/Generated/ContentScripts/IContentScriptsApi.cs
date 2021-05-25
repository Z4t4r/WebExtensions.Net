using System.Threading.Tasks;

namespace WebExtension.Net.ContentScripts
{
    /// <summary></summary>
    public partial interface IContentScriptsApi
    {
        /// <summary>Register a content script programmatically</summary>
        /// <param name="contentScriptOptions"></param>
        ValueTask Register(RegisteredContentScriptOptions contentScriptOptions);
    }
}
