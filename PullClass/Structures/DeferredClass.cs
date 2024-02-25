using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class DeferredClass {
    [JsonProperty("class_deferred_id")]
    public DeferredClassId DeferredId { get; private set; }
    
    [JsonProperty("class_id")]
    public ClassId Id { get; private set; }
    
    [JsonProperty("ext_class_id")]
    public ExtClassId ExtId { get; private set; }
    
    [JsonProperty("email")]
    public string Email { get; private set; }
    
    [JsonProperty("class_name")]
    public string Name { get; private set; }
    
    [JsonProperty("class_description")]
    public string Description { get; private set; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; private set; }
    
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; private set; }

    public override string ToString() {
        return "DeferredClass {" +
               ($"\n{nameof(DeferredId)}: {DeferredId}," + 
                $"\n{nameof(Id)}: {Id}," +
                $"\n{nameof(ExtId)}: {ExtId}," +
                $"\n{nameof(Email)}: {Email}," +
                $"\n{nameof(Name)}: {Name}," +
                $"\n{nameof(Description)}: {Description}," +
                $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                $"\n{nameof(UpdatedAt)}: {UpdatedAt}").Indent(4) + 
               "\n}";
    }
}
