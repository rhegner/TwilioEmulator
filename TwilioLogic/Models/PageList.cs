using System.Collections;
using System.Collections.Generic;

namespace TwilioLogic.Models
{
    public class PageList<T> : IReadOnlyList<T>
    {

        public PageList(List<T> innerList, bool hasMore)
        {
            InnerList = innerList;
            HasMore = hasMore;
        }

        private IList<T> InnerList { get; }

        public bool HasMore { get; }


        public T this[int index] => InnerList[index];

        public int Count => InnerList.Count;

        public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

    }
}
