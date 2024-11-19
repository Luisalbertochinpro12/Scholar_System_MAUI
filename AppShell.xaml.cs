namespace Scholar_System
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrando un ruta al nuevo archivo que creamos
            Routing.RegisterRoute(nameof(FormPage), typeof(FormPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
