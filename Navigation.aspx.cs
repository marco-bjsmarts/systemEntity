using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SystemEntity.MasterPages;

namespace SystemEntity.Navigation
{
    public partial class Navigation : BasePage
    {        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateTopNavigation();
            }
        }        

        private void PopulateTopNavigation()
        {
            
            topTreeView.SelectedNodeStyle.BackColor = System.Drawing.Color.Silver;

            TreeNode TopNode = new
                        TreeNode("Top Navigation", "1");

            topTreeView.Nodes.Add(TopNode);

            NavigationNode topNode = GetNode("1");

            lblTitle.Text = "Top Navigation";
            lblUrl.Text = topNode != null ? topNode.Url : "";

            SqlCommand sqlQuery = new SqlCommand(
                "Select TopMenu, TopNavigationId, TopParentId, TopMenuLink, TopMenuOrder From dbo.TopNavigation");
            DataSet resultSet;

            resultSet = RunQuery(sqlQuery);

            DataView view = new DataView(resultSet.Tables[0]);

            view.RowFilter = "TopParentId is NULL";

            view.Sort = "TopMenuOrder ASC";

            foreach (DataRowView row in view)
            {
                TreeNode NewNode = new
                        TreeNode(row["TopMenu"].ToString(),
                        row["TopNavigationId"].ToString());
                                
                TopNode.ChildNodes.Add(NewNode);                
                AddTopChildItems(resultSet.Tables[0], NewNode);
            }

            TopNode.Selected = true;
        }

        private void AddTopChildItems(DataTable table, TreeNode NewNode)
        {
            DataView viewItem = new DataView(table);

            viewItem.RowFilter = "TopParentId = " + NewNode.Value;

            viewItem.Sort = "TopMenuOrder ASC";

            foreach (DataRowView childView in viewItem)
            {                
                TreeNode childNode = new
                        TreeNode(childView["TopMenu"].ToString(),
                        childView["TopNavigationId"].ToString());

                NewNode.ChildNodes.Add(childNode);                
            }
        }

        private NavigationNode GetNode(String TopNavigationId)
        {
            NavigationNode node = new NavigationNode();

            SqlCommand sqlQuery = new SqlCommand(
                "Select [TopMenuLink], [TopMenuExternal] From dbo.TopNavigation where TopNavigationId=" + TopNavigationId);

            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["EntityConecctionString"].ConnectionString;

            SqlConnection conn =
                new SqlConnection(connectionString);

            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            dbAdapter.SelectCommand = sqlQuery;
            sqlQuery.Connection = conn;

            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                dbAdapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                Response.Write("Error has occurred: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            if (dt.Rows.Count > 0)
            {
                int external = int.Parse(dt.Rows[0]["TopMenuExternal"].ToString());
                node.IsChecked = external;
                node.Url = dt.Rows[0]["TopMenuLink"].ToString();
            }

            return node;
        }

        //private String GetNodeUrl(String TopNavigationId)
        //{
        //    SqlCommand sqlQuery = new SqlCommand(
        //        "Select TopMenuLink From dbo.TopNavigation where TopNavigationId=" + TopNavigationId);
            
        //    string connectionString =
        //        ConfigurationManager.ConnectionStrings
        //        ["EntityConecctionString"].ConnectionString;

        //    SqlConnection conn =
        //        new SqlConnection(connectionString);

        //    SqlDataAdapter dbAdapter = new SqlDataAdapter();
        //    dbAdapter.SelectCommand = sqlQuery;
        //    sqlQuery.Connection = conn;

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        conn.Open();
        //        dbAdapter.Fill(dt);
        //    }
        //    catch (SqlException ex)
        //    {
        //        Response.Write("Error has occurred: " + ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    if (dt.Rows.Count > 0)
        //        return dt.Rows[0]["TopMenuLink"].ToString();

        //    return string.Empty;
        //}

        //private DataSet RunQuery(SqlCommand sqlQuery)
        //{

        //    DataSet resultsDataSet = new DataSet();

        //    string connectionString =
        //        ConfigurationManager.ConnectionStrings
        //        ["EntityConecctionString"].ConnectionString;

        //    SqlConnection conn =
        //        new SqlConnection(connectionString);

        //    SqlDataAdapter dbAdapter = new SqlDataAdapter();
        //    dbAdapter.SelectCommand = sqlQuery;

        //    sqlQuery.Connection = conn;

        //    try
        //    {
        //        conn.Open();
        //        dbAdapter.Fill(resultsDataSet);
        //    }
        //    catch (SqlException ex)
        //    {
        //        Response.Write("Error has occurred: " + ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return resultsDataSet;
        //}

        //private void ExecuteNonQuery(String SQLStmt)
        //{
        //    SqlConnection conn = new SqlConnection(
        //        ConfigurationManager.ConnectionStrings
        //        ["EntityConecctionString"].ConnectionString
        //        );

        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = System.Data.CommandType.Text;
        //    cmd.CommandText = SQLStmt;
        //    cmd.Connection = conn;

        //    try
        //    {
        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (SqlException ex)
        //    {
        //        Response.Write("Error has occurred: " + ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        protected void topTreeView_SelectedNodeChanged(object sender, EventArgs e)
        {            
            NavigationNode node = GetNode(topTreeView.SelectedNode.Value.ToString());

            lblTitle.Text = topTreeView.SelectedNode.Text;
            lblUrl.Text = node.Url;
            chkAddExternal.Checked = node.IsChecked == 1 ? false : true;


            txtTitle1.Text = topTreeView.SelectedNode.Text;
            txtUrl1.Text = node.Url;
            chkEditExternal.Checked = node.IsChecked == 1 ? false : true;
        }

        protected void btnMoveUp_Click(object sender, EventArgs e)
        {

            TreeNode sourceNode = this.topTreeView.SelectedNode;
            if (sourceNode != null)
            {
                TreeNode parentNode = this.topTreeView.SelectedNode.Parent;
                if (parentNode != null)
                {
                    int index = -1;

                    for (int j = 0; j < parentNode.ChildNodes.Count; j++)
                    {
                        if (sourceNode == parentNode.ChildNodes[j])
                        {
                            index = j;
                            break;
                        }
                    }

                    if (index > 0)
                    {
                        int prevIndex = index - 1;

                        parentNode.ChildNodes.RemoveAt(index);

                        if (parentNode.ChildNodes[prevIndex].ChildNodes.Count > 0)
                        {            
                            parentNode.ChildNodes[prevIndex].ChildNodes.Add(sourceNode);
                            sourceNode.Selected = true;
                        }
                        else
                        {
                            if (parentNode.Text != "Top Navigation")
                            {
                                parentNode.ChildNodes.AddAt(prevIndex, sourceNode);                                
                            }
                            else
                            {
                                parentNode.ChildNodes[prevIndex].ChildNodes.Add(sourceNode);
                            }
                            
                            sourceNode.Selected = true;
                        }
                    }
                    else
                    {
                        if (parentNode.Text != "Top Navigation")
                        {
                            parentNode.ChildNodes.RemoveAt(index);

                            int indexParent = 0;

                            for (int j = 0; j < parentNode.Parent.ChildNodes.Count; j++)
                            {
                                if (parentNode == parentNode.Parent.ChildNodes[j])
                                {
                                    indexParent = j;                                    
                                    break;
                                }
                            }

                            TreeNode NewNode = new
                                            TreeNode(sourceNode.Text,
                                                sourceNode.Value);

                            parentNode.Parent.ChildNodes.AddAt(indexParent, NewNode);
                            NewNode.Selected = true;
                        }
                    }

                }
            }
        }

        protected void btnMoveDown_Click(object sender, EventArgs e)
        {
            TreeNode sourceNode = this.topTreeView.SelectedNode;
            if (sourceNode != null)
            {
                TreeNode parentNode = this.topTreeView.SelectedNode.Parent;
                if (parentNode != null)
                {
                    int index = -1;

                    for (int j = 0; j < parentNode.ChildNodes.Count; j++)
                    {
                        if (sourceNode == parentNode.ChildNodes[j])
                        {
                            index = j;
                            break;
                        }
                    }

                    int nextIndex = index + 1;

                    if (nextIndex < parentNode.ChildNodes.Count)
                    {
                        parentNode.ChildNodes.RemoveAt(index);

                        if (parentNode.ChildNodes[index].ChildNodes.Count > 0)
                        {
                            parentNode.ChildNodes[index].ChildNodes.AddAt(
                                parentNode.ChildNodes[index].ChildNodes.Count,
                                sourceNode);
                            sourceNode.Selected = true;
                        }
                        {
                            parentNode.ChildNodes.AddAt(nextIndex, sourceNode);
                            sourceNode.Selected = true;
                        }
                    }
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode sourceNode = this.topTreeView.SelectedNode;
            if (sourceNode != null)
            {
                TreeNode parentNode = this.topTreeView.SelectedNode.Parent;
                if (parentNode != null)
                {
                    int index = -1;

                    for (int j = 0; j < parentNode.ChildNodes.Count; j++)
                    {
                        if (sourceNode == parentNode.ChildNodes[j])
                        {
                            index = j;
                            break;
                        }
                    }

                    parentNode.ChildNodes.RemoveAt(index);

                    string SqlStmt = "DELETE FROM dbo.TopNavigation WHERE TopNavigationId = " + sourceNode.Value.ToString();
                    
                    ExecuteNonQuery(SqlStmt);
                }
            }
        }

        protected void btnAddLink_Click(object sender, EventArgs e)
        {
            TreeNode rootNode = topTreeView.Nodes[0];
            int value = rootNode.ChildNodes.Count + 1;            

            int external = 1;

            if (chkAddExternal.Checked)
                external = 0;

            string SqlStmt = "INSERT INTO dbo.TopNavigation ( [TopMenu],[TopMenuDescription],[TopMenuLink],[TopMenuOrder],[TopParentId],[TopMenuExternal]) "
                + " VALUES ('" + txtTitle.Text + "', '" + txtDesc.Text + "', '" + txtURL.Text + "', " + value + ", NULL," + external + " )";

            ExecuteNonQuery(SqlStmt);

            SqlCommand sqlQuery = new SqlCommand(
                "Select TopMenu, TopNavigationId, TopParentId, TopMenuLink, TopMenuOrder From dbo.TopNavigation");
            DataSet resultSet;

            resultSet = RunQuery(sqlQuery);

            DataView view = new DataView(resultSet.Tables[0]);

            view.RowFilter = "TopMenu = '" + txtTitle.Text + "'";

            if (view.Count > 0)
            {
                TreeNode NewNode = new TreeNode(txtTitle.Text, view[0]["TopNavigationId"].ToString());
                rootNode.ChildNodes.Add(NewNode);
                NewNode.Selected = true;
            }

            txtTitle.Text = "";
            txtURL.Text = "";
            txtDesc.Text = "";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            TreeNode sourceNode = this.topTreeView.SelectedNode;
            if (sourceNode != null)
            {
                TreeNode parentNode = this.topTreeView.SelectedNode.Parent;
                if (parentNode != null)
                {
                    int index = -1;

                    for (int j = 0; j < parentNode.ChildNodes.Count; j++)
                    {
                        if (sourceNode == parentNode.ChildNodes[j])
                        {
                            index = j;
                            break;
                        }
                    }

                    int value = index + 1;
                    TreeNode NewNode = new TreeNode(txtTitle1.Text, value.ToString());

                    parentNode.ChildNodes.RemoveAt(index);
                    parentNode.ChildNodes.AddAt(index, NewNode);
                    NewNode.Selected = true;

                    int external = 1;

                    if (chkAddExternal.Checked)
                        external = 0;

                    string SqlStmt = "UPDATE [dbo].[TopNavigation] "
                            + "SET [TopMenu] = '" + txtTitle1.Text + "'"
                            + ",[TopMenuDescription] = '" + txtDesc1.Text + "'"
                            + ",[TopMenuLink] = '" + txtUrl1.Text + "'"
                            + ",[TopMenuExternal] = " + external
                            + " WHERE [TopNavigationId] = " + sourceNode.Value;


                    ExecuteNonQuery(SqlStmt);
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {

            SaveTopNavigation();            

            Response.Redirect("~/SiteSettings/Settings.aspx");
        }

        protected void SaveTopNavigation()
        {
            int TopIndex = 1;

            foreach (TreeNode node in topTreeView.Nodes[0].ChildNodes)
            {
                if (node.ChildNodes.Count > 0)
                {
                    int TopChildIndex = 1;

                    foreach (TreeNode childNode in node.ChildNodes)
                    {
                        string SqlStmt = "UPDATE [dbo].[TopNavigation] "
                            + "SET [TopMenuOrder] = '" + TopChildIndex + "'"
                            + ",[TopParentId] = " + node.Value
                            + " WHERE [TopNavigationId] = " + childNode.Value;

                        ExecuteNonQuery(SqlStmt);

                        TopChildIndex = TopChildIndex + 1;
                    }

                    string SqlStmt2 = "UPDATE [dbo].[TopNavigation] "
                            + "SET [TopMenuOrder] = '" + TopIndex + "'"
                            + " WHERE [TopNavigationId] = " + node.Value;

                    ExecuteNonQuery(SqlStmt2);
                    TopIndex = TopIndex + 1;
                }
                else
                {
                    string SqlStmt = "UPDATE [dbo].[TopNavigation] "
                            + "SET [TopMenuOrder] = '" + TopIndex + "'"
                            + ", [TopParentId] = NULL "
                            + " WHERE [TopNavigationId] = " + node.Value;

                    ExecuteNonQuery(SqlStmt);
                    TopIndex = TopIndex + 1;
                }
            }
        }        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SiteSettings/Settings.aspx");
        }

    }


    public class NavigationNode
    {
        public int IsChecked { get; set; }
        public string Url { get; set; }
    }
}