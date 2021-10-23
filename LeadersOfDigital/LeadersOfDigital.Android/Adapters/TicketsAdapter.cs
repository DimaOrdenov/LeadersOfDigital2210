using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Converters;
using LeadersOfDigital.ViewModels.Setup;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace LeadersOfDigital.Android.Adapters
{
    public class TicketsAdapter : MvxRecyclerAdapter
    {
        public TicketsAdapter(IMvxAndroidBindingContext bindingContext)
            : base(bindingContext)
        {
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var bindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            return new TicketsResultsViewHolder(
                InflateViewForHolder(parent, viewType, bindingContext),
                bindingContext);
        }

        private class TicketsResultsViewHolder : MvxRecyclerViewHolder
        {
            private readonly TextView _price;
            private readonly TextView _route;
            private readonly TextView _timeRange;
            private readonly TextView _duration;
            private readonly ConstraintLayout _buyButton;

            public TicketsResultsViewHolder(View itemView, IMvxAndroidBindingContext context)
                : base(itemView, context)
            {
                _price = itemView.FindViewById<TextView>(Resource.Id.choose_tickets_item_price);
                _route = itemView.FindViewById<TextView>(Resource.Id.choose_tickets_item_route);
                _timeRange = itemView.FindViewById<TextView>(Resource.Id.choose_tickets_item_time_range);
                _duration = itemView.FindViewById<TextView>(Resource.Id.choose_tickets_item_duration);
                _buyButton = itemView.FindViewById<ConstraintLayout>(Resource.Id.choose_tickets_item_buy);

                this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<TicketsResultsViewHolder, TicketItemViewModel>();

                    set.Bind(_price)
                        .For(x => x.Text)
                        .To(vm => vm.FlightsResponse.Price)
                        .WithConversion<StringFormatConverter>("{0} ₽");

                    set.Bind(_route)
                        .For(x => x.Text)
                        .To(vm => vm.Route);
                    
                    set.Bind(_timeRange)
                        .For(x => x.Text)
                        .To(vm => vm.TimeRange);
                    
                    set.Bind(_duration)
                        .For(x => x.Text)
                        .To(vm => vm.Duration);

                    set.Bind(_buyButton)
                        .For(x => x.BindClick())
                        .To(vm => vm.BuyCommand);

                    set.Apply();
                });
            }
        }
    }
}
