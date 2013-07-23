using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ksg.Models;
using ksg.ViewModels;
using System.Web.Security;
using System.Data;
using System.Data.Entity.Validation;

namespace ksg.ViewModels
{
    public class KSGPropertyInfo
    {
        public Guid ObjectID { get; set; }
        public byte ValueTypeID { get; set; }
        public string ValueTypeName { get; set; }
        public long PropertyID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual IEnumerable<string> ValueTypes { get; set; }
    }

    public class LabelValuePair
    {
        public Guid id;
        public string value;
    }

    public interface IKSGObjectsRepository
    {
        IList<KSGObjectViewModel> GetAllObjects();
        KSGObjectViewModel GetObjectById(Guid ID);
        void AddObject(KSGObjectViewModel ksgObjectViewModel);
        void UpdateObject(KSGObjectViewModel ksgObjectViewModel);
    }

    public class KSGObjectsRepository : IKSGObjectsRepository 
    {
        private ksgobjectsEntities1 db = new ksgobjectsEntities1();

        public IList<KSGObjectViewModel> GetAllObjects()
        {
            return KSGObjectsMapper.GetList(db.ksgobjects.ToList());
        }

        public IEnumerable<LabelValuePair> FindObjectTitles(string titlepart)
        {
            var objects = db.ksgobjects.Where(item => item.Title.IndexOf(titlepart) >= 0);
            List<LabelValuePair> titles = new List<LabelValuePair>();
            foreach (var o in objects)
                titles.Add(new LabelValuePair { value = o.Title, id = o.ID });

            return titles;
        }

        public IEnumerable<ksgobject> GetObjectsByTitle(string title)
        {
            var objects = db.ksgobjects.Where(x => x.Title == title);
            return objects;
        }

        public KSGObjectViewModel GetObjectById(Guid ID)
        {
            KSGObjectViewModel vm = KSGObjectsMapper.GetModel(db.ksgobjects.Where(x => x.ID == ID).FirstOrDefault());
            vm.Properties = GetProperties(vm.InternalID);
            
            return vm;
        }

        public void AddObject(KSGObjectViewModel ksgObjectViewModel)
        {
            //add the description, get ID
            Resource r = new Resource();
            r.ResourceTypeID = db.ResourceTypes.Where(x => x.Description == "Text").First().ID;
            r.Value = ksgObjectViewModel.Description;
            db.Resources.Add(r);
            db.SaveChanges();

            //r = db.Resources.Las
            //add the picture, get ID
            Resource r2 = null;

            if (ksgObjectViewModel.DefaultPicValue != null)
            {
                r2 = new Resource();
                r2.ResourceTypeID = db.ResourceTypes.Where(x => x.Description == "File Path").First().ID;
                r2.Value = ksgObjectViewModel.DefaultPicValue;
                r2 = db.Resources.Add(r2);
                db.SaveChanges();
            }

            //add the object
            ksgobject o = new ksgobject();
            o.ID = Guid.NewGuid();
            o.Title = ksgObjectViewModel.Title;
            o.DescriptionID = r.ID;
            if (ksgObjectViewModel.IsCategory)
                o.Category = true;
            if(r2 != null)
                o.DefaultPicID = r2.ID;
            o.AuthorID = (Guid)Membership.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey;
            if (ksgObjectViewModel.DefaultParentID == null)
                o.DefaultParentID = new Guid();
            else
                o.DefaultParentID = ksgObjectViewModel.DefaultParentID;
            db.ksgobjects.Add(o);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
            }


            //add properties
            if (ksgObjectViewModel.Properties != null)
            {
                foreach (var prop in ksgObjectViewModel.Properties)
                {
                    AddProperty(o.ID, prop.Name, prop.ValueTypeName, ksgObjectViewModel.IsCategory, prop.Value);
                }
            }
           
        }

        public void AddPhoto(Guid objectID, string picURL)
        {
            Resource r = new Resource();
            r.ResourceTypeID = db.ResourceTypes.Where(x => x.Description == "File Path").First().ID;
            r.Value = picURL;
            db.Resources.Add(r);
            db.SaveChanges();

            //add a media 
            ksgmedia m = new ksgmedia();
            m.TypeID = db.ksgmediatypes.Where(x => x.Description == "Picture").First().ID;
            m.KSGObjectID = objectID;
            m.ResourceID = r.ID;
            db.ksgmedias.Add(m);
            db.SaveChanges();
        }

        public void ThumbPhoto(Guid memberID, long mediaID, bool thumbsUp = true)
        {
            //check if this media was rated already by user

            ksgmedia m = db.ksgmedias.Where(x => x.ID == mediaID).First();

            if (thumbsUp)
                m.UpVotes++;
            else
                m.DownVotes++;

            db.Entry(m).State = EntityState.Modified;
            db.SaveChanges();

            //add entry for vote
        }

    

        public void ChangeDescription(Guid objectID, string newDesc)
        {
            Resource r = new Resource();
            r.ResourceTypeID = db.ResourceTypes.Where(x => x.Description == "File Path").First().ID;
            r.Value = newDesc;
            db.Resources.Add(r);
            db.SaveChanges();

            ksgobject o = db.ksgobjects.Find(objectID);
            o.DescriptionID = r.ID;
            db.Entry(o).State = EntityState.Modified;
            db.SaveChanges();
        }

       

        public void AddParent(Guid objectID, Guid parentID)
        {
            ksgparent p = new ksgparent();
            p.ChildID = objectID;
            p.ParentID = parentID;
            p.UpVotes = 1;
        }

        public void ThumbParent(Guid memberID, Guid objectID, Guid parentID, bool thumbsUp  = true)
        {
            //check if this media was rated already by user

            ksgparent p = db.ksgparents.Where(x => x.ChildID == objectID && x.ParentID == parentID).First();

            if (thumbsUp)
                p.UpVotes++;
            else
                p.DownVotes++;

            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            //add entry for vote
        }

        public void AddProperty(Guid objectID, string name, string valueTypeName, bool isCategory, string value)
        {
            Resource r = null;
            byte resourceTypeID = db.ResourceTypes.Where(x => x.Description == valueTypeName).First().ID;

            //add the property's value (only done if this is not a category)
            if (!isCategory)
            {
                r = new Resource();
                r.ResourceTypeID = resourceTypeID;
                r.Value = value;
                db.Resources.Add(r);
                db.SaveChanges();
            }

            ksgproperty p = null;
            if (db.ksgproperties.Where(x => x.Name == name).Where(y => y.ValueTypeID == resourceTypeID).Count() != 0)
                p = db.ksgproperties.Where(x => x.Name == name).Where(y => y.ValueTypeID == resourceTypeID).First();
            if(p == null)
            {
                p = new ksgproperty();
                p.Name = name;
                p.ValueTypeID = resourceTypeID;
  
                db.ksgproperties.Add(p);
                db.SaveChanges();
            }


            ksgobjectproperty prop = new ksgobjectproperty();
            prop.ksgobjectid = objectID;
            prop.ksgpropertyid = p.ID;
            if (isCategory)
                prop.ValueID = null;
            else
                prop.ValueID = r.ID;
            db.ksgobjectproperties.Add(prop);
            db.SaveChanges();
        }

        public void ThumbProperty(Guid memberID, long propertyID, bool thumbsUp = true)
        {
            //check if this media was rated already by user

            ksgproperty p = db.ksgproperties.Where(x => x.ID == propertyID).First();

            if (thumbsUp)
                p.UpVotes++;
            else
                p.DownVotes++;

            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

        }

        public List<string> GetResourceTypes()
        {
            List<string> types = new List<string>();
            var dbtypes = db.ResourceTypes;
            foreach (var t in dbtypes)
                types.Add(t.Description);

            return types;
        }

        public List<KSGPropertyInfo> GetProperties(Guid objectID)
        {
            
            IList<ksgobjectproperty> p = db.ksgobjectproperties.Where(x => x.ksgobjectid == objectID).ToList();
            List<KSGPropertyInfo> infoList = new List<KSGPropertyInfo>();
            bool? isProperty = db.ksgobjects.Where(x => x.ID == objectID).FirstOrDefault().Category;
            //get the value types
            List<string> types = GetResourceTypes();

            foreach (var prop in p)
            {
                KSGPropertyInfo pi = new KSGPropertyInfo();
                pi.PropertyID = prop.ID;
                pi.Name = prop.ksgproperty.Name;
                if(isProperty == null)
                    pi.Value = prop.Resource.Value;
                pi.ObjectID = prop.ksgobjectid;
                pi.ValueTypeID = (byte)prop.ksgproperty.ValueTypeID;
                pi.ValueTypeName = db.ResourceTypes.Where(x => x.ID == pi.ValueTypeID).First().Description;
                pi.ValueTypes = types;
                infoList.Add(pi);
            }

            return infoList;
        }



        

        public void UpdateObject(KSGObjectViewModel ksgObjectViewModel)
        {
            ksgobject o = db.ksgobjects.Find(ksgObjectViewModel.InternalID);
            
            //check if the title has changed
            //(need to change titles to have a record for each)
            if (o.Title != ksgObjectViewModel.Title)
            {
                o.Title = ksgObjectViewModel.Title;
                db.Entry(o).State = EntityState.Modified;
                db.SaveChanges();
            }

            //check if default picture has been added
            string defaultPicValue;
            if (o.DefaultPicID == null)
                defaultPicValue = null;
            else
                defaultPicValue = db.Resources.Where(x => x.ID == o.DefaultPicID).First().Value;


            if (defaultPicValue != ksgObjectViewModel.DefaultPicValue)
            {
                Resource r2 = new Resource();
                r2.ResourceTypeID = db.ResourceTypes.Where(x => x.Description == "File Path").First().ID;
                r2.Value = ksgObjectViewModel.DefaultPicValue;
                r2 = db.Resources.Add(r2);
                db.SaveChanges();

                o.DefaultPicID = r2.ID;
                db.Entry(o).State = EntityState.Modified;
                db.SaveChanges();
            }

            //check Description if it needs to be changed
            Resource r = db.Resources.Find(o.DescriptionID);
            if (r.Value != ksgObjectViewModel.Description)
            {
                r = new Resource();
                r.ResourceTypeID = db.ResourceTypes.Where(x => x.Description == "Text").First().ID;
                r.Value = ksgObjectViewModel.Description;
                r = db.Resources.Add(r);
                db.SaveChanges();
                o.DescriptionID = r.ID;
                db.Entry(o).State = EntityState.Modified;
                db.SaveChanges();
            }
 
            //iterate through properties and see if we need to add or update properties
            foreach (var prop in ksgObjectViewModel.Properties)
            {
                var p2 = db.ksgobjectproperties.Where(x => x.ksgproperty.Name == prop.Name).First();

                if (p2 == null) //add the property
                    AddProperty(prop.ObjectID, prop.Name, prop.ValueTypeName, ksgObjectViewModel.IsCategory, prop.Value);
                else if(p2.Resource.Value != prop.Value)
                {
                    r = db.Resources.Where(x => x.Value == p2.Resource.Value).First();
                    if (r == null)
                    {
                        r = new Resource();
                        r.ResourceTypeID = prop.ValueTypeID;
                        r.Value = prop.Value;
                        db.Resources.Add(r);
                        db.SaveChanges();
                    }
                    else
                    {
                        r.Value = prop.Value;
                        db.Entry(r).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    p2.ValueID = r.ID;
                    db.Entry(p2).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        private static IList<KSGObjectViewModel> GetList(IList<ksgobject> list)
        {
            List<KSGObjectViewModel> newList = new List<KSGObjectViewModel>();

            foreach (var o in list)
                newList.Add(KSGObjectsMapper.GetModel(o));

            return newList;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        
    }
}