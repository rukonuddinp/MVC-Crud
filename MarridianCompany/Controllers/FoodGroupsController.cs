using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarridianCompany.Context;
using MarridianCompany.Models;
using System.IO;

namespace MarridianCompany.Controllers
{
    public class FoodGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public async Task<ActionResult> Index()
        {
            ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name");
            return View(await db.FoodGroups.ToListAsync());
        }

        public ActionResult GetFoodGroupWiseFoodDetails(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewData["id"] = id;
            List<FoodDetail> FoodDetails = db.FoodDetails.Where(e => e.FoodGroupID == id).ToList();

            if (FoodDetails == null)
            {
                return HttpNotFound();
            }

            return PartialView("FoodGroupWiseFoodDetails", FoodDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,FoodDetails")] FoodGroup FoodGroup, HttpPostedFileBase[] Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        if (FoodGroup.FoodDetails.Count == Image.Count())
                        {
                            for (int i = 0; i < FoodGroup.FoodDetails.Count; i++)
                            {
                                // To save a image to a folder
                                string picture = System.IO.Path.GetFileName(Image[i].FileName);
                                string path = System.IO.Path.Combine(Server.MapPath("~/Images"), picture);
                                Image[i].SaveAs(path);

                                // To store as byte[] in a Table of Database
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    Image[i].InputStream.CopyTo(ms);
                                    FoodGroup.FoodDetails[i].Image = ms.GetBuffer();
                                }
                            }
                        }
                        db.FoodGroups.Add(FoodGroup);
                        db.SaveChanges();
                        TempData["id"] = FoodGroup.ID;
                        return RedirectToAction("Index");
                    }
                }
                return View(FoodGroup);
            }
            catch (Exception)
            {
                return View(FoodGroup);
            }
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodGroup FoodGroup = await db.FoodGroups.FindAsync(id);
            if (FoodGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name", id);
            return PartialView(FoodGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,FoodDetails")] FoodGroup FoodGroup, HttpPostedFileBase[] file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    for (int i = 0; i < FoodGroup.FoodDetails.Count; i++)
                    {
                        if (file[i] != null)
                        {
                            // To save a image to a folder
                            string picture = System.IO.Path.GetFileName(file[i].FileName);
                            string path = System.IO.Path.Combine(Server.MapPath("~/Images"), picture);
                            file[i].SaveAs(path);

                            // To store as byte[] in a Table of Database
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file[i].InputStream.CopyTo(ms);
                                FoodGroup.FoodDetails[i].Image = ms.GetBuffer();
                            }
                        }
                    }
                }
                db.Entry(FoodGroup).State = EntityState.Modified;
                foreach(FoodDetail FoodDetail in FoodGroup.FoodDetails)
                {
                    db.Entry(FoodDetail).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                TempData["id"] = FoodGroup.ID;
                return RedirectToAction("Index");
            }
            return View(FoodGroup);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodGroup FoodGroup = await db.FoodGroups.FindAsync(id);
            if (FoodGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(FoodGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            FoodGroup FoodGroup = await db.FoodGroups.FindAsync(id);
            db.FoodGroups.Remove(FoodGroup);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
