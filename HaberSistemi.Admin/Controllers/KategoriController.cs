using HaberSistemi.Admin.Class;
using HaberSistemi.Core.Infrastructure;
using HaberSistemi.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HaberSistemi.Admin.CustomFilter;

namespace HaberSistemi.Admin.Controllers
{
    public class KategoriController : Controller
    {
        #region Kategori
        private readonly IKategoriRepository _kategoriRepository;
        public KategoriController(IKategoriRepository kategoriRepository)
        {
            _kategoriRepository = kategoriRepository; 
        }
        #endregion
        #region Kategori Listele
        [HttpGet]
        public ActionResult Index(int Sayfa = 1)
        {
            return View(_kategoriRepository.GetAll().OrderByDescending(x => x.ID).ToPagedList(Sayfa, 5));
        }
        #endregion
        #region Kategori Ekle
        [HttpGet]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View();
        }
        [HttpPost]
        public JsonResult Ekle(Kategori kategori)
        {
            try
            {
                _kategoriRepository.Insert(kategori);
                _kategoriRepository.Save();
                return Json(new ResultJson { Success = true, Message = "Kategori Ekleme İşlemi Başarılı" });                
            }
            catch (Exception ex)
            {
                //Loglama yapılabilir.
                return Json(new ResultJson { Success = false, Message = "Kategori Eklerken Hata Oluştu !" });
            }           
        }
        #endregion
        #region Kategori Düzenle
        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            Kategori DbKategori = _kategoriRepository.GetById(id);
            if (DbKategori == null)
            {
                throw new Exception("Kategori Bulunamadı");
            }
            SetKategoriListele();
            return View(DbKategori);
        }

        [HttpPost]
        [LoginFilter]
        public JsonResult Duzenle(Kategori kategori)
        {

            Kategori dbkategori = _kategoriRepository.GetById(kategori.ID);
            dbkategori.AktifMi = kategori.AktifMi;
            dbkategori.KategoriAdi = kategori.KategoriAdi;
            dbkategori.ParentID = kategori.ParentID;
            dbkategori.Url = kategori.Url;
            _kategoriRepository.Save();
            return Json(new ResultJson { Success = true, Message = "Düzenleme İşlemi Başarılı !" });

            // return Json(new ResultJson { Success=true,Message="Düzenleme işlemi sırasında bir hata oluştu !"});
        }
        #endregion
        #region Kategori Sil
        public JsonResult Sil(int ID)
        {
            Kategori dbKategori = _kategoriRepository.GetById(ID);
            if (dbKategori == null)
            {
                return Json(new ResultJson { Success = true, Message = "Kategori Bulunamadı !" });
            }
            _kategoriRepository.Delete(ID);
            _kategoriRepository.Save();
            return Json(new ResultJson { Success = true, Message = "Kategori Silme İşlemi Başarılı !" });
        }
        #endregion
        #region Set Kategori
        public void SetKategoriListele()
        {
            var KategoriList = _kategoriRepository.GetMany(x => x.ParentID == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }
        #endregion 
    }
}