using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberSistemi.Data.Model
{
    [Table("Resim")]
    public class Resim
    {
        public int ID { get; set; }
        public string ResimUrl { get; set; }
        private DateTime Tarih = DateTime.Now;
        private bool Aktif = true;
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
        public bool AktifMi
        {
            get
            {
                return Aktif;
            }
            set
            {
                Aktif = value;
            }
        }
        public int HaberID { get; set; }
        public Haber Haber { get; set; }
    }
}
