//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ksg.Models
{
    public partial class ksgmediatype
    {
        public ksgmediatype()
        {
            this.ksgmedias = new HashSet<ksgmedia>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ksgmedia> ksgmedias { get; set; }
    }
    
}
