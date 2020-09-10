using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace Net.FreeLibrary.Entity
{
    public abstract class BaseBO : IBaseBO, INotifyPropertyChanged
    {
        #region [ Private Fileds ]

        private List<String> _changeList = null;

        #endregion


        #region [ BaseBO Ctor ]

        /// <summary>
        /// BaseBO Ctor.
        /// </summary>
        protected BaseBO()
        {
            _changeList = new List<String>();
            this.PropertyChanged += BaseBO_PropertyChanged;
        }

        #endregion


        #region [ GetTableName method ]

        /// <summary>
        /// Gets Table Name Of BaseBO object.
        /// </summary>
        /// <returns>Returns Table Name Of BaseBO object.</returns>
        public virtual string GetTableName()
        {
            throw new NotImplementedException("Table Name is not implemented. You must be create GetTableName method.");
        }

        #endregion


        #region [ GetIdColumn method ]

        /// <summary>
        ///  Gets Identity Column Name Of BaseBO object.
        /// </summary>
        /// <returns>Returns Identity Column Name Of BaseBO object.</returns>
        public virtual string GetIdColumn()
        {
            throw new NotImplementedException("Id Column is not implemented. You must be create GetIdColumn method.");
        }

        #endregion


        #region [ GetColumnChangeList method ]

        /// <summary>
        /// Gets Column Name list with property changed.
        /// </summary>
        /// <returns>Returns Column Name list with property changed.</returns>
        public List<String> GetColumnChangeList()
        {
            return _changeList;
        }

        #endregion


        #region [ Equals method ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">object instance which inherits BaseBO object.</param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            bool result = false;

            if (object.ReferenceEquals(obj.GetType(), this.GetType()))
            {
                BaseBO bo = (BaseBO)obj;
                if (bo.GetTableName().Equals(this.GetTableName()) & bo.GetIdColumn().Equals(this.GetIdColumn()))
                {
                    PropertyInfo propInf = bo.GetType().GetProperty(bo.GetIdColumn());
                    object obj1 = propInf.GetValue(bo);
                    object obj2 = propInf.GetValue(this);
                    result = object.Equals(obj1, obj2);
                }
            }

            return result;
        }

        #endregion


        #region [ GetHashCode method ]

        /// <summary>
        /// Gets HashCode of object.
        /// </summary>
        /// <returns>Returns HashCode int.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion


        #region [ Serialize method ]

        /// <summary>
        /// Write Object serialization result to a file.
        /// </summary>
        /// <param name="fileName"> File Name for Serialization object writing. </param>
        /// <param name="append">if append is true Serialization continues with append.</param>
        public void Serialize(String fileName, Boolean append)
        {
            try
            {
                //StringBuilder strBuilder = new StringBuilder();
                PropertyInfo[] pInfos = this.GetType().GetProperties();
                string str = string.Empty;
                //strBuilder.AppendLine(String.Format("Name : {0} ", this.GetType().Name));
                str = String.Format("Name : {0} ", this.GetType().Name);
                Object objVal;

                foreach (PropertyInfo inf in pInfos)
                {
                    if (inf.Name.Equals(this.GetIdColumn()) == false)
                    {
                        objVal = inf.GetValue(this, null);
                        str = string.Concat(str, " : ", inf.Name, objVal, Environment.NewLine);
                        //strBuilder.AppendLine(String.Format("{0} : {1} ", inf.Name, objVal));
                    }
                }

                FileMode fMode = FileMode.OpenOrCreate;
                if (append)
                {
                    //strBuilder.AppendLine("/* ------------------------------------------------*/");
                    fMode = FileMode.Append;
                    str = string.Concat(str, "/* ------------------------------------------------*/", Environment.NewLine);
                }

                using (StreamWriter outfile = new StreamWriter(new FileStream(fileName, fMode)))
                {
                    //outfile.Write(strBuilder.ToString());
                    outfile.Write(str);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Write Object serialization result to a file.
        /// </summary>
        /// <param name="fileName"> File Name for Serialization object writing. </param>
        public void Serialize(String fileName)
        {
            try
            {
                Serialize(fileName, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ BaseBO_PropertyChanged method ]

        private void BaseBO_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool check = _changeList.Contains(e.PropertyName);
            check = !check;
            if (check)
            {
                _changeList.Add(e.PropertyName);
            }
        }

        #endregion


        #region [ PropertyChanged EventHandler ]

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        #region [ OnPropertyChanged method ]

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        #region [ ChangeSetCount Property ]

        /// <summary>
        /// Gets Count of Changed Property.
        /// </summary>
        public int ChangeSetCount
        {
            get
            {
                int count = _changeList.Count;
                return count;
            }
        }

        #endregion


        #region [ IsPropertyChanged method ]

        /// <summary>
        /// Returns true if Value of Property with given name changes, return true; else returns false.
        /// </summary>
        /// <param name="propName">Property Name</param>
        /// <returns>Returns bool object.</returns>
        public bool IsPropertyChanged(string propName)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(propName))
                {
                    result = _changeList.Contains(propName);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        #endregion


        #region [ Clear method ]

        /// <summary>
        /// Clears all change columns list.
        /// </summary>
        public void Clear()
        {
            _changeList.Clear();
            _changeList = new List<string>();
        }

        #endregion


    }
}