using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class User {
    [JsonProperty("id")]
    public UserId Id { get; private set; }
    
    [JsonProperty("ext_user_id")]
    public ExtUserId ExtId { get; private set; }
    
    [JsonProperty("email")]
    public PersonEmail Email { get; private set; }
    
    [JsonProperty("user_name")]
    public string UserName { get; private set; }
    
    [JsonProperty("is_zoom_configured")]
    public bool? IsZoomConfigured { get; private set; }
    
    [JsonProperty("roles")]
    public UserRole Roles { get; private set; }

    public override string ToString() {
        return "User {" +
               ($"\n{nameof(Id)}: {Id}," +
                $"\n{nameof(ExtId)}: {ExtId}," +
                $"\n{nameof(Email)}: {Email}," +
                $"\n{nameof(UserName)}: {UserName}," +
                $"\n{nameof(IsZoomConfigured)}: {IsZoomConfigured}," +
                $"\n{nameof(Roles)}: {Roles}").Indent(4) + 
               "\n}";
    }
}
