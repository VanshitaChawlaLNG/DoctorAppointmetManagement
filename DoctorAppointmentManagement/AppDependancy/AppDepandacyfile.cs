
using Lamar;

namespace DoctorAppointmentManagement.AppDependancy
{
    public class AppDepandacyfile:ServiceRegistry
    {

        public AppDepandacyfile()
        {
            Scan(Scanner =>
            {
                Scanner.TheCallingAssembly();
                Scanner.WithDefaultConventions();
            });
        }
    }
}
