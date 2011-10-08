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
    public partial class SelectServer : Form
    {
        public delegate void passServer(TextBox text);
        public SelectServer()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstServers.SelectedIndex == 0)
            {
                txtAddress.Text = "broadway.ny.us.dal.net";
                txtPort.Text = "7000";
            }

            else if (lstServers.SelectedIndex == 2)
            {
                txtAddress.Text = "irc.rizon.net";
                txtPort.Text = "6667";
            }
            else
            {
                txtAddress.Text = "";
                txtPort.Text = "";

            }
        }

        public String getSelectedServer()
        {
            return txtAddress.Text;
        }

        public int getSelectedPort()
        {
            return Convert.ToInt32(txtPort.Text);
        }

    }
}
