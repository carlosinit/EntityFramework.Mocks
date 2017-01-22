using System.Data.Entity;

namespace CarlosInIt.EntityFramework.Mocks.Examples.Model
{
    public class FleetContext : DbContext
    {
        #region Public Properties

        public virtual DbSet<Car> Cars { get; set; }

        #endregion Public Properties
    }
}