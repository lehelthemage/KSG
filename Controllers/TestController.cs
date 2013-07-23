using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ksg.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Test()
        {
            List<Models.Test> testList = new List<Models.Test>() {
                new Models.Test() { x = 5 }
            };

            Models.TestContainer tc = new Models.TestContainer()
            {
                Dummy = 0,
                tests = testList
            };

            return View(tc);
        }

        [HttpPost]
        public ActionResult Test(Models.TestContainer container)
        {

            return View(container);
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Test/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Test/Create

        public ActionResult Create()
        {

            List<Models.Test> testList = new List<Models.Test>() {
                new Models.Test() { x = 5 }
            };

            Models.TestContainer tc = new Models.TestContainer()
            {
                Dummy = 0,
                tests = testList
            };

            return View(testList);
        }

        //
        // POST: /Test/Create

        [HttpPost]
        public ActionResult Create(IList<Models.Test> tests)//Models.TestContainer tc)
        {
            return View(tests);
        }

        //
        // GET: /Test/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Test/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Test/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Test/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
