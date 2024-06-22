<%@ Page Title="Store Setup | DMS" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="StoreSetup.aspx.cs" Inherits="FakirDMS.UI.StoreSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <style type="text/css">
        .HideGridColumn {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="hfId" Value="0" />

            <div class="row">
                <div class="col-4">
                    <div class="panel panel-info">
                        <div class="panel-header">Create Room</div>
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Room Name <span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:TextBox runat="server" ID="room_txtName" placeholder="Room Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Is Active?
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:CheckBox runat="server" ID="room_cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-md-5 col-sm-5"></div>
                                <div class="col-md-3 col-sm-3">
                                    <asp:Button runat="server" ID="room_btnSave" OnClick="room_btnSave_Click" Text="Save" Style="width: 100%" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info" style="margin-top: 25px">
                        <div class="panel-header">Create Rack</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Room Name <span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:DropDownList runat="server" ID="rack_ddlRoomName" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Rack Name <span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:TextBox runat="server" ID="rack_txtRackName" placeholder="Rack Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Is Active?
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:CheckBox runat="server" ID="rack_cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-md-5 col-sm-5"></div>
                                <div class="col-md-3 col-sm-3">
                                    <asp:Button runat="server" ID="rack_btnSave" OnClick="rack_btnSave_Click" Text="Save" Style="width: 100%" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info" style="margin-top: 25px">
                        <div class="panel-header">Create Shelf</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Room Name
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:DropDownList runat="server" ID="shelf_ddlRoomName" OnSelectedIndexChanged="shelf_ddlRoomName_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Rack Name<span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:DropDownList runat="server" ID="shelf_ddlRackName" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Shelf Name <span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:TextBox runat="server" ID="shelf_txtShelfName" placeholder="Shelf Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Is Active?
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:CheckBox runat="server" ID="shelf_cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-md-5 col-sm-5"></div>
                                <div class="col-md-3 col-sm-3">
                                    <asp:Button runat="server" ID="shelf_btnSave" OnClick="shelf_btnSave_Click" Text="Save" Style="width: 100%" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info" style="margin-top: 25px">
                        <div class="panel-header">Create Box</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Room Name
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:DropDownList runat="server" ID="box_ddlRoomName" OnSelectedIndexChanged="box_ddlRoomName_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Rack Name
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:DropDownList runat="server" ID="box_ddlRackName" OnSelectedIndexChanged="box_ddlRackName_SelectedIndexChanged" AutoPostBack="true" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Shelf Name<span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:DropDownList runat="server" ID="box_ddlShelfName" CssClass="DropDownListStyle"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Box Name <span style="color: red">*</span>
                                </div>
                                <div class="col-md-10 col-sm-10">
                                    <asp:TextBox runat="server" ID="box_txtBoxName" placeholder="Box Name" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2">
                                    Is Active?
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <asp:CheckBox runat="server" ID="box_cbIsActive" Checked="true" CssClass="CheckBoxStyle" />
                                </div>
                                <div class="col-md-5 col-sm-5"></div>
                                <div class="col-md-3 col-sm-3">
                                    <asp:Button runat="server" ID="box_btnSave" OnClick="box_btnSave_Click" Text="Save" Style="width: 100%" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-8">
                    <div class="panel panel-info">
                        <div class="panel-header">Store Location</div>
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-10">
                                    <asp:TextBox runat="server" ID="txtSearch" placeholder="Search with Lookup text or discription" CssClass="TextBoxStyle"></asp:TextBox>
                                </div>
                                <div class="col-2" align="right">
                                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" />
                                    <asp:Button runat="server" ID="btnReload" OnClick="btnReload_Click" Text="Reload" />
                                </div>
                            </div>
                            <div style="margin: 10px"></div>
                            <asp:GridView ID="gvLookup" runat="server"
                                OnRowCommand="gvLookup_RowCommand"
                                OnPageIndexChanging="gvLookup_PageIndexChanging"
                                AutoGenerateColumns="false" AllowPaging="true" PageSize="15" Width="100%"
                                ShowHeaderWhenEmpty="True" CellPadding="8" CellSpacing="4" HorizontalAlign="Center" CssClass="ssGridToggle"
                                BackColor="#FCFCFC" BorderColor="#DADADA" BorderStyle="Solid" BorderWidth="1px">
                                <Columns>
                                    <asp:BoundField DataField="RoomName" HeaderText="Room" />
                                    <asp:BoundField DataField="RackName" HeaderText="Rack" />
                                    <asp:BoundField DataField="ShelfName" HeaderText="Shelf" />
                                    <asp:BoundField DataField="BoxName" HeaderText="Box" />
                                    <asp:TemplateField HeaderStyle-Width="190px" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="gHfRoomId" Value='<%#Eval("RoomId")%>' />
                                            <asp:HiddenField runat="server" ID="gHfRackId" Value='<%#Eval("RackId")%>' />
                                            <asp:HiddenField runat="server" ID="gHfShelfId" Value='<%#Eval("ShelfId")%>' />
                                            <asp:HiddenField runat="server" ID="gHfBoxId" Value='<%#Eval("BoxId")%>' />

                                            <asp:Button runat="server" ID="gvBtnRoom" Text="Room" />
                                            <asp:Button runat="server" ID="gvBtnRack" Text="Rack" />
                                            <asp:Button runat="server" ID="gvBtnShelf" Text="Shelf" />
                                            <asp:Button runat="server" ID="gvBtnBox" Text="Box" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No data found for the search criteria.
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
