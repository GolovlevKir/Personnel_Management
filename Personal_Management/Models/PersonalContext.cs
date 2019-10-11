using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Personal_Management.Models
{
    //использует EntityFramework для доступа к БД на основе некоторой модели
    public class PersonalContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Isp_Sroki> Isp_Sroki { get; set; }
        public DbSet<Pass_Dannie> Pass_Dannie { get; set; }
        public DbSet<Positions> Positions { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Rates> Rates { get; set; }
        public DbSet<Sbor_Docum> Sbor_Docum { get; set; }
        public DbSet<Sotrs> Sotrs { get; set; }
        public DbSet<status_isp_sroka> status_isp_sroka { get; set; }
        public DbSet<Work_Schedule> Work_Schedule { get; set; }
        public DbSet<Zar_Plata> Zar_Plata { get; set; }
    }
}