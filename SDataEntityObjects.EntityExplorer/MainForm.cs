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


namespace SDataEntityObjects.EntityExplorer
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public IClientContext Context;
        public TreeNodeCollection Nodes;
        public DataTable entityTable;
        public DataColumn PropertyColumn;
        public DataColumn ValueColumn;
        public DataColumn TypeColumn;
        public StringBuilder BreadCrumb;
        public LinqCompiler LQ;

        public string Servername;
        public int Port;
        public string Username;
        public string Password;
        public long MaxRequestSize;

        private void MainForm_Load(object sender, EventArgs e)
        {

            cbxInterface.Items.Add(typeof(ICE_SettlementDiscount));
            cbxInterface.Items.Add(typeof(ICE_VAT));
            cbxInterface.Items.Add(typeof(ICE_VATRegister));
            cbxInterface.Items.Add(typeof(INL_PostingCode));
            cbxInterface.Items.Add(typeof(IOP_OrderHeader));
            cbxInterface.Items.Add(typeof(IOP_OrderLine));
            cbxInterface.Items.Add(typeof(IPL_Bank));
            cbxInterface.Items.Add(typeof(IPL_Supplier));
            cbxInterface.Items.Add(typeof(ISL_Customer));
            cbxInterface.Items.Add(typeof(IST_Product));
            cbxInterface.Items.Add(typeof(IST_ProductGroup));
            cbxInterface.Items.Add(typeof(IST_StockGLCrossReference));

            cbxInterface.SelectedIndex = 0;

            entityTable = new DataTable();
            PropertyColumn = entityTable.Columns.Add("Property", typeof(string));
            ValueColumn = entityTable.Columns.Add("Value", typeof(string));
            TypeColumn = entityTable.Columns.Add("Object Type", typeof(string));

            // Default Settings
            Servername = "localhost";
            Port = 4444;
            Username = "manager";
            Password = "";
            MaxRequestSize = 20;

            UpdateConnectionInfo();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
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

            Cursor.Current = Cursors.WaitCursor;

            PopulateTree();

            Cursor.Current = Cursors.Arrow;



        }



        private void PopulateTree()
        {
            Type myType = cbxInterface.SelectedItem as Type;
            System.Collections.IEnumerator enumerator;

            Nodes = entityTreeView.Nodes;
            Nodes.Clear();

            lblStatus.Text = "Loading entities, please wait...";
            statusStrip.Refresh();
            if (chkEnableLINQ.Checked == false)
            {
                System.Reflection.MethodInfo method = Context.GetType().GetMethod("CreateQuery");
                System.Reflection.MethodInfo generic = method.MakeGenericMethod(myType);
                object result = generic.Invoke(Context, null);
                enumerator = (System.Collections.IEnumerator)result.GetType().InvokeMember("GetEnumerator", System.Reflection.BindingFlags.InvokeMethod, System.Type.DefaultBinder, result, null);
            }
            else
            {
                LQ = new LinqCompiler(txtLINQ.Text);
                PopulateQuerySource();
                object result = null;
                try
                {
                    result = LQ.Evaluate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    lblStatus.Text = "Query error occured: " + ex.GetType().ToString();
                    statusStrip.Refresh();
                    return;
                }

                enumerator = (System.Collections.IEnumerator)result.GetType().InvokeMember("GetEnumerator", System.Reflection.BindingFlags.InvokeMethod, System.Type.DefaultBinder, result, null);
            }
            try {
                while (enumerator.MoveNext())
                {
                    Object obj = enumerator.Current;
                    TreeNode tnp = new TreeNode();
                    string svalue;
                    object value = obj.GetType().InvokeMember("DisplayValue", System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, obj, null);
                    if (value == null)
                        svalue = "NULL";
                    else
                        svalue = value.ToString();
                    tnp.Text = svalue;
                    System.Reflection.PropertyInfo[] PropertyInfoArray = myType.GetProperties();
                    foreach (System.Reflection.PropertyInfo pi in PropertyInfoArray)
                    {
                        try
                        { 
                            string name = pi.Name;
                            value = obj.GetType().InvokeMember(pi.Name, System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, obj, null);
                            if (value == null)
                                svalue = "NULL";
                            else
                                svalue = value.ToString();
                            object tag = obj.GetType().InvokeMember(pi.Name, System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, obj, null);
                            TreeNode tn = new TreeNode(name + ": " + svalue);
                            tn.Tag = tag;
                            tn.Name = name;
                            tn.ToolTipText = svalue;
                            tnp.Nodes.Add(tn);

                            

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(pi.Name + ": " + ex.Message);
                        }
                    }

                    Nodes.Add(tnp);
                }
                lblStatus.Text = "Loading complete.";
                statusStrip.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                lblStatus.Text = "Loading error occured: " + ex.GetType().ToString();
                statusStrip.Refresh();
            }
        }

        private void entityTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                Type EntityInterfaceType;
                if (e.Node.Tag == null)
                    return;

                EntityInterfaceType = e.Node.Tag.GetType().GetInterfaces().First();
                System.Reflection.PropertyInfo[] PropertyInfoArray = EntityInterfaceType.GetProperties();
                if (EntityInterfaceType.Name.StartsWith("IList"))
                {
                    // IList Collection                    
                    EntityInterfaceType = e.Node.Tag.GetType().GetGenericArguments().First();
                    PropertyInfoArray = EntityInterfaceType.GetProperties();
                    Type ie = typeof(List<>);
                    Type[] typeArgs = { EntityInterfaceType };
                    Type constructed = ie.MakeGenericType(typeArgs);

                    System.Collections.IEnumerator enumerator = (System.Collections.IEnumerator)e.Node.Tag.GetType().InvokeMember("GetEnumerator", System.Reflection.BindingFlags.InvokeMethod, System.Type.DefaultBinder, e.Node.Tag, null);
                    enumerator.Reset();

                    while (enumerator.MoveNext())
                    {
                        Object obj = enumerator.Current;
                        TreeNode tnp = new TreeNode();
                        string svalue;
                        object value = obj.GetType().InvokeMember("DisplayValue", System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, obj, null);
                        if (value == null)
                            svalue = "NULL";
                        else
                            svalue = value.ToString();

                        tnp.Text = svalue;

                        foreach (System.Reflection.PropertyInfo pi in PropertyInfoArray)
                        {
                            try
                            {
                                string name = pi.Name;
                                value = obj.GetType().InvokeMember(pi.Name, System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, obj, null).ToString();
                                if (value == null)
                                    svalue = "NULL";
                                else
                                    svalue = value.ToString();
                                object tag = obj.GetType().InvokeMember(pi.Name, System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, obj, null);
                                TreeNode tn = new TreeNode(name + ": " + svalue);
                                tn.Tag = tag;
                                tn.Name = name;
                                tn.ToolTipText = svalue;
                                tnp.Nodes.Add(tn);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.WriteLine(pi.Name + ": " + ex.Message);
                            }
                        }

                        e.Node.Nodes.Add(tnp);

                    }
                }
                // GridView Management
                {
                    foreach (System.Reflection.PropertyInfo pi in PropertyInfoArray)
                    {
                        try
                        {
                            string name = pi.Name;
                            string sv;
                            object value = e.Node.Tag.GetType().InvokeMember(pi.Name, System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, e.Node.Tag, null);
                            if (value == null)
                            {
                                sv = "NULL";
                            }
                            else
                            {
                                sv = value.ToString();
                            }
                            object tag = e.Node.Tag.GetType().InvokeMember(pi.Name, System.Reflection.BindingFlags.GetProperty, System.Type.DefaultBinder, e.Node.Tag, null);
                            TreeNode tn = new TreeNode(name + ": " + sv);
                            tn.Tag = tag;
                            tn.Name = name;
                            tn.ToolTipText = sv;
                            e.Node.Nodes.Add(tn);
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException == null)
                            {
                                System.Diagnostics.Trace.WriteLine(pi.Name + ": " + ex.Message);
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine(pi.Name + ": " + ex.InnerException.Message);

                            }
                        }
                    }
                }
            }
            entityTable.Clear();
            foreach (TreeNode tn in e.Node.Nodes)
            {
                DataRow dr = entityTable.NewRow();
                dr[PropertyColumn] = tn.Name;
                dr[ValueColumn] = tn.ToolTipText;
                if (tn.Tag == null)
                    dr[TypeColumn] = "null";
                else
                    dr[TypeColumn] = tn.Tag.GetType().ToString();
                entityTable.Rows.Add(dr);
            }

            dgvEntity.DataSource = entityTable;
            dgvEntity.Refresh();

            // Breadcrumb Management
            BreadCrumb = new StringBuilder();
            TreeNode bctn = e.Node;            
            BuildCrumb(bctn);
            lblBreadCrumb.Text = BreadCrumb.ToString();
            lblBreadCrumb.Refresh();
            
            Cursor.Current = Cursors.Arrow;                
        }

        void BuildCrumb(TreeNode tn)
        {
            if (tn.Parent != null)
                BuildCrumb(tn.Parent);
            BreadCrumb.Append(tn.Text + " - ");            
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

        private void cbxInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = ((Type)cbxInterface.SelectedItem).Name;
            string initial = name.Substring(1,1).ToLower();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("from " + initial + " in " + name );
            sb.AppendLine("select " + initial);
            txtLINQ.Text = sb.ToString();
        }

        private void chkEnableLINQ_CheckedChanged(object sender, EventArgs e)
        {
                txtLINQ.Enabled = chkEnableLINQ.Checked;
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

        private void UpdateConnectionInfo()
        {

            this.Text = "EntityExplorer - " + Servername + ":" + Port.ToString() +
                                     " [" + Username + "]";

            lblOtherInfo.Text = "Row Limit: " + MaxRequestSize.ToString();
        }

        private void cmdClearResults_Click(object sender, EventArgs e)
        {
            entityTreeView.Nodes.Clear();
            dgvEntity.DataSource = null;
        }

        private void cmdAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void cmdLaunchSDataPad_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("SDataPad.exe");
        }
        




    }


}

