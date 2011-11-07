using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDataEntityObjects;
using SDataEntityObjects.SData;
using Sage.Entity.Interfaces;
using Sage.Platform.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using SDataEntityObjects.Linq.Compiler;

namespace SDataEntityObjects.SDataPad
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public IClientContext Context;
        public DataTable DT;
        public LinqCompiler LQ;

        public string Servername;
        public int Port;
        public string Username;
        public string Password;
        public long MaxRequestSize;

        private void cmdRunQuery_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            lblStatus.Text = "Query running, please wait...";
            statusStrip.Refresh();

            Context = ClientFactory.GetContext(
                new SDataClientContextFactory(),
                new SDataContextConfiguration()
                {
                    Servername = Servername,
                    Port = Port,
                    Username = Username,
                    Password = Password,
                    MaxRequestSize = MaxRequestSize
                }
            );

            LQ = new LinqCompiler(txtQuery.Text);
            PopulateQuerySource();

            object result = null;
            try
            {
                result = LQ.Evaluate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblStatus.Text = "Query Failed: " + ex.GetType().ToString() ;
                Cursor.Current = Cursors.Arrow;
                return;
            }

            try
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = result;
                dgvResults.DataSource = bs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblStatus.Text = "Binding Failed: " + ex.GetType().ToString();
                Cursor.Current = Cursors.Arrow;
                return;
            }


            foreach (DataGridViewColumn dgvc in dgvResults.Columns)
            {
                dgvResults.AutoResizeColumn(dgvc.Index);
            }

            lblStatus.Text = "Query complete, " + dgvResults.Rows.Count + " rows returned.";
            Cursor.Current = Cursors.Arrow;

        }

        void PopulateQuerySource()
        {
            LQ.AddSource("ICE_SettlementDiscount", Context.CreateQuery<ICE_SettlementDiscount>().AsEnumerable());
            LQ.AddSource("ICE_VAT", Context.CreateQuery<ICE_VAT>().AsEnumerable());
            LQ.AddSource("ICE_VATRegister", Context.CreateQuery<ICE_VATRegister>().AsEnumerable());
            LQ.AddSource("INL_PostingCode", Context.CreateQuery<INL_PostingCode>().AsEnumerable());
            LQ.AddSource("IOP_OrderHeader", Context.CreateQuery<IOP_OrderHeader>().AsEnumerable());
            LQ.AddSource("IOP_OrderLine", Context.CreateQuery<IOP_OrderLine>().AsEnumerable());
            LQ.AddSource("IPL_Bank", Context.CreateQuery<IPL_Bank>().AsEnumerable());
            LQ.AddSource("IPL_Supplier", Context.CreateQuery<IPL_Supplier>().AsEnumerable());
            LQ.AddSource("ISL_Customer", Context.CreateQuery<ISL_Customer>().AsEnumerable());
            LQ.AddSource("IST_Product", Context.CreateQuery<IST_Product>().AsEnumerable());
            LQ.AddSource("IST_ProductGroup", Context.CreateQuery<IST_ProductGroup>().AsEnumerable());
            LQ.AddSource("IST_StockGLCrossReference", Context.CreateQuery<IST_StockGLCrossReference>().AsEnumerable());
        }

        private void txtQuery_Enter(object sender, EventArgs e)
        {
            // Text Box selects all by default - select none
            txtQuery.Select(0, 0);
        }

        private void cmdConnectionSettings_Click(object sender, EventArgs e)
        {
            ConnectionForm connectionDialog = new ConnectionForm();

            connectionDialog.txtServerName.Text = this.Servername;
            connectionDialog.txtPort.Text = this.Port.ToString();
            connectionDialog.txtUserName.Text = this.Username;
            connectionDialog.txtPassword.Text = this.Password;
            connectionDialog.numRequestLimit.Value = Int32.Parse(this.MaxRequestSize.ToString());

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (connectionDialog.ShowDialog(this) == DialogResult.OK)
            {

                this.Servername = connectionDialog.txtServerName.Text;
                this.Port = Int32.Parse(connectionDialog.txtPort.Text);
                this.Username = connectionDialog.txtUserName.Text;
                this.Password = connectionDialog.txtPassword.Text;
                this.MaxRequestSize = Int32.Parse(connectionDialog.numRequestLimit.Value.ToString());

            }
            connectionDialog.Dispose();

            UpdateConnectionInfo();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Default Settings
            Servername = "localhost";
            Port = 4444;
            Username = "manager";
            Password = "";
            MaxRequestSize = 20;

            UpdateConnectionInfo();
        }

        private void UpdateConnectionInfo()
        {

            this.Text = "SDataPad - " + Servername + ":" + Port.ToString() +
                                     " [" + Username + "]";
            
            lblOtherInfo.Text = "Row Limit: " + MaxRequestSize.ToString();
        }

        private void dgvResults_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Do nothing (for now)
        }

        private void dgvResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Custom formatting for collections?
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            cbxExamples.SelectedIndex = -1;
            cbxExamples.Text = "[New Query]";
            txtQuery.Clear();
               
        }

        
        

        private void cbxExamples_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbxExamples.Text == "Example 1")
            txtQuery.Text = "from word in \"The quick brown fox jumps over the lazy dog\".Split()" + Environment.NewLine +
                            "orderby word.Length" + Environment.NewLine +
                            "select new {word}";

            if (cbxExamples.Text == "Example 2")

            txtQuery.Text = "from o in IOP_OrderHeader" + Environment.NewLine +
                            "orderby o.Id" + Environment.NewLine +
                            "where o.InvoiceCustomer == \"BIKESHOP\"" + Environment.NewLine +
                            "select new {o.Id, Name = o.SL_Customer_InvoiceCustomer.Name.Trim(), " + Environment.NewLine +
                            "OrderLineCount = o.OP_OrderLine_OrderLine.Count()}";

            if (cbxExamples.Text == "Example 3")
                txtQuery.Text = "from a in IAccount" + Environment.NewLine +
                                "orderby a.LastHistoryDate descending" + Environment.NewLine +
                                "select new {a.AccountName, a.LastHistoryDate, Owner = a.Owner.OwnerDescription," + Environment.NewLine +
                                " Account_Manager = a.AccountManager.UserInfo.FirstName + \" \" + a.AccountManager.UserInfo.LastName," + Environment.NewLine +
                                " Account_Manager_Email = a.AccountManager.UserInfo.Email}";

            if (cbxExamples.Text == "Example 4")
                txtQuery.Text = "from o in IOpportunity" + Environment.NewLine +
                                "where o.Status == \"Open\"" + Environment.NewLine +
                                "orderby o.SalesPotential descending" + Environment.NewLine +
                                "select new {o.Description," + Environment.NewLine +
                                " SalesPotential = System.String.Format(\"{0:c}\", o.SalesPotential)," + Environment.NewLine +
                                " Probability = System.String.Format(\"{0}%\", o.CloseProbability)," + Environment.NewLine +
                                " DaysOpen = o.DaysOpen," +
                                " FirstContact = o.Account.Contacts.First().NamePFL.Trim()}";

        }

        private void cmdClearResults_Click(object sender, EventArgs e)
        {
            dgvResults.DataSource = null;

        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                cmdRunQuery_Click(sender, e);
            if (e.KeyCode == Keys.F6)
                cmdClear_Click(sender, e);
            if (e.KeyCode == Keys.F7)
                cmdClearResults_Click(sender, e);
        }

        private void cbxExamples_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                cmdRunQuery_Click(sender, e);
            if (e.KeyCode == Keys.F6)
                cmdClear_Click(sender, e);
            if (e.KeyCode == Keys.F7)
                cmdClearResults_Click(sender, e);
        }

        private void cmdAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void cmdLaunchExplorer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("EntityExplorer.exe");
        }

    }
}
