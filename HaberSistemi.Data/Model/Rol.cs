using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSistemi.Data.Model
{
    [Table("Rol")]
   public class Rol
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Rol Adı :")]
        public string RolAdi { get; set; }
    }
}
