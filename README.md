SData Gobi/SLX Entity Translator Library and PowerTools
-------------------------------------------------------
*Important Note: These tools are not supported or sanctioned by Sage, use at your own risk.*

**Sage 1000 Note: You'll need have a Sage 1000 SData endpoint (named ExtSData) with (ideally) the follow IDM feeds enabled:**

  - CE_SettlementDiscount (as CESettlementDiscount)
  - CE_VAT (as CEVAT)
  - CE_VATRegister (as CEVATRegister)
  - NL_PostingCode (as NLPostingCode)
  - OP_OrderHeader (as OPOrderHeader)
  - OP_OrderLine (as OPOrderLine)
  - PL_Bank (as PLBank)
  - PL_Supplier (as PLSupplier)
  - SL_Customer (as SLCustomer)
  - ST_Product (as STProduct)
  - ST_ProductGroup (as STProductGroup)
  - ST_StockGLCrossReference (as STStockGLCrossReference)**

This repository contains the following tools:

*SDataEntityObjects*:
A .NET library that translates [SData](http://sdata.sage.com) feeds to and from Gobi/SLX standard Sage.Entity.Interfaces with full CRUD support

*EntityExplorer*:
A windows forms program that can be used to explore the Gobi/SLX entity model through SData:

![EntityExplorer] (https://github.com/mwilkinsonjr/SDataEntityObjects/blob/master/Screenshots/EntityExplorer.jpg?raw=true)

*SDataPad*:
A windows forms program that can be used to query the Gobi/SLX entity model through SData using LINQ syntax:

![SDataPad] (https://github.com/mwilkinsonjr/SDataEntityObjects/blob/master/Screenshots/SDataPad.jpg?raw=true)

*ConsoleTest*:
A console program which exercises the full SData CRUD support of SDataEntityObjects:

![ConsoleTest] (https://github.com/mwilkinsonjr/SDataEntityObjects/blob/master/Screenshots/ConsoleTest.jpg?raw=true)

This program requires following external dependencies (not included):

 - Sage.SData.Client.dll (Sage SData Client Library for .NET)
 - Sage.Platform.dll (from a Gobi/SLX Platform installation)
 - System.dll (from your local .NET install)
 - Sage.Entity.Intefaces.dll (Generated from your Gobi/SLX installation)
 
Also you will need to enable/compile/deploy in your Gobi/SLX any SData feeds you want to use with these tools.





