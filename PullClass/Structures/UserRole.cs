using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PullClass.Util;

namespace PullClass.Structures; 

[PublicAPI]
[Flags]
[JsonConverter(typeof(ClassApiFlagsEnumConverter<UserRole>))]
public enum UserRole {
    None = 0,
    [ClassApiRepresentation("admin")]
    Admin = 1 << 0,
    [ClassApiRepresentation("instructor")]
    Instructor = 1 << 1,
    [ClassApiRepresentation("instructional-designer")]
    InstructionalDesigner = 1 << 2
}
