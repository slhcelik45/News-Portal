using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSistemi.Data.Model
{
    [Table("Kategori")]
    public class Kategori
    {
        [Key]
        public int ID{ get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "{0} Karekterden az olamaz !"), 
         MaxLength(150, ErrorMessage = "{0} Karekterden fazla olamaz !")]
        public string KategoriAdi { get; set; }
        public int ParentID { get; set; }
        [MinLength(2, ErrorMessage = "{0} karekterden az olamaz !"),
          MaxLength(150, ErrorMessage = "{0} karekteden fazla olamaz !")]
        public string Url { get; set; }
        public bool AktifMi { get; set; }
        public virtual ICollection<Haber> Haber { get; set; }
    }
}
