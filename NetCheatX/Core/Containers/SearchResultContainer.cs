using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCheatX.Core.Extensions;

namespace NetCheatX.Core.Containers
{
    /// <summary>
    /// Represents a collection of ISearchResults.
    /// </summary>
    /// <typeparam name="T">A class or interface that inherits the <see cref="T:NetCheatX.Core.Interfaces.ISearchResult"/> interface.</typeparam>
    public class SearchResultContainer<T> : IEnumerable<T>, IList<T> where T : Interfaces.ISearchResult
    {
        private List<T> _innerList = null;
        private Types.SearchResultUpdatedEventArgs[] _updates = null;


        /// <summary>
        /// Occurs when a search result is added, removed, or changed
        /// </summary>
        public event EventHandler<Types.SearchResultUpdatedEventArgs[]> SearchResultUpdated;

        /// <summary>
        /// Gets the number of elements in the <see cref="ICollection{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _innerList.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<T>)_innerList).IsReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user must invoke the RaiseEvents() method. By default events are raised on every change made to the <see cref="ICollection{T}"/>.
        /// </summary>
        public bool UserRaiseEvents { get; set; } = false;

        /// <summary>
        /// Gets or sets the value associated with the specified index.
        /// </summary>
        /// <param name="index">The index of the element in the <see cref="SearchResultContainer{T}"/> to get or set.</param>
        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < _innerList.Count)
                    return _innerList[index];
                else
                    return default(T);
            }

            set
            {
                if (index > 0 && index < _innerList.Count)
                {
                    Array.Resize(ref _updates, _updates.Length + 1);
                    _updates[_updates.Length - 1] = new Types.SearchResultUpdatedEventArgs()
                    {
                        item = _innerList[index],
                        newitem = value,
                        type = Types.SearchResultEventType.Changed
                    };
                    if (!UserRaiseEvents)
                        RaiseEvents();

                    _innerList[index] = value;
                }
            }
        }

        /// <summary>
        /// Initializes new instance of <see cref="SearchResultContainer{T}"/>.
        /// </summary>
        public SearchResultContainer()
        {
            _innerList = new List<T>();
            _updates = new Types.SearchResultUpdatedEventArgs[0];
        }

        /// <summary>
        /// Pushes all unraised events out.
        /// </summary>
        public void RaiseEvents()
        {
            if (SearchResultUpdated != null && _updates.Length > 0)
            {
                SearchResultUpdated.Invoke(this, _updates);
                _updates = new Types.SearchResultUpdatedEventArgs[0];
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="IList{T}"/>.</param>
        public int IndexOf(T item)
        {
            return ((IList<T>)_innerList).IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="IList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="IList{T}"/>.</param>
        public void Insert(int index, T item)
        {
            ((IList<T>)_innerList).Insert(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            ((IList<T>)_innerList).RemoveAt(index);
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The ISearchResult to add to the <see cref="ICollection{T}"/>.</param>
        public void Add(T item)
        {
            _innerList.Add(item);

            Array.Resize(ref _updates, _updates.Length + 1);
            _updates[_updates.Length - 1] = new Types.SearchResultUpdatedEventArgs()
            {
                item = item,
                type = Types.SearchResultEventType.Created
            };
            if (!UserRaiseEvents)
                RaiseEvents();
        }

        /// <summary>
        /// Removes all elements from the <see cref="SearchResultContainer{T}"/>.
        /// </summary>
        public void Clear()
        {
            int off = _updates.Length;
            Array.Resize(ref _updates, off + _innerList.Count);
            for (int x = 0; x < _innerList.Count; x++)
            {
                _updates[off + x] = new Types.SearchResultUpdatedEventArgs()
                {
                    item = _innerList[x],
                    type = Types.SearchResultEventType.Removed
                };
            }
            if (!UserRaiseEvents)
                RaiseEvents();

            _innerList.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="SearchResultContainer{T}"/>.
        /// </summary>
        /// <param name="item">The value to locate in the sequence.</param>
        public bool Contains(T item)
        {
            return _innerList.Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="SearchResultContainer{T}"/> to a compatible one-dimenstional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="SearchResultContainer{T}"/>. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specified object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        public bool Remove(T item)
        {
            if (_innerList.Contains(item))
            {
                Array.Resize(ref _updates, _updates.Length + 1);
                _updates[_updates.Length - 1] = new Types.SearchResultUpdatedEventArgs()
                {
                    item = item,
                    type = Types.SearchResultEventType.Removed
                };

                if (!UserRaiseEvents)
                    RaiseEvents();
            }

            return ((ICollection<T>)_innerList).Remove(item);
        }

        /// <summary>
        /// Returns an enumerator the iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)_innerList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<T>)_innerList).GetEnumerator();
        }
    }
}
