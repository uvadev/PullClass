using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PullClass; 

[PublicAPI]
public class ClassApiException : Exception {
    public ClassApiException() { }
    public ClassApiException(string message) : base(message) { }
    public ClassApiException(string message, Exception innerException) : base(message, innerException) { }
    protected ClassApiException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context) { }
}