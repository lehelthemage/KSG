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
    public partial class ksgvote
    {
        public long ID { get; set; }
        public byte VoteTypeID { get; set; }
        public bool UpVote { get; set; }
        public System.Guid UserID { get; set; }
    
        public virtual ksgvotetype ksgvotetype { get; set; }
    }
    
}