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
    public partial class ksgparent
    {
        public long ID { get; set; }
        public System.Guid ChildID { get; set; }
        public System.Guid ParentID { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    
        public virtual ksgobject ksgobject { get; set; }
        public virtual ksgobject ksgobject1 { get; set; }
    }
    
}
