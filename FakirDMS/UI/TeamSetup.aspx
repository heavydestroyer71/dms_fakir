<%@ Page Title="Team Member Assign | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TeamSetup.aspx.cs" Inherits="FakirDMS.UI.TeamSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-4">
                    <div class="panel panel-info">
                        <div class="panel-header">Search - Supervisor</div>
                        <div class="panel-body">
                            <asp:HiddenField ID="hfSupervisorId" runat="server" />

                            <div class="row">
                                <div class="col-2">Login ID</div>
                                <div class="col-7">
                                    <asp:TextBox ID="txtSearchString" runat="server" placeholder="Search with User Name or Login ID" CssClass="TextBoxStyle"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoComplete_User" runat="server"
                                        ServiceMethod="GetEmployeeInfo"
                                        ServicePath="~/AutoServices.asmx"
                                        TargetControlID="txtSearchString"
                                        EnableCaching="false"
                                        MinimumPrefixLength="2"
                                        CompletionInterval="100"
                                        CompletionSetCount="10"
                                        FirstRowSelected="false"
                                        CompletionListCssClass="AutoExtender"
                                        CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-3" align="right">
                                    <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Search" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-2">Name</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtName" Enabled="false" placeholder="User Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Designation</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtDesignation" Enabled="false" placeholder="Designation" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Company</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtCompany" Enabled="false" placeholder="Company" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Department</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtDepartment" Enabled="false" placeholder="Department" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 20px"></div>
                    <div class="panel panel-success">
                        <div class="panel-header">Search - Team Member</div>
                        <div class="panel-body">
                            <asp:HiddenField ID="hfMemberId" runat="server" />

                            <div class="row">
                                <div class="col-2">Login ID</div>
                                <div class="col-7">
                                    <asp:TextBox ID="txtCopyLoginId" runat="server" placeholder="Search with User Name or Login ID" CssClass="TextBoxStyle"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoComplete_CopyUser" runat="server"
                                        ServiceMethod="GetEmployeeInfo"
                                        ServicePath="~/AutoServices.asmx"
                                        TargetControlID="txtCopyLoginId"
                                        EnableCaching="false"
                                        MinimumPrefixLength="2"
                                        CompletionInterval="100"
                                        CompletionSetCount="10"
                                        FirstRowSelected="false"
                                        CompletionListCssClass="AutoExtender"
                                        CompletionListItemCssClass="AutoExtenderList"
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight">
                                    </asp:AutoCompleteExtender>
                                </div>
                                <div class="col-3" align="right">
                                    <asp:Button runat="server" ID="btnCopyRefresh" OnClick="btnCopyRefresh_Click" Text="Search" />
                                    <asp:Button runat="server" ID="btnCopyClear" OnClick="btnCopyClear_Click" Text="Clear" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-2">Name</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtCopyName" Enabled="false" placeholder="User Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Designation</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtCopyDesignation" Enabled="false" placeholder="Designation" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Company</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtCopyCompany" Enabled="false" placeholder="Company" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-2">Department</div>
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtCopyDepartment" Enabled="false" placeholder="Department" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12" style="margin-top: 15px; text-align: center">
                                    <asp:Button runat="server" ID="btnSaveTeamMember" OnClick="btnSaveTeamMembern_Click" Text="Add as Team Member" CssClass="btn btn-md btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-8">
                    <div class="panel panel-info">
                        <div class="panel-header">Supervisor & Team Member Mapping</div>
                        <div class="panel-body">

                            <asp:GridView runat="server" ID="gvTeamMember" OnRowCommand="gvTeamMember_RowCommand"
                                AutoGenerateColumns="false" ShowFooter="False" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL." ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                            <asp:HiddenField ID="hfMemberId" runat="server" Value='<%#Eval("MemberId") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="Name" />
                                    <asp:BoundField DataField="ContactNo" HeaderText="Contact" />
                                    <asp:BoundField DataField="Designation" HeaderText="Designation" />
                                    <asp:BoundField DataField="Department" HeaderText="Department" />
                                    <asp:BoundField DataField="Company" HeaderText="Company" />
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <span onclick="return confirm('Are you sure want to delete?')">
                                                <asp:LinkButton ID="btnDelete" Text="Remove" runat="server" CommandName="Delete" />
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Role found for the Search Criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
<%--        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvTeamMember" />
        </Triggers>--%>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="updateProgress" runat="server">
        <ProgressTemplate>
            <div class="loading-panel">
                <div class="loading-container">
                    <center>
                        <div style="background-color: white; height: 120px; width: 400px; padding-top: 50px;" class="border border-info rounded-5">
                            <span>Processing, Please wait a moment...</span>
                            <br />
                            <img src="<%= this.ResolveUrl("~/Images/loading-logo.gif")%>" width="350px" alt="Please wait..." />
                        </div>
                    </center>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
