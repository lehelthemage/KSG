using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using ksg.Models;

namespace ksg.ViewModels
{
    public class KSGObjectAddData
    {
        public ksgobject ksgobject;
        public Resource PicResource;
        public Resource DescResource;

    }

    public class KSGObjectsMapper
    {
        public static IList<KSGObjectViewModel> GetList(IList<ksgobject> list)
        {
            List<KSGObjectViewModel> newList = new List<KSGObjectViewModel>();

            foreach (var o in list)
                newList.Add(GetModel(o));

            return newList;
        }


        public static KSGObjectViewModel GetModel(Models.ksgobject ksgObject)
        {
            KSGObjectViewModel model = new KSGObjectViewModel();

            model.InternalID = ksgObject.ID;
            model.Title = ksgObject.Title;
            using (var db = new ksgobjectsEntities1())
            {
                var defaultpics = db.Resources.Where(x => x.ID == ksgObject.DefaultPicID);
                if(defaultpics.Count() != 0)
                    model.DefaultPicValue = defaultpics.First().Value;
            }

            
            model.AuthorUserName = Membership.GetUser(ksgObject.AuthorID).UserName;
            
            model.ParentNames = new List<string>();
            model.ParentIDs = new List<Guid>();
            model.IsCategory = ksgObject.Category != null ? true : false;
            

            using (var db = new ksgobjectsEntities1())
            {
                foreach (var parent in db.ksgparents.Where(x => x.ChildID == ksgObject.ID))
                {
                    var parentObject = db.ksgobjects.Where(x => x.ID == parent.ParentID).First();
                    model.ParentNames.Add(parentObject.Title);
                    model.ParentIDs.Add(parentObject.ID);
                }


                model.Description = db.Resources.Where(y => y.ID == ksgObject.DescriptionID).First().Value;
            }

            return model;
        }
    }
}