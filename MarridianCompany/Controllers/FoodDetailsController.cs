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
    public class FoodDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Create(long? id)
        {
            if (id == null)
            {
                ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name");
            }
            else
            {
                ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name", id);
            }
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,Description,EntryDate,Quantity,FoodGroupID")] FoodDetail FoodDetail, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // To save a image to a folder
                    string picture = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"), picture);
                    file.SaveAs(path);

                    // To store as byte[] in a Table of Database
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        FoodDetail.Image = ms.GetBuffer();
                    }
                    db.FoodDetails.Add(FoodDetail);
                    await db.SaveChangesAsync();
                    TempData["id"] = FoodDetail.FoodGroupID;
                    return RedirectToAction("Index", "FoodGroups");
                }
                else
                {
                    ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name", FoodDetail.FoodGroupID);
                    return PartialView(FoodDetail);
                }
            }
            ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name", FoodDetail.FoodGroupID);
            return PartialView(FoodDetail);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDetail FoodDetail = await db.FoodDetails.FindAsync(id);
            if (FoodDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name", FoodDetail.FoodGroupID);
            return PartialView(FoodDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,Description,Image,EntryDate,Quantity,FoodGroupID")] FoodDetail FoodDetail, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // To save a image to a folder
                    string picture = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"), picture);
                    file.SaveAs(path);

                    // To store as byte[] in a Table of Database
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        FoodDetail.Image = ms.GetBuffer();
                    }
                }
                db.Entry(FoodDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["id"] = FoodDetail.FoodGroupID;
                return RedirectToAction("Index", "FoodGroups");
            }
            ViewBag.FoodGroupID = new SelectList(db.FoodGroups, "ID", "Name", FoodDetail.FoodGroupID);
            return PartialView(FoodDetail);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDetail FoodDetail = await db.FoodDetails.FindAsync(id);
            if (FoodDetail == null)
            {
                return HttpNotFound();
            }
            return PartialView(FoodDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            FoodDetail FoodDetail = await db.FoodDetails.FindAsync(id);
            db.FoodDetails.Remove(FoodDetail);
            await db.SaveChangesAsync();
            TempData["id"] = FoodDetail.FoodGroupID;
            return RedirectToAction("Index", "FoodGroups");
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
