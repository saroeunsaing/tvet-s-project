using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace RPITST.Forms
{
    public partial class Frm_RptERL : Form
    {
        SQL_Control sql = new SQL_Control();
        public Frm_RptERL()
        {
            InitializeComponent();
        }

        private void Frm_ERL_Load(object sender, EventArgs e)
        {
            //DataView dview = new DataView();
            //dview.Table = sql.LoadReport("select * from score where specialty = N'acc' and year = 1","score").Tables["score"];
            /*
            //Reports.CR_ERL myreport = new Reports.CR_ERL();
            //myreport.SetDataSource(dview);
            //CR_DATA.ReportSource = myreport;
            //_ = crystalReportViewer1.DataBindings;
            Reports.CR_FR14ERL rpt = new Reports.CR_FR14ERL();
            //CALL CRYSTALL REPORT
            DataView dview = new DataView();
            //dview.Table = sql.LoadReport("select * from registerkh WHERE level = N'" + Frm_FR14_StudentList.levels + "' and specialty = N'" + Frm_FR14_StudentList.specialty + "' and years = N'" + Frm_FR14_StudentList.years + "' ", "register_2022").Tables["register_2022"];
            DataSet ds = new DataSet();
            //rpt.SetDataSource(dview);
            //CR_DATA.ReportSource = rpt;

            //CrystalReport1 cr = new CrystalReport1();
            rpt.SetDataSource(ds);
            CR_DATA.ReportSource = rpt;
            try
            {
                // using ReadXml method of DataSet read XML data from books.xml file 
                ds.ReadXml(@"file.xml");
                // copy XML data from temp dataset to our typed data set 
                //ds.Tables[0].Merge(ds.Tables[0]);
                //prepare report for preview 

                rpt.SetDataSource(ds.Tables[0]);
                CR_DATA.DisplayGroupTree = false;
                CR_DATA.ReportSource = rpt;

                //ParameterFields pfield = new ParameterFields();

                //ParameterField level = new ParameterField();
                //ParameterField specialty = new ParameterField();
                //ParameterField year = new ParameterField();
                //ParameterField semester = new ParameterField();
                //ParameterField academic = new ParameterField();
                //ParameterDiscreteValue p_level;
                //ParameterDiscreteValue p_specialty;
                //ParameterDiscreteValue p_year;
                //ParameterDiscreteValue p_semester;
                //ParameterDiscreteValue p_academic;
                //level.ParameterFieldName = "@levels";//pdate is a crystal report parameter name
                //level.CurrentValues.Clear();
                //p_level = new ParameterDiscreteValue();
                //level.CurrentValues.Add(p_level);
                //p_level.Value = "rpitst-tveto-Level01";

                //pfield.Add(level);

                //specialty.ParameterFieldName = "@specialtys";//no is a crystal report parameter name
                //specialty.CurrentValues.Clear();
                //p_specialty = new ParameterDiscreteValue();
                //specialty.CurrentValues.Add(p_specialty);
                //p_specialty.Value = "rpitst-tveto-BAICT01";

                //pfield.Add(specialty);

                //year.ParameterFieldName = "@years";//date is a crystal report       parameter name
                //year.CurrentValues.Clear();
                //p_year = new ParameterDiscreteValue();
                //year.CurrentValues.Add(p_year);
                //p_year.Value = "rpitst-tveto-year-02";

                //pfield.Add(year);

                //semester.ParameterFieldName = "@semesters";//date is a crystal report       parameter name
                //semester.CurrentValues.Clear();
                //p_semester = new ParameterDiscreteValue();
                //semester.CurrentValues.Add(p_semester);
                //p_semester.Value = "rpitst-tveto-Sem-001";

                //pfield.Add(semester);

                //academic.ParameterFieldName = "@academics";//date is a crystal report       parameter name
                //academic.CurrentValues.Clear();
                //p_academic = new ParameterDiscreteValue();
                //academic.CurrentValues.Add(p_academic);
                //p_academic.Value = "2021-2022";

                //pfield.Add(academic);

                //CR_DATA.ParameterFieldInfo = pfield;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            
            */
        }
        private void BindReport(string query)
        {
            ReportDocument crystalReport = new ReportDocument();
            CR_DATA.DisplayGroupTree = false;
            string filename = Application.StartupPath.Substring(0, Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf("\\")).LastIndexOf("\\")) + @"\Reports\CR_EF0014RL.rpt";
            crystalReport.Load(filename);
            Data.DataSet_RPITST dsCustomers = GetData(query, crystalReport);
            crystalReport.SetDataSource(dsCustomers);
            this.CR_DATA.ReportSource = crystalReport;
            this.CR_DATA.RefreshReport();
        }
        

        private Data.DataSet_RPITST GetData(string query, ReportDocument crystalReport)
        {
            string conString = "Data Source=GURUTECH\\LOCALHOST;Initial Catalog=rpitst_project; User ID=sa;Password=123 ";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@levels", SqlDbType.NVarChar,100).Value = "rpitst-tveto-Level03";
            cmd.Parameters.Add("@specialtys", SqlDbType.NVarChar, 100).Value = "rpitst-tveto-HDAUT01";
            cmd.Parameters.Add("@years", SqlDbType.NVarChar, 100).Value = "rpitst-tveto-year-01";
            cmd.Parameters.Add("@semesters", SqlDbType.NVarChar, 100).Value = "rpitst-tveto-Sem-001";
            cmd.Parameters.Add("@academics", SqlDbType.NVarChar, 100).Value = "2021-2022";

            using (SqlConnection con = new SqlConnection(conString))
            {
                Data.DataSet_RPITST dsCustomers = new Data.DataSet_RPITST();
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    //Get the List of all TextObjects in Section2.
                    List<TextObject> textObjects = crystalReport.ReportDefinition.Sections["Section2"].ReportObjects.OfType<TextObject>().ToList();
                    for (int i = 0; i < textObjects.Count; i++)
                    {
                        //Set the name of Column in TextObject.
                        textObjects[i].Text = string.Empty;
                        if (sdr.FieldCount > i)
                        {
                            textObjects[i].Text = sdr.GetName(i);
                        }
                    }
                    //Load all the data rows in the Dataset.
                    while (sdr.Read())
                    {

                        DataRow dr = dsCustomers.Tables[0].Rows.Add();
                        for (int i = 0; i < textObjects.Count; i++)
                        {

                            for (int j = 0; j < i; j++)
                            {
                                dr[j] = sdr[i];
                            }
                        }
                    }
                }
                con.Close();
                return dsCustomers;
                // You can also use an ArrayList instead of List<> 

                // Read rows from DataReader and populate the DataTable 

                /* ref: https://stackoverflow.com/questions/10529997/load-data-from-datareader */



            
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.BindReport("sp_scores_param");
        }
    }
}
