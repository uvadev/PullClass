using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using PullClass.Id;
using PullClass.Structures;
using static PullClass.Util.Extensions;

namespace PullClass;

[PublicAPI]
public class ClassApi {
    private readonly HttpClient client;

    public ClassApi(string baseUrl, string clientKey, string clientSecret) {
        client = new HttpClient();
        client.BaseAddress = new Uri(new Uri(baseUrl), "api/v1/");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientSecret);
    }

    public async IAsyncEnumerable<ScheduledClass> StreamScheduledClasses() {
        var args = new[] {
            ("deferred", false.ToStr())
        };

        var response = await client.GetAsync("classes" + BuildDuplicateKeyQueryString(args));

        await foreach (var item in StreamDeserializeList<ScheduledClass>(response)) {
            yield return item;
        }
    }
    
    public async IAsyncEnumerable<DeferredClass> StreamDeferredClasses() {
        var args = new[] {
            ("deferred", true.ToStr())
        };

        var response = await client.GetAsync("classes" + BuildDuplicateKeyQueryString(args));

        await foreach (var item in StreamDeserializeList<DeferredClass>(response)) {
            yield return item;
        }
    }

    public async Task<ScheduledClass> GetScheduledClass<I>(I classHandle) where I: EntityId<string>, IConcreteClassHandle {
        var args = new[] {
            ("deferred", false.ToStr()),
            (classHandle.GetKeyName(), (string) classHandle)
        };
        
        var response = await client.GetAsync("classes" + BuildDuplicateKeyQueryString(args));

        var redundantList = await DeserializeObject<List<ScheduledClass>>(response);
        return redundantList[0];
    }
    
    public async Task<DeferredClass> GetDeferredClass<I>(I classHandle) where I: EntityId<string>, IConcreteClassHandle {
        var args = new[] {
            ("deferred", true.ToStr()),
            (classHandle.GetKeyName(), (string) classHandle)
        };
        
        var response = await client.GetAsync("classes" + BuildDuplicateKeyQueryString(args));

        var redundantList = await DeserializeObject<List<DeferredClass>>(response);
        return redundantList[0];
    }

    public async IAsyncEnumerable<Enrollment> StreamEnrollments<I>(I classHandle) where I: EntityId<string>, IConcreteClassHandle {
        var args = new[] {
            (classHandle.GetKeyName(), (string) classHandle)
        };
        
        var response = await client.GetAsync("class/enrollments" + BuildDuplicateKeyQueryString(args));
        
        await foreach (var item in StreamDeserializeList<Enrollment>(response)) {
            yield return item;
        }
    }

    public async Task<LaunchLink> GetLaunchLink<C, P>(C classHandle, P personHandle) where C: EntityId<string>, IConcreteClassHandle
                                                                                     where P: EntityId<string>, ILaunchablePersonHandle {
        var args = new[] {
            (classHandle.GetKeyName(), (string) classHandle),
            (personHandle.GetKeyName(), (string) personHandle)
        };

        var response = await client.GetAsync("class/enrollments/launch" + BuildDuplicateKeyQueryString(args));
        return await DeserializeObject<LaunchLink>(response);
    }

    public async IAsyncEnumerable<User> StreamUsers(uint chunkSize = 1000) {
        var requestFunc = async (PaginationParams p) => {
            var args = p.AsArgs().ToArray();
            
            return await client.GetAsync("users" + BuildDuplicateKeyQueryString(args));
        };

        await foreach (var user in StreamDeserializeListPaginated<User>(requestFunc, chunkSize)) {
            yield return user;
        }
    }

    public async Task<User> GetUser<U>(U userHandle) where U: EntityId<string>, IUserHandle {
        var args = new[] {
            (userHandle.GetKeyName(), userHandle.Id)
        };

        var response = await client.GetAsync("users" + BuildDuplicateKeyQueryString(args));
        var redundantList = await DeserializeObject<List<User>>(response);
        return redundantList[0];
    }
    
    public async Task<User> GetUser(UserId userHandle) {
        var args = new[] {
            (userHandle.GetKeyName(), userHandle.Id.ToString())
        };

        var response = await client.GetAsync("users" + BuildDuplicateKeyQueryString(args));
        var redundantList = await DeserializeObject<List<User>>(response);
        return redundantList[0];
    }

    public async Task<AttendanceReport> GetAttendanceReport<C>(C classHandle) where C: EntityId<string>, IConcreteClassHandle {
        var args = new[] {
            (classHandle.GetKeyName(), classHandle.Id)
        };

        var response = await client.GetAsync("reporting/attendance" + BuildDuplicateKeyQueryString(args));
        return await DeserializeObject<AttendanceReport>(response);
    }

    public async IAsyncEnumerable<MetricsItem> StreamMetrics<C>(C classHandle, 
                                                                DateTime? startingAfter = null, 
                                                                DateTime? endingBefore = null,
                                                                uint? limit = null) where C: EntityId<string>, IConcreteClassHandle {
        var args = new[] {
            (classHandle.GetKeyName(), classHandle),
            ("starting_after", startingAfter?.ToUnixTime().ToString()),
            ("ending_before", endingBefore?.ToUnixTime().ToString()),
            ("limit", limit?.ToString())
        };

        var response = await client.GetAsync("reporting/metrics" + BuildDuplicateKeyQueryString(args));
        await foreach (var metric in StreamDeserializeList<MetricsItem>(response)) {
            yield return metric;
        }
    }

    public async IAsyncEnumerable<ClassDate> StreamClassDates(ClassId classId) {
        var args = new[] {
            (classId.GetKeyName(), (string) classId)
        };
        
        var response = await client.GetAsync("schedules" + BuildDuplicateKeyQueryString(args));
        await foreach (var date in StreamDeserializeList<ClassDate>(response)) {
            yield return date;
        }
    }

    public async IAsyncEnumerable<Template> StreamTemplates() {
        var response = await client.GetAsync("templates");
        await foreach (var template in StreamDeserializeList<Template>(response)) {
            yield return template;
        }
    }

    public async Task<Template> GetTemplate(TemplateId templateHandle) {
        var args = new[] {
            (templateHandle.GetKeyName(), (string) templateHandle)
        };
        
        var response = await client.GetAsync("templates" + BuildDuplicateKeyQueryString(args));
        var redundantList = await DeserializeObject<List<Template>>(response);
        return redundantList[0];
    }

    private async IAsyncEnumerable<TElement> StreamDeserializeListPaginated<TElement>(Func<PaginationParams, Task<HttpResponseMessage>> requestFunc,
                                                                                      uint limit = 1000) {
        ulong currentPage = 1;
        for (;;) {
            var responseContent = await (await requestFunc(new PaginationParams(currentPage, limit))).Content
                                                                                                     .ReadAsStringAsync();
            var responseContentJson = JToken.Parse(responseContent);
            CheckOkOrThrow(responseContentJson);

            if (responseContentJson is JObject obj && obj.TryGetValue("pagination", out var paginationJson)) {
                var body = obj["data"];
                var pagination = paginationJson.ToObject<Pagination>();

                if (body == null) {
                    throw new ClassApiException("JSON body has `pagination` property but is missing `data` property.");
                }
                
                var page = body.ToObject<List<TElement>>();
                
                foreach (var e in page) {
                    yield return e;
                }

                if (pagination.NextPage is { } nextPage && nextPage > currentPage) {
                    currentPage = nextPage;
                } else {
                    yield break;
                }
            } else {
                Console.WriteLine("WARNING: Using paginating StreamDeserializeListPaginated method on non-paginated response. Is this intended?");
                var page = responseContentJson.ToObject<List<TElement>>();
                foreach (var e in page) {
                    yield return e;
                }
                yield break;
            }
        }
    }
    
    private async IAsyncEnumerable<TElement> StreamDeserializeList<TElement>(HttpResponseMessage response) {
        var responseContent = await response.Content
                                            .ReadAsStringAsync();

        var responseContentJson = JToken.Parse(responseContent);
        CheckOkOrThrow(responseContentJson);

        var usingPagination = responseContentJson is JObject obj && obj.ContainsKey("pagination");

        List<TElement> page;
        
        if (usingPagination) {
            var body = responseContentJson["data"];
            if (body == null) {
                throw new ClassApiException("JSON body has `pagination` property but is missing `data` property.");
            }
            page = body.ToObject<List<TElement>>();
            Console.WriteLine("WARNING: Using non-paginating StreamDeserializeList method on paginated response. Only the first page will be returned.");
        } else {
            page = responseContentJson.ToObject<List<TElement>>();
        }
        
        foreach (var e in page) {
            yield return e;
        }
    }

    private async Task<TElement> DeserializeObject<TElement>(HttpResponseMessage response) {
        var responseContent = await response.Content
                                            .ReadAsStringAsync();

        var responseContentJson = JToken.Parse(responseContent);
        CheckOkOrThrow(responseContentJson);

        return responseContentJson.ToObject<TElement>();
    }

    private void CheckOkOrThrow(JToken jt) {
        if (!CheckOk(jt, out var errString, out var errData)) {
            throw new RemoteClassApiException(errString, errData);
        }
    }

    private bool CheckOk(JToken jt, out string errString, out JObject errData) {
        if (jt is JObject obj && obj.ContainsKey("error")) {
            var err = obj.ToObject<ErrorResult>();
            errString = err.Error;
            errData = err.Data;
            return false;
        }
        
        errString = null;
        errData = null;
        return true;
    }

    private readonly struct PaginationParams {
        public readonly ulong Page;
        public readonly uint Limit;
        
        public PaginationParams(ulong page, uint limit) {
            Page = page;
            Limit = limit;
        }

        public List<(string, string)> AsArgs() {
            return new List<(string, string)> {
                ("page", Page.ToString()),
                ("limit", Limit.ToString())
            };
        }
    }
}