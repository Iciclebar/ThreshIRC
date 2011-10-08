using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ThreshIRC
{
    public partial class Server : Form
    {
        public String server;
        public int port;
        private Conn conn;
        public Server(String server, String port)
        {
            this.server = server;
            this.port = Convert.ToInt32(port);
            InitializeComponent();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Server_Load(object sender, EventArgs e)
        {
            conn = new Conn(this);
            conn.start();
        }
    }


}
