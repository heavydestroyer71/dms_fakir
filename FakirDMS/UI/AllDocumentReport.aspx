<%@ Page Title="Bill Status Report | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AllDocumentReport.aspx.cs" Inherits="FakirDMS.UI.AllDocumentReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <style>
        #gridContainer {
        max-height: 400px;
        overflow-y: auto;
        position: relative;
        }

        /* Hide original header to avoid duplication */
        .styledGridView thead {
            visibility: hidden;
        }

        /* Style for the fixed cloned header */
        .fixedHeader {
            display: table;
            width: 100%;
            position: sticky;
            top: 0;
            background-color: #f1f1f1;
            z-index: 10;
            box-shadow: 0 2px 2px -1px rgba(0, 0, 0, 0.4);
        }

        /* Additional styling for header row */
        .GridViewHeader th {
            background-color: #e9e9e9;
            font-weight: bold;
        }
    </style>
    <script>
		document.addEventListener("DOMContentLoaded", function () {
			const gridContainer = document.getElementById("gridContainer");
			const gridView = document.querySelector(".styledGridView");
			const originalHeader = gridView.querySelector("thead");

			// Proceed only if the GridView has data rows
			if (originalHeader && gridView.querySelector("tbody tr")) {
				// Clone the header row
				const clonedHeader = originalHeader.cloneNode(true);
				clonedHeader.classList.add("fixedHeader");
				gridContainer.insertBefore(clonedHeader, gridContainer.firstChild);
			}
		});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <%--<asp:UpdatePanel runat="server" ID="uppdatePanel">
        <ContentTemplate>--%>

            <div class="panel panel-info">
                <div class="panel-header">Document - Search Criteria</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-1">Company</div>
                        <div class="col-2">
                            <asp:DropDownList runat="server" ID="ddlCompany" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Category</div>
                        <div class="col-5">
                            <asp:DropDownList runat="server" ID="ddlCategory" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Status</div>
                        <div class="col-2">
                        <asp:DropDownList runat="server" ID="ddlStatus" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        
                        
                        
                    </div>

                    <div class="row">

                    <div class="col-1">Supplier</div>
                    <div class="col-2">
                    <asp:TextBox runat="server" ID="txtSupplier" placeholder="Suppler Name" CssClass="TextBoxStyle"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoComplete_Supplier" runat="server"
                    ServiceMethod="GetSupplierInfo"
                    ServicePath="~/AutoServices.asmx"
                    TargetControlID="txtSupplier"
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

                    <div class="col-1">Expense Type</div>
                    <div class="col-5">
                    <asp:DropDownList runat="server" ID="ddlExpense" CssClass="DropDownListStyle"></asp:DropDownList>
                    </div>

                    <div  class="col-1">Tracking No</div>
                    <div class="col-2">
                    <asp:TextBox ID="txtRefNo" runat="server" placeholder="Search with tracking number" CssClass="TextBoxStyle"></asp:TextBox>
                    </div>

                    </div>



                    <div class="row">
                        
                        <div class="col-1">Search With</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtSearchWith" placeholder="Search with PO, LC, MRR and Challan" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>

                        <div class="col-1">
                            My Document
                        </div>

                                               
                        <div class="col-5">
                            <div class="row">

                                <div class="col-2">
                                <asp:CheckBox runat="server" ID="cbIsForwarded" CssClass="CheckBoxStyle" />
                                </div>
                               
                                <div class="col-2">
                                From Date
                                </div>
                                <div class="col-3 calendarContainer">
                                <asp:TextBox runat="server" ID="txtFromDate" placeholder="MM/DD/YYYY" CssClass="TextBoxStyle"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Validator_txtFromDate" runat="server"
                                ControlToValidate="txtFromDate" Display="Dynamic" ForeColor="Red" ValidationExpression="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"
                                ErrorMessage=" Date Format must be Month/Day/Year."></asp:RegularExpressionValidator>
                                <asp:MaskedEditExtender ID="Masked_txtFromDate" runat="server"
                                Enabled="True" Mask="99/99/9999" MaskType="Date"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtFromDate">
                                </asp:MaskedEditExtender>
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" CssClass="CalenderTheme"
                                PopupButtonID="txtFromDate" TargetControlID="txtFromDate" Format="MM/dd/yyyy">
                                </asp:CalendarExtender>
                                </div>
                                <div class="col-2">
                                To Date
                                </div>
                                <div class="col-3 calendarContainer">
                                <asp:TextBox runat="server" ID="txtToDate" placeholder="MM/DD/YYYY" CssClass="TextBoxStyle"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="Validator_txtToDate" runat="server"
                                ControlToValidate="txtToDate" Display="Dynamic" ForeColor="Red" ValidationExpression="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"
                                ErrorMessage=" Date Format must be Month/Day/Year."></asp:RegularExpressionValidator>
                                <asp:MaskedEditExtender ID="Masked_txtToDate" runat="server"
                                Enabled="True" Mask="99/99/9999" MaskType="Date"
                                OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtToDate">
                                </asp:MaskedEditExtender>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="CalenderTheme"
                                PopupButtonID="txtToDate" TargetControlID="txtToDate" Format="MM/dd/yyyy">
                                </asp:CalendarExtender>
                                </div>
                                </div>
                        </div>
                         
                       
                         <div class="col-1">Action</div>
                       
                        <div class="col-2">
                        <div class="row">
                       
                       
                        <div class="col-3">
                        <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="bg-gradient-primary" OnClick="btnSearch_Click" />
                        </div>

                        <div class="col-3">
                        <asp:Button runat="server" ID="btnReload" Text="Reload" CssClass="bg-gradient-success" OnClick="btnReload_Click" />
                        </div>

                        <div class="col-6">
                        <asp:Button runat="server" ID="btnExcel" Text="Excel Preview" CssClass="bg-gradient-secondary" OnClick="btnExcel_Click" />
                        </div>

                        </div>
                        </div>
                   
                        

                    </div>
                </div>
            </div>

            <div style="margin-top: 20px"></div>
            <div class="panel panel-info">
                <div class="panel-header">Workflow Documents (<asp:Label ID="lblWorkflowCount" runat="server" Text="0"></asp:Label>)</div>
                <div class="panel-body">
                        <asp:GridView ID="gvWorkflowDocuments" runat="server"
                        OnRowCommand="gvWorkflowDocuments_RowCommand"
                        OnPageIndexChanging="gvWorkflowDocuments_PageIndexChanging"
                        AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="styledGridView"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="TranID" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn">
                                <ItemTemplate>
                                    <asp:Label ID="lblTranID" runat="server" Text='<%#Eval("DocumentID") %>'></asp:Label>
                                    <asp:Label ID="lblBillRefNo" runat="server" Text='<%#Eval("BillRefNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tracking No.">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblReturnPath" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' ToolTip='<%#Eval("BillRefNo") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ref Tracking">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblRefReturnPath" runat="server" NavigateUrl='<%#Eval("RefDocumentPath") %>' Text='<%#Eval("RefDocumentNo") %>' ToolTip='<%#Eval("RefDocumentNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Company"  HeaderText="Company" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
                            <asp:BoundField DataField="SupplierName" HeaderText="Supplier" />
                            <asp:BoundField DataField="PoNo" HeaderText="PO No." />
                            <asp:BoundField DataField="PiNo" HeaderText="PI No." />
                            <asp:BoundField DataField="LcNo" HeaderText="LC No." />
                            <asp:BoundField DataField="MrrNo" HeaderText="MRR No." />
                            <asp:BoundField DataField="ReqNo" HeaderText="Req. No." />

                            <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Assignee" HeaderText="Assignee" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="CurrentUser" HeaderText="User" />
                            <asp:BoundField DataField="RoleName" HeaderText="Flow Name" />
                            <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" HeaderStyle-Width="95px" />
                            <asp:BoundField DataField="LastModified" HeaderText="Last Update" HeaderStyle-Width="95px" />
                            <asp:BoundField DataField="Waiting" HeaderText="Waiting" HeaderStyle-Width="80px" />
                            <asp:TemplateField HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="sbtnViewHistory" runat="server"  CommandName="ViewHistory" Width="40px" ImageUrl="~/assets/img/history.png" ToolTip="Click here to view document movement history" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No workflow document found for the search criteria.
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="GridViewHeader" />
                        <FooterStyle CssClass="GridViewFooterStyle" />
                        <PagerStyle CssClass="GridViewPagerStyle" />
                        <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                    </asp:GridView>
                        
                </div>
            </div>


            <asp:LinkButton Text="" ID="lnkFake" runat="server" />
            <asp:ModalPopupExtender runat="server" ID="modalExtenderHistory" PopupControlID="PanelDetails" TargetControlID="lnkFake"
                PopupDragHandleControlID="PopupDetailsHeader" Drag="true" BackgroundCssClass="ModalPopupBG">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="PanelDetails" Style="display: none;">
                <div class="HellowWorldPopup">
                    <div class="PopupHeader" id="PopupDetailsHeader">
                        <div style="width: 100%; display: table;">
                            <div style="min-width: 550px; display: table-cell;">
                                <span style="font-weight: bold; font-size: larger; padding-left: 10px;">Movement History</span>
                            </div>
                            <div style="display: table-cell; vertical-align: middle;" align="right">
                                <asp:ImageButton ID="ImageButton2" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
                            </div>
                        </div>
                    </div>
                    <div class="PopupBody">
                        <div width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%; padding: 10px;" cellpadding="0" cellspacing="0">
                            <asp:GridView ID="gvHistory" runat="server" Width="100%"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>

       <%-- </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvWorkflowDocuments" />
            <asp:AsyncPostBackTrigger ControlID="btnExcel" />

            
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
