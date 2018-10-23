using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSistemi.Data.Model
{
    [Table("Etiket")]
    public class Etiket
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Etiket Adı :")]
        public string EtiketAdi { get; set; }
        public virtual ICollection<Haber> Haber { get; set; }

    }
}
