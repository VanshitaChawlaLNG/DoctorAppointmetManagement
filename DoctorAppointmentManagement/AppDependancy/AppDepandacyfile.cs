using Lamar;

namespace DoctorAppointmentManagement.AppDependancy
{
    public class AppDepandacyfile : ServiceRegistry
    {
        public AppDepandacyfile()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory(assembly =>
                   assembly.GetName().Name.StartsWith("DoctorAppointmentManagement."));
            });
        }
    }
}


