using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PullClass.Structures; 

[PublicAPI]
public class LaunchLink {
    [JsonProperty("launch_url")]
    public string LaunchUrl { get; private set; }

    public override string ToString() {
        return "LaunchLink {" + 
               $"\n{nameof(LaunchUrl)}: {LaunchUrl}" +
               "\n}";
    }
}
