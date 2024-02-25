using System;

namespace PullClass.Util; 

[AttributeUsage(AttributeTargets.Field)]
internal sealed class ClassApiRepresentationAttribute : Attribute {
    internal string Representation { get; }

    public ClassApiRepresentationAttribute(string representation) {
        Representation = representation;
    }
}
