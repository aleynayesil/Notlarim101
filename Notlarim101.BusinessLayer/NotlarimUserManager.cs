using System;
using Notlarim101.Common.Helper;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;
using Notlarim101.Entity.ValueObject;

namespace Notlarim101.BusinessLayer
{
    public class NotlarimUserManager
    {
        //Kullanici username kontrolu yapmaliyim
        //kullnici email kontrolu yapmaliyim
        //Kayit islemini gerceklestirmeliyim
        //Activasyon e-postasi gonderimi

        private Repository<NotlarimUser> ruser = new Repository<NotlarimUser>();

        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
            NotlarimUser user = ruser.Find(s => s.Username == data.Username || s.Email == data.Email);

            BusinessLayerResult<NotlarimUser> lr = new BusinessLayerResult<NotlarimUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    lr.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanici adi kayitli");
                }

                if (user.Email == data.Email)
                {
                    lr.AddError(ErrorMessageCode.EmailalreadyExist, "Email kayitli");
                }
                //throw new Exception("Kayitli kullanici yada e-posta adresi");
            }
            else
            {
                int dbResult = ruser.Insert(new NotlarimUser()
                {
                    Name = data.Name,
                    Surname = data.Surname,
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ProfileImageFilename = "login.jpeg",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    //repository e tasindi
                    //ModifiedOn = DateTime.Now,
                    //CreatedOn = DateTime.Now,
                    //ModifiedUsername = "system"
                });
                if (dbResult > 0)
                {
                    lr.Result = ruser.Find(s => s.Email == data.Email && s.Username == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{lr.Result.ActivateGuid}";
                    string body = $"Merhaba {lr.Result.Username}; <br><br> Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'> Tıklayın </a>.";
                    MailHelper.SendMail(body, lr.Result.Email, "Notlarim101 hesap aktifleştirme");

                    //activasyon mail i atilacak
                    //lr.Result.ActivateGuid;
                }
            }

            return lr;
        }

        public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //Giris kontrolu
            //Hesap aktif edilmismi kontrolu

            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result = ruser.Find(s => s.Username == data.Username && s.Password == data.Password);
            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanici adi aktiflestirilmemis!!!");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lutfen Mailinizi kontrol edin...");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPasswordWrong, "kullanici adi yada sifre uyusmuyor.");
            }

            return res;
        }

        public BusinessLayerResult<NotlarimUser> ActivateUser(Guid id)
        {
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result = ruser.Find(x => x.ActivateGuid == id);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu hesap daha önce Aktive edilmiş");
                    return res;
                }
                res.Result.IsActive = true;
                ruser.Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Yine mi sen?");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> GetUserById(int id)
        {
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result = ruser.Find(s => s.Id == id);
            if (res.Result == null)
            {//???
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> UpdateProfile(NotlarimUser data)
        {
            NotlarimUser user = ruser.Find(s => s.Id != data.Id && (s.Username == data.Username || s.Email == data.Email));
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanıcı adı daha önce kaydedilmiş.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Bu  E-posta adresi daha önce kaydedilmiş.");
                }
                return res;
            }
            res.Result = ruser.Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (!string.IsNullOrEmpty(data.ProfileImageFilename))
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }
            if (ruser.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil Güncellenemedi.");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> RemoveUserById(int id)
        {
            NotlarimUser user = ruser.Find(s => s.Id == id);
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            if (user != null)
            {
                if (ruser.Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı");
            }
            return res;
        }
    }
}
