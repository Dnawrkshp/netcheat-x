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
    public class SearchResultContainer<T> : IEnumerable<T> where T : Interfaces.ISearchResult
    {
        private List<T> _innerList = null;

        /// <summary>
        /// Occurs when a search result is added.
        /// </summary>
        public event EventHandler<Types.SearchResultChangedEventArgs> ResultAdded;

        /// <summary>
        /// Occurs when a search result is removed.
        /// </summary>
        public event EventHandler<Types.SearchResultChangedEventArgs> ResultRemoved;


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
                    if (ResultAdded != null)
                        ResultRemoved.Invoke(this, new Core.Types.SearchResultChangedEventArgs() { OriginalResult = _innerList[index], NewResult = value });
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
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The ISearchResult to add to the <see cref="ICollection{T}"/>.</param>
        public void Add(T item)
        {
            _innerList.Add(item);
            
            if (ResultAdded != null)
                ResultAdded.Invoke(this, new Core.Types.SearchResultChangedEventArgs() { OriginalResult = null, NewResult = item });
        }

        /// <summary>
        /// Removes all elements from the <see cref="SearchResultContainer{T}"/>.
        /// </summary>
        public void Clear()
        {
            while (_innerList.Count > 0)
            {
                if (ResultRemoved != null)
                    ResultRemoved.Invoke(this, new Core.Types.SearchResultChangedEventArgs() { OriginalResult = _innerList[0], NewResult = null });

                _innerList.RemoveAt(0);
            }
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
            if (_innerList.Contains(item) && ResultRemoved != null)
                ResultRemoved.Invoke(this, new Core.Types.SearchResultChangedEventArgs() { OriginalResult = item, NewResult = null });

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
