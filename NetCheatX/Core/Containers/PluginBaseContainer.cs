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
    /// Represents a collection of IPluginBases.
    /// </summary>
    /// <typeparam name="T">A class or interface that inherits the <see cref="T:NetCheatX.Core.Interfaces.IPluginBase"/> interface.</typeparam>
    public class PluginBaseContainer<T> : IEnumerable<T> where T : Interfaces.IPluginBase
    {
        private List<T> _innerList = null;
        private List<Interfaces.IPluginBase> _parentPlugins = null;


        /// <summary>
        /// Occurs when a plugin is added.
        /// </summary>
        public event EventHandler<Types.PluginBaseChangedEventArgs> PluginAdded;

        /// <summary>
        /// Occurs when a plugin is removed.
        /// </summary>
        public event EventHandler<Types.PluginBaseChangedEventArgs> PluginRemoved;


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
        /// <param name="index">The index of the element in the <see cref="PluginBaseContainer{T}"/> to get or set.</param>
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
                    _innerList[index] = value;
                    if (PluginAdded != null)
                        PluginAdded.Invoke(this, new Core.Types.PluginBaseChangedEventArgs() { ParentPlugin = _parentPlugins[index], Plugin = value });
                }
            }
        }

        /// <summary>
        /// Initializes new instance of <see cref="PluginBaseContainer{T}"/>.
        /// </summary>
        public PluginBaseContainer()
        {
            _innerList = new List<T>();
            _parentPlugins = new List<Interfaces.IPluginBase>();
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="parent">The parent plugin of the object to add to the <see cref="ICollection{T}"/>.</param>
        /// <param name="item">The IPluginBase to add to the <see cref="ICollection{T}"/>.</param>
        public void Add(Interfaces.IPluginBase parent, T item)
        {
            // Ensure the item doesn't already exist
            string cmp = item.ToBase64String();
            foreach (T cmpItem in _innerList)
            {
                if (cmp == cmpItem.ToBase64String())
                    return;
            }

            _innerList.Add(item);
            _parentPlugins.Add(parent);

            if (PluginAdded != null)
                PluginAdded.Invoke(this, new Core.Types.PluginBaseChangedEventArgs() { ParentPlugin = parent, Plugin = item });
        }

        /// <summary>
        /// Removes all elements from the <see cref="PluginBaseContainer{T}"/>.
        /// </summary>
        public void Clear()
        {
            while (_innerList.Count > 0)
            {
                if (PluginRemoved != null)
                    PluginRemoved.Invoke(this, new Core.Types.PluginBaseChangedEventArgs() { ParentPlugin = _parentPlugins[0], Plugin = _innerList[0] });

                _innerList.RemoveAt(0);
                _parentPlugins.RemoveAt(0);
            }
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="PluginBaseContainer{T}"/>.
        /// </summary>
        /// <param name="item">The value to locate in the sequence.</param>
        public bool Contains(T item)
        {
            return _innerList.Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="PluginBaseContainer{T}"/> to a compatible one-dimenstional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="PluginBaseContainer{T}"/>. The Array must have zero-based indexing.</param>
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
            if (_innerList.Contains(item) && PluginRemoved != null)
                PluginRemoved.Invoke(this, new Core.Types.PluginBaseChangedEventArgs() { ParentPlugin = _parentPlugins[_innerList.IndexOf(item)], Plugin = item });

            _parentPlugins.RemoveAt(_innerList.IndexOf(item));
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
