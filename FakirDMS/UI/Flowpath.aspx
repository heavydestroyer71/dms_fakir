<%@ Page Title="Flowpath | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Flowpath.aspx.cs" Inherits="FakirDMS.UI.Flowpath" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">

	<style>
		fieldset {
			margin-top: 7px !important;
			border: solid 1px #bce8f1 !important;
			padding: 0px 5px;
		}

		legend {
			background-color: #d9edf7 !important;
			color: #2B7E8F !important;
			padding-left: 5px !important;
			font-size: 14px !important;
			width: 100%;
			max-width: 100%;
			margin-bottom: 0px;
		}

		.ssGridToggle tr td {
			padding: 3px !important;
		}
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div class="row">
				<div class="col-sm-5">
					<div class="panel panel-info">
						<div class="panel-header">Search Category</div>
						<div class="panel-body">

							<div class="row">
								<div class="col-2">Category</div>
								<div class="col-8">
									<asp:DropDownList ID="ddlCategory" runat="server" CssClass="DropDownListStyle"></asp:DropDownList>
								</div>
								<div class="col-2">
									<asp:Button ID="btnRefresh" runat="server" Text="Search" OnClick="btnRefresh_Click" Style="width: 100%" />
								</div>
							</div>

						</div>
					</div>

					<div class="panel panel-info" style="margin-top: 15px">
						<div class="panel-header">Create Flow</div>
						<div class="panel-body">
							<div class="row">
								<div class="col-sm-2">
									Flow <span style="color: red">*</span>
								</div>
								<div class="col-sm-6">
									<asp:HiddenField runat="server" ID="hfFlowId" />
									<asp:TextBox runat="server" ID="txtFlowName" placeholder="Flow Name" CssClass="TextBoxStyle"></asp:TextBox>
								</div>
								<div class="col-sm-2">
									Serial No <span style="color: red">*</span>
								</div>
								<div class="col-sm-2">
									<asp:TextBox runat="server" ID="txtSerialNo" placeholder="Serial No" CssClass="TextBoxStyle"></asp:TextBox>
								</div>
							</div>
							<div class="row">
								<div class="col-sm-2">Description</div>
								<div class="col-sm-6">
									<asp:TextBox runat="server" ID="txtDiscription" placeholder="Description" CssClass="TextBoxStyle"></asp:TextBox>
								</div>
								<div class="col-sm-2">TNA Days</div>
								<div class="col-sm-2">
									<asp:TextBox runat="server" ID="txtTnaDays" placeholder="TNA" CssClass="TextBoxStyle"></asp:TextBox>
								</div>
							</div>
							<fieldset>
								<legend>Details Related</legend>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbPOEnable" Text="PO Enable?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbPIEnable" Text="PI Enable?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbLCEnable" Text="LC Enable?" CssClass="CheckBoxStyle" />
									</div>
								</div>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbMREnable" Text="MRR Enable?" CssClass="CheckBoxStyle" />
									</div>
									<%--<div class="col-sm-4">
                                        <asp:CheckBox runat="server" ID="cbChallanEnable" Text="Challan Enable?" CssClass="CheckBoxStyle" />
                                    </div>--%>
								</div>
							</fieldset>
							<fieldset>
								<legend>Bill Related</legend>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbBillEnable" Text="Bill No. Enable?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbAmountEnable" Text="Amount Enable?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbDiscountEnable" Text="Discount Enable?" CssClass="CheckBoxStyle" />
									</div>
								</div>
							</fieldset>

							<fieldset><legend>Accounts Related</legend>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbAccountEnable" Text="Account Information" CssClass="CheckBoxStyle" /></div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbAccountPayableVarNo" Text="Payable Varchar No" CssClass="CheckBoxStyle" /></div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbAccountPayableVarDate" Text="Payable Varchar Date" CssClass="CheckBoxStyle" /></div>
								</div>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbAccountPaymentVarNo" Text="Payment Varchar No" CssClass="CheckBoxStyle" /></div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbAccountPaymentVarDate" Text="Payment Varchar Date" CssClass="CheckBoxStyle" /></div>
									<div class="col-sm-4"></div>
								</div>
							</fieldset>

							<fieldset>
								<legend>Attachment Related</legend>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbUpload" Text="Can Upload?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbDownload" Text="Can Download?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbDelete" Text="Can Delete?" CssClass="CheckBoxStyle" />
									</div>
								</div>
							</fieldset>
							<fieldset>
								<legend>Role Related</legend>
								<div class="row">
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbIsSupervisor" Text="Is Supervisor?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbIsTeamMember" Text="Is Team Member?" CssClass="CheckBoxStyle" />
									</div>
								</div>
							</fieldset>

							<fieldset>
								<legend>Flow Related</legend>
								<div class="row">

									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbIsApprover" Text="Is Approver?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbIsCloser" Text="Is Closer?" CssClass="CheckBoxStyle" />
									</div>
									<div class="col-sm-4">
										<asp:CheckBox runat="server" ID="cbIsActive" Text="Is Active?" Checked="true" CssClass="CheckBoxStyle" />
									</div>
								</div>
							</fieldset>
							<div class="row">
								<div class="col-12">
									<div style="margin-top: 20px; text-align: center">
										<asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" Width="150px" CssClass="btn btn-md btn-success" />
										<asp:Button runat="server" ID="btnClear" Text="Clear" OnClick="btnClear_Click" Width="150px" CssClass="btn btn-md btn-danger" />
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-7">

					<div class="panel panel-info">
						<div class="panel-header">Category Flow List</div>
						<div class="panel-body">

							<asp:GridView ID="gvCategoryFlow" runat="server"
								OnRowCommand="gvCategoryFlow_RowCommand"
								OnPageIndexChanging="gvCategoryFlow_PageIndexChanging"
								AutoGenerateColumns="false" AllowPaging="true" PageSize="10" Width="100%"
								ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
								BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
								<AlternatingRowStyle BackColor="WhiteSmoke" />
								<Columns>
									<asp:BoundField DataField="FlowId" HeaderText="FlowId" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
									<asp:BoundField DataField="SerialNo" HeaderText="SL" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right" />
									<asp:BoundField DataField="FlowName" HeaderText="Flow Name" />
									<asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="HideGridColumn" HeaderStyle-CssClass="HideGridColumn" />
									<asp:BoundField DataField="TnaDays" ItemStyle-HorizontalAlign="Center" HeaderText="TNA" />
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PO">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbPoEnable" Checked='<%#Eval("IsPO")%>' Enabled="false" ToolTip="Flow user can enter PO information." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PI">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbPiEnable" Checked='<%#Eval("IsPI")%>' Enabled="false" ToolTip="Flow user can enter PI information." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="LC">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbLcEnable" Checked='<%#Eval("IsLC")%>' Enabled="false" ToolTip="Flow user can enter LC information." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="MRR">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbMrEnable" Checked='<%#Eval("IsMR")%>' Enabled="false" ToolTip="Flow user can enter MRR information." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Challan">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbClEnable" Checked='<%#Eval("IsCL")%>' Enabled="false" ToolTip="Flow user can enter challan information." />
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Bill">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbBillEnable" Checked='<%#Eval("IsBill")%>' Enabled="false" ToolTip="Flow user can enter bill related information." />
										</ItemTemplate>
									</asp:TemplateField>
									      <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Payable Voucher no">
											  <ItemTemplate>
												<asp:CheckBox runat="server" ID="gcbIsPayableVarchardate" Checked='<%# Eval("IsAccountPayableVarDate") %>' Enabled="false" ToolTip="Flow user can enter bill related information." />


											  </ItemTemplate>
									      </asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="payment Voucher Date">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsPayableVarchardate" Checked='<%#Eval("IsAccountPayableVarDate")%>' Enabled="false" ToolTip="Flow user can enter bill related information." />
										</ItemTemplate>
									</asp:TemplateField>


							<%-- 	<asp:TemplateField ItemStyle-HorizontalAlign="Center"HeaderText="payment varchar Date">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsPaymentVarcharNo" Checked='<%# Eval("IspaymentVarNo") %>' Enabled="false" ToolTip="Flow user can enter bill related information." />

										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center"HeaderText="payment varchar Date">
										<ItemTemplate>
											<asp:CheckBox runat="server"ID="gcbIsPaymentVarchardate"Checked='<%#Eval("IsPaymentDate")%>'Enabled="false"ToolTip="Flow user can enter bill related information." />
										</ItemTemplate>
									</asp:TemplateField>
								--%>


									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="payment varchar Date">
    <ItemTemplate>
        <asp:CheckBox runat="server" ID="gcbIsPaymentVarcharNo" Checked='<%# Eval("IspaymentVarNo") %>' Enabled="false" ToolTip="Flow user can enter bill related information." />
    </ItemTemplate>
</asp:TemplateField>

<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="payment varchar Date">
    <ItemTemplate>
        <asp:CheckBox runat="server" ID="gcbIsPaymentVarchardate" Checked='<%# Eval("IsPaymentDate") %>' Enabled="false" ToolTip="Flow user can enter bill related information." />
    </ItemTemplate>
</asp:TemplateField>
									
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Amount">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbAmountEnable" Checked='<%#Eval("IsAmount")%>' Enabled="false" ToolTip="Flow user can enter bill amount information." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Discount">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbDiscountEnable" Checked='<%#Eval("IsDiscount")%>' Enabled="false" ToolTip="Flow user can enter bill discount information." />
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Accounts">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbAccountEnable" Checked='<%#Eval("IsAccounts")%>' Enabled="false" ToolTip="Flow user can enter accounts information." />
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Upload">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbUpload" Checked='<%#Eval("IsCanUpload")%>' Enabled="false" ToolTip="Flow user can upload attachment file." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Download">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbDownlaod" Checked='<%#Eval("IsCanDownload")%>' Enabled="false" ToolTip="Flow user can download attachment." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbDelete" Checked='<%#Eval("IsCanDelete")%>' Enabled="false" ToolTip="Flow user can delete own attachment." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Supervisor">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsSupervisor" Checked='<%#Eval("IsSupervisor")%>' Enabled="false" ToolTip="Flow user can assign document." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Member">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsTeamMember" Checked='<%#Eval("IsTeamMember")%>' Enabled="false" ToolTip="Flow user can assign document." />
										</ItemTemplate>
									</asp:TemplateField>

									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Approver">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsApprover" Checked='<%#Eval("IsApprover")%>' Enabled="false" ToolTip="Flow user can approve document." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Closer">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsCloser" Checked='<%#Eval("IsCloser")%>' Enabled="false" ToolTip="Flow user can close document." />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active">
										<ItemTemplate>
											<asp:CheckBox runat="server" ID="gcbIsActive" Checked='<%#Eval("IsActive")%>' Enabled="false" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<asp:ImageButton runat="server" ID="btnSelect" CommandName="SelectRow" ToolTip="Click here to select for edit" ImageUrl="~/assets/img/edit.png" Width="20px" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<asp:ImageButton runat="server" ID="gBtnDelete" CommandName="DeleteRow" ToolTip="Click here to delete this Category flow" ImageUrl="~/assets/img/delete.png" Width="25px" />
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
								<EmptyDataTemplate>
									No Category flow found for the search criteria.
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
		</ContentTemplate>
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
