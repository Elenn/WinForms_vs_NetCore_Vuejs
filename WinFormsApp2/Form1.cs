using System.Collections.Generic;
using System.Data; 
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        } 

        private async void button1_Click(object sender, EventArgs e)
        {
            try 
            {  
                String query = "select * from Products"; 
                DataTable data = DbHelper.GetDataTable(query); 
                dataGridView1.DataSource = data;  
            } 
            catch (Exception es) 
            { 
                MessageBox.Show(es.Message); 
            }
        }
    }
}
