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
        public IList<IAccount> AccountList;
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
            Type[] typesEntity = System.Reflection.Assembly.GetAssembly(typeof(IAccount)).GetTypes();
            foreach (Type typeEntity in typesEntity)
            {
                if (typeEntity.IsInterface)
                {
                    cbxInterface.Items.Add(typeEntity);
                    System.Diagnostics.Trace.WriteLine(typeEntity.Name);
                }

            }
            cbxInterface.SelectedIndex = 0;

            entityTable = new DataTable();
            PropertyColumn = entityTable.Columns.Add("Property", typeof(string));
            ValueColumn = entityTable.Columns.Add("Value", typeof(string));
            TypeColumn = entityTable.Columns.Add("Object Type", typeof(string));

            // Default Settings
            Servername = "localhost";
            Port = 3333;
            Username = "lee";
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
            LQ.AddSource("IAccount", Context.CreateQuery<IAccount>().AsEnumerable());
            LQ.AddSource("IAccountOperatingCompany", Context.CreateQuery<IAccountOperatingCompany>().AsEnumerable());
            LQ.AddSource("IAccountProduct", Context.CreateQuery<IAccountProduct>().AsEnumerable());
            LQ.AddSource("IAccountSummary", Context.CreateQuery<IAccountSummary>().AsEnumerable());
            LQ.AddSource("IAccountSyncView", Context.CreateQuery<IAccountSyncView>().AsEnumerable());
            LQ.AddSource("IActivity", Context.CreateQuery<IActivity>().AsEnumerable());
            LQ.AddSource("IActivityTrackingEvent", Context.CreateQuery<IActivityTrackingEvent>().AsEnumerable());
            LQ.AddSource("IActivityTrackingEventExtract", Context.CreateQuery<IActivityTrackingEventExtract>().AsEnumerable());
            LQ.AddSource("IActivityTrackingEventHistory", Context.CreateQuery<IActivityTrackingEventHistory>().AsEnumerable());
            LQ.AddSource("IActivityTrackingEvtAntn", Context.CreateQuery<IActivityTrackingEvtAntn>().AsEnumerable());
            LQ.AddSource("IAddress", Context.CreateQuery<IAddress>().AsEnumerable());
            LQ.AddSource("IAdHocGroup", Context.CreateQuery<IAdHocGroup>().AsEnumerable());
            LQ.AddSource("IAppIdMapping", Context.CreateQuery<IAppIdMapping>().AsEnumerable());
            LQ.AddSource("IAreaCategoryIssue", Context.CreateQuery<IAreaCategoryIssue>().AsEnumerable());
            LQ.AddSource("IAssociation", Context.CreateQuery<IAssociation>().AsEnumerable());
            LQ.AddSource("IAttachment", Context.CreateQuery<IAttachment>().AsEnumerable());
            LQ.AddSource("ICampaign", Context.CreateQuery<ICampaign>().AsEnumerable());
            LQ.AddSource("ICampaignProduct", Context.CreateQuery<ICampaignProduct>().AsEnumerable());
            LQ.AddSource("ICampaignStage", Context.CreateQuery<ICampaignStage>().AsEnumerable());
            LQ.AddSource("ICampaignTarget", Context.CreateQuery<ICampaignTarget>().AsEnumerable());
            LQ.AddSource("ICampaignTargetsView", Context.CreateQuery<ICampaignTargetsView>().AsEnumerable());
            LQ.AddSource("ICampaignTask", Context.CreateQuery<ICampaignTask>().AsEnumerable());
            LQ.AddSource("ICommodityGroup", Context.CreateQuery<ICommodityGroup>().AsEnumerable());
            LQ.AddSource("ICompetitor", Context.CreateQuery<ICompetitor>().AsEnumerable());
            LQ.AddSource("IContact", Context.CreateQuery<IContact>().AsEnumerable());
            LQ.AddSource("IContactLeadSource", Context.CreateQuery<IContactLeadSource>().AsEnumerable());
            LQ.AddSource("IContactSyncView", Context.CreateQuery<IContactSyncView>().AsEnumerable());
            LQ.AddSource("IContract", Context.CreateQuery<IContract>().AsEnumerable());
            LQ.AddSource("IContractIncident", Context.CreateQuery<IContractIncident>().AsEnumerable());
            LQ.AddSource("IContractItem", Context.CreateQuery<IContractItem>().AsEnumerable());
            LQ.AddSource("ICountryCodeMapping", Context.CreateQuery<ICountryCodeMapping>().AsEnumerable());
            LQ.AddSource("IDeDupJob", Context.CreateQuery<IDeDupJob>().AsEnumerable());
            LQ.AddSource("IDeDupResult", Context.CreateQuery<IDeDupResult>().AsEnumerable());
            LQ.AddSource("IDefect", Context.CreateQuery<IDefect>().AsEnumerable());
            LQ.AddSource("IDefectActivityItem", Context.CreateQuery<IDefectActivityItem>().AsEnumerable());
            LQ.AddSource("IDefectActivityRate", Context.CreateQuery<IDefectActivityRate>().AsEnumerable());
            LQ.AddSource("IDefectHistory", Context.CreateQuery<IDefectHistory>().AsEnumerable());
            LQ.AddSource("IDefectProblem", Context.CreateQuery<IDefectProblem>().AsEnumerable());
            LQ.AddSource("IDefectProduct", Context.CreateQuery<IDefectProduct>().AsEnumerable());
            LQ.AddSource("IDefectReturn", Context.CreateQuery<IDefectReturn>().AsEnumerable());
            LQ.AddSource("IDefectSolution", Context.CreateQuery<IDefectSolution>().AsEnumerable());
            LQ.AddSource("IDefectTicket", Context.CreateQuery<IDefectTicket>().AsEnumerable());
            LQ.AddSource("IDepartment", Context.CreateQuery<IDepartment>().AsEnumerable());
            LQ.AddSource("IERPEmailAddress", Context.CreateQuery<IERPEmailAddress>().AsEnumerable());
            LQ.AddSource("IERPPhoneNumber", Context.CreateQuery<IERPPhoneNumber>().AsEnumerable());
            LQ.AddSource("IERPSalesOrder", Context.CreateQuery<IERPSalesOrder>().AsEnumerable());
            LQ.AddSource("IERPTradingAccount", Context.CreateQuery<IERPTradingAccount>().AsEnumerable());
            LQ.AddSource("IEvent", Context.CreateQuery<IEvent>().AsEnumerable());
            LQ.AddSource("IExchangeRate", Context.CreateQuery<IExchangeRate>().AsEnumerable());
            LQ.AddSource("IHistory", Context.CreateQuery<IHistory>().AsEnumerable());
            LQ.AddSource("IHistoryMarketingServiceClick", Context.CreateQuery<IHistoryMarketingServiceClick>().AsEnumerable());
            LQ.AddSource("IHistoryMarketingServiceOpen", Context.CreateQuery<IHistoryMarketingServiceOpen>().AsEnumerable());
            LQ.AddSource("IHistoryMarketingServiceUndeliverable", Context.CreateQuery<IHistoryMarketingServiceUndeliverable>().AsEnumerable());
            LQ.AddSource("IImportHistory", Context.CreateQuery<IImportHistory>().AsEnumerable());
            LQ.AddSource("IImportHistoryItem", Context.CreateQuery<IImportHistoryItem>().AsEnumerable());
            LQ.AddSource("IImportTemplate", Context.CreateQuery<IImportTemplate>().AsEnumerable());
            LQ.AddSource("IIndexDefinition", Context.CreateQuery<IIndexDefinition>().AsEnumerable());
            LQ.AddSource("IIndexGeneral", Context.CreateQuery<IIndexGeneral>().AsEnumerable());
            LQ.AddSource("IIndexSchedule", Context.CreateQuery<IIndexSchedule>().AsEnumerable());
            LQ.AddSource("IIndexStatistics", Context.CreateQuery<IIndexStatistics>().AsEnumerable());
            LQ.AddSource("ILead", Context.CreateQuery<ILead>().AsEnumerable());
            LQ.AddSource("ILeadAddress", Context.CreateQuery<ILeadAddress>().AsEnumerable());
            LQ.AddSource("ILeadAddressHistory", Context.CreateQuery<ILeadAddressHistory>().AsEnumerable());
            LQ.AddSource("ILeadHistory", Context.CreateQuery<ILeadHistory>().AsEnumerable());
            LQ.AddSource("ILeadHistoryHistory", Context.CreateQuery<ILeadHistoryHistory>().AsEnumerable());
            LQ.AddSource("ILeadHistoryQualification", Context.CreateQuery<ILeadHistoryQualification>().AsEnumerable());
            LQ.AddSource("ILeadHistoryResponse", Context.CreateQuery<ILeadHistoryResponse>().AsEnumerable());
            LQ.AddSource("ILeadImportMap", Context.CreateQuery<ILeadImportMap>().AsEnumerable());
            LQ.AddSource("ILeadQualification", Context.CreateQuery<ILeadQualification>().AsEnumerable());
            LQ.AddSource("ILeadSource", Context.CreateQuery<ILeadSource>().AsEnumerable());
            LQ.AddSource("ILibraryDirs", Context.CreateQuery<ILibraryDirs>().AsEnumerable());
            LQ.AddSource("ILibraryDocs", Context.CreateQuery<ILibraryDocs>().AsEnumerable());
            LQ.AddSource("ILiteratureItem", Context.CreateQuery<ILiteratureItem>().AsEnumerable());
            LQ.AddSource("ILitRequest", Context.CreateQuery<ILitRequest>().AsEnumerable());
            LQ.AddSource("ILitRequestItem", Context.CreateQuery<ILitRequestItem>().AsEnumerable());
            LQ.AddSource("IMarketingServiceCampaign", Context.CreateQuery<IMarketingServiceCampaign>().AsEnumerable());
            LQ.AddSource("IMarketingServiceClick", Context.CreateQuery<IMarketingServiceClick>().AsEnumerable());
            LQ.AddSource("IMarketingServiceOpen", Context.CreateQuery<IMarketingServiceOpen>().AsEnumerable());
            LQ.AddSource("IMarketingServiceRecipient", Context.CreateQuery<IMarketingServiceRecipient>().AsEnumerable());
            LQ.AddSource("IMarketingServiceUndeliverable", Context.CreateQuery<IMarketingServiceUndeliverable>().AsEnumerable());
            LQ.AddSource("IMarketingServiceUserInfo", Context.CreateQuery<IMarketingServiceUserInfo>().AsEnumerable());
            LQ.AddSource("IOpportunity", Context.CreateQuery<IOpportunity>().AsEnumerable());
            LQ.AddSource("IOpportunityCampaign", Context.CreateQuery<IOpportunityCampaign>().AsEnumerable());
            LQ.AddSource("IOpportunityCompetitor", Context.CreateQuery<IOpportunityCompetitor>().AsEnumerable());
            LQ.AddSource("IOpportunityContact", Context.CreateQuery<IOpportunityContact>().AsEnumerable());
            LQ.AddSource("IOpportunityProduct", Context.CreateQuery<IOpportunityProduct>().AsEnumerable());
            LQ.AddSource("IOwner", Context.CreateQuery<IOwner>().AsEnumerable());
            LQ.AddSource("IOwnerJoin", Context.CreateQuery<IOwnerJoin>().AsEnumerable());
            LQ.AddSource("IOwnerRights", Context.CreateQuery<IOwnerRights>().AsEnumerable());
            LQ.AddSource("IOwnerSecurityProfile", Context.CreateQuery<IOwnerSecurityProfile>().AsEnumerable());
            LQ.AddSource("IOwnerView", Context.CreateQuery<IOwnerView>().AsEnumerable());
            LQ.AddSource("IPackage", Context.CreateQuery<IPackage>().AsEnumerable());
            LQ.AddSource("IPackageKitChildView", Context.CreateQuery<IPackageKitChildView>().AsEnumerable());
            LQ.AddSource("IPackageProduct", Context.CreateQuery<IPackageProduct>().AsEnumerable());
            LQ.AddSource("IPickListItemView", Context.CreateQuery<IPickListItemView>().AsEnumerable());
            LQ.AddSource("IPickListView", Context.CreateQuery<IPickListView>().AsEnumerable());
            LQ.AddSource("IProcess", Context.CreateQuery<IProcess>().AsEnumerable());
            LQ.AddSource("IProcessInstanceState", Context.CreateQuery<IProcessInstanceState>().AsEnumerable());
            LQ.AddSource("IProcessInstanceStatus", Context.CreateQuery<IProcessInstanceStatus>().AsEnumerable());
            LQ.AddSource("IProcessTrackingEvent", Context.CreateQuery<IProcessTrackingEvent>().AsEnumerable());
            LQ.AddSource("IProcessTrackingEventAnnotation", Context.CreateQuery<IProcessTrackingEventAnnotation>().AsEnumerable());
            LQ.AddSource("IProcessTrackingEventHistory", Context.CreateQuery<IProcessTrackingEventHistory>().AsEnumerable());
            LQ.AddSource("IProcessTypeCategory", Context.CreateQuery<IProcessTypeCategory>().AsEnumerable());
            LQ.AddSource("IProcessTypeInformation", Context.CreateQuery<IProcessTypeInformation>().AsEnumerable());
            LQ.AddSource("IProdPackageKitView", Context.CreateQuery<IProdPackageKitView>().AsEnumerable());
            LQ.AddSource("IProduct", Context.CreateQuery<IProduct>().AsEnumerable());
            LQ.AddSource("IProductProgram", Context.CreateQuery<IProductProgram>().AsEnumerable());
            LQ.AddSource("IQualification", Context.CreateQuery<IQualification>().AsEnumerable());
            LQ.AddSource("IQualificationCategory", Context.CreateQuery<IQualificationCategory>().AsEnumerable());
            LQ.AddSource("IResourceList", Context.CreateQuery<IResourceList>().AsEnumerable());
            LQ.AddSource("IResourceSchedule", Context.CreateQuery<IResourceSchedule>().AsEnumerable());
            LQ.AddSource("IResponseProduct", Context.CreateQuery<IResponseProduct>().AsEnumerable());
            LQ.AddSource("IReturn", Context.CreateQuery<IReturn>().AsEnumerable());
            LQ.AddSource("IReturnAddress", Context.CreateQuery<IReturnAddress>().AsEnumerable());
            LQ.AddSource("IReturnReceivedProduct", Context.CreateQuery<IReturnReceivedProduct>().AsEnumerable());
            LQ.AddSource("IReturnShippedProduct", Context.CreateQuery<IReturnShippedProduct>().AsEnumerable());
            LQ.AddSource("IRole", Context.CreateQuery<IRole>().AsEnumerable());
            LQ.AddSource("ISalesOrder", Context.CreateQuery<ISalesOrder>().AsEnumerable());
            LQ.AddSource("ISalesOrderAddress", Context.CreateQuery<ISalesOrderAddress>().AsEnumerable());
            LQ.AddSource("ISalesOrderItem", Context.CreateQuery<ISalesOrderItem>().AsEnumerable());
            LQ.AddSource("ISalesOrderSyncView", Context.CreateQuery<ISalesOrderSyncView>().AsEnumerable());
            LQ.AddSource("ISalesProcessAudit", Context.CreateQuery<ISalesProcessAudit>().AsEnumerable());
            LQ.AddSource("ISalesProcesses", Context.CreateQuery<ISalesProcesses>().AsEnumerable());
            LQ.AddSource("ISalesQuotationSyncView", Context.CreateQuery<ISalesQuotationSyncView>().AsEnumerable());
            LQ.AddSource("ISecuredAction", Context.CreateQuery<ISecuredAction>().AsEnumerable());
            LQ.AddSource("ISecuredActionRole", Context.CreateQuery<ISecuredActionRole>().AsEnumerable());
            LQ.AddSource("ISlxContractSyncLog", Context.CreateQuery<ISlxContractSyncLog>().AsEnumerable());
            LQ.AddSource("ISlxLocation", Context.CreateQuery<ISlxLocation>().AsEnumerable());
            LQ.AddSource("ISlxLogItem", Context.CreateQuery<ISlxLogItem>().AsEnumerable());
            LQ.AddSource("ISlxPrice", Context.CreateQuery<ISlxPrice>().AsEnumerable());
            LQ.AddSource("ISlxPriceList", Context.CreateQuery<ISlxPriceList>().AsEnumerable());
            LQ.AddSource("ISyncDigest", Context.CreateQuery<ISyncDigest>().AsEnumerable());
            LQ.AddSource("ISyncJob", Context.CreateQuery<ISyncJob>().AsEnumerable());
            LQ.AddSource("ISyncResult", Context.CreateQuery<ISyncResult>().AsEnumerable());
            LQ.AddSource("ITargetResponse", Context.CreateQuery<ITargetResponse>().AsEnumerable());
            LQ.AddSource("ITeam", Context.CreateQuery<ITeam>().AsEnumerable());
            LQ.AddSource("ITicket", Context.CreateQuery<ITicket>().AsEnumerable());
            LQ.AddSource("ITicketAccountProduct", Context.CreateQuery<ITicketAccountProduct>().AsEnumerable());
            LQ.AddSource("ITicketActivity", Context.CreateQuery<ITicketActivity>().AsEnumerable());
            LQ.AddSource("ITicketActivityItem", Context.CreateQuery<ITicketActivityItem>().AsEnumerable());
            LQ.AddSource("ITicketActivityRate", Context.CreateQuery<ITicketActivityRate>().AsEnumerable());
            LQ.AddSource("ITicketAreaOwner", Context.CreateQuery<ITicketAreaOwner>().AsEnumerable());
            LQ.AddSource("ITicketHistory", Context.CreateQuery<ITicketHistory>().AsEnumerable());
            LQ.AddSource("ITicketProblem", Context.CreateQuery<ITicketProblem>().AsEnumerable());
            LQ.AddSource("ITicketProblemSolutionType", Context.CreateQuery<ITicketProblemSolutionType>().AsEnumerable());
            LQ.AddSource("ITicketProblemType", Context.CreateQuery<ITicketProblemType>().AsEnumerable());
            LQ.AddSource("ITicketSolution", Context.CreateQuery<ITicketSolution>().AsEnumerable());
            LQ.AddSource("ITicketSolutionType", Context.CreateQuery<ITicketSolutionType>().AsEnumerable());
            LQ.AddSource("IUnitOfMeasure", Context.CreateQuery<IUnitOfMeasure>().AsEnumerable());
            LQ.AddSource("IUnitOfMeasureGroup", Context.CreateQuery<IUnitOfMeasureGroup>().AsEnumerable());
            LQ.AddSource("IUrgency", Context.CreateQuery<IUrgency>().AsEnumerable());
            LQ.AddSource("IUser", Context.CreateQuery<IUser>().AsEnumerable());
            LQ.AddSource("IUserActivity", Context.CreateQuery<IUserActivity>().AsEnumerable());
            LQ.AddSource("IUserCalendar", Context.CreateQuery<IUserCalendar>().AsEnumerable());
            LQ.AddSource("IUserInfo", Context.CreateQuery<IUserInfo>().AsEnumerable());
            LQ.AddSource("IUserNotification", Context.CreateQuery<IUserNotification>().AsEnumerable());
            LQ.AddSource("IUserProfile", Context.CreateQuery<IUserProfile>().AsEnumerable());
            LQ.AddSource("IUserRole", Context.CreateQuery<IUserRole>().AsEnumerable());
            LQ.AddSource("IUserSubscription", Context.CreateQuery<IUserSubscription>().AsEnumerable());
            LQ.AddSource("IUserTrackingEvent", Context.CreateQuery<IUserTrackingEvent>().AsEnumerable());
            LQ.AddSource("IUserTrackingEventAnnotation", Context.CreateQuery<IUserTrackingEventAnnotation>().AsEnumerable());
            LQ.AddSource("IUserTrackingEvtExt", Context.CreateQuery<IUserTrackingEvtExt>().AsEnumerable());
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

