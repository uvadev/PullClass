using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class Enrollment {
    [JsonProperty("student_id")]
    public StudentId StudentId { get; private set; }
    
    [JsonProperty("meeting_id")]
    public MeetingId MeetingId { get; private set; }
    
    [JsonProperty("user_name")]
    public string UserName { get; private set; }
    
    [JsonProperty("first_name")]
    public string FirstName { get; private set; }
    
    [JsonProperty("last_name")]
    public string LastName { get; private set; }
    
    [JsonProperty("email")]
    public string Email { get; private set; }
    
    [JsonProperty("ext_person_id")]
    public ExtPersonId PersonId { get; private set; }
    
    [JsonProperty("role")]
    public EnrollmentRole Role { get; private set; }

    public override string ToString() {
        return "Enrollment {" +
               ($"\n{nameof(StudentId)}: {StudentId}," +
                $"\n{nameof(MeetingId)}: {MeetingId}," +
                $"\n{nameof(UserName)}: {UserName}," +
                $"\n{nameof(FirstName)}: {FirstName}," +
                $"\n{nameof(LastName)}: {LastName}," +
                $"\n{nameof(Email)}: {Email}," +
                $"\n{nameof(PersonId)}: {PersonId}," +
                $"\n{nameof(Role)}: {Role}").Indent(4) +
               "\n}";
    }
}
