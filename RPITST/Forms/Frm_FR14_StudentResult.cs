using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace RPITST.Forms
{
    public partial class Frm_FR14_StudentResult : Form
    {

        SQL_Control sql = new SQL_Control();
        public Frm_FR14_StudentResult()
        {
            InitializeComponent();
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {



            sql.AddParam("@levels", Cmb_Level.SelectedValue == null ? DBNull.Value : Cmb_Level.SelectedValue);
            sql.AddParam("@specialtys", Cmb_Specialty.SelectedValue == null ? DBNull.Value : Cmb_Specialty.SelectedValue);
            sql.AddParam("@semesters", Cmb_Semester.SelectedValue == null ? DBNull.Value : Cmb_Semester.SelectedValue);
            sql.AddParam("@years", Cmb_Level.SelectedValue == null ? DBNull.Value : Cmb_Year.SelectedValue);
            sql.AddParam("@academics", Cmb_Academic.Text);

            sql.ExecProc("sp_scores_param", Dgv_Data);
            try
            {



                Dgv_Data.Columns["level"].Visible = false;
                Dgv_Data.Columns["specialty"].Visible = false;
                Dgv_Data.Columns["years"].Visible = false;
                Dgv_Data.Columns["semester"].Visible = false;
                Dgv_Data.Columns["batch"].Visible = false;
                Dgv_Data.Columns["academic"].Visible = false;
                Dgv_Data.Columns[0].HeaderText = "កូដ";
                Dgv_Data.Columns[1].HeaderText = "គោត្តនាម នាម";
                Dgv_Data.Columns[2].HeaderText = "ជាឡាំង";
                Dgv_Data.Columns[3].HeaderText = "ភេទ";
                Dgv_Data.Columns[4].HeaderText = "ថ្ងៃខែឆ្នាំកំណើត";

            }
            catch (Exception ex)
            {
                //CAPTURE ERROR
                MessageBox.Show("ExecQuery Error: \n" + "សូមជ្រើសរើស អោយបានត្រឹមត្រូវ​ \n" + ex.Message);
            }

        }

        private void Frm_FR14_StudentResult_Load(object sender, EventArgs e)
        {
           
            /* display date time using minus 1 with now */
            int years = DateTime.Now.Year - 1;
            int next = DateTime.Now.Year;
            sql.cmbx("SELECT * FROM academics where nameen = '" + Convert.ToString(years) + "-" + Convert.ToString(next) + "'", Cmb_Academic, "id", "nameen");
            /* load data to combobox with diplaymember and display values*/
            sql.cmbx("SELECT * FROM levels", Cmb_Level, "id", "namekh");
        }

        private void Cmb_Level_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* clear combobox before load data again */
            this.Cmb_Specialty.DataSource = null;
            this.Cmb_Specialty.Items.Clear();

            /* load data to combobox with diplaymember and display values*/
            sql.cmbx("SELECT * FROM specialtys where level='" + Cmb_Level.SelectedValue + "'", Cmb_Specialty, "id", "namekh");



        }

        private void Cmb_Specialty_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* clear combobox before load data again */
            this.Cmb_Year.DataSource = null;
            this.Cmb_Year.Items.Clear();

            /* load data to combobox with diplaymember and display values*/
            sql.cmbx("SELECT * FROM years_regkh where specialty=N'" + Cmb_Specialty.Text + "' and level =N'" + Cmb_Level.Text + "' ", Cmb_Year, "id", "years");
            if (this.Dgv_Data.DataSource != null)
            {
                this.Dgv_Data.DataSource = null;
            }
            else
            {
                this.Dgv_Data.Rows.Clear();
            }
        }

        private void Cmb_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* clear combobox before load data again */
            this.Cmb_Batch.DataSource = null;
            this.Cmb_Batch.Items.Clear();

            /* load data to combobox with diplaymember and display values*/
            sql.cmbx("SELECT * FROM years_regkh where  years=N'" + Cmb_Year.Text + "' and specialty=N'" + Cmb_Specialty.Text + "' and level =N'" + Cmb_Level.Text + "' ", Cmb_Batch, "batch_id", "batch");
        }

        private void Cmb_Semester_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Cmb_Semester_DropDown(object sender, EventArgs e)
        {
            /* load data to combobox with diplaymember and display values*/
            sql.cmbx("SELECT * FROM semesters", Cmb_Semester, "id", "namekh");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Hide();
            Frm_Dashboard frm = new Frm_Dashboard();
            frm.Show();
        }

        private void btn_Maximize_Click(object sender, EventArgs e)
        {
            btn_Restore.Visible = true;
            btn_Maximize.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void Btn_Print_Click(object sender, EventArgs e)
        {

            Frm_RptERL frm = new Frm_RptERL();
            frm.ShowDialog();
        }

        private void btn_Restore_Click(object sender, EventArgs e)
        {
            btn_Restore.Visible = false;
            btn_Maximize.Visible = true;
            this.WindowState = FormWindowState.Maximized;
        }
        
        private void iconButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in Dgv_Data.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in Dgv_Data.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            ds.WriteXml("file.xml", XmlWriteMode.WriteSchema);



        }

        private DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            var dt = new DataTable();
            foreach(DataGridViewColumn column in dgv.Columns)
            {
                if(column.Visible)
                {
                    dt.Columns.Add();
                }
            }
            object[] cellValue = new object[dgv.Columns.Count];
            foreach (DataGridViewRow row in dgv.Rows)
            {
                for(int i = 0 ; i < cellValue.Length; i++)
                {
                    cellValue[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(cellValue);
            }
            return dt;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.Dgv_Data.Width, this.Dgv_Data.Height);
            Dgv_Data.DrawToBitmap(bm, new Rectangle(0, 0, this.Dgv_Data.Width, this.Dgv_Data.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }

        private void btn_Minimize_Click(object sender, EventArgs e)
        {

        }
    }
}
