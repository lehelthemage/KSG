using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;
using ksg.Models;

namespace ksg.ViewModels
{
    [Table("aspnet_Users")]
    public class Users
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
    }


    public class KSGObjectViewModel
    {
        //public Int64 ID { get; set; }
        public Guid InternalID { get; set; }
        [Required(ErrorMessage="A title is required.")]
        [StringLength(50, MinimumLength=2, ErrorMessage="must be between 2 and 5 characters")]
        public string Title { get; set; }
        public string DefaultPicValue { get; set; }
        public string AuthorUserName { get; set; }
        public ICollection<string> ParentNames { get; set; }
        public ICollection<Guid> ParentIDs { get; set; }
        public string DefaultParentTitle { get; set; }
        public Guid DefaultParentID { get; set; }
        public virtual IEnumerable<KSGPropertyInfo> Properties { get; set; }
        public string Description { get; set; }
        public bool IsCategory { get; set; }

        
    }
}