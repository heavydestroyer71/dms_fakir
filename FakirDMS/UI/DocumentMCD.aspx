<%@ Page Title="Document | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" Async="true" AutoEventWireup="true" CodeBehind="DocumentMCD.aspx.cs" Inherits="FakirDMS.UI.DocumentMCD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfCategoryId" />
            <asp:HiddenField runat="server" ID="hfDocumentId" Value="0" />
            <asp:HiddenField runat="server" ID="hfIsApprover" Value="0" />
            <asp:HiddenField runat="server" ID="hfSubmitCount" Value="0" />
            <asp:HiddenField runat="server" ID="hfIsPoRequired" Value="0" />

            <%--General Information--%>
            <div class="panel panel-info">
                <div class="panel-header">General Information (<span runat="server" id="divPageHeader"></span>)</div>
                <div class="panel-body">
                    <asp:HiddenField runat="server" ID="hfRefDocumentID" />
                    <div class="row">
                        <div class="col-1">Company <span style="color: red">*</span></div>
                        <div class="col-2">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-1">Expense Type <span style="color: red">*</span></div>
                        <div class="col-2">
                            <asp:DropDownList ID="ddlExpenseType" runat="server" CssClass="DropDownListStyle"></asp:DropDownList></td>
                        </div>
                        <div class="col-1">Entry Date</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtEntryDate" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Tracking No.</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtBillRefNo" runat="server" Enabled="False" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1">Remarks</div>
                        <div class="col-9">
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                       <%-- <div class="col-1">Ref# Tracking.</div>
                        <div class="col-2">
                            <asp:TextBox ID="txtRefTracking" runat="server" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-2">
                            <span style="vertical-align: text-bottom !important">
                                <asp:Button runat="server" ID="btnLoadRefNo" OnClick="btnLoadRefNo_Click" Text="..." CssClass="btn btn-xs btn-outline-info" Style="padding: 0px; width: 30px;" />
                            </span>
                        </div>--%>
                        <div class="col-2">
                            <asp:Button runat="server" ID="btnCreate" OnClick="btnCreate_Click" CssClass="btn btn-sm btn-primary" Text="Create" />
                        </div>
                       
                    </div>
                </div>
            </div>

            <%--Purchase Order & pi Information--%>
            <div class="row" style="margin-top: 15px">
                <%--Purchase Order Information--%>
                <div class="col-6 scrollable">

                    <asp:GridView ID="gvPurchaseOrder" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:TemplateField HeaderText="Purchase Order" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="gHlPOPath" runat="server" NavigateUrl='<%#Eval("ReportPath") %>' Text='<%#Eval("RefNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Supplier" DataField="SupplierName" />
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" ID="glbShowPoPopup" OnClick="glbShowPoPopup_Click" CssClass="btn btn-success btn-xs fa fa-plus"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="glbPODelete" OnClick="glbPODelete_Click" CssClass="btn btn-danger btn-xs fa fa-trash-o"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                </div>

                <%--Proforma Invoice Information--%>
                <div class="col-6 scrollable">
                    <asp:GridView ID="gvProformaInvoice" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:TemplateField HeaderText="Proforma Invoice" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="gHlPIPath" runat="server" NavigateUrl='<%#Eval("ReportPath") %>' Text='<%#Eval("RefNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" ID="glbShowPiPopup" OnClick="glbShowPiPopup_Click" CssClass="btn btn-success btn-xs fa fa-plus"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="glbPIDelete" OnClick="glbPIDelete_Click" CssClass="btn btn-danger btn-xs fa fa-trash-o"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                </div>
            </div>

            <%--LC, Challan & MRR Information--%>
            <div class="row" style="margin-top: 15px">
                <%--LC Information--%>
                <div class="col-4 scrollable">
                    <asp:GridView ID="gvLC" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:TemplateField HeaderText="BTB LC" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="gHlLCPath" runat="server" NavigateUrl='<%#Eval("ReportPath") %>' Text='<%#Eval("RefNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" ID="glbShowLcPopup" OnClick="glbShowLcPopup_Click" CssClass="btn btn-success btn-xs fa fa-plus"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="glbLCDelete" OnClick="glbLCDelete_Click" CssClass="btn btn-danger btn-xs fa fa-trash-o"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                </div>
                <%--MRR Information--%>
                <div class="col-8 scrollable">
                   <div class="row">
                   <div >
                    <asp:GridView ID="gvMRR" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:TemplateField HeaderText="MRR No." ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="gHlMRRPath" runat="server" NavigateUrl='<%#Eval("ReportPath") %>' Text='<%#Eval("RefNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="MRR Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:BoundField HeaderText="Challan No." DataField="ChallanNo" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:BoundField HeaderText="Challan Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:BoundField HeaderText="MRR Amount" DataField="MrrAmt" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" ID="glbShowMrrPopup" OnClick="glbShowMrrPopup_Click" CssClass="btn btn-success btn-xs fa fa-plus"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="glbMrrDelete" OnClick="glbMrrDelete_Click" CssClass="btn btn-danger btn-xs fa fa-trash-o"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                    </div>
                    </div>

                    <div class="row" >
                   <%-- <div class="col-8" >--%>
                     <div class="col-7"> </div>
                    <div class="col-2">Total MRR Amount: </div> 
                    <div class="col-3">
                    <asp:TextBox ID="txtTotalMrrAmt" runat="server" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                    </div>
                   <%-- </div>--%>
                    </div>

                </div>
                
                <%--Challan Information--%>
               <%-- <div class="col-4 scrollable">
                    <asp:GridView ID="gvChallan" runat="server" Width="100%" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="RowId" DataField="RowId" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:BoundField HeaderText="Id" DataField="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                            <asp:TemplateField HeaderText="Challan" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="gHlMRRPath" runat="server" NavigateUrl='<%#Eval("ReportPath") %>' Text='<%#Eval("RefNo") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Date" DataField="Date" ItemStyle-Width="150px" HeaderStyle-Width="150px" />
                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:LinkButton runat="server" ID="glbShowChallanPopup" OnClick="glbShowChallanPopup_Click" CssClass="btn btn-success btn-xs fa fa-plus"></asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="glbChallanDelete" OnClick="glbChallanDelete_Click" CssClass="btn btn-danger btn-xs fa fa-trash-o"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridViewHeader" />
                    </asp:GridView>
                </div>--%>
            </div>

            <%--Bill Information--%>
            <div runat="server" class="panel panel-info" id="divBillInfo" style="margin-top: 15px">
                <div class="panel-header">Party Bill Information</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-1">Bill No</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtBillNo" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Bill Amount</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtBillAmount" OnTextChanged="txtBillAmount_TextChanged" AutoPostBack="true" onkeypress="return CheckNumericOnly(this);" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Discount Amount</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtDiscountAmount" OnTextChanged="txtDiscountAmount_TextChanged" AutoPostBack="true" onkeypress="return CheckNumericOnly(this);" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Final Amount</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtFinalAmount" Enabled="false" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1">
                            Bill Date
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtBillDate" CssClass="TextBoxStyle"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender_txtSalesDate" runat="server" CssClass="CalenderTheme"
                                PopupButtonID="txtBillDate" TargetControlID="txtBillDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </div>
                        <div class="col-1">
                            VAT Amount
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVatAmount" OnTextChanged="txtVatAmount_TextChanged" AutoPostBack="true"  onkeypress="return CheckNumericOnly(this);" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            VAT Challan
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVATChallanNo" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            VAT Date
                        </div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVATDate" CssClass="TextBoxStyle"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender_txtVATDate" runat="server" CssClass="CalenderTheme"
                                PopupButtonID="txtVATDate" TargetControlID="txtVATDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </div>
                    </div>
                </div>
            </div>
            
            <%--Account Information--%>
            <div runat="server" id="divAccountInfo" class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Accounts Information</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-1">Voucher No.</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVoucherNo" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-1">Voucher Date</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtVoucherDate" CssClass="TextBoxStyle"></asp:TextBox>
                            <asp:CalendarExtender ID="CalExtVoucherDate" runat="server" CssClass="CalenderTheme"
                                PopupButtonID="txtVoucherDate" TargetControlID="txtVoucherDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </div>
                        <div class="col-1">Payment Amount</div>
                        <div class="col-2">
                            <asp:TextBox runat="server" ID="txtPaymentAmount" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-"></div>
                        <div class="col-2">
                            <asp:CheckBox runat="server" ID="cbBillClosed" Text="Bill Close" CssClass="CheckBoxStyle" />
                        </div>
                    </div>
                </div>
            </div>

            <%--Attachemnt Uploader--%>
            <div runat="server" id="divUploader" class="panel panel-info" style="margin-top: 15px">
                <div class="panel-header">Attachment Uploader</div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-1">Type</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
                        </div>
                        <div class="col-sm-8">
                            <asp:TextBox runat="server" ID="txtFileRemarks" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-1">Select File</div>
                        <div class="col-sm-9">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <div class="col-sm-2" align="right">
                            <asp:Button ID="btnUploadDocument" runat="server" OnClick="btnUploadDocument_Click" Text="Upload Attachment" />
                        </div>
                    </div>
                </div>
            </div>

            <%--Raise User Button Panel--%>
            <div style="margin-top: 15px"></div>
            <div runat="server" id="divButtonPrepare" class="panel panel-info">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-12" align="center">
                            <asp:Button runat="server" ID="btnSaveDocument" OnClick="btnSaveDocument_Click" Text="Save" CssClass="btn btn-primary" />
                           
                            <a  href="javascript://" onclick="javascript:window.open('../UI/PrintPreview.aspx?DocumentID=<%=hfDocumentId.Value%>',null, 'width=800,height=700,left=300,top=10,scrollbars=0,menubar=0,location=0,directories=0,resizable=0');return false">
                            <i class="btn btn-info" >Report Priview</i>
                            </a>

                            <asp:Button runat="server" ID="btnSubmitDocument" OnClick="btnSubmitDocument_Click" Text="Submit" CssClass="btn btn-success" />
                            <asp:Button runat="server" ID="btnDefault" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>

            <%--Flowpath User Button Panel--%>
            <div runat="server" id="divButtonApprove" class="panel panel-success">
                <div class="panel-header">Movement Process</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-1">Recommendation</div>
                        <div class="col-sm-5">
                            <asp:TextBox runat="server" ID="txtRemarksBoss" TextMode="MultiLine" Rows="2" CssClass="TextBoxStyle"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <asp:DropDownList runat="server" ID="ddlRevertTo" Style="width: 200px; height: 38px; font-size: 1rem; padding: 0.375rem 0.75rem;"></asp:DropDownList>
                            <%--<asp:Button runat="server" ID="btnWorkflowSave" OnClick="btnSaveDocument_Click" Text="Save" CssClass="btn btn-primary" />--%>
                            <asp:Button runat="server" ID="btnWorkflowReject" OnClick="btnWorkflowReject_Click" Text="Reject" CssClass="btn btn-warning" />
                            <asp:Button runat="server" ID="btnWorkflowForward" OnClick="btnWorkflowForward_Click" Text="Submit" CssClass="btn btn-success" OnClientClick="if (!confirm('Are you sure you want Submit/Approve?')) return false;" />

                            <asp:Button runat="server" ID="btnWorkflowDecline" OnClick="btnWorkflowDecline_Click" Text="Delete" CssClass="btn btn-danger" />
                            <asp:Button runat="server" ID="btnWorkflowBackToList" OnClick="btnWorkflowBackToList_Click" Text="Back to List" CssClass="btn btn-secondary" />
                        </div>
                    </div>
                </div>
            </div>

            <%--Comments and Attachment Preview--%>
            <div class="row" style="margin-top: 15px">
                <div class="col-6">
                    <%--Comments Regarding The Documents--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Comments</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvComment" runat="server" Width="100%" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-Width="95px" HeaderStyle-Width="95px" />
                                    <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                    <asp:BoundField DataField="User" HeaderText="User" ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                                    <asp:BoundField DataField="Action" HeaderText="Status" ItemStyle-Width="120px" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="Waiting" HeaderText="Waiting Hour" ItemStyle-Width="80px" HeaderStyle-Width="80px" />
                                </Columns>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-6">
                    <%--Documents Preview--%>
                    <div class="panel panel-info">
                        <div class="panel-header">Attachments</div>
                        <div class="panel-body">
                            <asp:GridView ID="gvAttachment" runat="server" Width="100%"
                                AutoGenerateColumns="false" BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle">
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <Columns>
                                    <asp:BoundField DataField="TransID" HeaderText="File" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="ContentType" HeaderText="ContentType" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                    <asp:BoundField DataField="DocumentTypeName" HeaderText="Document Type" />
                                    <asp:BoundField DataField="Title" HeaderText="File Name" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Upload By" />
                                    <asp:BoundField DataField="EntryDate" HeaderText="Upload Date" />
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="gBtnDownload" OnClick="gBtnDownload_Click" ToolTip="Click here to download" CssClass="btn btn-success btn-xs fa fa-cloud-download"></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="gBtnPreview" OnClick="gBtnPreview_Click" ToolTip="Click here to view" CssClass="btn btn-primary btn-xs fa fa-print"></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="gBtnDelete" OnClick="gBtnDelete_Click" ToolTip="Click here to delete" CssClass="btn btn-danger btn-xs fa fa-trash-o"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No attachemnt file found for the search criteria.
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="GridViewHeader" />
                                <FooterStyle CssClass="GridViewFooterStyle" />
                                <PagerStyle CssClass="GridViewPagerStyle" />
                                <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <%--Details Related Popup Panel--%>
            <asp:LinkButton runat="server" ID="lnkFakeDetails" />
            <asp:ModalPopupExtender runat="server" ID="modalExtenderDetails" PopupControlID="PanelDetails" TargetControlID="lnkFakeDetails"
                PopupDragHandleControlID="PopupDetailsHeader" Drag="true" BackgroundCssClass="ModalPopupBG" Y="100">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="PanelDetails" Style="display: none;">
                <div class="ModalPopupStyle">
                    <div class="PopupHeader" id="PopupDetailsHeader">
                        <div style="width: 100%; display: table;">
                            <div style="min-width: 550px; display: table-cell; vertical-align: middle;">
                                <span runat="server" id="PanelHeaderName" style="font-weight: bold; font-size: larger; padding-left: 10px;"></span>
                            </div>
                            <div style="display: table-cell; vertical-align: middle;" align="right">
                                <asp:ImageButton ID="ImageButton2" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
                            </div>
                        </div>
                    </div>
                    <div class="PopupBody">
                        <div width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%; padding: 10px;" cellpadding="0" cellspacing="0">
                            <div runat="server" id="divTextBoxDetails" class="row">
                                <div class="col-sm-2 col-md-2">
                                    <asp:HiddenField runat="server" ID="phfType" Value="" />
                                    Search
                                </div>
                                <div class="col-sm-6 col-md-6">
                                    <asp:TextBox runat="server" ID="ptxtSearchWith" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-sm-1 col-md-1">
                                    <asp:LinkButton runat="server" ID="plbSearchDetails" OnClick="plbSearchDetails_OnClick" CssClass="btn btn-primary btn-sm fa fa-search"></asp:LinkButton>
                                </div>
                                <div class="col-sm-3 col-md-3">
                                 <asp:CheckBox runat="server" ID="cbIsCategory" Text="Expense Based?" CssClass="CheckBoxStyle" Checked="true" />
                                </div>
                                <hr />
                            </div>
                            <div class="row">
                                <div class="col-sm-12 scrollable">
                                    <asp:GridView runat="server" ID="gvDetails"
                                        AutoGenerateColumns="False" Width="100%"
                                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="gvCbSelect" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="HideGridColumn" ItemStyle-CssClass="HideGridColumn" />
                                            <asp:TemplateField HeaderText="Ref No">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="gHlReportPath" runat="server" NavigateUrl='<%#Eval("ReportPath") %>' Text='<%#Eval("RefNo") %>' Font-Underline="true" ToolTip='<%#Eval("RefNo") %>' Target="_blank"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Date" HeaderText="Date" />
                                             <asp:BoundField DataField="ChallanNo" HeaderText="Challan No." />
                                            <asp:BoundField DataField="Supplier" HeaderText="Supplier" />
                                            <asp:BoundField DataField="Category" HeaderText="Category"  />
                                            <asp:BoundField DataField="MrrAmt" HeaderText="MRR Amount" ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No data found for the search criteria.
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="GridViewHeader" />
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12" style="padding-top: 10px;" align="center">
                                    <asp:Button runat="server" ID="btnAddToTracking" OnClick="btnAddToTracking_Click" Text="Add Reference" CssClass="btn btn-sm btn-success" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>


            <%--Reference Document Panel--%>
            <asp:LinkButton runat="server" ID="lnkFakeRef" />
            <asp:ModalPopupExtender ID="modalExtenderRef" runat="server" PopupControlID="PopupRefDocument" TargetControlID="lnkFakeRef"
                PopupDragHandleControlID="PopupRefHeader" Drag="true" BackgroundCssClass="ModalPopupBG" Y="100">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="PopupRefDocument" Style="display: none">
                <div class="ModalPopupStyle">
                    <div class="PopupHeader" id="PopupRefHeader">
                        <div style="width: 100%; display: table;">
                            <div style="min-width: 400px; display: table-cell; vertical-align: middle;">
                                <span style="font-weight: bold; font-size: larger; padding-left: 10px;">Search - Document</span>
                            </div>
                            <div style="display: table-cell; vertical-align: middle;" align="right">
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="25px" ImageUrl="~/assets/img/close.png" />
                            </div>
                        </div>
                    </div>
                    <div class="PopupBody">
                        <table width="100%" style="border: Solid 2px aqua; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>Search:
                                    <asp:TextBox runat="server" ID="txtSearchRefNo"  placeholder="Enter tracking number" Width="220px"></asp:TextBox>
                                    <asp:Button runat="server" ID="btnSearchRefNo" Text="Search" OnClick="btnSearchRefNo_Click" CssClass="btn btn-primary btn-sm" />
                                    <asp:Button runat="server" ID="btnSearchClear" Text="Relolad" OnClick="btnSearchClear_Click" CssClass="btn btn-warning btn-sm" />
                                                             
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvDraftDocuments" runat="server"
                                        OnPageIndexChanging="gvDraftDocuments_PageIndexChanging"
                                        AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
                                        ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                        BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnSelectAsset" OnClick="btnSelectAsset_Click" CommandName='<%#Eval("DocumentID") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillRefNo")%>' Text="Select" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tracking No.">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lblTo" runat="server" NavigateUrl='<%#Eval("ReturnPath") %>' Text='<%#Eval("BillRefNo") %>' Target="_blank"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Company" HeaderText="Company" />
                                            <asp:BoundField DataField="ExpenseTypeName" HeaderText="Expense Type" />
                                            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                            <asp:BoundField DataField="RoleName" HeaderText="Role" />
                                            <asp:BoundField DataField="PONO" HeaderText="PO No." ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="LCNo" HeaderText="LC No." ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="MRRNo" HeaderText="MRR No." ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Own document found for the search criteria.
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="GridViewHeader" />
                                        <FooterStyle CssClass="GridViewFooterStyle" />
                                        <PagerStyle CssClass="GridViewPagerStyle" />
                                        <PagerSettings FirstPageText="First" NextPageText="Next" PreviousPageText="Prev" LastPageText="Last" Mode="NumericFirstLast" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadDocument" />
            <asp:PostBackTrigger ControlID="gvAttachment" />

            <asp:PostBackTrigger ControlID="gvPurchaseOrder" />
            <asp:PostBackTrigger ControlID="gvProformaInvoice" />
            <asp:PostBackTrigger ControlID="gvLC" />
            <asp:PostBackTrigger ControlID="gvMRR" />
           <%-- <asp:PostBackTrigger ControlID="gvChallan" />--%>
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
