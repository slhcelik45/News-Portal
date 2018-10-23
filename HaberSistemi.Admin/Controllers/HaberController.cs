using HaberSistemi.Admin.CustomFilter;
using HaberSistemi.Core.Infrastructure;
using HaberSistemi.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Text;

namespace HaberSistemi.Admin.Controllers
{
    public class HaberController : Controller
    {
        #region VeriTabanı
        private readonly IHaberRepository _haberRepository;
        private readonly IKategoriRepository _kategoriRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IResimRepository _resimRepository;
        private readonly IEtiketRepository _etiketRepository;
        public HaberController(IHaberRepository haberRepository, IKategoriRepository kategoriRepository, IKullaniciRepository kullaniciRepository,IResimRepository resimRepository,IEtiketRepository etiketRepository)
        {
            _haberRepository = haberRepository;
            _kullaniciRepository = kullaniciRepository;
            _kategoriRepository = kategoriRepository;
            _resimRepository = resimRepository;
            _etiketRepository = etiketRepository;
        }

        #endregion
        #region Haber Listele
        [LoginFilter]
        public ActionResult Index(int Sayfa=1)
        {
            var HaberListesi = _haberRepository.GetAll();
            return View(HaberListesi.OrderByDescending(x=>x.ID).ToPagedList(Sayfa,20));
        }
        #endregion
        #region Haber Ekle
        [HttpGet]
        [LoginFilter]
        public ActionResult Ekle()
        {
            SetKategoriListele();
            return View(); 
        }

        [HttpPost]   
        [LoginFilter]
        public ActionResult Ekle(Haber haber,int KategoriID,HttpPostedFileBase VitrinResim,IEnumerable<HttpPostedFileBase> DetayResim,string Etiket)
        {
            //var SessionControl = HttpContext.Session["KullaniciEmail"];
            //Kullanici kullanici = _kullaniciRepository.GetById(Convert.ToInt32(SessionControl));

            haber.KullaniciID = 3;
            haber.KategoriID = KategoriID;            

            if (VitrinResim != null)
            {
                string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                string Uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYol = "/External/Haber/" + DosyaAdi + Uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYol));
                haber.Resim = TamYol;
            }
            _haberRepository.Insert(haber);
            _haberRepository.Save();
            
            _etiketRepository.EtiketEkle(haber.ID,Etiket);

            string CokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if (CokluResim != "")
            {
                foreach (var file in DetayResim)
                {
                    if (file.ContentLength > 0)
                    {
                        string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string Uzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                        string TamYol = "/External/Haber/" + DosyaAdi + Uzanti;
                        file.SaveAs(Server.MapPath(TamYol));

                        var resim = new Resim
                        {
                            ResimUrl = TamYol,
                            EklemeTarihi = DateTime.Now,
                            AktifMi = true
                        };
                        resim.HaberID = haber.ID;
                        _resimRepository.Insert(resim);
                        _resimRepository.Save();
                    }
                }
            }
            TempData["Bilgi"] = "Haber Ekleme İşleminiz Başarılı !";
                return RedirectToAction("Index", "Haber");        
            
        }
        #endregion
        #region Haber Sil
        public ActionResult Sil(int id)
        {
            Haber dbhaber = _haberRepository.GetById(id);
            var dbDetayResim = _resimRepository.GetMany(x => x.HaberID == id);
            if (dbhaber==null)
            {
                throw new Exception("Haber Bulunamadı !");
            }          
            string file_name = dbhaber.Resim;
            string yol = Server.MapPath(file_name);
            FileInfo file = new FileInfo(yol);
            if (file.Exists)//Dosyanın varlığını kontrol ediyor.Fiziksel olarak var ise siliyor.
            {
                file.Delete();
            }
            if (dbDetayResim!=null)
            {
                foreach (var item in dbDetayResim)
                {
                    string detayPath = Server.MapPath(item.ResimUrl);
                    FileInfo files = new FileInfo(detayPath);
                    if (files.Exists)
                    {
                        files.Delete();
                    }
                }
            }
            _haberRepository.Delete(id);
            _haberRepository.Save();
            TempData["Bilgi"] = "Haber Başarılı Bir Şekilde Silindi !";

            return RedirectToAction("Index", "Haber"); 
        }
        #endregion
        #region Haber Düzenle
        [HttpGet]
        [LoginFilter]
        public ActionResult Duzenle(int id)
        {
            Haber gelenHaber = _haberRepository.GetById(id);
            #region Etiket
            var gelenEtiket = gelenHaber.Etiket.Select(x => x.EtiketAdi).ToArray();
            HaberEtiketModel model = new HaberEtiketModel
            {
                Haber = gelenHaber,
                Etiketler = _etiketRepository.GetAll(),
                GelenEtiketler = gelenEtiket
            };
            StringBuilder birlestir = new StringBuilder();
            foreach (var tag in model.GelenEtiketler)
            {
                birlestir.Append(tag.ToString());
                birlestir.Append(",");
            }
            model.EtiketAd = birlestir.ToString();
            #endregion

            if (gelenHaber == null)
            {
                throw new Exception("Haber Bulunamadı");
            }
            else
            {
                SetKategoriListele();
                return View(model);
            }
        }

        [HttpPost]
        [LoginFilter]
        public ActionResult Duzenle(Haber haber,HttpPostedFileBase VitrinResim,IEnumerable<HttpPostedFileBase> DetayResim,string EtiketAd)
        {
            Haber gelenHaber = _haberRepository.GetById(haber.ID);
            gelenHaber.Baslik = haber.Baslik;
            gelenHaber.Aciklama = haber.Aciklama;
            gelenHaber.KisaAciklama = haber.KisaAciklama;
            gelenHaber.AktifMi = haber.AktifMi;
            gelenHaber.KategoriID=haber.KategoriID;

            if (VitrinResim!=null)
            {
                string dosyaAdi = gelenHaber.Resim;
                string dosyaYolu = Server.MapPath(dosyaAdi);
                FileInfo dosya = new FileInfo(dosyaYolu);
                if (dosya.Exists)
                {
                    dosya.Delete();
                }
                string fileName = Guid.NewGuid().ToString().Replace("-", "");
                string Uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
                string TamYol = "/External/Haber/" + fileName + Uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYol));
                gelenHaber.Resim = TamYol;               
            }
            string cokluResim = System.IO.Path.GetExtension(Request.Files[1].FileName);
            if (cokluResim!="")
            {
                foreach (var item in DetayResim)
                {
                    string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    string resimUzanti = System.IO.Path.GetExtension(Request.Files[1].FileName);
                    string CokluTamyol = "/External/Haber/+" + DosyaAdi + resimUzanti;
                    item.SaveAs(Server.MapPath(CokluTamyol));                   

                    var img = new Resim
                    {
                        ResimUrl = CokluTamyol,
                        EklemeTarihi = DateTime.Now,
                        AktifMi = true
                    };
                    img.HaberID = gelenHaber.ID;
                    _resimRepository.Insert(img);
                    _resimRepository.Save(); 
                }
            }
            _etiketRepository.EtiketEkle(haber.ID, EtiketAd);
            _haberRepository.Save();
            TempData["Bilgi"] = "Güncelleme İşlemi Başarılı";
            return RedirectToAction("Index", "Haber");
        }
        #endregion
        #region Set kategori Listesi
        public void SetKategoriListele(object kategori = null)
        {
            var KategoriList = _kategoriRepository.GetMany(x => x.ParentID == 0).ToList();
            ViewBag.Kategori = KategoriList;
        }
        #endregion
        #region Aktif / Pasif Yap
        public ActionResult Onay(int id)
        {
            Haber gelenHaber = _haberRepository.GetById(id);
            if (gelenHaber.AktifMi==true)
            {
                gelenHaber.AktifMi = false;
                _haberRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı .";
                return RedirectToAction("Index", "Haber");
            }
            else if (gelenHaber.AktifMi==false)
            {
                gelenHaber.AktifMi = true;
                _haberRepository.Save();
                TempData["Bilgi"] = "İşleminiz Başarılı .";
                return RedirectToAction("Index", "Haber");
            }
            return View();
        }
        #endregion
        #region Ressim Sil
        public ActionResult ResimSil(int id)
        {
            Resim dbResim = _resimRepository.GetById(id);
            if (dbResim==null)
            {
                throw new Exception("Resim Bulunamadı !");
            }
            string dosyaAdi = dbResim.ResimUrl;
            string path = Server.MapPath(dosyaAdi);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            _resimRepository.Delete(id);
            _resimRepository.Save();
            TempData["Bilgi"] = "Silme İşlemi Başarılı !";
            return RedirectToAction("Index", "Haber");
        }
        #endregion
    }
}