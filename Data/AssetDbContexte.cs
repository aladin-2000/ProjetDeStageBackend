using backEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Data{
    public class AssetDbContexte : DbContext{

        public AssetDbContexte(DbContextOptions<AssetDbContexte> options) : base(options)
        {
        }
        public DbSet<Asset> Assets { get; set; }



    }
}