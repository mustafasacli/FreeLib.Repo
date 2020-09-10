using Net.FreeLibrary.Core;
using System;

namespace Net.FreeLibrary.Conf
{
    public sealed class DbProperty
    {
        private string _connType = string.Empty;
        private string _connString = string.Empty;
        private string _invariantName = string.Empty;
        private Hashmap _keys = null;

        public DbProperty()
        {
            _keys = new Hashmap();
        }

        public void Add(string key, object value)
        {
            try
            {
                _keys.Set(key, value);
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
                _keys.Remove(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Hashmap Keys
        {
            get { return _keys; }
            set { _keys = value; }
        }

        public string ConnString
        {
            get { return _connString; }
            set { _connString = value; }
        }

        public string ConnType
        {
            get { return _connType; }
            set { _connType = value; }
        }

        public string InvariantName
        {
            get { return _invariantName; }
            set { _invariantName = value; }
        }
    }
}