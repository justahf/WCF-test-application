using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.ServiceModel;


namespace FormsClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //refresh list
            TestServiceClient client = new TestServiceClient();
            string[][] rows;
            try
            {
                client.Open();
                rows = client.GetAllContacts();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            dataGridView1.Rows.Clear();
            for (int i=0;i<rows.Length;i++)
            {
                dataGridView1.Rows.Add(rows[i][0], rows[i][1]);
            }

            foreach (Control c in Controls)
                c.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //add contact
            Form2 form2 = new Form2();
            form2.ShowDialog();
            ContactData data = form2.ReturnData();
            TestServiceClient client = new TestServiceClient();
            try
            {
                client.Open();
                client.PushContact(data.name, data.number);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //remove contact
            int index = dataGridView1.CurrentRow.Index;
            TestServiceClient client = new TestServiceClient();
            try
            {
                int number = Convert.ToInt32(dataGridView1.Rows[index].Cells[1].Value.ToString());
                client.Open();
                client.DeleteContact(number);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //modify contact
            int index = dataGridView1.CurrentRow.Index;
            try
            {
                string name = dataGridView1.Rows[index].Cells[0].Value.ToString();
                int number = Convert.ToInt32(dataGridView1.Rows[index].Cells[1].Value.ToString());
                Form2 form2 = new Form2(new ContactData(name, number));
                form2.ShowDialog();
                ContactData data = new ContactData();
                data = form2.ReturnData();
                TestServiceClient client = new TestServiceClient();
                client.Open();
                client.ModifyContact(number, data.number);
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //search name
            dataGridView1.Rows.Clear();
            Form2 form2 = new Form2(1);
            form2.ShowDialog();
            ContactData data = form2.ReturnData();
            TestServiceClient client = new TestServiceClient();
            try
            {
                client.Open();
                string name = client.SearchName(data.number);
                client.Close();
                if (name != null)
                    dataGridView1.Rows.Add(name, data.number.ToString());
                else
                {
                    foreach (Control c in Controls)
                        c.Enabled = false;
                    button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Form2 form2 = new Form2(2);
            form2.ShowDialog();
            ContactData data = form2.ReturnData();
            TestServiceClient client = new TestServiceClient();
            try
            {
                client.Open();
                int[] phones = client.SearchPhone(data.name);
                client.Close();

                int count = phones.Count();
                if (count != 0)
                    for (int i = 0; i < count; i++)
                        dataGridView1.Rows.Add(data.name, phones[i].ToString());
                else
                {
                    foreach (Control c in Controls)
                        c.Enabled = false;
                    button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




    }
}
