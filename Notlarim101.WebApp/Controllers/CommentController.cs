﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.WebApp.Data;
using Notlarim101.WebApp.Models;

namespace Notlarim101.WebApp.Controllers
{
    public class CommentController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CommentManager cmm = new CommentManager();

        // GET: Comment
        public ActionResult Index()
        {
            return View(cmm);
        }

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
      
        public ActionResult Create(Comment comment, int? noteId)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (noteId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Note note = nm.Find(x => x.Id == noteId);

                if (note == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Note = note;
                comment.Owner = CurrentSession.User;
            }
            if (cmm.Insert(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost]
        
        public ActionResult Edit(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment==null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;
            if (cmm.Update(comment)>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s=>s.Id==id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (cmm.Delete(comment)>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        ////// POST: Comment/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Comment comment = cmm.Find(s=>s.Id==id);

        //    return RedirectToAction("Index");
        //}
        public ActionResult ShowNoteComments(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Note note = nm.Find(s => s.Id == id);
            Note note = nm.QList().Include("Comments").FirstOrDefault(s => s.Id == id);
            if (note==null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialComment", note.Comments);
        }
    }
}