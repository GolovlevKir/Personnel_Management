using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Personal_Management.Models
{
    //используется EntityFramework для доступа к БД на основе некоторой модели
    public class PersonalContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<DaysOfWeek> DaysOfWeek { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Isp_Sroki> Isp_Sroki { get; set; }
        public DbSet<Pass_Dannie> Pass_Dannie { get; set; }
        public DbSet<Posit_Responsibilities> Posit_Responsibilities { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<Rates> Rates { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Sbor_Docum> Sbor_Docum { get; set; }
        public DbSet<Sotrs> Sotrs { get; set; }
        public DbSet<status_isp_sroka> status_isp_sroka { get; set; }
        public DbSet<Steps> Steps { get; set; }
        public DbSet<Work_Schedule> Work_Schedule { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<ZayavkaNaSobes> ZayavkaNaSobes { get; set; }
        public DbSet<Obrabotka> Obrabotka { get; set; }
    }

        public interface IReposit : IDisposable
    {
        List<Rates> GetComputerList();
        Rates GetComputer(int id);
        void Create(Rates item);
        void Update(Rates item);
        void Delete(int id);
        void Save();
    }

    public class ComputerRepository : IReposit
    {
        private PersonalContext db;
        public ComputerRepository()
        {
            this.db = new PersonalContext();
        }
        public List<Rates> GetComputerList()
        {
            return db.Rates.ToList();
        }
        public Rates GetComputer(int id)
        {
            return db.Rates.Find(id);
        }

        public void Create(Rates c)
        {
            db.Rates.Add(c);
        }

        public void Update(Rates c)
        {
            db.Entry(c).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Rates c = db.Rates.Find(id);
            if (c != null)
                db.Rates.Remove(c);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}