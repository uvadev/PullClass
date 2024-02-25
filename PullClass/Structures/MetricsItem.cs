using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class MetricsItem {
    [JsonProperty("student_id")]
    public StudentId StudentId { get; private set; }
    
    [JsonProperty("email")]
    public PersonEmail Email { get; private set; }
    
    [JsonProperty("ext_person_id")]
    public ExtPersonId ExtPersonId { get; private set; }
    
    [JsonProperty("event_start")]
    public DateTime? EventStart { get; private set; }
    
    [JsonProperty("event_end")]
    public DateTime? EventEnd { get; private set; }
    
    [JsonProperty("event_duration")]
    public long? EventDuration { get; private set; }
    
    [JsonProperty("event_type")]
    public MetricsEventType? EventType { get; private set; }
    
    [JsonProperty("event_value")]
    public string EventValue { get; private set; }

    public override string ToString() {
        return "MetricsItem {" +
               ($"\n{nameof(StudentId)}: {StudentId}," +
                $"\n{nameof(Email)}: {Email}," +
                $"\n{nameof(ExtPersonId)}: {ExtPersonId}," +
                $"\n{nameof(EventStart)}: {EventStart}," +
                $"\n{nameof(EventEnd)}: {EventEnd}," +
                $"\n{nameof(EventDuration)}: {EventDuration}," +
                $"\n{nameof(EventType)}: {EventType}," +
                $"\n{nameof(EventValue)}: {EventValue}").Indent(4) + 
               "\n}";
    }
}

[PublicAPI]
[JsonConverter(typeof(ClassApiEnumConverter<MetricsEventType>))]
public enum MetricsEventType {
    [ClassApiRepresentation("ATTENDANCE")]
    Attendance,
    [ClassApiRepresentation("FEEDBACK")]
    Feedback,
    [ClassApiRepresentation("FOCUSED")]
    Focused,
    [ClassApiRepresentation("IS_AUDIO_MUTED")]
    IsAudioMuted,
    [ClassApiRepresentation("IS_TALKING")]
    IsTalking,
    [ClassApiRepresentation("IS_VIDEO_ON")]
    IsVideoOn,
    [ClassApiRepresentation("PRESENTING")]
    Presenting,
    [ClassApiRepresentation("RAISED_HAND")]
    RaisedHand,
    [ClassApiRepresentation("REACTION")]
    Reaction,
    [ClassApiRepresentation("SPOTLIGHT")]
    Spotlight,
    [ClassApiRepresentation("STARS")]
    Stars,
    [ClassApiRepresentation("PROCTOR_STATUS")]
    ProctorStatus,
    [ClassApiRepresentation("TAB_OPENED")]
    TabOpened,
    [ClassApiRepresentation("ZOOMSTATE_STUDENT_ID")]
    ZoomStateStudentId,
    [ClassApiRepresentation("TEMP_ROLE")]
    TempRole,
    [ClassApiRepresentation("USER_NAME")]
    UserName,
    [ClassApiRepresentation("PLAYBACK_JOIN")]
    PlaybackJoin,
    [ClassApiRepresentation("PLAYBACK_START")]
    PlaybackStart,
    [ClassApiRepresentation("PLAYBACK_PAUSE")]
    PlaybackPause,
    [ClassApiRepresentation("PUBLIC_CHAT")]
    PublicChat,
    [ClassApiRepresentation("PRIVATE_CHAT")]
    PrivateChat
}
