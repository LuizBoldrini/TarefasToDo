namespace TarefasToDo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Login", typeof(Views.Usuarios.LoginPage));
        }
    }
}
