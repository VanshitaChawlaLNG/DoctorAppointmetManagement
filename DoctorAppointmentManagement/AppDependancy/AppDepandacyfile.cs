using Lamar;

namespace DoctorAppointmentManagement.AppDependancy
{
    public class AppDepandacyfile : ServiceRegistry
    {
        public AppDepandacyfile()
        {
            Scan(Scanner =>
            {
                Scanner.Assembly("DoctorAppointmentManagement.Services");
                Scanner.WithDefaultConventions();

                
                Scanner.IncludeNamespace("DoctorAppointmentManagement.Services.AppointmentServices");
                Scanner.IncludeNamespace("DoctorAppointmentManagement.Services.AddTimingData");
                // Add more namespaces if needed
            });
        }
    }
}


