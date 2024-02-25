using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
[JsonConverter(typeof(ClassApiEnumConverter<EnrollmentRole>))]
public enum EnrollmentRole {
    [ClassApiRepresentation("student")]
    Student,
    [ClassApiRepresentation("teacher")]
    Teacher,
    [ClassApiRepresentation("assistant")]
    Assistant
}
