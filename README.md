

SData Gobi/SLX Entity Translator Library and PowerTools
-------------------------------------------------------

*Important Note: These tools are not supported or sanctioned by Sage, use at your own risk.*

This repository contains the following tools:

*SDataEntityObjects*:
A .NET library that translates [SData](http://sdata.sage.com) feeds to and from Gobi/SLX standard Sage.Entity.Interfaces with full CRUD support

*EntityExplorer*:
A windows forms program that can be used to explore the Gobi/SLX entity model through SData

*SDataPad*:
A windows forms program that can be used to query the Gobi/SLX entity model through SData using LINQ syntax.

*ConsoleTest*:
a console program which exercises the full SData CRUD support of SDataEntityObjects 

This program requires following dependencies (not included):

- Sage.SData.Client.dll (Sage SData Client Library for .NET)
- Sage.Platform.dll (from a Gobi/SLX Platform installation)
- System.dll (from your local .NET install)
- Sage.Entity.Intefaces.dll (Generated from your Gobi/SLX installation)



