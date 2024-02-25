using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class ClassDate {
    [JsonProperty("schedule_id")]
    public ScheduleId Id { get; private set; }
    
    [JsonProperty("class_id")]
    public ClassId ClassId { get; private set; }
    
    [JsonProperty("start_time")]
    [JsonConverter(typeof(UnixTimeConverter))]
    public DateTime? StartTime { get; private set; }
    
    [JsonProperty("end_time")]
    [JsonConverter(typeof(UnixTimeConverter))]
    public DateTime? EndTime { get; private set; }

    public override string ToString() {
        return "ClassDate {" +
               ($"\n{nameof(Id)}: {Id}," +
                $"\n{nameof(ClassId)}: {ClassId}," +
                $"\n{nameof(StartTime)}: {StartTime}," +
                $"\n{nameof(EndTime)}: {EndTime}").Indent(4) +
               "\n}";
    }
}
