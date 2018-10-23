using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HaberSistemi.Data.Model
{
    [Table("Haber")]
    public class Haber 
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Haber Başlık")]
        [MaxLength(255, ErrorMessage = "Çok Faza Girdiniz !")]
        [Required(ErrorMessage = "Haber Başlığı Boş Geçilemez !")] 
        public string Baslik { get; set; }
        [Display(Name = "Kısa Açıklama")]        
        public string KisaAciklama { get; set; }
        [Display(Name = "Uzun Açıklama")]        
        public string Aciklama { get; set; }
        public int Okunma { get; set; }
        [Display(Name = "Aktif Mi")]
        public bool AktifMi { get; set; }
        private DateTime Tarih = DateTime.Now;
        [Display(Name = "Ekleme Tarihi")]
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
        [Display(Name = "Haber Resim")]
        [MaxLength(255, ErrorMessage = "Çok Faza Girdiniz !")]
        [Required]
        public string Resim { get; set; }
        public int KullaniciID { get; set; }
        public int KategoriID { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual ICollection<Resim> Resims { get; set; }
        public virtual ICollection<Etiket> Etiket { get; set; }
        public virtual Kategori Kategori { get; set; }              
    }
}
