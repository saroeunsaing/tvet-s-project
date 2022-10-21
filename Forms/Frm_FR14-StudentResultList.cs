using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Microsoft.Office.Interop;

namespace RPITST.Forms
{
    public partial class Frm_FR14_StudentResultList : Form
    {

        SQL_Control sql = new SQL_Control();

        public Frm_FR14_StudentResultList()
        {
            InitializeComponent();
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
            this.WindowState = FormWindowState.Maximized;

        }

        private void btn_Restore_Click(object sender, EventArgs e)
        {
            btn_Restore.Visible = false;
            btn_Maximize.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void Frm_FR14_StudentResultList_Load(object sender, EventArgs e)
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



        private void Dgv_Data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Dgv_Data_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Vertical text from column 0, or adjust below, if first column(s) to be skipped
            //if (e.RowIndex == -1 && e.ColumnIndex >= 5)
            //{
            //    e.PaintBackground(e.CellBounds, true);
            //    e.Graphics.TranslateTransform(e.CellBounds.Left, e.CellBounds.Bottom);
            //    e.Graphics.RotateTransform(270);
            //    e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, Brushes.Black, 5, 5);
            //    e.Graphics.ResetTransform();

            //    e.Handled = true;
            //}



        }

        private void Dgv_Data_Validated(object sender, EventArgs e)
        {
            //Dgv_Data.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //Dgv_Data.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            //Dgv_Data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //Dgv_Data.Columns[6].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }
        private void retrive()
        {
            sql.AddParam("@levels", Cmb_Level.SelectedValue == null ? DBNull.Value : Cmb_Level.SelectedValue);
            sql.AddParam("@specialtys", Cmb_Specialty.SelectedValue == null ? DBNull.Value : Cmb_Specialty.SelectedValue);
            sql.AddParam("@semesters", Cmb_Semester.SelectedValue == null ? DBNull.Value : Cmb_Semester.SelectedValue);
            sql.AddParam("@years", Cmb_Level.SelectedValue == null ? DBNull.Value : Cmb_Year.SelectedValue);
            sql.AddParam("@academics", Cmb_Academic.Text);

            sql.ExecProc("sp_scores_params", Dgv_Data);
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            retrive();

        }

        private void Dgv_Data_VisibleChanged(object sender, EventArgs e)
        {
            if (Dgv_Data.Visible)
            {

                DataGridView_AutoSize();
                DataGridView_CentreHeaders();
                // Etc...
            }
        }
        private void DataGridView_AutoSize()
        {
            var col = Dgv_Data.Columns;

            for (int i = 0; i < col.Count; i++)
            {
                if (i == 0) { col[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
                if (i == 1) { col[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
                // Etc...
            }
        }
        private void DataGridView_CentreHeaders()
        {
            // Centre Column Cells Content
            Dgv_Data.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Centre (Column and Row) Headers    
            Dgv_Data.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Dgv_Data.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            // Set Font
            Dgv_Data.ColumnHeadersDefaultCellStyle.Font = new Font("Suwannaphum", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        private void Dgv_Data_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //// Vertical text from column 0, or adjust below, if first column(s) to be skipped
            //if (e.RowIndex == -1 && e.ColumnIndex >= 4)
            //{
            //    e.PaintBackground(e.CellBounds, true);
            //    e.Graphics.TranslateTransform(e.CellBounds.Left, e.CellBounds.Bottom);
            //    e.Graphics.RotateTransform(270);
            //    e.Graphics.DrawString(e.FormattedValue.ToString(), e.CellStyle.Font, Brushes.Black, 5, 5);
            //    //e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //    //Dgv_Data.ColumnHeaderCellChanged.WrapMode =DataGridViewTriState.True;

            //    //Dgv_Data.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //    e.Graphics.ResetTransform();
            //    e.Handled = true;
            //}
        }

        private void Dgv_Data_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.Value != null)
            //    e.Value = ((string)e.Value).Trim();
        }

        private void Dgv_Data_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            // Dgv_Data.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Dgv_Data.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Dgv_Data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Btn_Remove_Click(object sender, EventArgs e)
        {

            try
            {
                for (int i = 0; i < Dgv_Data.SelectedRows.Count; i++)

                {

                    sql.AddParam("@nameen", Dgv_Data.SelectedRows[i].Cells[1].Value.ToString());
                    sql.ExecQuery("DELETE FROM scores Where nameen=@nameen;", true);



                    // Dgv_Data.Rows.RemoveAt(Dgv_Data.SelectedRows[0].Index);

                }
                MessageBox.Show("All Scores Was Deleted");
                retrive();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {


        }  




        public void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void Btn_Export_Click(object sender, EventArgs e)
        {
            //var dia = new System.Windows.Forms.SaveFileDialog();
            //dia.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //dia.Filter = "Excel Worksheets (*.xlsx)|*.xlsx|xls file (*.xls)|*.xls|All files (*.*)|*.*";
            //if (dia.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            //{
            //    DataTable data = null;// use the DataSource of the DataGridView here
            //    var excel = new OfficeOpenXml.ExcelPackage();
            //    var ws = excel.Workbook.Worksheets.Add("worksheet-name");
            //    // you can also use LoadFromCollection with an `IEnumerable<SomeType>`
            //    ws.Cells["A1"].LoadFromDataTable(data, true, OfficeOpenXml.Table.TableStyles.Light1);
            //    ws.Cells[ws.Dimension.Address.ToString()].AutoFitColumns();

            //    using (var file = File.Create(dia.FileName))
            //        excel.SaveAs(file);
            //}
            //////Microsoft.Office.Interop.Excel.Application xlApp;
            //////Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //////Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //////object misValue = System.Reflection.Missing.Value;

            //////xlApp = new Microsoft.Office.Interop.Excel.Application();
            //////xlWorkBook = xlApp.Workbooks.Add(misValue);
            //////xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //////int i = 0;
            //////int j = 0;

            //////for (i = 0; i <= Dgv_Data.RowCount - 1; i++)
            //////{
            //////    for (j = 0; j <= Dgv_Data.ColumnCount - 1; j++)
            //////    {
            //////        DataGridViewCell cell = Dgv_Data[j, i];
            //////        xlWorkSheet.Cells[i + 1, j + 1] = cell.Value;
            //////    }
            //////}

            //////xlWorkBook.SaveAs("csharp.net-informations.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //////xlWorkBook.Close(true, misValue, misValue);
            //////xlApp.Quit();

            //////releaseObject(xlWorkSheet);
            //////releaseObject(xlWorkBook);
            //////releaseObject(xlApp);

            //////MessageBox.Show("Excel file created , you can find the file c:\\csharp.net-informations.xls");



            // creating Excel Application
            string fileName = String.Empty;
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.
            // store its reference to worksheet
            try
            {
                //Fixed:(Microsoft.Office.Interop.Excel.Worksheet)
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                // changing the name of active sheet
                worksheet.Name = "Exported from RPITST";
                // storing header part in Excel
                for (int i = 1; i < Dgv_Data.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = Dgv_Data.Columns[i - 1].HeaderText;
                }
                // storing Each row and column value to excel sheet
                for (int i = 0; i < Dgv_Data.Rows.Count; i++)
                {
                    for (int j = 0; j < Dgv_Data.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = Dgv_Data.Rows[i].Cells[j].Value.ToString();
                       
                    }
                }


                // Save The Application
                SaveFileDialog saveFileExcel = new SaveFileDialog();

                saveFileExcel.Filter = "Excel files |*.xls|All files (*.*)|*.*";
                saveFileExcel.FilterIndex = 2;
                saveFileExcel.RestoreDirectory = true;


                if (saveFileExcel.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileExcel.FileName;
                    //Fixed-old code :11 para->add 1:Type.Missing
                    workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                }
                else
                {
                    return;

                    // Exit from the application
                    //app.Quit();
                }
            }
            catch (Exception)
            {
                //Statement;
            }
            finally
            {
                app.Quit();
                workbook = null;
                app = null;
            }
        }
    }

}
