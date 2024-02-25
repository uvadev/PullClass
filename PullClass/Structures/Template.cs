using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class Template {
    [JsonProperty("template_id")]
    public TemplateId Id { get; private set; }
    
    [JsonProperty("name")]
    public string Name { get; private set; }
    
    [JsonProperty("description")]
    public string Description { get; private set; }
    
    [JsonProperty("status")]
    public TemplateStatus Status { get; private set; }

    public override string ToString() {
        return "Template {" +
               ($"\n{nameof(Id)}: {Id}," +
                $"\n{nameof(Name)}: {Name}," +
                $"\n{nameof(Description)}: {Description}," +
                $"\n{nameof(Status)}: {Status}").Indent(4) + 
               "\n}";
    }
}

[PublicAPI]
[JsonConverter(typeof(ClassApiEnumConverter<TemplateStatus>))]
public enum TemplateStatus {
    [ClassApiRepresentation("draft")]
    Draft,
    [ClassApiRepresentation("published")]
    Published
}
