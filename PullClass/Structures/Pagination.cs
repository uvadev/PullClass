using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PullClass.Structures; 

[PublicAPI]
public class Pagination {
    [JsonProperty("total_records")]
    public ulong TotalRecords { get; private set; }
    
    [JsonProperty("current_page")]
    public ulong CurrentPage { get; private set; }
    
    [JsonProperty("total_pages")]
    public ulong TotalPages { get; private set; }
    
    [JsonProperty("next_page")]
    public ulong? NextPage { get; private set; }
    
    [JsonProperty("prev_page")]
    public ulong? PrevPage { get; private set; }
}
