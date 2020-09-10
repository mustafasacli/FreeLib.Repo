using Net.FreeLibrary.Util;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Net.FreeLibrary.Conf
{
    class FreeConfiguration
    {
        private static readonly String XML_FILE_PATH = @"C:/FreeLib/config/conf.xml";
        private static readonly String PROP_NODE = "appsettings/dblogins/dblogin";
        private static readonly String SAVE_NODE = "appsettings/appconfig/log-setting";
        //private static Hashtable _properties;
        private static String _logType;
        private static String _saveType;
        private static String _savePath;
        private static List<string> paramHashList = new List<string>()
        {
           "uid",
           "userid",
           "logonid",
           "user",
           "username",
           "pwd",
           "password"
        };

        static FreeConfiguration()
        {
            //  _properties = new Hashtable();
            PrepareConf();
        }
        /*
        public static String Get(String key)
        {
            if (_properties.ContainsKey(key) == true)
            {
                Object obj = _properties[key];
                return obj != null ? obj.ToString() : String.Empty;
            }
            else
                return String.Empty;
        }
        */
        public static DbProperty BuildProperty(String connectionName)
        {
            try
            {
                // Buraya ekleme kısımları gelecek.
                DbProperty propUtil = new DbProperty();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XML_FILE_PATH);
                XmlNodeList propNodeList = xmlDoc.SelectNodes(PROP_NODE);
                foreach (XmlNode node in propNodeList)
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.Attributes["name"].InnerText.Equals(connectionName))
                        {
                            foreach (XmlAttribute attr in node.Attributes)
                            {
                                /*
                                if (attr.Name.Equals("name") == false)
                                    propUtil.Add(attr.Name, attr.Value);
                                */
                                if (attr.Name.Equals("conn-str"))
                                {
                                    propUtil.ConnString = attr.Value;
                                    continue;
                                }

                                if (attr.Name.Equals("type"))
                                {
                                    propUtil.ConnType = attr.Value;
                                    continue;
                                }

                                if (attr.Name.Equals("invariant"))
                                {
                                    propUtil.ConnType = attr.Value;
                                    continue;
                                }
                            }

                            XmlNodeList xnlNdLst = node.SelectNodes("property");
                            string strPrm;
                            foreach (XmlNode xml_node in xnlNdLst)
                            {
                                try
                                {
                                    strPrm = xml_node.Attributes["key"].InnerText;
                                    strPrm = strPrm.ToLower().Replace('ı', 'i');
                                    strPrm = strPrm.Replace(" ", "");
                                    if (paramHashList.Contains(strPrm))
                                    {
                                        propUtil.Add(
                                            xml_node.Attributes["key"].InnerText,
                                            SecurityUtil.DecodeString(xml_node.Attributes["value"].InnerText)
                                            );
                                    }
                                    else
                                    {
                                        propUtil.Add(
                                            xml_node.Attributes["key"].InnerText,
                                            xml_node.Attributes["value"].InnerText
                                            );

                                    }
                                }
                                catch
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }

                return propUtil;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static String SaveType
        {
            get { return _saveType; }
        }

        public static String LogType
        {
            get
            {
                return _logType;
            }
        }

        public static String SaveFilePath
        {
            get
            {
                return _savePath;
            }
        }

        private static void PrepareConf()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XML_FILE_PATH);
                XmlNodeList saveNodeList = xmlDoc.SelectNodes(SAVE_NODE);

                foreach (XmlNode node in saveNodeList)
                {
                    _logType = node.SelectSingleNode("log-type").InnerText;
                    _saveType = node.SelectSingleNode("save-type").InnerText;
                    _savePath = node.SelectSingleNode("log-path").InnerText;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}