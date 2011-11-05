using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sage.SData.Client.Core;

namespace SDataEntityObjects.SData.Linq
{
    public class SDataResourceCollectionRequestExt : SDataResourceCollectionRequest 
    {
        public string Include
        {
            get
            {
                return Uri.Include;
            }
            set
            {
                Uri.Include = value;

            }
        }


        public SDataResourceCollectionRequestExt(ISDataService service) : base(service) { }
        
        

    }
}
