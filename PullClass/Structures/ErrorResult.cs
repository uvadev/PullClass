using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PullClass.Structures; 

[PublicAPI]
public class ErrorResult {
    [JsonProperty("error")]
    public string Error { get; private set; }
    
    [JsonProperty("data")]
    [CanBeNull]
    public JObject Data { get; private set; }
}
