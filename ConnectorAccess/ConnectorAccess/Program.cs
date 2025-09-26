using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using System.IO;
using log4net.Config;

namespace ConnectorAccess
{
    static class Program
    {
        public static SystemUser systemUserLogged = null;
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            XmlConfigurator.Configure();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
            Environment.Exit(0); // <-- força encerramento completo
        }
    }
}
