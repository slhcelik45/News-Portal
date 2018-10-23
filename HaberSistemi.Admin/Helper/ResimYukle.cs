using HaberSistemi.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSistemi.Admin.Helper
{
    public static class ResimYukle
    {
        public static string Resim(HttpPostedFileBase ResimURL,Slider slider)
        {
            string Dosya = Guid.NewGuid().ToString().Replace("-", "");
            string[] Uzanti = ResimURL.ContentType.Split('/');
            string TamYol = "/External/Slider^" + Dosya + "." + Uzanti[1];

            ResimURL.SaveAs(System.Web.HttpContext.Current.Server.MapPath(TamYol));
            slider.ResimUrl = TamYol;

            return slider.ResimUrl;
        }
    }
}