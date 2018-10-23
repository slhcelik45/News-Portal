using HaberSistemi.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaberSistemi.Data.Model;
using System.Linq.Expressions;
using HaberSistemi.Data.DataContext;
using System.Data.Entity.Migrations;

namespace HaberSistemi.Core.Repository
{
    public class EtiketRepository : IEtiketRepository
    {
        private readonly HaberContext _context = new HaberContext();
        public int Count()
        {
            return _context.Etiket.Count();
        } 

        public void Delete(int id)
        {
            var Etiket = GetById(id);
            if (Etiket!=null)
            {
                _context.Etiket.Remove(Etiket);
            }
        }

        public IQueryable<Etiket> Etiketler(string[] etiketler)
        {
            return _context.Etiket.Where(x => etiketler.Contains(x.EtiketAdi));
        }

        public Etiket Get(Expression<Func<Etiket, bool>> expression)
        {
            return _context.Etiket.FirstOrDefault(expression);
        }

        public IEnumerable<Etiket> GetAll()
        {
            return _context.Etiket.Select(x => x);
        }

        public Etiket GetById(int id)
        {
            return _context.Etiket.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<Etiket> GetMany(Expression<Func<Etiket, bool>> expression)
        {
            return _context.Etiket.Where(expression);
        }

        public void EtiketEkle(int HaberID, string Etiket)
        {
            if (Etiket!=null && Etiket!="")
            {
                string[] Etikets = Etiket.Split(',');
                foreach (var tag in Etikets)
                {
                    Etiket etiket = this.Get(x => x.EtiketAdi.ToLower() == tag.ToLower().Trim());
                    if (etiket==null)
                    {
                        etiket = new Etiket();
                        etiket.EtiketAdi = tag;
                        this.Insert(etiket);
                        this.Save();
                    }
                }
                this.HaberEtiketEkle(HaberID, Etikets);
            }         
        }

        public void Insert(Etiket obj)
        {
            _context.Etiket.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Etiket obj)
        {
            _context.Etiket.AddOrUpdate();
        }

        public void HaberEtiketEkle(int HaberID, string[] etiketler)
        {
            var Haber = _context.Haber.FirstOrDefault(x => x.ID == HaberID);

            var gelenEtiket = this.Etiketler(etiketler);
            Haber.Etiket.Clear();
            gelenEtiket.ToList().ForEach(etiket => Haber.Etiket.Add(etiket));
            _context.SaveChanges();
        }
    }
}
