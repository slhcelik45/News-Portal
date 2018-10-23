using HaberSistemi.Core.Infrastructure;
using HaberSistemi.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaberSistemi.Admin.Controllers
{
    public class AccountController : Controller
    {
        #region Kullanıcı
        private readonly IKullaniciRepository _kullaniciRepository;
        public AccountController(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }
        #endregion
        // GET: Accout
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //POST:Accout
        [HttpPost]
        public ActionResult Login(Kullanici kullanici)
        {
            var KullaniciVarMi = _kullaniciRepository.GetMany(x => x.Email == kullanici.Email && x.Sifre == kullanici.Sifre && x.AktifMi == true).SingleOrDefault();
            if (KullaniciVarMi != null)
            {
                if (KullaniciVarMi.Rol.RolAdi == "Admin")
                {
                    Session["KullaniciEmail"] = KullaniciVarMi.Email;
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Mesaj = "Yekisiz Kullanıcı !";
                return View();
            }
            ViewBag.Mesaj = "Kullanıcı Değilsiniz !";
            return View();
        }
    }
}