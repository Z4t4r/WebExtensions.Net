using JsBind.Net;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebExtensions.Net.Downloads
{
    // Type Class
    /// <summary>Parameters that combine to specify a predicate that can be used to select a set of downloads.  Used for example in search() and erase()</summary>
    [BindAllProperties]
    public partial class DownloadQuery : BaseObject
    {
        /// <summary>Number of bytes received so far from the host, without considering file compression.</summary>
        [JsonPropertyName("bytesReceived")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? BytesReceived { get; set; }

        /// <summary>Indication of whether this download is thought to be safe or known to be suspicious.</summary>
        [JsonPropertyName("danger")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DangerType? Danger { get; set; }

        /// <summary>Limits results to downloads that ended after the given ms since the epoch.</summary>
        [JsonPropertyName("endedAfter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DownloadTime EndedAfter { get; set; }

        /// <summary>Limits results to downloads that ended before the given ms since the epoch.</summary>
        [JsonPropertyName("endedBefore")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DownloadTime EndedBefore { get; set; }

        /// <summary></summary>
        [JsonPropertyName("endTime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EndTime { get; set; }

        /// <summary>Why a download was interrupted.</summary>
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public InterruptReason? Error { get; set; }

        /// <summary></summary>
        [JsonPropertyName("exists")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Exists { get; set; }

        /// <summary>Absolute local path.</summary>
        [JsonPropertyName("filename")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Filename { get; set; }

        /// <summary>Limits results to <see href='#type-DownloadItem'>DownloadItems</see> whose <c>filename</c> matches the given regular expression.</summary>
        [JsonPropertyName("filenameRegex")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FilenameRegex { get; set; }

        /// <summary>Number of bytes in the whole file post-decompression, or -1 if unknown.</summary>
        [JsonPropertyName("fileSize")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? FileSize { get; set; }

        /// <summary></summary>
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Id { get; set; }

        /// <summary>Setting this integer limits the number of results. Otherwise, all matching <see href='#type-DownloadItem'>DownloadItems</see> will be returned.</summary>
        [JsonPropertyName("limit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Limit { get; set; }

        /// <summary>The file's MIME type.</summary>
        [JsonPropertyName("mime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Mime { get; set; }

        /// <summary>Setting elements of this array to <see href='#type-DownloadItem'>DownloadItem</see> properties in order to sort the search results. For example, setting <c>orderBy='startTime'</c> sorts the <see href='#type-DownloadItem'>DownloadItems</see> by their start time in ascending order. To specify descending order, prefix <c>orderBy</c> with a hyphen: '-startTime'.</summary>
        [JsonPropertyName("orderBy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> OrderBy { get; set; }

        /// <summary>True if the download has stopped reading data from the host, but kept the connection open.</summary>
        [JsonPropertyName("paused")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Paused { get; set; }

        /// <summary>This array of search terms limits results to <see href='#type-DownloadItem'>DownloadItems</see> whose <c>filename</c> or <c>url</c> contain all of the search terms that do not begin with a dash '-' and none of the search terms that do begin with a dash.</summary>
        [JsonPropertyName("query")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Query { get; set; }

        /// <summary>Limits results to downloads that started after the given ms since the epoch.</summary>
        [JsonPropertyName("startedAfter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DownloadTime StartedAfter { get; set; }

        /// <summary>Limits results to downloads that started before the given ms since the epoch.</summary>
        [JsonPropertyName("startedBefore")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DownloadTime StartedBefore { get; set; }

        /// <summary></summary>
        [JsonPropertyName("startTime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StartTime { get; set; }

        /// <summary>Indicates whether the download is progressing, interrupted, or complete.</summary>
        [JsonPropertyName("state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public State? State { get; set; }

        /// <summary>Number of bytes in the whole file, without considering file compression, or -1 if unknown.</summary>
        [JsonPropertyName("totalBytes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? TotalBytes { get; set; }

        /// <summary>Limits results to downloads whose totalBytes is greater than the given integer.</summary>
        [JsonPropertyName("totalBytesGreater")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? TotalBytesGreater { get; set; }

        /// <summary>Limits results to downloads whose totalBytes is less than the given integer.</summary>
        [JsonPropertyName("totalBytesLess")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? TotalBytesLess { get; set; }

        /// <summary>Absolute URL.</summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Url { get; set; }

        /// <summary>Limits results to <see href='#type-DownloadItem'>DownloadItems</see> whose <c>url</c> matches the given regular expression.</summary>
        [JsonPropertyName("urlRegex")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UrlRegex { get; set; }
    }
}
