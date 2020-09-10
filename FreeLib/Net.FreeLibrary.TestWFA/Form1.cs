using Net.FreeLibrary.Client;
using Net.FreeLibrary.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Net.FreeLibrary.TestWFA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void Connect()
        {
            using (IConnection conn = ConnectionFactory.Build(ConnectionTypes.SqlServer))
            {
                conn.ConnectionString = "";
            }
        }
    }
}
