using JetBrains.Annotations;

namespace PullClass.Id;

[PublicAPI]
public abstract class EntityId<T> {
    public readonly T Id;

    protected EntityId(T id) {
        Id = id;
    }

    public static implicit operator T(EntityId<T> e) => e.Id;

    public abstract string GetKeyName();
    
    public override string ToString() {
        return $"{GetType().Name}({Id})";
    }
}

[PublicAPI]
public interface IClassHandle { }

[PublicAPI]
public interface IConcreteClassHandle : IClassHandle { }

[PublicAPI]
public interface IPersonHandle { }

[PublicAPI]
public interface ILaunchablePersonHandle : IPersonHandle { }

[PublicAPI]
public interface IUserHandle { }

[PublicAPI]
public class ClassId : EntityId<string>, IConcreteClassHandle {
    public ClassId(string id) : base(id) { }

    public static explicit operator ClassId(string s) => new(s);

    public override string GetKeyName() => "class_id";
}

[PublicAPI]
public class ExtClassId : EntityId<string>, IConcreteClassHandle {
    public ExtClassId(string id) : base(id) { }

    public static explicit operator ExtClassId(string s) => new(s);
    
    public override string GetKeyName() => "ext_class_id";
}

[PublicAPI]
public class MeetingId : EntityId<string> {
    public MeetingId(string id) : base(id) { }

    public static explicit operator MeetingId(string s) => new(s);
    
    public override string GetKeyName() => "meeting_id";
}

[PublicAPI]
public class DeferredClassId : EntityId<uint>, IClassHandle {
    public DeferredClassId(uint id) : base(id) { }

    public static explicit operator DeferredClassId(uint s) => new(s);

    public override string GetKeyName() => "class_deferred_id";
}

[PublicAPI]
public class StudentId : EntityId<string>, IPersonHandle {
    public StudentId(string id) : base(id) { }

    public static explicit operator StudentId(string s) => new(s);
    
    public override string GetKeyName() => "student_id";
}

[PublicAPI]
public class UserId : EntityId<long>, IUserHandle {
    public UserId(long id) : base(id) { }

    public static explicit operator UserId(long i) => new(i);
    
    public override string GetKeyName() => "id";
}

[PublicAPI]
public class ExtUserId : EntityId<string>, IUserHandle {
    public ExtUserId(string id) : base(id) { }

    public static explicit operator ExtUserId(string s) => new(s);
    
    public override string GetKeyName() => "ext_user_id";
}

[PublicAPI]
public class PersonEmail : EntityId<string>, ILaunchablePersonHandle, IUserHandle {
    public PersonEmail(string id) : base(id) { }

    public static explicit operator PersonEmail(string s) => new(s);
    
    public override string GetKeyName() => "email";
}

[PublicAPI]
public class ExtPersonId : EntityId<string>, ILaunchablePersonHandle {
    public ExtPersonId(string id) : base(id) { }

    public static explicit operator ExtPersonId(string s) => new(s);
    
    public override string GetKeyName() => "ext_person_id";
}

[PublicAPI]
public class SessionId : EntityId<string> {
    public SessionId(string id) : base(id) { }

    public static explicit operator SessionId(string s) => new(s);

    public override string GetKeyName() => "session_id";
}

[PublicAPI]
public class ScheduleId : EntityId<string> {
    public ScheduleId(string id) : base(id) { }

    public static explicit operator ScheduleId(string s) => new(s);

    public override string GetKeyName() => "schedule_id";
}

[PublicAPI]
public class TemplateId : EntityId<string> {
    public TemplateId(string id) : base(id) { }

    public static explicit operator TemplateId(string s) => new(s);

    public override string GetKeyName() => "template_id";
}
