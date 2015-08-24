using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsClient
{
    public partial class Form2 : Form
    {
        private ContactData data;

        public ContactData ReturnData()
        {
            return data;
        }

        public Form2()
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
            data = new ContactData();
            button1.Text = "Add Contact";
            button1.Click += button1Click_close;
        }

        public Form2(ContactData _data)
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
            data = _data;
            textBox1.Text = data.name;
            textBox1.ReadOnly = true;
            textBox2.Text = data.number.ToString();
            button1.Text = "Change phone number";
            button1.Click += button1Click_close;
        }

        public Form2(int state)
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
            data = new ContactData();
            if (state==1)
                //request for number input
            {
                textBox1.ReadOnly = true;
                button1.Text = "Search name";
                button1.Click += button1Click_close_data1;
            }
            if (state==2)
                //request for name input
            {
                textBox2.ReadOnly = true;
                button1.Text = "Search phone";
                button1.Click += button1Click_close_data2;
            }

        }

        private void button1Click_close(object sender, EventArgs e)
        {
            try
            {
                data.name = textBox1.Text;
                data.number = Convert.ToInt32(textBox2.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Close();
        }

        private void button1Click_close_data1(object sender, EventArgs e)
        {
            try
            {
                data.number = Convert.ToInt32(textBox2.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Close();
        }

        private void button1Click_close_data2(object sender, EventArgs e)
        {
            try
            {
                data.name = textBox1.Text;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason == CloseReason.UserClosing)
                //e.Cancel = true;
        }
    }
}
