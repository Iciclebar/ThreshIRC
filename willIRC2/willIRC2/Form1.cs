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
            this.txtText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
    	}
    	
        private void Form1_HandleCreated(object sender, EventArgs e)
        {
            
        }


        public void AppendText(string Message)
        {
        	if (txtDisplay != null) {
            	txtDisplay.Text += Message + Environment.NewLine;

        	}
        }
     

  
        
        private void Form1_OnFormClosed(object sender, FormClosedEventArgs e) {
        	//conn.stop();
        	Application.Exit();
        }

        private void txtDisplay_TextChanged_1(object sender, EventArgs e)
        {

            txtDisplay.SelectionStart = txtDisplay.Text.Length;
            txtDisplay.ScrollToCaret();
        }
        
  

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conn = new Conn(this);
            conn.start();
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.stop();
            }

            catch (Exception f)
            {
                txtDisplay.Text=txtDisplay.Text+ f;
            }
           
        }

       

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
           
            
            if (e.KeyChar == (char)Keys.Enter)
            {


                if (txtText.Text.Trim() == String.Empty)
                {
                    e.Handled = true;
                }
                else
                {
                    SendDelegate(txtText.Text);
                    string tempText = "<Will> " + txtText.Text;
                    AppendText(tempText);
                    e.Handled = true;
                    txtText.Clear();
                }
                }
            }
            
        

        private void selectServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        void SendDelegate(string sendChat)
        {
            try
            {
                this.BeginInvoke(new PublicSend(conn.sendText), new object[] { sendChat });
            }
            catch { txtDisplay.Text += "Please Connection to as Server"; }
        }

        void listUserDelegate(string channel, string[] nicks, bool last)
           {
                this.BeginInvoke(new NamesEventHandler(conn.listUsers), new object[] { channel, nicks, last });
            
           }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void filesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
            	

    }


}
    

    
