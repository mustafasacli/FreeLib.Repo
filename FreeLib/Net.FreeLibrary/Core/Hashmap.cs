using System;
using System.Collections;
using System.Collections.Generic;

namespace Net.FreeLibrary.Core
{
    public sealed class Hashmap
    {
        private Hashtable h = null;
        private List<string> _keys = null;

        public Hashmap()
        {
            h = new Hashtable();
            _keys = new List<string>();
        }

        /// <summary>
        /// Returns true if Hasmap is empty, else  returns false.
        /// </summary>
        /// <returns>returns a bool object</returns>
        public bool IsEmpty()
        {
            bool rt = false;
            rt = _keys.Count == 0;
            return rt;
        }

        /// <summary>
        /// Returns keys count of Hashmap.
        /// </summary>
        /// <returns>Returns an int object.</returns>
        public int Count()
        {
            int _count = 0;
            _count = _keys.Count;
            return _count;
        }



        /// <summary>
        /// Gets keys as a string array.
        /// </summary>
        /// <returns>Returns a string array.</returns>
        public string[] Keys()
        {
            string[] arr = _keys.ToArray();
            return arr;
        }

        public object Get(string key)
        {
            object val = null;

            if (_keys.IndexOf(key) > -1)
            {
                val = h[key];
            }

            return val;
        }

        public void Set(string key, object value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("Key can not be null, empty or white space.");
                }

                if (_keys.IndexOf(key) == -1)
                {
                    h.Add(key, value);
                    _keys.Add(key);
                    return;
                }

                h[key] = value;
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(string key)
        {
            try
            {
                h.Remove(key);
                _keys.Remove(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Contains(string key)
        {
            bool rt = false;
            rt = _keys.Contains(key);
            return rt;
        }
    }
}
