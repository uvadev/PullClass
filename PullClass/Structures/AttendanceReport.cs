using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Id;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
public class AttendanceReport {
    [JsonProperty("class_id")]
    public ClassId ClassId { get; private set; }
    
    [JsonProperty("sessions")]
    public List<AttendanceSessionItem> SessionItems { get; private set; }

    public override string ToString() {
        return "AttendanceReport {" +
               ($"\n{nameof(ClassId)}: {ClassId}," +
                $"\n{nameof(SessionItems)}: {SessionItems.ToPrettyString()}").Indent(4) + 
               "\n}";
    }
}

[PublicAPI]
public class AttendanceSessionItem {
    [JsonProperty("session_id")]
    public SessionId SessionId { get; private set; }
    
    [JsonProperty("start_time")]
    public DateTime? StartTime { get; private set; }
    
    [JsonProperty("end_time")]
    public DateTime? EndTime { get; private set; }
    
    [JsonProperty("students")]
    public List<AttendanceStudentItem> StudentItems { get; private set; }

    public override string ToString() {
        return "AttendanceSessionItem {" +
               ($"\n{nameof(SessionId)}: {SessionId}," +
                $"\n{nameof(StartTime)}: {StartTime}," +
                $"\n{nameof(EndTime)}: {EndTime}," +
                $"\n{nameof(StudentItems)}: {StudentItems.ToPrettyString()}").Indent(4) +
               "\n}";
    }
}

[PublicAPI]
public class AttendanceStudentItem {
    [JsonProperty("student_id")]
    public StudentId StudentId { get; private set; }
    
    [JsonProperty("email")]
    public PersonEmail StudentEmail { get; private set; }
    
    [JsonProperty("ext_person_id")]
    public ExtPersonId ExtStudentId { get; private set; }
    
    [JsonProperty("present")]
    public bool? Present { get; private set; }
    
    [JsonProperty("tardy")]
    public bool? Tardy { get; private set; }
    
    [JsonProperty("join_time")]
    public DateTime? JoinTime { get; private set; }
    
    [JsonProperty("leave_time")]
    public DateTime? LeaveTime { get; private set; }

    public override string ToString() {
        return "AttendanceStudentItem {" +
               ($"\n{nameof(StudentId)}: {StudentId}," +
                $"\n{nameof(StudentEmail)}: {StudentEmail}," +
                $"\n{nameof(ExtStudentId)}: {ExtStudentId}," +
                $"\n{nameof(Present)}: {Present}," +
                $"\n{nameof(Tardy)}: {Tardy}," +
                $"\n{nameof(JoinTime)}: {JoinTime}," +
                $"\n{nameof(LeaveTime)}: {LeaveTime}").Indent(4) + 
               "\n}";
    }
}
