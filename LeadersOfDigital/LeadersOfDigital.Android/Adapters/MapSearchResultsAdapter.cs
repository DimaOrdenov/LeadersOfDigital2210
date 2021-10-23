using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using LeadersOfDigital.ViewModels.Map;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace LeadersOfDigital.Android.Adapters
{
    public class MapSearchResultsAdapter : MvxRecyclerAdapter
    {
        public MapSearchResultsAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var bindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            return new MapSearchResultsViewHolder(
                InflateViewForHolder(parent, viewType, bindingContext),
                bindingContext);
        }

        private class MapSearchResultsViewHolder : MvxRecyclerViewHolder
        {
            private readonly ImageView _icon;
            private readonly TextView _title;
            private readonly TextView _address;
            private readonly TextView _distance;

            public MapSearchResultsViewHolder(View itemView, IMvxAndroidBindingContext context)
                : base(itemView, context)
            {
                _icon = itemView.FindViewById<ImageView>(Resource.Id.map_search_result_item_icon);
                _title = itemView.FindViewById<TextView>(Resource.Id.map_search_result_item_title);
                _address = itemView.FindViewById<TextView>(Resource.Id.map_search_result_item_address);
                _distance = itemView.FindViewById<TextView>(Resource.Id.map_search_result_item_distance);

                this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<MapSearchResultsViewHolder, MapSearchResultItemViewModel>();

                    set.Bind(_title)
                        .For(x => x.Text)
                        .To(vm => vm.Place.Name);

                    set.Bind(_address)
                        .For(x => x.Text)
                        .To(vm => vm.Place.FormattedAddress);

                    set.Bind(_distance)
                        .For(x => x.Text)
                        .To(vm => vm.DistanceStr);

                    set.Apply();
                });
            }
        }
    }
}
