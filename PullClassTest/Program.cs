using System;
using System.Linq;
using System.Threading.Tasks;
using PullClass;
using PullClass.Structures;

namespace PullClassTest;

internal static class Program {
    
    // This program demonstrates some usage of the library.
    
    public static async Task Main(string[] args) {
        var api = new ClassApi(
            Environment.GetEnvironmentVariable("CLASS_BASE_URL"),
            Environment.GetEnvironmentVariable("CLASS_KEY"),
            Environment.GetEnvironmentVariable("CLASS_SECRET")
        );

        ScheduledClass sc = null;
        await foreach (var scheduledClass in api.StreamScheduledClasses()) {
            sc = scheduledClass;
            Console.WriteLine(scheduledClass);
        }

        if (sc == null) {
            return;
        }

        Console.WriteLine(await api.GetScheduledClass(sc.Id));

        await foreach (var enrollment in api.StreamEnrollments(sc.Id)) {
            Console.WriteLine(enrollment);
        }

        User us = null;
        await foreach (var user in api.StreamUsers().Where(u => u.Roles != UserRole.None)) {
            us ??= user;
            Console.WriteLine(user);
        }

        if (us == null) {
            return;
        }

        Console.WriteLine(await api.GetUser(us.Id));

        Console.WriteLine(await api.GetAttendanceReport(sc.Id));
        await foreach (var metric in api.StreamMetrics(sc.Id)) {
            Console.WriteLine(metric);
        }
        
        await foreach (var date in api.StreamClassDates(sc.Id)) {
            Console.WriteLine(date);
        }
    }
}