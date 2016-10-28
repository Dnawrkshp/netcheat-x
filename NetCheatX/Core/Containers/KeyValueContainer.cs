using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCheatX.Core.Containers
{
    /// <summary>
    /// Represents a collection of keys and values.
    /// </summary>
    public class KeyValueContainer<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _innerDictionary = null;


        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// 
        /// For get, if the Key does not exist in <see cref="KeyValueContainer{TKey, TValue}"/>, the Value returned is the default value for the type of the value parameter.
        /// For set, if the Key does not exist in <see cref="KeyValueContainer{TKey, TValue}"/>, the element is added with the supplied Key and Value.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        public TValue this[TKey key]
        {
            get
            {
                if (!((IDictionary<TKey, TValue>)_innerDictionary).Keys.Contains(key))
                    return default(TValue);
                return ((IDictionary<TKey, TValue>)_innerDictionary)[key];
            }

            set
            {
                if (!((IDictionary<TKey, TValue>)_innerDictionary).Keys.Contains(key))
                    ((IDictionary<TKey, TValue>)_innerDictionary).Add(key, value);
                else
                    ((IDictionary<TKey, TValue>)_innerDictionary)[key] = value;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_innerDictionary).Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_innerDictionary).IsReadOnly;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueContainer{TKey,TValue}"/> with default settings.
        /// </summary>
        public KeyValueContainer()
        {
            _innerDictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing keys of the <see cref="KeyValueContainer{TKey, TValue}"/>.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_innerDictionary).Keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{T}"/> containing the values of the <see cref="KeyValueContainer{TKey, TValue}"/>.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return ((IDictionary<TKey, TValue>)_innerDictionary).Values;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to use as the value of the element to add.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_innerDictionary).Add(item);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="KeyValueContainer{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            ((IDictionary<TKey, TValue>)_innerDictionary).Add(key, value);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        public void Clear()
        {
            ((IDictionary<TKey, TValue>)_innerDictionary).Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The value to locate in the sequence.</param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="KeyValueContainer{TKey, TValue}"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="KeyValueContainer{TKey, TValue}"/>.</param>
        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="ICollection{T}"/>. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_innerDictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator the iterates through the collection.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).GetEnumerator();
        }

        /// <summary>
        /// Removes the first occurrence of a specified object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).Remove(item);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="KeyValueContainer{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The object to remove from the <see cref="ICollection{T}"/>.</param>
        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)_innerDictionary).GetEnumerator();
        }
    }
}
