using System.Collections.Generic;

namespace TwilioLogic.Models
{
    public class Page<T>
    {

        public Page(int currentPage, int totalItems, int itemsPerPage, List<T> items)
        {
            CurrentPage = currentPage;
            TotalPages = ((totalItems % itemsPerPage) == 0) ? (totalItems / itemsPerPage) : ((totalItems / itemsPerPage) + 1);
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            Items = items;
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }
        public int ItemsPerPage { get; }
        public List<T> Items { get; }
    }
}
