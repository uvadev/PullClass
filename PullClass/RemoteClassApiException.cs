using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace PullClass; 

[PublicAPI]
public class RemoteClassApiException : ClassApiException {
    [CanBeNull] 
    public JObject ErrData { get; private set; }

    public RemoteClassApiException(JObject errData = null) {
        ErrData = errData;
    }

    public RemoteClassApiException(string message, JObject errData = null) : base(message) {
        ErrData = errData;
    }

    public RemoteClassApiException(string message, Exception innerException, JObject errData = null) : base(message, innerException) {
        ErrData = errData;
    }

    protected RemoteClassApiException([NotNull] SerializationInfo info, StreamingContext context, JObject errData = null) : base(info, context) {
        ErrData = errData;
    }
}
