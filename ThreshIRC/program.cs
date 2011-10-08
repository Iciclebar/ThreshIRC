using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text;
using System.IO;

namespace ThreshIRC
{
   
 
        // A simple form that represents a window in our application
        public class AppForm2 : System.Windows.Forms.Form
        {
        
            public AppForm2()
            {
                this.Size = new System.Drawing.Size(300, 300);
                this.Text = "AppForm2";
            }

            private void InitializeComponent()
            {
                this.SuspendLayout();
                // 
                // AppForm2
                // 
                this.ClientSize = new System.Drawing.Size(284, 262);
                this.Name = "AppForm2";
                this.ResumeLayout(false);

            }
        }


        // The class that handles the creation of the application windows
        class MyApplicationContext : ApplicationContext
        {

            private int formCount;
            private SelectServer form1;
            private AppForm2 form2;

            private MyApplicationContext()
            {
                formCount = 0;


                // Create both application forms and handle the Closed event
                // to know when both forms are closed.
                form1 = new SelectServer();
                form1.Closed += new EventHandler(OnFormClosed);
                formCount++;

                form2 = new AppForm2();
                form2.Closed += new EventHandler(OnFormClosed);
                formCount++;

                // Show both forms.
                form1.Show();
                form2.Show();
            }

            private void OnFormClosed(object sender, EventArgs e)
            {
                // When a form is closed, decrement the count of open forms.

                // When the count gets to 0, exit the app by calling
                // ExitThread().
                formCount--;
                if (formCount == 0)
                {
                    ExitThread();
                }
            }

            [STAThread]
            static void Main(string[] args)
            {

                // Create the MyApplicationContext, that derives from ApplicationContext,
                // that manages when the application should exit.

                MyApplicationContext context = new MyApplicationContext();

                // Run the application with the specific context. It will exit when
                // all forms are closed.
                Application.Run(context);

            }
        }
    }

