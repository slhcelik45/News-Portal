using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSistemi.Data.Model
{
    [Table("Kullanici")]
    public class Kullanici
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(150, ErrorMessage = "Lütfen 50 Karekterden Fazla Değer Girmeyiniz !")]
        [Display(Name = "Ad Soyad :")]
        public string AdSoyad { get; set; }
        [Required]
        [Display(Name = "E-mail :")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
          ErrorMessage = "Geçerli E-mail Adresi Giriniz !")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "Lütfen 16 Karekterden Fazla Deger Girmeyiniz !")]       
        public string Sifre { get; set; }
        private DateTime Tarih = DateTime.Now;
        [Display(Name = "Kayıt Tarihi :")]
        public DateTime KayitTarihi
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
        [Display(Name = "Aktif Mi :")]
        public bool AktifMi { get; set; }
        public virtual Rol Rol { get; set; }
    }
}
