using JsBind.Net;
using System.Text.Json.Serialization;

namespace WebExtensions.Net.Cookies
{
    // Type Class
    /// <summary>Information to filter the cookies being retrieved.</summary>
    [BindAllProperties]
    public partial class GetAllDetails : BaseObject
    {
        /// <summary>Restricts the retrieved cookies to those whose domains match or are subdomains of this one.</summary>
        [JsonPropertyName("domain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Domain { get; set; }

        /// <summary>Restricts the retrieved cookies to those whose first-party domains match this one. This attribute is required if First-Party Isolation is enabled. To not filter by a specific first-party domain, use `null` or `undefined`.</summary>
        [JsonPropertyName("firstPartyDomain")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FirstPartyDomain { get; set; }

        /// <summary>Filters the cookies by name.</summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; }

        /// <summary>Restricts the retrieved cookies to those whose path exactly matches this string.</summary>
        [JsonPropertyName("path")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Path { get; set; }

        /// <summary>Filters the cookies by their Secure property.</summary>
        [JsonPropertyName("secure")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Secure { get; set; }

        /// <summary>Filters out session vs. persistent cookies.</summary>
        [JsonPropertyName("session")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Session { get; set; }

        /// <summary>The cookie store to retrieve cookies from. If omitted, the current execution context's cookie store will be used.</summary>
        [JsonPropertyName("storeId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StoreId { get; set; }

        /// <summary>Restricts the retrieved cookies to those that would match the given URL.</summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Url { get; set; }
    }
}
