
namespace RPITST.Forms
{
    partial class Frm_RptERL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CR_DATA = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CR_DATA
            // 
            this.CR_DATA.ActiveViewIndex = -1;
            this.CR_DATA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CR_DATA.Cursor = System.Windows.Forms.Cursors.Default;
            this.CR_DATA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CR_DATA.Location = new System.Drawing.Point(0, 0);
            this.CR_DATA.Margin = new System.Windows.Forms.Padding(4);
            this.CR_DATA.Name = "CR_DATA";
            this.CR_DATA.Size = new System.Drawing.Size(1067, 554);
            this.CR_DATA.TabIndex = 0;
            this.CR_DATA.ToolPanelWidth = 267;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(644, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Frm_RptERL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CR_DATA);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Frm_RptERL";
            this.Text = "Frm_ERL";
            this.Load += new System.EventHandler(this.Frm_ERL_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer CR_DATA;
        private System.Windows.Forms.Button button1;
    }
}