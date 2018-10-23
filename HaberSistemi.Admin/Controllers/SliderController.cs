using HaberSistemi.Admin.Class;
using HaberSistemi.Admin.CustomFilter;
using HaberSistemi.Core.Infrastructure;
using HaberSistemi.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using HaberSistemi.Admin.Helper;
using System.IO;

namespace HaberSistemi.Admin.Controllers
{
    public class SliderController : Controller
    {
        #region SliderDB
        private readonly ISliderRepository _sliderRepository;
        public SliderController(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }
        #endregion       
        #region Slider Listele
        public ActionResult Index(int sayfa = 1)
        {
            var slider = _sliderRepository.GetAll();
            return View(slider.OrderByDescending(x => x.ID).ToPagedList(sayfa, 10));
        }
        #endregion
        #region Slider Ekle
        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [LoginFilter]
        public JsonResult Ekle(Slider slider, HttpPostedFileBase ResimURL)
        {
            if (slider.ResimUrl != null)
            {
                if (ResimURL.ContentLength > 0)
                {
                    string Dosya = Guid.NewGuid().ToString().Replace("-", "");
                    string Uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    string ResimYolu = "/External/Slider/" + Dosya + Uzanti;
                    ResimURL.SaveAs(Server.MapPath(ResimYolu));
                    slider.ResimUrl = ResimYolu;
                }
                _sliderRepository.Insert(slider);
                try
                {
                    _sliderRepository.Save();
                    return Json(new ResultJson { Success = true, Message = "Slider Ekleme İşlemi Başarılı :)" });
                }
                catch (Exception)
                {

                    return Json(new ResultJson { Success = false, Message = "Slider Ekleme İşlemi Başarısız !" });
                }
            }
            return Json(new ResultJson { Success = false, Message = "Slider Ekleme İşlemi Başarılı !" });
        }
        #endregion
        #region Slider Düzenle
        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int ID)
        {
            var gelenSlider = _sliderRepository.GetById(ID);
            if (gelenSlider != null)
            {
                return View(gelenSlider);
            }
            return View();
        }
        [HttpPost]
        [LoginFilter]
        public JsonResult Duzenle(Slider slider, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                Slider DbSlider = _sliderRepository.GetById(slider.ID);
                DbSlider.Baslik = slider.Baslik;
                DbSlider.Url = slider.Url;
                DbSlider.Aciklama = slider.Aciklama;
                DbSlider.AktifMi = slider.AktifMi;
                DbSlider.EklemeTarihi = DateTime.Now;

                if (ResimURL != null && ResimURL.ContentLength > 0)
                {
                    if (DbSlider.ResimUrl != null)
                    {
                        string URL = DbSlider.ResimUrl;
                        string resimPath = Server.MapPath(URL);
                        FileInfo files = new FileInfo(resimPath);
                        if (files.Exists)
                        {
                            files.Delete();
                        }
                    }
                    ResimYukle.Resim(ResimURL, slider);
                    DbSlider.ResimUrl = slider.ResimUrl;
                }
                try
                {
                    _sliderRepository.Save();
                    return Json(new ResultJson { Success = true, Message = slider.Baslik + " Slider Güncelleme İşlemi Başarılı :)" });
                }
                catch (Exception ex)
                {

                    return Json(new ResultJson { Success = false, Message = slider.Baslik + " Slider Güncelleme İşlemi Başarısız :(" });
                }
            }
            return Json(new ResultJson { Success = false, Message = " Bilinmeyen Hata Oluştu :(" });
        }
        #endregion
        #region Slider Sil
        public JsonResult Sil(Slider slider)
        {
            Slider dbSlider = _sliderRepository.GetById(slider.ID);
            if (dbSlider == null)
            {
                return Json(new ResultJson { Success = false, Message = slider.Baslik + " Slider Bulunamadı :(" });
            }
            try
            {
                if (dbSlider.ResimUrl != null)
                {
                    string ResimUrl = dbSlider.ResimUrl;
                    string ResimPath = Server.MapPath(ResimUrl);
                    FileInfo file = new FileInfo(ResimPath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _sliderRepository.Delete(slider.ID);
                _sliderRepository.Save();
                return Json(new ResultJson { Success = true, Message = slider.Baslik + " Slider Silme İşlemi Başarılı :)" });
            }
            catch (Exception)
            {

                return Json(new ResultJson { Success = false, Message = slider.Baslik + " Slider Silme İşlemi Başarısız :(" });
            }
        }
        #endregion
    }
}
