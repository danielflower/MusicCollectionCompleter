using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCollectionCompleter.Desktop.Services
{
    public class AlbumList : IList<Album>, INotifyCollectionChanged
    {
        private IList<Album> _listImplementation = new List<Album>();


        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnChanged(NotifyCollectionChangedEventArgs e)
        {
            MainWindow.OnGuiThread(() => CollectionChanged?.Invoke(this, e));
        }

        public IEnumerator<Album> GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _listImplementation).GetEnumerator();
        }

        public void Add(Album item)
        {
            _listImplementation.Add(item);
            OnChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            _listImplementation.Clear();
        }

        public bool Contains(Album item)
        {
            return _listImplementation.Contains(item);
        }

        public void CopyTo(Album[] array, int arrayIndex)
        {
            _listImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(Album item)
        {
            int index = _listImplementation.IndexOf(item);
            var removed = index >= 0;
            if (removed)
            {
                _listImplementation.RemoveAt(index);
                OnChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            }
            return removed;
        }

        public int Count
        {
            get { return _listImplementation.Count; }
        }

        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly; }
        }

        public int IndexOf(Album item)
        {
            return _listImplementation.IndexOf(item);
        }

        public void Insert(int index, Album item)
        {
            _listImplementation.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        public Album this[int index]
        {
            get { return _listImplementation[index]; }
            set { _listImplementation[index] = value; }
        }
    }
}
