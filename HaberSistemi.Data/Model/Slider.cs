using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Slider")]
    public class Slider
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Başlık :")]
        [MinLength(3,ErrorMessage ="En az {0} Karekter Olabilir !"),
         MaxLength(255,ErrorMessage ="{1} Karekterden Fazla Olamaz !")]
        public string Baslik { get; set; }
        [Display(Name = "URL :")]
        [MinLength(3, ErrorMessage = "En az {0} Karekter Olabilir !"),
         MaxLength(255, ErrorMessage = "{1} Karekterden Fazla Olamaz !")]
        public string Url { get; set; }
        [Display(Name = "Aciklama :")]
        [MinLength(3, ErrorMessage = "En az {0} Karekter Olabilir !"),
         MaxLength(255, ErrorMessage = "{1} Karekterden Fazla Olamaz !")]
        public string Aciklama { get; set; }
        [Display(Name = "Resim :")]
        [Required(ErrorMessage ="Zorunlu Alan ! Boş Geçilmez ")]
        [MinLength(3, ErrorMessage = "En az {0} Karekter Olabilir !"),
         MaxLength(255, ErrorMessage = "{1} Karekterden Fazla Olamaz !")]
        public string ResimUrl { get; set; }
        [Display(Name ="Aktif Mi :")]
        public bool AktifMi { get; set; }
        private DateTime Tarih = DateTime.Now;
        [Display(Name ="Ekleme Tarihi :")]
        public DateTime EklemeTarihi
        {
            get
            {
                return Tarih;
            }
            set
            {
                Tarih = value;
            }
        }

    }
}
