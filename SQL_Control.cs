﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Data.SqlTypes;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace RPITST
{
    class SQL_Control: SQL_Connection
    {
        public SqlConnection cn = new SqlConnection();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataAdapter da = new SqlDataAdapter();
        public SqlDataReader dr;
        public DataTable dt;
        public DataSet ds;

        //internal object LoadReport(string v)
        //{
        //    throw new NotImplementedException();
        //}





        #region EXECUTEQUERY   
        // QUERY PARAMETERSdfdsf
        public List<SqlParameter> Params = new List<SqlParameter>();
        public int RecordCount;
        public string Exception;
        public string ReturnQuery;
        public void ExecQuery(String Query , Boolean ReturnIdentity = false)
        {
            //RESET QUERY STATS
            RecordCount = 0;
            Exception = "";

            using (cn = GetConnection()) 
            {
                try
                {
                    cn.Open();
                    // CREATE DB COMMAND
                    cmd = new SqlCommand(Query, cn);
                    // LOAD PARAMS INTO DB COMMAND
                    Params.ForEach(p => cmd.Parameters.Add(p));
                    // CLEAR PARAM LIST
                    Params.Clear();
                    // EXECUTE COMMAND & FILL DATASET
                    dt = new DataTable();
  
                    da = new SqlDataAdapter(cmd);
                    RecordCount = da.Fill(dt);

                if(ReturnIdentity == true) 
                  {
                        ReturnQuery = "SELECT @@IDENTITY As LastID;";
                        // @@IDENTITY - SESSION
                        // SCOPE_IDENTITY() - SESSION & SCOPE
                        // IDENT_CURRENT(tablename) - LAST IDENT IN TABLE, ANY SCOPE, ANY SESSION
                        cmd = new SqlCommand(ReturnQuery, cn);
                        dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        RecordCount = da.Fill(dt);
                  }
                }
                catch (Exception ex)
                {
                    // CAPTURE ERROR
                    Exception = "ExecQuery Error: \n" + ex.Message;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }
        public void ExecProc(String Query, DataGridView dgv)
        {
            //RESET QUERY STATS
            RecordCount = 0;
            Exception = "";

            using (cn = GetConnection())
            {
                try
                {
                    cn.Open();
                    // CREATE DB COMMAND
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Query;
                    cmd.Connection = cn;
                // LOAD PARAMS INTO DB COMMAND
                    if (Params != null)
                    {
                        Params.ForEach(p => cmd.Parameters.Add(p));
                    }
               
                    // CLEAR PARAM LIST
                    Params.Clear();
                    // EXECUTE COMMAND & FILL DATASET
                    dt = new DataTable();
                    

                    da.SelectCommand = cmd;

                    da.Fill(dt);
                    DataRow dr = dt.NewRow();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dgv.DataSource = dt;
                        dgv.Columns[0].HeaderText = "គោត្តនាម នាម";
                        dgv.Columns[1].HeaderText = "ជាឡាំង";
                        dgv.Columns[2].HeaderText = "ភេទ";
                        dgv.Columns[3].HeaderText = "ថ្ងៃខែឆ្នាំកំណើត";
                    }
                    else
                    {
                        MessageBox.Show("No Data");
                    }
                    //dgv.DataSource = dt;


                    cn.Close();
                    da.Dispose();
                    

            }
            catch (Exception ex)
            {
                //CAPTURE ERROR
                Exception = "ExecQuery Error: \n" + ex.Message;
            }
            finally
            {

                if (cn.State == ConnectionState.Open)
                {

                    cn.Close();
                }
            }
        }
    }

        
        public string XmlString<X>(X obj, bool omitXmlDeclaration = false, bool omitNameSpace = false)
        {
            var xSer = new XmlSerializer(typeof(X));

            var settings = new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Indent = true, Encoding = new UTF8Encoding(false) };
            using (var ms = new MemoryStream())
            using (var writer = XmlWriter.Create(ms, settings))
            {
                if (!omitNameSpace)
                {
                    xSer.Serialize(writer, obj);
                }
                else
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    xSer.Serialize(writer, obj, ns);
                }
                ms.Seek(0, 0);
                return new UTF8Encoding(false).GetString(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }
        public void ExecProcXML()
        {
            

            using (cn = GetConnection())
            {
                using (var cmd = new SqlCommand("SELECT * FROM scores", cn))
                {
                    //setup cmd with type of sql to execute
                    //establish a data reader
                    cn.Open();//at the latest possible moment
                    using (var dr = cmd.ExecuteReader())
                    {
                        //do assignments
                        XmlString(@"c:\file.xml", true, true);
                    }
                }
            }
        }

        // ADD PARAMS
        public void AddParam(String Name, Object Value)
        {
            SqlParameter NewParam = new SqlParameter(Name, Value);
            Params.Add(NewParam);
        }
        public Boolean HasException(Boolean Report  = false)
        {
            if(String.IsNullOrEmpty(Exception))
            {
                return false;
            }
            if(Report == true)
            {
                MessageBox.Show(Exception, "Exception:");
            }
            return true;
        }

        #endregion

        #region Report
        public DataSet LoadReport(string qty,string table)
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                da = new SqlDataAdapter(qty, cn);
                Data.DataSet_RPITST ds = new Data.DataSet_RPITST();
                ds.EnforceConstraints = false;
                da.Fill(ds, table);
                return ds;
            }
            //CALL CRYSTALL REPORT
            //DataView dview = new DataView();
            //dview.Table = sql.LoadReport("score").Tables["score"];

            //Reports.CR_ERL myreport = new Reports.CR_ERL();
            //myreport.SetDataSource(dview);
            //crystalReportViewer1.ReportSource = myreport;
            ////_ = crystalReportViewer1.DataBindings;
        }
        public DataSet LoadProc(string qty)
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn; //database connection
                cmd.CommandText = qty; //  Stored procedure name
                cmd.CommandType = CommandType.StoredProcedure; // set it to stored proc
                                                               //add parameter if necessary
                
                // LOAD PARAMS INTO DB COMMAND
                if (Params != null)
                {
                    Params.ForEach(p => cmd.Parameters.Add(p));
                }

                // CLEAR PARAM LIST
                Params.Clear();
                da = new SqlDataAdapter(cmd);
                Data.DataSet_RPITST ds = new Data.DataSet_RPITST();

                
                ds.EnforceConstraints = false;
                da.Fill(ds, "DataTable");
                return ds;
            }
            //CALL CRYSTALL REPORT
            //DataView dview = new DataView();
            //dview.Table = sql.LoadReport("score").Tables["score"];

            //Reports.CR_ERL myreport = new Reports.CR_ERL();
            //myreport.SetDataSource(dview);
            //crystalReportViewer1.ReportSource = myreport;
            ////_ = crystalReportViewer1.DataBindings;
        }

        #endregion
        public string GetMaxID(string table, string field, int num, string pre, string defualt)

        {
            string id;
            using (var cn = GetConnection())
            {
                cn.Open();
                cmd = new SqlCommand("Select top 1 " + field + " From " + table + " order by " + field + " DESC", cn);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string s = dr.GetString(0);
                    s = s.Substring(num, s.Length - 2);

                    int str = Convert.ToInt32(s) + 1;
                    id = pre + Convert.ToString(str).PadLeft(s.Length, '0');
                }
                else
                {
                    id = defualt;
                }

                dr.Close();

                return id;

                cn.Close();
            }
        }

        //function for combobox with display value and value Member
        public void cmbx(string qty, ComboBox cbx, string index, string value)
        {
            using (var cn = GetConnection())
            {
                //  cn.Open();
                da = new SqlDataAdapter(qty, cn);
                dt = new DataTable();

                da.Fill(dt);


                cbx.DataSource = dt;
                cbx.DisplayMember = value;
                cbx.ValueMember = index;
                //diable blue highlight in combobox
                cbx.DropDownStyle = ComboBoxStyle.DropDownList;

                //how to call function above: cmbx("SELECT * FROM semesters", comboboxname, "id", "nameen");
            }
        }
        public void retrieve(string qty, DataGridView dgv)
        {
            using (var cn = GetConnection())
            {

                cn.Open();
                da = new SqlDataAdapter(qty, cn);

                dt = new DataTable();

                da.Fill(dt);

                dgv.DataSource = dt;

            }
        }
        
        public void Dgv_Score(string qty, DataGridView dgv)
        {
            cn = GetConnection();
            cn.Open();
            SqlCommand command = new SqlCommand(qty, cn);
            SqlDataReader reader = command.ExecuteReader();
            SqlDataReader sqlDataReader = reader;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("namekh");
            dataTable.Columns.Add("nameen");
            dataTable.Columns.Add("gender");
            dataTable.Columns.Add("dob");
            while (sqlDataReader.Read())
            {
                DataRow row = dataTable.NewRow();
                row["id"] = sqlDataReader["id"];
                row["namekh"] = sqlDataReader["namekh"];
                row["nameen"] = sqlDataReader["nameen"];
                row["gender"] = sqlDataReader["gender"];
                row["dob"] = sqlDataReader["dob"];
                dataTable.Rows.Add(row);
            }
            dgv.DataSource = dataTable;
            //dgv.DataBindings();
            cn.Close();

        }
    



        /*
       

        DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();
            dt.AsEnumerable()
                .Select(r => r[pivotColumn.ColumnName].ToString())
                .Distinct().ToList()
                .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];
            }

            return result;
        }
        */

        /*
        public void BindGrid(DataGridView gvDetails, ComboBox cboX, ComboBox cboY, ComboBox cboZ)
        {

            ////string query = @"DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)
            //            DECLARE @ColumnName AS NVARCHAR(MAX)
            //            SELECT @ColumnName = ISNULL(@ColumnName + ',','')+ QUOTENAME(subject) FROM (SELECT DISTINCT subject FROM score) AS student
            //            SET @DynamicPivotQuery = ';WITH CTE AS(SELECT subject,student,score FROM score)
            //            SELECT student,'+@ColumnName+' FROM CTE
            //            PIVOT (MAX(score) FOR [subject] IN('+@ColumnName+')) p
            //            ORDER BY student DESC'
            //                EXEC(@DynamicPivotQuery)";

            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select * from score";
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        //using (DataTable dt = new DataTable())
                        //{
                        //gvDetails.AutoGenerateColumns = true;
                        //sda.Fill(dt);
                        //gvDetails.DataSource = dt;
                        //// gvDetails.DataBindings();


                        //SqlCommand cmd = new SqlCommand("select * from tbl_data", con);
                        //con.Open();
                        //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        con.Close();

                        gvDetails.DataSource = dt;

                        //}
                    }
                }
            }
        } */



        // DataTable m_dataTable;
        // DataTable table { get { return m_dataTable; } set { m_dataTable = value; } }

        /* for form_score in project */
        /*
        private const string m_choiceCol = "Score";

        class Options
        {
            public int m_Index { get; set; }
            public string m_Text { get; set; }
        }
        
        public void retriev_scores(string qty,DataGridView dgv)
        {
            using (var cn = GetConnection())
            {
                cmd = new SqlCommand(qty, cn);
                da = new SqlDataAdapter(cmd);

                dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;

                
                if (!dgv.Columns.Contains(m_choiceCol))
                {
                    DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                    txtCol.Name = m_choiceCol;
                    dgv.Columns.Add(txtCol);
                    dgv.Columns[1].ReadOnly = true;
                    dgv.Columns[0].ReadOnly = true;
                    dgv.Columns[2].ReadOnly = true;
                    dgv.Columns[3].ReadOnly = true;
                }

                //List<Options> oList = new List<Options>();
                //oList.Add(new Options() { m_Index = 0, m_Text = "None" });
                //for (int i = 1; i < 10; i++)
                //{
                //    oList.Add(new Options() { m_Index = i, m_Text = "Op" + i });
                //}

                //for (int i = 0; i < dgv.Rows.Count - 1; i += 2)
                //{
                   // DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();

                //Setup A
                //c.DataSource = oList;
                //c.Value = oList[0].m_Text;
                //c.ValueMember = "m_Text";
                //c.DisplayMember = "m_Text";
                //c.ValueType = typeof(string);

                ////Setup B
                //c.DataSource = oList;
                //c.Value = 0;
                //c.ValueMember = "m_Index";
                //c.DisplayMember = "m_Text";
                //c.ValueType = typeof(int);

                //Result is the same A or B
                //dgv[m_choiceCol, i] = c;
                //}
                //ref: https://stackoverflow.com/questions/1814423/datagridview-how-to-set-a-cell-in-editing-mode
            }


        }
        */

    }


}