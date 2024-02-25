using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace PullClass.Util; 

internal static class Extensions {

    internal static async Task<HttpResponseMessage> GetAsyncWithHeaders(this HttpClient client, string url, IEnumerable<(string, string)> headers) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        
        foreach (var (key, val) in headers) {
            requestMessage.Headers.Add(key, val);
        }

        return await client.SendAsync(requestMessage);
    }
    
    internal static async Task<HttpResponseMessage> PostAsyncWithHeaders(this HttpClient client, string url, HttpContent content, IEnumerable<(string, string)> headers) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Content = content;

        foreach (var (key, val) in headers) {
            requestMessage.Headers.Add(key, val);
        }

        return await client.SendAsync(requestMessage);
    }
    
    internal static HttpContent BuildHttpArguments([NotNull] IEnumerable<(string, string)> args) {
        var pairs = args.Where(a => a.Item2 != null)
                        .Select(a => new KeyValuePair<string, string>(a.Item1, a.Item2));

        var content = new FormUrlEncodedContent(pairs);
            
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        return content;
    }
    
    internal static HttpContent BuildHttpJsonBody(JToken json) {
        return new StringContent(json.ToString(), Encoding.Default, "application/json");
    }
    
    internal static string BuildDuplicateKeyQueryString([NotNull] params ValueTuple<string, string>[] args) {
        var entries = new List<string>();
        
        foreach (var (key, val) in args) {
            if (val != null) {
                entries.Add($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(val)}");
            }
        }

        if (entries.Count == 0) {
            return string.Empty;
        }

        return "?" + string.Join("&", entries);
    }
    
    internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response) {
        if (!response.IsSuccessStatusCode) {
            throw new ClassApiException(response.Content.ReadAsStringAsync().Result);
        }

        return response;
    }
        
    internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response, Func<string, Exception> exceptionSupplier) {
        if (!response.IsSuccessStatusCode) {
            throw exceptionSupplier.Invoke(response.Content.ReadAsStringAsync().Result);
        }

        return response;
    }

    internal static string ToStr(this bool b) {
        // The default ToString() returns a capitalized True/False which doesn't work in URLs/JSON
        return b ? "true" 
                 : "false";
    }
    
    [Pure]
    internal static string Indent(this string value, int spaces) {
        var split = value.Split('\n');
        for (var i = 0; i < split.Length - 1; i++) {
            split[i] += "\n";
        }

        var sb = new StringBuilder();
            
        foreach (var s in split) {
            sb.Append(new string(' ', spaces)).Append(s);
        }

        return sb.ToString();
    }
    
    [CanBeNull]
    [Pure]
    internal static string GetApiRepresentation([NotNull] this Enum en) {
        MemberInfo[] member = en.GetType().GetMember(en.ToString());

        if (member.Length <= 0) 
            return null;
            
        object[] attribute = member[0].GetCustomAttributes(typeof(ClassApiRepresentationAttribute), false);
            
        return attribute.Length > 0 ? ((ClassApiRepresentationAttribute) attribute[0]).Representation 
                                    : null;
    }
    
    [Pure]
    private static IEnumerable<string> GetApiRepresentations([NotNull] this IEnumerable<Enum> ie) {
        return ie.Select(e => e.GetApiRepresentation());
    }
    
    [Pure]
    internal static IEnumerable<string> GetFlagsApiRepresentations([NotNull] this Enum en) {
        return en.GetFlags().GetApiRepresentations();
    }
    
    [Pure]
    private static IEnumerable<E> GetFlags<E>(this E en) where E : Enum {
        ulong flag = 1;
            
        foreach (var value in Enum.GetValues(en.GetType()).Cast<E>()) {
            var bits = Convert.ToUInt64(value);
                
            while (flag < bits) {
                flag <<= 1;
            }

            if (flag == bits && en.HasFlag(value)) {
                yield return value;
            }
        }
    }
    
    [CanBeNull]
    [Pure]
    internal static T? ToApiRepresentedEnum<T>(this string str) where T: struct, Enum {
        foreach (T field in Enum.GetValues(typeof(T))) {
            var representation = field.GetApiRepresentation();
            if (str == representation) {
                return field;
            }
        }
        return null;
    }
    
    [Pure]
    // note: since c# does not allow us to express that T: Flags, this method casts to dynamic internally.
    // so be careful when calling this that T: Flags.
    internal static T ToApiRepresentedFlagsEnum<T>([NotNull] this IEnumerable<string> ie) where T: struct, Enum {
        Debug.Assert(Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) != null);
            
        return (T) ie.SelectNotNullValue(s => s.ToApiRepresentedEnum<T>())
                     .Cast<dynamic>()
                     .DefaultIfEmpty(0)
                     .Aggregate((a, b) => a | b);
    }

    [Pure]
    internal static T CoalesceFlags<T>([NotNull] this IEnumerable<T> ie) where T: struct, Enum {
        Debug.Assert(Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) != null);
        

        return (T) ie.Cast<dynamic>()
                     .DefaultIfEmpty(0)
                     .Aggregate((a, b) => a | b);
    }
    
    [NotNull]
    [Pure]
    private static IEnumerable<O> SelectNotNullValue<I, O>([CanBeNull] [ItemCanBeNull] this IEnumerable<I> ie, 
                                                           [NotNull] Func<I, O?> f) where O: struct {
        return ie?.DiscardNull().Select(f).DiscardNullValue() ?? Enumerable.Empty<O>();
    }
    
    [ItemNotNull]
    [Pure]
    private static IEnumerable<T> DiscardNull<T>([ItemCanBeNull] this IEnumerable<T> ie) {
        return ie.Where(e => e != null);
    }
    
    [Pure]
    private static IEnumerable<T> DiscardNullValue<T>(this IEnumerable<T?> ie) where T: struct {
        return ie.Where(e => e != null)
                 .Select(e => e.Value);
    }

    [Pure]
    internal static string ToPrettyString<T>([NotNull] [ItemNotNull] this IEnumerable<T> enumerable) {
        var strings = enumerable.Select(e => e.ToString());
        return "[\n" + string.Join(", ", strings).Indent(4) + "\n]";
    }
    
    [Pure]
    public static string ToIso8601Date(this DateTime dateTime) {
        var s = JsonConvert.SerializeObject(dateTime);
        return s.Substring(1, s.Length - 2);
    }
    
    [Pure]
    public static long ToUnixTime(this DateTime dateTime) {
        return ((DateTimeOffset) dateTime).ToUnixTimeSeconds();
    }
}

public class UnixTimeConverter : DateTimeConverterBase {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
            return;
        }
            
        writer.WriteRawValue(((DateTime) value).ToUnixTime().ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.Value == null) {
            return null;
        }

        var val = serializer.Deserialize<long>(reader);
        return DateTimeOffset.FromUnixTimeSeconds(val).DateTime;
    }
}

public class ClassApiEnumConverter<E> : JsonConverter where E: struct, Enum {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
            return;
        }

        string name = ((E) value).GetApiRepresentation();
        
        serializer.Serialize(writer, name);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        var name = serializer.Deserialize<string>(reader);
        return name.ToApiRepresentedEnum<E>();
    }

    public override bool CanConvert(Type objectType) {
        return objectType == typeof(E) || objectType == typeof(E?);
    }
}

public class ClassApiFlagsEnumConverter<F> : JsonConverter where F: struct, Enum {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
            return;
        }
        
        var flags = (F) value;
        
        writer.WriteStartArray();
        foreach (string flag in flags.GetFlagsApiRepresentations()) {
            serializer.Serialize(writer, flag);
        }
        writer.WriteEndArray();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        var names = serializer.Deserialize<List<string>>(reader);
        return names.ToApiRepresentedFlagsEnum<F>();
    }

    public override bool CanConvert(Type objectType) {
        return objectType == typeof(F) || objectType == typeof(F?);
    }
}
