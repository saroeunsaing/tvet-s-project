using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPITST.Forms
{
    public partial class Frm_Provisional : Form
    {
        public Frm_Provisional()
        {
            InitializeComponent();
        }

        private void Frm_Provisional_Load(object sender, EventArgs e)
        {
            Reports.CR_Provisional_Certificate_ cryRpt = new Reports.CR_Provisional_Certificate_();
            cryRpt.Load("Reports\\CR_Provisional_Certificate_.rpt");
            crystalReportViewer1.ReportSource = cryRpt;
            crystalReportViewer1.Refresh();
        }
    }
}
