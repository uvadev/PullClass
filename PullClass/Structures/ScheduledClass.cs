using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class ScheduledClass {
    [JsonProperty("class_id")]
    public ClassId Id { get; private set; }
    
    [JsonProperty("ext_class_id")]
    public ExtClassId ExtId { get; private set; }
    
    [JsonProperty("class_name")]
    public string Name { get; private set; }
    
    [JsonProperty("class_description")]
    public string Description { get; private set; }
    
    [JsonProperty("meeting_id")]
    public MeetingId MeetingId { get; private set; }
    
    [JsonProperty("host")]
    public string Host { get; private set; }
    
    [JsonProperty("invite_link")]
    public string InviteLink { get; private set; }
    
    [JsonProperty("password")]
    public string Password { get; private set; }

    public override string ToString() {
        return "ScheduledClass {" +
               ($"\n{nameof(Id)}: {Id}," +
                $"\n{nameof(ExtId)}: {ExtId}," +
                $"\n{nameof(Name)}: {Name}," +
                $"\n{nameof(Description)}: {Description}," +
                $"\n{nameof(MeetingId)}: {MeetingId}," +
                $"\n{nameof(Host)}: {Host}," +
                $"\n{nameof(InviteLink)}: {InviteLink}," +
                $"\n{nameof(Password)}: {Password}").Indent(4) + 
               "\n}";
    }
}
