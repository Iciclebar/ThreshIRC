using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sharkbite.Irc;

namespace willIRC2
{
    public partial class Form1 : Form
    {
    	private Conn conn;
    	
    	public Form1() {
    		InitializeComponent();
    	}
    	
        private void Form1_HandleCreated(object sender, EventArgs e)
        {
            conn = new Conn( this);
            conn.start();
        }


        public void AppendText(string Message)
        {
        	if (txtDisplay != null) {
            	txtDisplay.Text += Message + Environment.NewLine;
        	}
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
        	conn.stop();
        	Application.Exit();
        }
        
        private void Form1_OnFormClosed(object sender, FormClosedEventArgs e) {
        	conn.stop();
        	Application.Exit();
        }

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {

        }


    }


}
    

    
