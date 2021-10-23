using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace LeadersOfDigital.Helpers
{
    public static class CollectionExtensions
    {
        public static MvxObservableCollection<TItem> ToMvxObservableCollection<TItem>(this IEnumerable<TItem> collection) =>
            new (collection);
    }
}
