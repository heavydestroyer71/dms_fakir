<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="FakirDMS.Home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <link href="../assets/css/dashboard.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <div class="row" id="dashboard_all" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #00C0EF; border-bottom: 2px solid #00C0EF;">
                <span class="info-box-icon" style="background-color: #00C0EF; color: #ffffff;">
                    <i class="fa fa-floppy-o" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Parking Status</span>
                    <div class="row">
                        <div class="col-4">Total<asp:LinkButton runat="server" ID="txtParkingTotal" Text="0" ForeColor="#00C0EF" class="info-box-number"></asp:LinkButton></div>
                        <div class="col-4">On-Time<asp:LinkButton runat="server" ID="txtParkingOntime" Text="0" ForeColor="#00C0EF" class="info-box-number"></asp:LinkButton></div>
                        <div class="col-4">Delay<asp:LinkButton runat="server" ID="txtParkingDelay" Text="0" ForeColor="#00C0EF" class="info-box-number"></asp:LinkButton></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #f48449; border-bottom: 2px solid #f48449;">
                <span class="info-box-icon" style="background-color: #f48449; color: white;">
                    <i class="fa fa-paper-plane-o" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Posting Status</span>
                    <div class="row">
                        <div class="col-4">Total<asp:LinkButton runat="server" ID="txtPostingTotal" Text="0" ForeColor="#f48449" class="info-box-number"></asp:LinkButton></div>
                        <div class="col-4">On-Time<asp:LinkButton runat="server" ID="txtPostingOntime" Text="0" ForeColor="#f48449" class="info-box-number"></asp:LinkButton></div>
                        <div class="col-4">Delay<asp:LinkButton runat="server" ID="txtPostingDelay" Text="0" ForeColor="#f48449" class="info-box-number"></asp:LinkButton></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #32c07a; border-bottom: 2px solid #32c07a;">
                <span class="info-box-icon" style="background-color: #32c07a; color: white;">
                    <i class="fa fa-thumbs-o-up" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Closing Status</span>
                    <div class="row">
                        <div class="col-4">Total<asp:LinkButton runat="server" ID="txtClosingTotal" Text="0" ForeColor="#32c07a" class="info-box-number"></asp:LinkButton></div>
                        <div class="col-4">On-Time<asp:LinkButton runat="server" ID="txtClosingOntime" Text="0" ForeColor="#32c07a" class="info-box-number"></asp:LinkButton></div>
                        <div class="col-4">Delay<asp:LinkButton runat="server" ID="txtClosingDelay" Text="0" ForeColor="#32c07a" class="info-box-number"></asp:LinkButton></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #ff3232; border-bottom: 2px solid #ff3232;">
                <span class="info-box-icon" style="background-color: #ff3232; color: white;">
                    <i class="fa fa-trash-o" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Allocation Status</span>
                    <div class="row">
                        <div class="col-4">Total<asp:LinkButton runat="server" ID="txtAllocationTotal" Text="20" ForeColor="#ff3232" class="info-box-number"></asp:LinkButton></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" id="dashboard_status" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #00C0EF; border-bottom: 2px solid #00C0EF;">
                <span class="info-box-icon" style="background-color: #00C0EF; color: #ffffff;">
                    <i class="fa fa-floppy-o" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Own Draft Document</span>
                    <asp:LinkButton runat="server" ID="lbtnOwnDrafted" OnClick="lbtnOwnDrafted_Click" ForeColor="#00C0EF" class="info-box-number"></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #f48449; border-bottom: 2px solid #f48449;">
                <span class="info-box-icon" style="background-color: #f48449; color: white;">
                    <i class="fa fa-paper-plane-o" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Own Submited Document</span>
                    <asp:LinkButton runat="server" ID="lbtnOwnSubmitted" OnClick="lbtnOwnSubmitted_Click" ForeColor="#f48449" class="info-box-number"></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #32c07a; border-bottom: 2px solid #32c07a;">
                <span class="info-box-icon" style="background-color: #32c07a; color: white;">
                    <i class="fa fa-thumbs-o-up" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Own Approved Document</span>
                    <asp:LinkButton runat="server" ID="lbtnOwnApproved" OnClick="lbtnOwnApproved_Click" ForeColor="#32c07a" class="info-box-number"></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-3">
            <div class="info-box" style="border: 1px solid #ff3232; border-bottom: 2px solid #ff3232;">
                <span class="info-box-icon" style="background-color: #ff3232; color: white;">
                    <i class="fa fa-trash-o" aria-hidden="true"></i></span>
                <div class="info-box-content">
                    <span class="info-box-text">Own Rejected Document</span>
                    <asp:LinkButton runat="server" ID="lbtnOwnRejected" OnClick="lbtnOwnRejected_Click" ForeColor="#ff3232" class="info-box-number"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="dashboard-arrow row" id="dashboard_total" runat="server" visible="false">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="cmn">
                <div class="top-title background-b">
                    Total Draft Document
                </div>
                <div class="stat b">
                    <div class="icon">
                        <div class="icon-circle b"><i class="fa fa-2x fa-user"></i></div>
                    </div>
                    <div class="number b">
                        Total : <span runat="server" id="spanTotalDrafted"></span>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="cmn">
                <div class="top-title background-c">
                    Total Submited Document
                </div>
                <div class="stat c">
                    <div class="icon">
                        <div class="icon-circle c"><i class="fa fa-2x fa-comments"></i></div>
                    </div>
                    <div class="number c">
                        Total : <span runat="server" id="spanTotalSubmitted"></span>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="cmn">
                <div class="top-title background-a">
                    Total Approved Document			
                </div>
                <div class="stat a">
                    <div class="icon">
                        <div class="icon-circle a"><i class="fa fa-2x fa-check"></i></div>
                    </div>
                    <div class="number a">
                        Total : <span runat="server" id="spanTotalApproved"></span>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="cmn">
                <div class="top-title background-d">
                    Total Rejected Document
                </div>
                <div class="stat d">
                    <div class="icon">
                        <div class="icon-circle d"><i class="fa fa-2x fa-trash"></i></div>
                    </div>
                    <div class="number d">
                        Total : <span runat="server" id="spanTotalRejected"></span>
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="header-style">
                        Own Document
                    </div>
                    <div style="overflow-x: auto">
                        <asp:GridView ID="gvOwnDocuments" runat="server"
                            AutoGenerateColumns="false" Width="100%"
                            ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                            BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                            <Columns>
                                <asp:TemplateField HeaderText="Tracking No.">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="ghlTracking" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Company" HeaderText="Company" />
                                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                            <EmptyDataTemplate>
                                Sorry! No own document has been found.
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridViewHeader" />
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                        </asp:GridView>
                    </div>

                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="header-style">
                        Workflow Document
                    </div>
                    <div style="overflow-x: auto">
                        <asp:GridView ID="gvWorkflowDocument" runat="server"
                            AutoGenerateColumns="false" Width="100%"
                            ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                            BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                            <Columns>
                                <asp:TemplateField HeaderText="Tracking No.">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="ghlTracking" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Company" HeaderText="Company" />
                                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                            <EmptyDataTemplate>
                                Sorry! No workflow document has been found.
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridViewHeader" />
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
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
