using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;
using Notlarim101.Entity.ValueObject;
using Notlarim101.WebApp.ViewModel;

namespace Notlarim101.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Test test = new Test();
            ////test.InsertTest();
            ////test.UpdateTest();
            ////test.DeleteTest();
            //test.CommentTest();

            NoteManager nm = new NoteManager();

            return View(nm.GetAllNotes().OrderByDescending(s => s.ModifiedOn).ToList());
        }


        public ActionResult ByCategoryId(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(s => s.ModifiedOn).ToList());
        }

        public ActionResult ByCategoryTitle(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryByTitle(id);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.OrderByDescending(s => s.ModifiedOn).ToList());
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                NotlarimUserManager num = new NotlarimUserManager();
                BusinessLayerResult<NotlarimUser> res = num.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/UserActivate/1234-2345-2345467";
                    }

                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }

                Session["login"] = res.Result;//session a kullanici bilgilerini aktarma
                return RedirectToAction("Index");//yonlendirme
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //bool hasError = false;
            if (ModelState.IsValid)
            {
                NotlarimUserManager num = new NotlarimUserManager();
                BusinessLayerResult<NotlarimUser> res = num.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }

                //try
                //{
                //    user = num.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("",ex.Message);
                //}

                //if (user==null)
                //{
                //    return View(model);
                //}
                //return RedirectToAction("RegisterOk");
                //if (model.Username == "aaa" && model.Email == "aaa@aaa.com")
                //{
                //    ModelState.AddModelError("", "Kullanici adi kullaniliyor.");
                //    ModelState.AddModelError("", "Bu email kullaniliyor.");
                //    return View(model);
                //}
                //if (model.Username=="aaa")
                //{
                //    ModelState.AddModelError("","Kullanici adi kullaniliyor.");
                //    //return View(model);
                //    //hasError = true;
                //}

                //if (model.Email=="aaa@aaa.com")
                //{
                //    ModelState.AddModelError("","Bu email kullaniliyor.");
                //    //return View(model);
                //    //hasError = true;
                //}


                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}
                //return RedirectToAction("RegisterOk");
                //if (hasError==true)
                //{
                //    return View(model);
                //}
                //else
                //{
                //    return RedirectToAction("RegisterOk");
                //}

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayit başarılı",
                    RedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınız aktive a-ediniz. Hesabınızı aktive etmeden not ekleyemez beğeni yapamazsınız.");

                return View("Ok",notifyObj);
            }
            return View(model);
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            NotlarimUserManager num = new NotlarimUserManager();
            BusinessLayerResult<NotlarimUser> res = num.ActivateUser(id);
            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }
            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }

            return View(errors);
        }

        public ActionResult ShowProfile()
        {
            NotlarimUser currentUser = Session["login"] as NotlarimUser;
            NotlarimUserManager num = new NotlarimUserManager();
            BusinessLayerResult<NotlarimUser> res = num.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                //kullanıcı hata ekranına yönlendirilecek
            }
            return View(res.Result);
        }
        public ActionResult EditProfile()
        {
            NotlarimUserManager num = new NotlarimUserManager();
            NotlarimUser currentUser=Session["login"] as NotlarimUser;
            BusinessLayerResult<NotlarimUser> res = num.GetUserById(currentUser.Id);
            if (res.Errors.Count>0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(NotlarimUser model, HttpPostedFileBase profileImage)
        {
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (profileImage!=null && (profileImage.ContentType=="image/jpeg" || profileImage.ContentType == "image/jpg" || profileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{profileImage.ContentType.Split('/')[1]}";//1uzantısını alır
                    profileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                NotlarimUserManager num = new NotlarimUserManager();
                BusinessLayerResult<NotlarimUser> res = num.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Profil Güncellenemedi",
                        Items = res.Errors,
                        RedirectingUrl ="/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }
                Session["login"] = res.Result;
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        public ActionResult DeleteProfile()
        {
            NotlarimUserManager num = new NotlarimUserManager();
            NotlarimUser currentUser = Session["login"] as NotlarimUser;
            BusinessLayerResult<NotlarimUser> res = num.RemoveUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil Silinemedi",
                    Items = res.Errors,
                    RedirectingUrl="/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult DeleteProfile(int id)
        //{
        //    //NotlarimUserManager num = new NotlarimUserManager();
        //    //BusinessLayerResult<NotlarimUser> res=
        //    return View();
        //}
       
        //public ActionResult TestNotify()
        //{
        //    ErrorViewModel model = new ErrorViewModel()
        //    {
        //        Header = "Yönlendirme",
        //        Title = "UYARI",
        //        RedirectingTimeOut = 10000,
        //        Items = new List<ErrorMessageObj>()
        //        {
        //            new ErrorMessageObj(){Message="Test basarili 1"},
        //            new ErrorMessageObj(){Message="Test basarili 1"},
        //        }
        //    };
        //    return View("Error", model);
        //}
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}