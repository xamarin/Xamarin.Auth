using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Auth
{
    public abstract partial class FormAuthenticator : Authenticator
    {
        public IList<FormAuthenticatorField> Fields { get; set; }

        public Uri CreateAccountLink { get; set; }

        public FormAuthenticator(Uri createAccountLink = null)
        {
            Fields = new List<FormAuthenticatorField>();
            CreateAccountLink = createAccountLink;
        }

        public string GetFieldValue(string key)
        {
            return Fields.FirstOrDefault(x => x.Key == key)?.Value;
        }

        public abstract Task<Account> SignInAsync(CancellationToken cancellationToken = default);
    }

    public class FormAuthenticatorField
    {
        public FormAuthenticatorField()
        {
        }

        public FormAuthenticatorField(string key, string title, FormAuthenticatorFieldType fieldType, string placeholder = "", string defaultValue = "")
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("key must not be blank", nameof(key));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title must not be blank", nameof(title));

            Key = key;
            Title = title;
            Placeholder = placeholder ?? string.Empty;
            Value = defaultValue ?? string.Empty;
            FieldType = fieldType;
        }

        public string Key { get; set; }

        public string Title { get; set; }

        public string Placeholder { get; set; }

        public string Value { get; set; }

        public FormAuthenticatorFieldType FieldType { get; set; }
    }

    public enum FormAuthenticatorFieldType
    {
        PlainText,
        Email,
        Password,
    }
}
