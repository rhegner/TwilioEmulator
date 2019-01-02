using System.Collections.Generic;

namespace TwilioLogic.Models
{
    public class Page<T>
    {

        public Page(long currentPage, long totalItems, long itemsPerPage, List<T> items)
        {
            CurrentPage = currentPage;
            TotalPages = ((totalItems % itemsPerPage) == 0) ? (totalItems / itemsPerPage) : ((totalItems / itemsPerPage) + 1);
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            Items = items;
        }

        public long CurrentPage { get; }
        public long TotalPages { get; }
        public long TotalItems { get; }
        public long ItemsPerPage { get; }
        public List<T> Items { get; }
    }
}
