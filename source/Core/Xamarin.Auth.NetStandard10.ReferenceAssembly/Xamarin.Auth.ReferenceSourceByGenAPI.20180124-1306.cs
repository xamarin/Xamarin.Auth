namespace Core
{
    public partial class Decimal
    {
        public Decimal() { }
        public static decimal Round(decimal d, int p) { return default(decimal); }
    }
    public partial class Type
    {
        public Type() { }
        public static System.TypeCode GetTypeCode(System.Type type) { return default(System.TypeCode); }
    }
}
namespace Plugin.Threading
{
    public partial interface IRunInvoker
    {
        void BeginInvokeOnUIThread(System.Action action);
    }
    public partial class UIThreadRunInvoker : Plugin.Threading.IRunInvoker
    {
        public UIThreadRunInvoker() { }
        public void BeginInvokeOnUIThread(System.Action action) { }
    }
}
namespace System
{
    public static partial class DecimalExtensions
    {
        public static decimal Round(this decimal d) { return default(decimal); }
        public static decimal Round(this decimal d, int p) { return default(decimal); }
    }
    public enum TypeCode
    {
        Boolean = 3,
        Byte = 6,
        Char = 4,
        DateTime = 16,
        DBNull = 2,
        Decimal = 15,
        Double = 14,
        Empty = 0,
        Int16 = 7,
        Int32 = 9,
        Int64 = 11,
        Object = 1,
        SByte = 5,
        Single = 13,
        String = 18,
        UInt16 = 8,
        UInt32 = 10,
        UInt64 = 12,
    }
}
namespace System.Json
{
    public partial class JsonArray : System.Json.JsonValue, System.Collections.Generic.ICollection<System.Json.JsonValue>, System.Collections.Generic.IEnumerable<System.Json.JsonValue>, System.Collections.Generic.IList<System.Json.JsonValue>, System.Collections.IEnumerable
    {
        public JsonArray(System.Collections.Generic.IEnumerable<System.Json.JsonValue> items) { }
        public JsonArray(params System.Json.JsonValue[] items) { }
        public override int Count { get { return default(int); } }
        public bool IsReadOnly { get { return default(bool); } }
        public sealed override System.Json.JsonValue this[int index] { get { return default(System.Json.JsonValue); } set { } }
        public override System.Json.JsonType JsonType { get { return default(System.Json.JsonType); } }
        public void Add(System.Json.JsonValue item) { }
        public void AddRange(System.Collections.Generic.IEnumerable<System.Json.JsonValue> items) { }
        public void AddRange(params System.Json.JsonValue[] items) { }
        public void Clear() { }
        public bool Contains(System.Json.JsonValue item) { return default(bool); }
        public void CopyTo(System.Json.JsonValue[] array, int arrayIndex) { }
        public int IndexOf(System.Json.JsonValue item) { return default(int); }
        public void Insert(int index, System.Json.JsonValue item) { }
        public bool Remove(System.Json.JsonValue item) { return default(bool); }
        public void RemoveAt(int index) { }
        public override void Save(System.IO.Stream stream) { }
        System.Collections.Generic.IEnumerator<System.Json.JsonValue> System.Collections.Generic.IEnumerable<System.Json.JsonValue>.GetEnumerator() { return default(System.Collections.Generic.IEnumerator<System.Json.JsonValue>); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
    }
    public partial class JsonObject : System.Json.JsonValue, System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>>, System.Collections.Generic.IDictionary<string, System.Json.JsonValue>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>>, System.Collections.IEnumerable
    {
        public JsonObject(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>> items) { }
        public JsonObject(params System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>[] items) { }
        public override int Count { get { return default(int); } }
        public sealed override System.Json.JsonValue this[string key] { get { return default(System.Json.JsonValue); } set { } }
        public override System.Json.JsonType JsonType { get { return default(System.Json.JsonType); } }
        public System.Collections.Generic.ICollection<string> Keys { get { return default(System.Collections.Generic.ICollection<string>); } }
        bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Json.JsonValue>>.IsReadOnly { get { return default(bool); } }
        public System.Collections.Generic.ICollection<System.Json.JsonValue> Values { get { return default(System.Collections.Generic.ICollection<System.Json.JsonValue>); } }
        public void Add(System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue> pair) { }
        public void Add(string key, System.Json.JsonValue value) { }
        public void AddRange(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>> items) { }
        public void AddRange(params System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>[] items) { }
        public void Clear() { }
        public override bool ContainsKey(string key) { return default(bool); }
        public void CopyTo(System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>[] array, int arrayIndex) { }
        public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>> GetEnumerator() { return default(System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue>>); }
        public bool Remove(string key) { return default(bool); }
        public override void Save(System.IO.Stream stream) { }
        bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Json.JsonValue>>.Contains(System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue> item) { return default(bool); }
        bool System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.String,System.Json.JsonValue>>.Remove(System.Collections.Generic.KeyValuePair<string, System.Json.JsonValue> item) { return default(bool); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
        public bool TryGetValue(string key, out System.Json.JsonValue value) { value = default(System.Json.JsonValue); return default(bool); }
    }
    public partial class JsonPrimitive : System.Json.JsonValue
    {
        public JsonPrimitive(bool value) { }
        public JsonPrimitive(byte value) { }
        public JsonPrimitive(char value) { }
        public JsonPrimitive(System.DateTime value) { }
        public JsonPrimitive(System.DateTimeOffset value) { }
        public JsonPrimitive(decimal value) { }
        public JsonPrimitive(double value) { }
        public JsonPrimitive(System.Guid value) { }
        public JsonPrimitive(short value) { }
        public JsonPrimitive(int value) { }
        public JsonPrimitive(long value) { }
        public JsonPrimitive(sbyte value) { }
        public JsonPrimitive(float value) { }
        public JsonPrimitive(string value) { }
        public JsonPrimitive(System.TimeSpan value) { }
        public JsonPrimitive(ushort value) { }
        public JsonPrimitive(uint value) { }
        public JsonPrimitive(ulong value) { }
        public JsonPrimitive(System.Uri value) { }
        public override System.Json.JsonType JsonType { get { return default(System.Json.JsonType); } }
        public override void Save(System.IO.Stream stream) { }
    }
    public enum JsonType
    {
        Array = 3,
        Boolean = 4,
        Number = 1,
        Object = 2,
        String = 0,
    }
    public abstract partial class JsonValue : System.Collections.IEnumerable
    {
        protected JsonValue() { }
        public virtual int Count { get { return default(int); } }
        public virtual System.Json.JsonValue this[int index] { get { return default(System.Json.JsonValue); } set { } }
        public virtual System.Json.JsonValue this[string key] { get { return default(System.Json.JsonValue); } set { } }
        public abstract System.Json.JsonType JsonType { get; }
        public virtual bool ContainsKey(string key) { return default(bool); }
        public static System.Json.JsonValue Load(System.IO.Stream stream) { return default(System.Json.JsonValue); }
        public static System.Json.JsonValue Load(System.IO.TextReader textReader) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (bool value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (byte value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (char value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (System.DateTime value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (System.DateTimeOffset value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (decimal value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (double value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (System.Guid value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (short value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (int value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (long value) { return default(System.Json.JsonValue); }
        public static implicit operator bool (System.Json.JsonValue value) { return default(bool); }
        public static implicit operator byte (System.Json.JsonValue value) { return default(byte); }
        public static implicit operator char (System.Json.JsonValue value) { return default(char); }
        public static implicit operator System.DateTime (System.Json.JsonValue value) { return default(System.DateTime); }
        public static implicit operator System.DateTimeOffset (System.Json.JsonValue value) { return default(System.DateTimeOffset); }
        public static implicit operator decimal (System.Json.JsonValue value) { return default(decimal); }
        public static implicit operator double (System.Json.JsonValue value) { return default(double); }
        public static implicit operator System.Guid (System.Json.JsonValue value) { return default(System.Guid); }
        public static implicit operator short (System.Json.JsonValue value) { return default(short); }
        public static implicit operator int (System.Json.JsonValue value) { return default(int); }
        public static implicit operator long (System.Json.JsonValue value) { return default(long); }
        public static implicit operator sbyte (System.Json.JsonValue value) { return default(sbyte); }
        public static implicit operator float (System.Json.JsonValue value) { return default(float); }
        public static implicit operator string (System.Json.JsonValue value) { return default(string); }
        public static implicit operator System.TimeSpan (System.Json.JsonValue value) { return default(System.TimeSpan); }
        public static implicit operator ushort (System.Json.JsonValue value) { return default(ushort); }
        public static implicit operator uint (System.Json.JsonValue value) { return default(uint); }
        public static implicit operator ulong (System.Json.JsonValue value) { return default(ulong); }
        public static implicit operator System.Uri (System.Json.JsonValue value) { return default(System.Uri); }
        public static implicit operator System.Json.JsonValue (sbyte value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (float value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (string value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (System.TimeSpan value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (ushort value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (uint value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (ulong value) { return default(System.Json.JsonValue); }
        public static implicit operator System.Json.JsonValue (System.Uri value) { return default(System.Json.JsonValue); }
        public static System.Json.JsonValue Parse(string jsonString) { return default(System.Json.JsonValue); }
        public virtual void Save(System.IO.Stream stream) { }
        public virtual void Save(System.IO.TextWriter textWriter) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return default(System.Collections.IEnumerator); }
        public override string ToString() { return default(string); }
    }
}
namespace System.Reflection
{
    public static partial class Extensions
    {
        public static readonly System.Collections.Generic.Dictionary<System.Type, System.TypeCode> TypeCodes;
        public static System.TypeCode GetTypeCode(this System.Type type) { return default(System.TypeCode); }
    }
}
namespace Xamarin.Auth
{
    public partial class Account
    {
        public Account() { }
        public Account(string username) { }
        public Account(string username, System.Collections.Generic.IDictionary<string, string> properties) { }
        public Account(string username, System.Collections.Generic.IDictionary<string, string> properties, System.Net.CookieContainer cookies) { }
        public Account(string username, System.Net.CookieContainer cookies) { }
        public virtual System.Net.CookieContainer Cookies { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get 
            { 
                //Unable to resolve assembly 'Assembly(Name=System.Net.Primitives, Version=3.9.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
                return default(System.Net.CookieContainer); 
            } 
        }
        public virtual System.Collections.Generic.Dictionary<string, string> Properties 
        { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]
            get 
            { 
                //Unable to resolve assembly 'Assembly(Name=System.Collections, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
                return default(System.Collections.Generic.Dictionary<string, string>); 
            } 
        }
        public virtual string Username { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static Xamarin.Auth.Account Deserialize(string serializedString) { return default(Xamarin.Auth.Account); }
        public string Serialize() { return default(string); }
        public override string ToString() { return default(string); }
    }
    public partial class AccountResult
    {
        public AccountResult() { }
        public string AccountType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Name { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Token { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public abstract partial class AccountStore
    {
        protected AccountStore() { }
        public static Xamarin.Auth.AccountStore Create() { return default(Xamarin.Auth.AccountStore); }
        public static Xamarin.Auth.AccountStore Create(string password) { return default(Xamarin.Auth.AccountStore); }
        public abstract void Delete(Xamarin.Auth.Account account, string serviceId);
        public abstract System.Threading.Tasks.Task DeleteAsync(Xamarin.Auth.Account account, string serviceId);
        public abstract System.Collections.Generic.IEnumerable<Xamarin.Auth.Account> FindAccountsForService(string serviceId);
        public abstract System.Threading.Tasks.Task<System.Collections.Generic.List<Xamarin.Auth.Account>> FindAccountsForServiceAsync(string serviceId);
        public abstract void Save(Xamarin.Auth.Account account, string serviceId);
        public abstract System.Threading.Tasks.Task SaveAsync(Xamarin.Auth.Account account, string serviceId);
    }
    public partial class AccountStoreException : Xamarin.Auth.AuthException
    {
        public AccountStoreException(string operation) { }
        public AccountStoreException(string operation, System.Exception exc) { }
        public string Operation { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public partial class AuthenticationUI
    {
        public AuthenticationUI() { }
        public static Xamarin.Auth.AuthenticationUIType AuthenticationUIType 
        { 
            [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get 
            { return default(Xamarin.Auth.AuthenticationUIType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public enum AuthenticationUIType
    {
        EmbeddedBrowser = 0,
        Native = 1,
    }
    public abstract partial class Authenticator
    {
        protected int classlevel_depth;
        public Authenticator() { }
        public bool AllowCancel { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Func<Xamarin.Auth.Account, Xamarin.Auth.AccountResult> GetAccountResult { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<Xamarin.Auth.Account, Xamarin.Auth.AccountResult>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool HasCompleted { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } }
        protected bool IgnoreErrorsWhenCompleted { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Func<bool> IsAuthenticated { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<bool>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool ShowErrors { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Title { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public event System.EventHandler<Xamarin.Auth.AuthenticatorCompletedEventArgs> Completed { add { } remove { } }
        public event System.EventHandler<Xamarin.Auth.AuthenticatorErrorEventArgs> Error { add { } remove { } }
        public static void ClearCookies() { }
        [System.Diagnostics.DebuggerStepThroughAttribute]
        //[System.Runtime.CompilerServices.AsyncStateMachineAttribute(typeof(Xamarin.Auth.Authenticator.<ClearCookiesAsync>d__43))]
        public static System.Threading.Tasks.Task ClearCookiesAsync() 
        { 
            //Unable to resolve assembly 'Assembly(Name=System.Threading.Tasks, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
            return default(System.Threading.Tasks.Task); 
        }
        protected virtual object GetPlatformUI() { return default(object); }
        public void OnCancelled() { }
        public void OnError(System.Exception exception) { }
        public void OnError(string message) { }
        public void OnSucceeded(string username, System.Collections.Generic.IDictionary<string, string> accountProperties) { }
        public void OnSucceeded(Xamarin.Auth.Account account) { }
        public override string ToString() { return default(string); }
    }
    public partial class AuthenticatorCompletedEventArgs : System.EventArgs
    {
        public AuthenticatorCompletedEventArgs(Xamarin.Auth.Account account) { }
        public Xamarin.Auth.Account Account { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Xamarin.Auth.Account); } }
        public bool IsAuthenticated { get { return default(bool); } }
    }
    public partial class AuthenticatorErrorEventArgs : System.EventArgs
    {
        public AuthenticatorErrorEventArgs(System.Exception exception) { }
        public AuthenticatorErrorEventArgs(string message) { }
        public System.Exception Exception { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Exception); } }
        public string Message { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } }
    }
    public partial class AuthException : System.Exception
    {
        public AuthException() { }
        public AuthException(string message) { }
        public AuthException(string message, System.Exception inner) { }
    }
    public static partial class FileHelper
    {
        public static string GetLocalStoragePath() { return default(string); }
    }
    public abstract partial class FormAuthenticator : Xamarin.Auth.Authenticator
    {
        public FormAuthenticator(System.Uri createAccountLink=null) { }
        public System.Uri CreateAccountLink { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Uri); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.IList<Xamarin.Auth.FormAuthenticatorField> Fields { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.IList<Xamarin.Auth.FormAuthenticatorField>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string GetFieldValue(string key) { return default(string); }
        public abstract System.Threading.Tasks.Task<Xamarin.Auth.Account> SignInAsync(System.Threading.CancellationToken cancellationToken);
    }
    public partial class FormAuthenticatorField
    {
        public FormAuthenticatorField() { }
        public FormAuthenticatorField(string key, string title, Xamarin.Auth.FormAuthenticatorFieldType fieldType, string placeholder="", string defaultValue="") { }
        public Xamarin.Auth.FormAuthenticatorFieldType FieldType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Xamarin.Auth.FormAuthenticatorFieldType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Key { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Placeholder { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Title { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Value { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
    }
    public enum FormAuthenticatorFieldType
    {
        Email = 1,
        Password = 2,
        PlainText = 0,
    }
    public delegate System.Threading.Tasks.Task<string> GetUsernameAsyncFunc(System.Collections.Generic.IDictionary<string, string> accountProperties);
    public enum HttpWebClientFrameworkType
    {
        HttpClient = 1,
        WebRequest = 0,
    }
    public partial class LibraryUtilities
    {
        public static readonly string MessageNotImplementedException;
        public LibraryUtilities() { }
    }
    public static partial class OAuth1
    {
        public static System.Net.HttpWebRequest CreateRequest(string method, System.Uri uri, System.Collections.Generic.IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string tokenSecret) 
        {
            //Unable to resolve assembly 'Assembly(Name=System.Net.Requests, Version=3.9.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
            return default(System.Net.HttpWebRequest); 
        }
        public static string EncodeString(string unencoded) { return default(string); }
        public static string GetAuthorizationHeader(string method, System.Uri uri, System.Collections.Generic.IDictionary<string, string> parameters, string consumerKey, string consumerSecret, string token, string tokenSecret) { return default(string); }
        public static string GetBaseString(string method, System.Uri uri, System.Collections.Generic.IDictionary<string, string> parameters) { return default(string); }
        public static string GetSignature(string method, System.Uri uri, System.Collections.Generic.IDictionary<string, string> parameters, string consumerSecret, string tokenSecret) { return default(string); }
    }
    public partial class OAuth1Authenticator : Xamarin.Auth.WebRedirectAuthenticator
    {
        public OAuth1Authenticator(string consumerKey, string consumerSecret, System.Uri requestTokenUrl, System.Uri authorizeUrl, System.Uri accessTokenUrl, System.Uri callbackUrl, Xamarin.Auth.GetUsernameAsyncFunc getUsernameAsync=null, bool isUsingNativeUI=false) : base (default(System.Uri), default(System.Uri)) { }
        public Xamarin.Auth.HttpWebClientFrameworkType HttpWebClientFrameworkType { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Xamarin.Auth.HttpWebClientFrameworkType); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override System.Threading.Tasks.Task<System.Uri> GetInitialUrlAsync(System.Collections.Generic.Dictionary<string, string> query_parameters=null) { return default(System.Threading.Tasks.Task<System.Uri>); }
        public override void OnPageLoaded(System.Uri url) { }
        public override string ToString() { return default(string); }
    }
    public partial class OAuth1Request : Xamarin.Auth.Request
    {
        public OAuth1Request(string method, System.Uri url, System.Collections.Generic.IDictionary<string, string> parameters, Xamarin.Auth.Account account, bool includeMultipartsInSignature=false) : base (default(string), default(System.Uri), default(System.Collections.Generic.IDictionary<string, string>), default(Xamarin.Auth.Account)) { }
        protected virtual string GetAuthorizationHeader() { return default(string); }
        public override System.Threading.Tasks.Task<Xamarin.Auth.Response> GetResponseAsync(System.Threading.CancellationToken cancellationToken) { return default(System.Threading.Tasks.Task<Xamarin.Auth.Response>); }
    }
    public partial class OAuth2Authenticator : Xamarin.Auth.WebRedirectAuthenticator
    {
        public OAuth2Authenticator(string clientId, string clientSecret, string scope, System.Uri authorizeUrl, System.Uri redirectUrl, System.Uri accessTokenUrl, Xamarin.Auth.GetUsernameAsyncFunc getUsernameAsync=null, bool isUsingNativeUI=false) : base (default(System.Uri), default(System.Uri)) { }
        public OAuth2Authenticator(string clientId, string scope, System.Uri authorizeUrl, System.Uri redirectUrl, Xamarin.Auth.GetUsernameAsyncFunc getUsernameAsync=null, bool isUsingNativeUI=false) : base (default(System.Uri), default(System.Uri)) { }
        public string AccessTokenName { get { return default(string); } set { } }
        public System.Uri AccessTokenUrl { get { return default(System.Uri); } set { } }
        public System.Uri AuthorizeUrl { get { return default(System.Uri); } }
        public string ClientId { get { return default(string); } }
        public string ClientSecret { get { return default(string); } }
        public bool DoNotEscapeScope { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        protected bool IsAuthorizationCodeFlow { get { return default(bool); } }
        protected bool IsImplicitFlow { get { return default(bool); } }
        protected bool IsProofKeyCodeForExchange { get { return default(bool); } }
        public string Scope { get { return default(string); } }
        public Xamarin.Auth.OAuth2.State State { get { return default(Xamarin.Auth.OAuth2.State); } set { } }
        public System.Collections.Generic.Dictionary<string, string> CreateRequestQueryParameters(System.Collections.Generic.Dictionary<string, string> custom_query_parameters=null) { return default(System.Collections.Generic.Dictionary<string, string>); }
        public override System.Threading.Tasks.Task<System.Uri> GetInitialUrlAsync(System.Collections.Generic.Dictionary<string, string> custom_query_parameters=null) { return default(System.Threading.Tasks.Task<System.Uri>); }
        protected System.Collections.Generic.List<string> OAuthFlowResponseTypeVerification() { return default(System.Collections.Generic.List<string>); }
        protected virtual void OnCreatingInitialUrl(System.Collections.Generic.IDictionary<string, string> query) { }
        protected override void OnPageEncountered(System.Uri url, System.Collections.Generic.IDictionary<string, string> query, System.Collections.Generic.IDictionary<string, string> fragment) { }
        protected override void OnRedirectPageLoaded(System.Uri url, System.Collections.Generic.IDictionary<string, string> query, System.Collections.Generic.IDictionary<string, string> fragment) { }
        public virtual void OnRetrievedAccountProperties(System.Collections.Generic.IDictionary<string, string> accountProperties) { }
        [System.Diagnostics.DebuggerStepThroughAttribute]
        //[System.Runtime.CompilerServices.AsyncStateMachineAttribute(typeof(Xamarin.Auth.OAuth2Authenticator.<RequestAccessTokenAsync>d__45))]
        public System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string, string>> RequestAccessTokenAsync(System.Collections.Generic.IDictionary<string, string> queryValues) { return default(System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string, string>>); }
        public System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string, string>> RequestAccessTokenAsync(string code) { return default(System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string, string>>); }
        public override string ToString() { return default(string); }
        protected void Verify() { }
        public System.Collections.Generic.List<string> VerifyOAuth2FlowResponseType(System.Uri accessTokenUrl, string clientSecret, string[] curtomResponseTypes) { return default(System.Collections.Generic.List<string>); }
    }
    public partial class OAuth2Request : Xamarin.Auth.Request
    {
        public OAuth2Request(string method, System.Uri url, System.Collections.Generic.IDictionary<string, string> parameters, Xamarin.Auth.Account account) : base (default(string), default(System.Uri), default(System.Collections.Generic.IDictionary<string, string>), default(Xamarin.Auth.Account)) { }
        public string AccessTokenParameterName { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public static System.Uri GetAuthenticatedUrl(Xamarin.Auth.Account account, System.Uri unauthenticatedUrl, string accessTokenParameterName="access_token") { return default(System.Uri); }
        public static string GetAuthorizationHeader(Xamarin.Auth.Account account) { return default(string); }
        protected override System.Uri GetPreparedUrl() { return default(System.Uri); }
    }
    public partial class Request
    {
        protected readonly System.Collections.Generic.List<Xamarin.Auth.Request.Part> Multiparts;
        public Request(string method, System.Uri url, System.Collections.Generic.IDictionary<string, string> parameters=null, Xamarin.Auth.Account account=null) { }
        public virtual Xamarin.Auth.Account Account { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(Xamarin.Auth.Account); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string Method { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public System.Collections.Generic.IDictionary<string, string> Parameters { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.IDictionary<string, string>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public System.Uri Url { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Uri); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public virtual void AddMultipartData(string name, System.IO.Stream data, string mimeType="", string filename="") { }
        public void AddMultipartData(string name, string data) { }
        protected virtual System.Uri GetPreparedUrl() { return default(System.Uri); }

        //protected virtual System.Net.Http.HttpRequestMessage GetPreparedWebRequest() 
        //{ 
        //    //Unable to resolve assembly 'Assembly(Name=System.Net.Http, Version=1.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
        //    return default(System.Net.Http.HttpRequestMessage); 
        //}
        public virtual System.Threading.Tasks.Task<Xamarin.Auth.Response> GetResponseAsync() { return default(System.Threading.Tasks.Task<Xamarin.Auth.Response>); }
        [System.Diagnostics.DebuggerStepThroughAttribute]
        //[System.Runtime.CompilerServices.AsyncStateMachineAttribute(typeof(Xamarin.Auth.Request.<GetResponseAsync>d__23))]
        public virtual System.Threading.Tasks.Task<Xamarin.Auth.Response> GetResponseAsync(System.Threading.CancellationToken cancellationToken) { return default(System.Threading.Tasks.Task<Xamarin.Auth.Response>); }
        protected partial class Part
        {
            public System.IO.Stream Data;
            public string Filename;
            public string MimeType;
            public string Name;
            public string TextData;
            public Part() { }
        }
    }
    public partial class Response : System.IDisposable
    {
        protected Response() { }
        //public Response(System.Net.Http.HttpResponseMessage response) { }
        public virtual System.Collections.Generic.IDictionary<string, string> Headers { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.IDictionary<string, string>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public virtual System.Uri ResponseUri { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Uri); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public virtual System.Net.HttpStatusCode StatusCode { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Net.HttpStatusCode); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]protected set { } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        ~Response() { }
        public virtual System.IO.Stream GetResponseStream() 
        { 
            // Unable to resolve assembly 'Assembly(Name=System.IO, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
            return default(System.IO.Stream); 
        }
        public virtual System.Threading.Tasks.Task<System.IO.Stream> GetResponseStreamAsync() { return default(System.Threading.Tasks.Task<System.IO.Stream>); }
        public virtual string GetResponseText() { return default(string); }
        public virtual System.Threading.Tasks.Task<string> GetResponseTextAsync() { return default(System.Threading.Tasks.Task<string>); }
        public override string ToString() { return default(string); }
    }
    public abstract partial class WebAuthenticator : Xamarin.Auth.Authenticator
    {
        protected bool is_using_native_ui;
        protected System.Collections.Generic.Dictionary<string, string> request_parameters;
        protected WebAuthenticator() { }
        public bool ClearCookiesBeforeLogin { get { return default(bool); } set { } }
        public string Host { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool IsUsingNativeUI { get { return default(bool); } }
        public System.Func<object> PlatformUIMethod { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<object>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.Dictionary<string, string> RequestParameters { get { return default(System.Collections.Generic.Dictionary<string, string>); } set { } }
        public string Scheme { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(string); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public event System.EventHandler BrowsingCompleted { add { } remove { } }
        public object AuthenticationUIPlatformSpecificEmbeddedBrowser() { return default(object); }
        public object AuthenticationUIPlatformSpecificNative() { return default(object); }
        public static new void ClearCookies() { }
        public abstract System.Threading.Tasks.Task<System.Uri> GetInitialUrlAsync(System.Collections.Generic.Dictionary<string, string> custom_query_parameters=null);
        protected override object GetPlatformUI() { return default(object); }
        protected object GetPlatformUIEmbeddedBrowser() { return default(object); }
        protected virtual object GetPlatformUINative() { return default(object); }
        public bool IsUriEncodedDataString(string s) { return default(bool); }
        protected virtual void OnBrowsingCompleted() { }
        public abstract void OnPageLoaded(System.Uri url);
        public virtual void OnPageLoading(System.Uri url) { }
        protected void ShowErrorForNativeUI(string v) { }
        protected void ShowErrorForNativeUIAlert(string v) { }
        protected void ShowErrorForNativeUIDebug(string v) { }
        public override string ToString() { return default(string); }
    }
    public static partial class WebEx
    {
        public static System.Collections.Generic.IDictionary<string, string> FormDecode(string encodedString) { return default(System.Collections.Generic.IDictionary<string, string>); }
        public static string GetCookie(this System.Net.CookieContainer containers, System.Uri domain, string name) { return default(string); }
        public static System.Text.Encoding GetEncodingFromContentType(string contentType) 
        { 
            //Unable to resolve assembly 'Assembly(Name=System.Text.Encoding, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a)' referenced by 'Assembly(Name=Xamarin.Auth, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null, Location=./source/Core/Xamarin.Auth.Portable/bin/Debug/Xamarin.Auth.dll)'.
return default(System.Text.Encoding); }
        public static System.Threading.Tasks.Task<System.Net.WebResponse> GetResponseAsync(this System.Net.WebRequest request) { return default(System.Threading.Tasks.Task<System.Net.WebResponse>); }
        public static string GetResponseText(this System.Net.WebResponse response) { return default(string); }
        public static string GetValueFromJson(string json, string key) { return default(string); }
        public static string HtmlEncode(string text) { return default(string); }
        public static System.Collections.Generic.Dictionary<string, string> JsonDecode(string encodedString) { return default(System.Collections.Generic.Dictionary<string, string>); }
    }
    public partial class WebRedirectAuthenticator : Xamarin.Auth.WebAuthenticator
    {
        public WebRedirectAuthenticator(System.Uri initialUrl, System.Uri redirectUrl) { }
        public System.Collections.Generic.IDictionary<string, string> Fragment { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.IDictionary<string, string>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool IsLoadableRedirectUri { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public System.Collections.Generic.IDictionary<string, string> Query { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Collections.Generic.IDictionary<string, string>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool ShouldEncounterOnPageLoaded { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public bool ShouldEncounterOnPageLoading { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(bool); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public override System.Threading.Tasks.Task<System.Uri> GetInitialUrlAsync(System.Collections.Generic.Dictionary<string, string> custom_query_parameters=null) { return default(System.Threading.Tasks.Task<System.Uri>); }
        protected virtual void OnPageEncountered(System.Uri url, System.Collections.Generic.IDictionary<string, string> query, System.Collections.Generic.IDictionary<string, string> fragment) { }
        public override void OnPageLoaded(System.Uri url) { }
        public override void OnPageLoading(System.Uri url) { }
        protected virtual void OnRedirectPageLoaded(System.Uri url, System.Collections.Generic.IDictionary<string, string> query, System.Collections.Generic.IDictionary<string, string> fragment) { }
        public override string ToString() { return default(string); }
    }
    public static partial class WebUtilities
    {
        public static string EncodeString(string unencoded) { return default(string); }
        public static string FormEncode(this System.Collections.Generic.IDictionary<string, string> inputs) { return default(string); }
    }
}
namespace Xamarin.Auth.OAuth2
{
    public partial class GrantType
    {
        public GrantType(Xamarin.Auth.OAuth2Authenticator a) { }
    }
    public partial class ResponseType
    {
        public ResponseType(Xamarin.Auth.OAuth2Authenticator a) { }
    }
    public partial class State
    {
        public State() { }
        public System.Func<ulong, string> RandomStateGeneratorFunc { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(System.Func<ulong, string>); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string RandomString { get { return default(string); } set { } }
        public string RandomStringUriEscaped { get { return default(string); } }
        public ulong StateStringLength { [System.Runtime.CompilerServices.CompilerGeneratedAttribute]get { return default(ulong); } [System.Runtime.CompilerServices.CompilerGeneratedAttribute]set { } }
        public string GenerateOAuth2StateRandom(ulong number_of_characters=(ulong)16) { return default(string); }
    }
}
namespace Xamarin.Auth.Presenters
{
    public partial class OAuthLoginPresenter
    {
        public static System.Action<Xamarin.Auth.Authenticator> PlatformLogin;
        public OAuthLoginPresenter() { }
        public event System.EventHandler<Xamarin.Auth.AuthenticatorCompletedEventArgs> Completed { add { } remove { } }
        public void Login(Xamarin.Auth.Authenticator authenticator) { }
    }
}
namespace Xamarin.Utilities
{
    public static partial class ExceptionEx
    {
        public static string GetUserMessage(this System.Exception error) { return default(string); }
    }
}
