using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ksg.ViewModels;
using System.IO;

namespace ksg.Controllers
{
    public class KSGObjectController : Controller
    {
        readonly KSGObjectsRepository _ksgRepository = new KSGObjectsRepository();

        //
        // GET: /Default1/

        public ActionResult Index()
        {
            IList<KSGObjectViewModel> models = _ksgRepository.GetAllObjects();
            return View(models);
        }

        //
        // GET: /Default1/Details/5

        public ActionResult Details(Guid id)
        {
            KSGObjectViewModel model = _ksgRepository.GetObjectById(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //
        // GET: /Default1/Create

        [Authorize]
        public ActionResult Create()
        {
            KSGObjectViewModel m = new KSGObjectViewModel();
            List<KSGPropertyInfo> list = new List<KSGPropertyInfo>();
            KSGPropertyInfo pi = new KSGPropertyInfo();
            pi.Name = "hello";
            pi.ValueTypes = _ksgRepository.GetResourceTypes();
            pi.Value = "blah";
            list.Add(pi);
            m.Properties = list;
            return View(m);
        }

        //
        // POST: /Default1/Create

        [Authorize]
        [HttpPost]
        public ActionResult Create(KSGObjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                _ksgRepository.AddObject(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        public ActionResult ParentAC(string term)
        {
            var filteredTitles = _ksgRepository.FindObjectTitles(term);
            return Json(filteredTitles, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ParentProperties(string id)
        {
            var objects = _ksgRepository.GetObjectsByTitle(id);
            if (objects.Count() == 0)
                return null;
            var obj = objects.First();
            var props = _ksgRepository.GetProperties(obj.ID);
            return Json(props, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetNewProperty()
        {
            var prop =  new ksg.ViewModels.KSGPropertyInfo();
            prop.ValueTypes = _ksgRepository.GetResourceTypes();
            return PartialView("NewKSGProperty", prop);
        }

        //
        // GET: /Default1/Edit/5

        [Authorize]
        public ActionResult Edit(Guid id)
        {

            KSGObjectViewModel ksgobject = _ksgRepository.GetObjectById(id);
            if (ksgobject == null)
            {
                return HttpNotFound();
            }
           
            return View(ksgobject);
        }

        //
        // POST: /Default1/Edit/5

        [Authorize]
        [HttpPost]
        public ActionResult Edit(KSGObjectViewModel ksgobject)
        {
            if (ModelState.IsValid)
            {
                _ksgRepository.UpdateObject(ksgobject);
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
            }
            
            KSGObjectViewModel newksgobject = _ksgRepository.GetObjectById(ksgobject.InternalID);
            return RedirectToAction("Details", new { id = ksgobject.InternalID});
        }

        [Authorize]
        [HttpPost]
        public JsonResult UploadPic()
        {       
                //string uploadFileName = this.Request.Headers["X-File-Name"];
           
                if(Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                {
                    string uploadFileName = Path.GetFileName(Request.Files[0].FileName);

                    // Get the uploads physical directory on the server
                    string directory = this.Server.MapPath("~/Uploads");
                    string format = uploadFileName.Substring(uploadFileName.LastIndexOf("."));
                    string file = Guid.NewGuid().ToString() + format;
                    string localFile = string.Format("{0}\\{1}", directory, file);
                    string relFile = string.Format("Uploads/{0}", file);
                    // If file exists already, delete it (optional)
                    if (System.IO.File.Exists(file) == true) System.IO.File.Delete(localFile);

                    // Save file to server
                    Request.Files[0].SaveAs(localFile);

                    return Json(new { success = new { picpath = relFile } }, JsonRequestBehavior.AllowGet);
                }

                return Json("");
            
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            _ksgRepository.Dispose();
            base.Dispose(disposing);
        }
    

    }
}
