using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDataEntityObjects;
using Sage.Entity.Interfaces;
using SDataEntityObjects.SData;
using SDataEntityObjects.ConsoleTest.Diagnostics;

namespace SDataEntityObjects.ConsoleTest
{
    class Program
    {
        private static IClientContext CreateSDataContext()
        {
            return ClientFactory.GetContext(
                new SDataClientContextFactory(),
                new SDataContextConfiguration()
                {
                    Servername = "localhost",
                    Port = 3333,
                    Username = "lee",
                    Password = String.Empty
                });
        }

        static void Main(string[] args)
        {
            using (IClientContext context = CreateSDataContext())
            {
                    PerformTests(context);
            }
            Console.WriteLine("All Tests done.");
        }

        private static void PerformTests(IClientContext context)
        {
            IAccount Account;
            IList<IAccount> AccountList;

            using (new CustomStopWatch("Test 1"))
            {
                Console.WriteLine("Test 1: Get an SLX entity by ID (Account with Nested Address)");
                Account = context.GetById<IAccount>("AGHEA0002669", "Address");
                Console.WriteLine("Account.AccountName = " + Account.AccountName);
                Console.WriteLine("Account.Address.City = " + Account.Address.City);                
            }
            Console.WriteLine("--- Press any key to continue ---"); 
            Console.ReadKey();
            using (new CustomStopWatch("Test 2"))
            {
                Console.WriteLine("Test 2: Get an entity calculated property");
                Console.WriteLine("Account.Address.FullAddress = " + Account.Address.FullAddress);
            }
            Console.WriteLine("--- Press any key to continue ---");
            Console.ReadKey();            
            using (new CustomStopWatch("Test 3"))
            {
                Console.WriteLine("Test 3: Call an entity method");
                Console.WriteLine("Account.Address.FormatFullAddress() returns: " + Account.Address.FormatFullAddress());
            }
            Console.WriteLine("--- Press any key to continue ---");
            Console.ReadKey();
            using (new CustomStopWatch("Test 4"))
            {
                Console.WriteLine("Test 4: Lazy Load entity traversal");                
                Console.WriteLine("Account.Contact.Count returns:" + 
                    Account.Contacts.Count.ToString());
                foreach (IContact Contact in Account.Contacts)
                {
                    Console.WriteLine("Contact NamePFL & Title = " + 
                        Contact.NamePFL + ", " + Contact.Title);
                }
                Console.WriteLine("Account.Opportunities.Count returns:" + 
                    Account.Opportunities.Count.ToString());
                foreach (IOpportunity Opportunity in Account.Opportunities)
                {
                    Console.WriteLine("Opportunity Description & SalesPotential = " + 
                        Opportunity.Description + ", " + Opportunity.SalesPotential.ToString());
                    Console.WriteLine(" OpportunityProduct Count = " + 
                        Opportunity.Products.Count.ToString());
                    foreach (IOpportunityProduct OpportunityProduct in Opportunity.Products)
                    {
                        Console.WriteLine("  Product Name, Quantity & Price = " + 
                            OpportunityProduct.Product.Name + ", " + 
                            OpportunityProduct.Quantity.ToString() + ", " + 
                            OpportunityProduct.ExtendedPrice.ToString());
                    }
                }
            }
            Console.WriteLine("--- Press any key to continue ---");
            Console.ReadKey();
            using (new CustomStopWatch("Test 5"))
            {
                Console.WriteLine("Test 5: LINQ Query for SLX Entities");
                AccountList = context.CreateQuery<IAccount>().Where(x => x.AccountName.StartsWith("Sa")).ToList();
                Console.WriteLine("There are " + AccountList.Count + " Accounts that start with 'Sa':");
                foreach (IAccount LAccount in AccountList)
                {
                    Console.WriteLine("Account Name & Address City = " + LAccount.AccountName + ", " + LAccount.Address.City);
                }
            }
            Console.WriteLine("--- Press any key to continue ---");
            Console.ReadKey();
            using (new CustomStopWatch("Test 6"))
            {
                Console.WriteLine("Test 6: Create New SLX Entities");
                IAccount newAccount = context.CreateNew<IAccount>();
                newAccount.AccountName = "Sage (UK) Limited";
                newAccount.MainPhone = "+44 (0)191 294 3000";
                newAccount.Fax = "+44 (0) 118 927 0615";
                newAccount.WebAddress = "www.sage.co.uk";
                newAccount.Save();
                Console.WriteLine("Account " + newAccount.ToString() + "Created, " +
                    "ID = " + newAccount.Id.ToString());
                // Save creates a Primary Address object automatically
                newAccount.Address.Description = "Mailing";
                newAccount.Address.Address1 = "Sage House";
                newAccount.Address.Address2 = "Wharfdale Road";
                newAccount.Address.City = "Winnersh";
                newAccount.Address.State = "Wokingham";
                newAccount.Address.PostalCode = "RG41 5RD";
                newAccount.Address.Country = "England";
                newAccount.Address.Save();
                newAccount.Save();
                Console.WriteLine("Account Primary Address Created, ID = " + newAccount.Address.Id.ToString());
                // Related Contact
                //IContact Contact = context.CreateNew<IContact>();
                //newAccount.Contacts.Add(Contact);
                //newAccount.Save();
                //Contact.FirstName = "Michael";
                //Contact.LastName = "Wilkinson";
                //Contact.Suffix = "Jr.";
                //Contact.Title = "CRM/SalesLogix Consultant";                
                //Contact.WorkPhone = newAccount.MainPhone;
                //Contact.Fax = newAccount.Fax;
                //Contact.Mobile = "+44 (0)777 354 8144";
                //Contact.Save();
                //Console.WriteLine("Contact Created, ID = " + Contact.Id.ToString());
                // Save creates a Primary Address object automatically
                //Contact.Address.Address1 = newAccount.Address.Address1;
                //Contact.Address.Address2 = newAccount.Address.Address2;
                //Contact.Address.Address3 = newAccount.Address.Address3;
                //Contact.Address.Address4 = newAccount.Address.Address4;
                //Contact.Address.City = newAccount.Address.City;
                //Contact.Address.State = newAccount.Address.State;
                //Contact.Address.PostalCode = newAccount.Address.PostalCode;
                //Contact.Address.Country = newAccount.Address.County;
                //Contact.Address.Save();
                //Contact.Save();
                //Console.WriteLine("Contact Address Created, ID = " + Contact.Address.Id.ToString());
                // Related Opportunity
                //IOpportunity Opportunity = context.CreateNew<IOpportunity>();
                //Opportunity.Account = newAccount;
                //Opportunity.Description = "Sage UK Microsoft Programs";
                //Opportunity.CloseProbability = 30;
                //Opportunity.EstimatedClose = DateTime.Now.AddDays(10);
                //Opportunity.Save();
                //Console.WriteLine("Opportunity Created, ID = " + Opportunity.Id.ToString());
                //// Related Opportunity Contact
                //IOpportunityContact OpportunityContact = context.CreateNew<IOpportunityContact>();
                //OpportunityContact.Opportunity = Opportunity;
                //OpportunityContact.Contact = Contact;
                //OpportunityContact.Influence = "Decision Maker";
                //OpportunityContact.Save();
                //// Related Opportunity Products
                //IList<IProduct> ProductList = context.CreateQuery<IProduct>().Where(x => x.Name.StartsWith("MS")).ToList();
                //foreach (IProduct Product in ProductList)
                //{
                //    IOpportunityProduct OpportunityProduct = context.CreateNew<IOpportunityProduct>();
                //    OpportunityProduct.Opportunity = Opportunity;
                //    OpportunityProduct.Product = Product;
                //    OpportunityProduct.Quantity = 2;
                //    OpportunityProduct.Price = Product.Price;
                //    OpportunityProduct.CalculateExtendedPrice();
                //    OpportunityProduct.Save();
                //    Console.WriteLine("Opportunity Product Created, ID = " + OpportunityProduct.Id.ToString());
                //}                
                //Opportunity.CalculateSalesPotential();
                //Opportunity.ValidateOpportunity();
                //Opportunity.Save();
                                  
            }


            



            Console.WriteLine("--- Press any key to continue ---");
            Console.ReadKey();
            using (new CustomStopWatch("Test 8"))
            {
                Console.WriteLine("Test 8: Delete SLX Entities");
                AccountList = context.CreateQuery<IAccount>().Where(x => x.AccountName.Equals("Sage (UK) Limited")).ToList();
                foreach (IAccount LAccount in AccountList)
                {
                    LAccount.Delete();
                }
            }
            Console.WriteLine("--- Press any key to continue ---");
            Console.ReadKey();





                //if (account.AccountExtension.Id == null)
                //{
                //    account.AccountExtension.CustomerId = 100000218;
                //    //account.AccountExtension.AccountId = account.Id.ToString();
                //    //account.AccountExtension.Account = account;
                //    account.AccountExtension.Save();
                //}
                //else
                //{
                //    account.AccountExtension.CustomerId++;
                //    account.AccountExtension.Save();
                //    Console.WriteLine("Extensions bereits im System vorhanden: {0}", account.AccountExtension.CustomerId);
                //}

                //account.AccountExtension.Account = account;
                //account.AccountExtension.Save();

                //IAccountExtension extension = account.AccountExtension;

                //if (extension == null)
                //    Console.WriteLine("extension is missing");
                //else
                //    Console.WriteLine(extension.CustomerId);

                //Console.WriteLine(account.AccountName);

                //using (new CustomStopWatch("All Accounts with a"))
                //{
                //    int counter = 0;

                //    foreach (var account in context.CreateQuery<IAccount>().Where(x => x.Id == "AA2EK0013901"))
                //    {
                //        //Console.WriteLine("{0}, {1}", account.AccountName, account.Id);
                //        counter++;
                //    }
                //    Console.WriteLine("Received {0} rows", counter);
                //}


                //IAccount newAccount = context.CreateNew<IAccount>();

                //newAccount.AccountName = "Testkunde 123";
                //newAccount.Save();

                //var thing = context.CreateQuery<IAccount>();



                //foreach (var account in context.CreateQuery<IAccount>().Where(x => x.AccountName.StartsWith("BE Bosworth")))
                //{
                //    var result = account.CanChangeOwner();
                //    Console.WriteLine("CanChangeOwner returns a {0} value", result.ToString());

                //    Console.WriteLine("{0}", account.ShippingAddress.City);

                //    foreach (var contact in account.Contacts)
                //    {

                //        Console.WriteLine("{0}, {1}", contact.Id, contact.FirstName);
                //    }

                //}

                //{
                //    var opp = context.GetById<IOpportunity>("ODEMOA000002");

                //    foreach (IOpportunityCompetitor competitor in opp.GetClosedWonLostOppCompetitors())
                //    {
                //        Console.WriteLine(competitor.Strengths);
                //    }
                //}

                //foreach (var opportunity in context.CreateQuery<IOpportunity>().Where(x => x.Account.AccountName.StartsWith("Abbott")))
                //{
                //    Console.WriteLine("{0}, {1}", opportunity.Id, opportunity.Description);
                //    foreach (var item in collection)
                //    {

                //    } opportunity.ValidateOpportunity();
                //}
            
        }




    
    
    
    }
}
