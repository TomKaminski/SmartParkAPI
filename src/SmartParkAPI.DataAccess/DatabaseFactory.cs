using SmartParkAPI.Model;

namespace SmartParkAPI.DataAccess
{
    public class DatabaseFactory:IDatabaseFactory
    {
        private ParkingAthContext _context;
        private bool _disposed;

        public ParkingAthContext Get()
        {
            return _context ?? (_context = new ParkingAthContext());
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                _context.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
