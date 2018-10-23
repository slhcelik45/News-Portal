using HaberSistemi.Data.Model;
using System.Linq;

namespace HaberSistemi.Core.Infrastructure
{
    public interface IEtiketRepository:IRepository<Etiket>
    {
        IQueryable<Etiket> Etiketler(string[] etiketler);

        void EtiketEkle(int HaberID, string Etiket);
        void HaberEtiketEkle(int HaberID, string[] etiketler);
       
    }
}
