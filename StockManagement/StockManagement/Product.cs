using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockManagement
{
    public partial class Product : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=Stock;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record
        int ProductCode = 0;
        public Product()
        {
            InitializeComponent();
            LoadData();
        }
        private void Products_Load(object sender, EventArgs e)
        {
            P_Status.SelectedIndex = 0;
            LoadData();
        }
        //Insert Data
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (P_Name.Text != "" && P_Status.Text != "")
            {
                con.Open();
                bool status = false;
                if (P_Status.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                cmd = new SqlCommand("insert into Products(ProductName,ProductStatus) VALUES('" + P_Name.Text + "','" + status + "')", con);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Inserted Successfully");
                P_Name.Text = "";
                LoadData();
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
        }
        //Display Data in DataGridView
        public void LoadData()
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock].[dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Inactive";
                }
            }
            con.Close();
        }
        //Clear Data 
        private void ClearData()
        {
            P_Name.Text = "";
            P_Status.Text = "";
            ProductCode = 0;
        }
        //dataGridView1 RowHeaderMouseClick Event
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ProductCode = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            P_Name.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            P_Status.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
        //Update Record
        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (P_Name.Text != "" && P_Status.Text != "")
            {
                cmd = new SqlCommand("update Products set ProductName=@name,ProductStatus=@status where ProductCode=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", ProductCode);
                cmd.Parameters.AddWithValue("@name", P_Name.Text);
                bool status = false;
                if (P_Status.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                cmd.Parameters.AddWithValue("@status", status);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated Successfully");
                con.Close();
                P_Name.Text = "";
                P_Status.Text = "";
                LoadData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }
        //Delete Record
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (ProductCode != 0)
            {
                cmd = new SqlCommand("delete Products where ProductCode=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", ProductCode);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Successfully!");
                LoadData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
            }
        }
    }
}
