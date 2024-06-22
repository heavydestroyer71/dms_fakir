<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillReceiveSlip.aspx.cs" Inherits="FokirDMS.UI.Reports.BillReceiveSlip" %>

<!DOCTYPE html>

<html lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,500;1,800&display=swap" rel="stylesheet">
    <style>
      body {
        margin: 0;
        font-family: 'Open Sans', sans-serif;
      }
      .page-wrapper{
        width: 100%;
        height: 100%;
        padding: 0px;
        margin: 0;
        display: flex;
        justify-content: center;
     
      }
      .page-body {
        display: block;
        width: 80%;
        height: auto;
        padding: 0px;
        margin: 0px;
      }
      .page-body .block-header,
      .block-bottom {
        width: 100%;
        height: auto;
        padding: 20px;
        margin: 0;
        display: flex;
        justify-content: center;
        /* align-items: center; */
        border: 1px solid #000;
        overflow: hidden;
      }
      .page-body .block-header .block-left,
      .page-body .block-bottom .block-left {
        width: 80%;
        height: auto;
        padding: 5px;
        margin: 0;
        display: flex;
        justify-content: center;
        
        /* align-items: center; */
      }
      .page-body .block-header .block-right,
      .page-body .block-bottom .block-right {
        width: 20%;
        height: 100%;
        padding: 50px;
        margin: 0;
        display: flex;
        justify-content: start;
        align-items: start;
        overflow: hidden;

      }
      h1 {
        font-size: 24px;
        font-weight: 700;
        font-family: 'Open Sans', sans-serif;
        letter-spacing: 1px;
        color: #000;
        width: 100%;
        padding: 5px;
        margin-left: -5%;
        display: flex;
        justify-content: end;
      }
      p {
        font-size: 16px;
        font-weight: 700;
        font-family: 'Open Sans', sans-serif;
        letter-spacing: 1px;
        color: #000;
        width: 100%;
        padding: 5px;
        margin: 0;
        display: flex;
        justify-content: start;
      }
      form {
        display: block;
        padding: 0;
        margin: 0;
        width: 100%;
      }
      .group-block {
        width: 100%;
        height: auto;
        padding: 0;
        margin: 0;
        display: flex;
        justify-content: start;
      }
      .group-block label {
        width: 40%;
        height: auto;
        padding: 0;
        margin: 0;
        display: flex;
        justify-content: space-between;
        align-items: center;
      }
      .group-block input {
        width: 60%;
        height: 30px;
        outline: none;
        font-size: 16px;
        font-weight: 500;
        font-family: 'Open Sans', sans-serif;
        color: #000;
        padding: 10px;
        margin: 0;
        border: none;
      }
      textarea{
        width: 60%;
        outline: none;
        font-size: 16px;
        font-weight: 400;
        font-family: 'Open Sans', sans-serif;
        color: #000;
        padding: 10px;
        margin: 0;
        border: none;
      }
      span {
        font-size: 22px;
        font-weight: 700;
        font-family: 'Open Sans', sans-serif;
        color: #000;
      }
      textarea:focus
      input:focus {
        outline: none;
      }
      textarea::placeholder
      input::placeholder {
        font-size: 12px;
        font-weight: 700;
        font-family: 'Open Sans', sans-serif;
        color: gray;
      }
      .page-body .block-footer .block-right .image {
        width: 50px;
        height: 50px;
        border: 1px solid #000;
        padding: 5px;
        background: #fff;
        background-position: center;
        object-fit: cover;
        margin-top: 50px;
      }
      hr {
        border: 2px dashed #000;
        border-style: dashed;
        margin-left: 10px;
        /* color: #fff;
        background-color: #fff; */
      }
    </style>
    <title>Bill Acknowledgement Slip</title>
  </head>

  <body>
    <div class="page-wrapper">
        <div class="page-body">
            <!-- first part -->
            <div class="block-header">
              <div class="block-left">
                <form>
                  <h1>Bill Acknowledgement Slip (Vendor Copy)</h1>
                  <div class="group-block">
                    <label>Tracking Number<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gTrackingNo%>" />
                  </div>
                  <div class="group-block">
                    <label>Company<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gCompany%>" />
                  </div>
                  <div class="group-block">
                    <label>Supplier Name<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gSupplierName%>" />
                  </div>
                  <div class="group-block">
                    <label>PO Number<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gPoNo%>" />
                  </div>
                  <div class="group-block">
                    <label>Invoice Number<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gInvoiceNo%>"/>
                  </div>
                  <div class="group-block">
                    <label>Invoice Amount<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gInvoiceAmount%>"/>
                  </div>
                  <div class="group-block">
                    <label>Received Date & Time<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gReceivedDate%>"/>
                  </div>
                  <div class="group-block">
                    <label>Documents Received List<span>:</span></label>
                   <%-- <input type="text" aria-multiline="true" aria-setsize="2000"   readonly="readonly" value="<%=gDocReceiptList%>"/>--%>
                   <textarea readonly="readonly" aria-atomic="true" aria-expanded="false" ><%=gDocReceiptList%></textarea>
                  </div>
                  <div class="group-block">
                    <label>Received By<span>:</span></label>
                    <input type="text" readonly="readonly" value="<%=gReceivedBy%>"/>
                  </div>
                  <div>
                    <p>This is Software Generated Copy,Signature is Not Required.</p>
                  </div>
                </form>
              </div>
              <div >
                <img class="image"   src="<%=gQrCode%>" />
            </div>
            <!-- first part end -->
           
          </div>
          <hr />
          <!-- second part -->
          <div class="block-bottom">
            <div class="block-left">
              <form>
                <h1>Bill Acknowledgement Slip (FFL Copy)</h1>
                <div class="group-block">
                  <label>Tracking Number<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gTrackingNo%>"/>
                </div>
                <div class="group-block">
                  <label>Company<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gCompany%>"/>
                </div>
                <div class="group-block">
                <label>Expense Type<span>:</span></label>
                <input type="text" readonly="readonly" value="<%=gExpenseType%>"/>
                </div>
                  
                <div class="group-block">
                  <label>Supplier Name<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gSupplierName%>"/>
                </div>
                <div class="group-block">
                  <label>PO Number<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gPoNo%>"/>
                </div>
                <div class="group-block">
                  <label>Invoice Number<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gInvoiceNo%>"/>
                </div>
                <div class="group-block">
                  <label>Invoice Amount<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gInvoiceAmount%>"/>
                </div>
                <div class="group-block">
                  <label>Received Data & Time<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gReceivedDate%>"/>
                </div>
                <div class="group-block" >
                  <label>Documents Received<span>:</span></label>
                  <textarea readonly="readonly" aria-atomic="true" aria-expanded="false" ><%=gDocReceiptList%></textarea>
                </div>
                <div class="group-block">
                  <label>Received By<span>:</span></label>
                  <input type="text" readonly="readonly" value="<%=gReceivedBy%>"/>
                </div>
                <div>
                  <p>This is Software Generated Copy,Signature is Not Required.</p>
                </div>
              </form>
            </div>
            <div>
              <img class="image"   src="<%=gQrCode%>" />
            </div>
          </div>
    </div>
    </div>


  </body>
</html>
