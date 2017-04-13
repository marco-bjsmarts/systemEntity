<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Navigation.aspx.cs" Inherits="SystemEntity.Navigation.Navigation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        
    </script>
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="container-fluid">
    <div class="row">
        <div class="col-sm-4" ><h2>Top Navigation</h2></div>
        <div class="col-sm-8" ></div>
    </div>
    <div class="row">
        <div class="col-sm-4" >Specify the navigation items to display in top navigation for this Web site. This navigation is shown at the top of the page in most Web sites.</div>
        <div class="col-sm-8" >Maximum number of dynamic items to show within this level of navigation: <asp:TextBox ID="topMaxItems" runat="server" Width="10%" /></div>
    </div>
    <div class="row">
        <div class="col-sm-4" ><h4>Top Navigation: Editing and Sorting </h4></div>
        <div class="col-sm-8" >
                      
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4" >Use this section to reorder and modify the navigation items under this site. You can create, delete and edit navigation links and headings. You can also move navigation items under headings and choose to display or hide navigation links. </div>
        <div class="col-sm-8" >
            <table class="table table-bordered">
                <tr>
                    <td>
                    <div class="Row">
                        <div class="col-sm-2" ><asp:LinkButton ID="btnMoveUp" runat="server" OnClick="btnMoveUp_Click" >Move Up</asp:LinkButton></div>
                        <div class="col-sm-2" ><asp:LinkButton ID="btnMoveDown" runat="server" OnClick="btnMoveDown_Click" >Move Down</asp:LinkButton></div>
                        <div class="col-sm-2" ><asp:LinkButton ID="btnEdit" data-toggle="modal" runat="server" data-target="#Edit" >Edit</asp:LinkButton></div>
                        <div class="col-sm-2" ><asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" >Delete</asp:LinkButton></div>
                        <div class="col-sm-2" ><asp:LinkButton ID="btnLink" data-toggle="modal" runat="server" data-target="#addLink" >Add Link</asp:LinkButton></div>
                    </div>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:TreeView ID="topTreeView" runat="server" OnSelectedNodeChanged="topTreeView_SelectedNodeChanged" >
                            
                        </asp:TreeView>
                    </td>
                </tr>
            </table>  
            <table class="table table-bordered">
                <tr>
                    <td>
                        Selected Item
                    </td>
                </tr>
                <tr>
                    <td>
                        Title: <asp:Label ID="lblTitle" runat="server"></asp:Label><br />
                        URL: <asp:Label ID="lblUrl" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>    
    <div class="row">
        <div class="col-sm-10" ></div>
        <div class="col-sm-2" >
            <asp:Button ID="btnOK" runat="server" Text="OK" class="btn" OnClick="btnOK_Click"/>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn" OnClick="btnCancel_Click"/>
        </div>
    </div>
    </div>    

    <div id="addLink" class="modal fade" >
        <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="H1" runat="server">Navigation Link</h4>
            </div>
            <div class="modal-body">
                <table class="table">
                <tr>
                    <td>Title:</td>
                    <td><asp:TextBox ID="txtTitle" runat="server" width="262px" MaxLength="255" /></td>
                </tr>
                <tr>
                    <td>URL:</td>
                    <td><asp:TextBox ID="txtURL" runat="server" width="262px" MaxLength="255" />&nbsp;&nbsp;<asp:Button ID="btnBrowse" runat="server" class="btn" Text="Browse..." /></td>
                </tr>
                <tr>
                    <td>External Link:</td>
                    <td><asp:CheckBox ID="chkAddExternal" runat="server" /></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Rows="3" Columns="34" /></td>
                </tr>
                </table>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnAddLink" runat="server" 
                  Text="OK" 
                  CssClass="btn"
                  title="OK" 
                  OnClick="btnAddLink_Click"
                  />
                <asp:Button ID="btnmodal1" runat="server" 
                  Text="Cancel" 
                  CssClass="btn"
                  title="Cancel" 
                  data-dismiss="modal" />                
            </div>
        </div>
    </div>
    </div>

    <div id="Edit" class="modal fade" >
        <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title" id="H2" runat="server">Navigation Link</h4>
            </div>
            <div class="modal-body">
                <table class="table">
                <tr>
                    <td>Title:</td>
                    <td><asp:TextBox ID="txtTitle1" runat="server" width="262px" MaxLength="255" /></td>
                </tr>
                <tr>
                    <td>URL:</td>
                    <td><asp:TextBox ID="txtUrl1" runat="server" width="262px" MaxLength="255" />&nbsp;&nbsp;<asp:Button ID="btnBrowse1" runat="server" class="btn" Text="Browse..." /></td>
                </tr>
                <tr>
                    <td>External Link:</td>
                    <td><asp:CheckBox ID="chkEditExternal" runat="server" /></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox ID="txtDesc1" runat="server" TextMode="MultiLine" Rows="3" Columns="34" /></td>
                </tr>
                </table>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnUpdate" runat="server" 
                  Text="OK" 
                  CssClass="btn"
                  title="OK" 
                  OnClick="btnUpdate_Click"
                  />
                <asp:Button ID="btnCancel1" runat="server" 
                  Text="Cancel" 
                  CssClass="btn"
                  title="Cancel" 
                  data-dismiss="modal" />                
            </div>
        </div>
    </div>
    </div>
    
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="QuickLoginUI" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftColumnContent" runat="server">
</asp:Content>
