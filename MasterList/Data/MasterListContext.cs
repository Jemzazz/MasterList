using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterList.Models;

namespace MasterList.Data
{
    public class MasterListContext : DbContext
    {
        public MasterListContext (DbContextOptions<MasterListContext> options)
            : base(options)
        {
        }

        public DbSet<MasterList.Models.MList> MList { get; set; } = default!;
    }
}
