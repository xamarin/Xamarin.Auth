using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using LoginAccounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NativeSampleAndroid
{
    public class ProvidersAdapter : RecyclerView.Adapter
    {
        private readonly List<ProviderItem> providers = new List<ProviderItem>();

        public ProvidersAdapter()
        {
            foreach (var group in OAuthProvider.GetGroupedProviders())
            {
                providers.Add(new ProviderItem(group.Key));
                providers.AddRange(group.Value.Select(p => new ProviderItem(p)));
            }
        }

        public event Action<ProviderItem> ItemClicked;

        public override int ItemCount => providers.Count;

        public override int GetItemViewType(int position)
        {
            return providers[position].Provider == null
                ? Resource.Layout.provider_header
                : Resource.Layout.provider_item;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is ViewHolder vh)
                vh.Bind(providers[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.From(parent.Context);

            var itemView = inflater.Inflate(viewType, parent, false);
            var vh = new ViewHolder(itemView);

            itemView.Click += (sender, e) => ItemClicked?.Invoke(vh.ProviderItem);

            return vh;
        }

        public void NotifyItemChanged(ProviderItem item)
        {
            var index = providers.IndexOf(item);
            if (index != -1)
                NotifyItemChanged(index);
        }

        public class ProviderItem
        {
            public ProviderItem(string providerName)
            {
                ProviderName = providerName;
            }

            public ProviderItem(OAuthProvider provider)
            {
                ProviderName = provider.ProviderName;
                Provider = provider;
            }

            public string ProviderName { get; }
            public OAuthProvider Provider { get; }
            public bool IsAuthenticated { get; set; }
        }

        private class ViewHolder : RecyclerView.ViewHolder
        {
            private readonly TextView variant;
            private readonly TextView description;
            private readonly TextView check;
            private readonly TextView name;

            public ViewHolder(View itemView)
                : base(itemView)
            {
                // item
                variant = itemView.FindViewById<TextView>(Resource.Id.variant);
                description = itemView.FindViewById<TextView>(Resource.Id.description);
                check = itemView.FindViewById<TextView>(Resource.Id.check);

                // header
                name = itemView.FindViewById<TextView>(Resource.Id.name);
            }

            public ProviderItem ProviderItem { get; private set; }

            public void Bind(ProviderItem item)
            {
                ProviderItem = item;

                if (name != null)
                {
                    name.Text = item.ProviderName;
                }
                else
                {
                    variant.Text = item.Provider.ProviderVariant;
                    description.Text = item.Provider.Description;
                    check.Visibility = item.IsAuthenticated ? ViewStates.Visible : ViewStates.Gone;
                }
            }
        }
    }
}
