using TarefasToDo.Models.Usuarios;

namespace TarefasToDo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Login", typeof(Views.Usuarios.LoginPage));
        }
        private async void Sair_Clicked(object sender, EventArgs e)
            {
                bool confirmar = await DisplayAlert("Aviso", "Deseja realmente sair?", "Sim", "Cancelar");
                if (confirmar)
                {
                    AppState.UsuarioAtual = null;
                    await Shell.Current.GoToAsync("///LoginPage", true);
                }
            }
        }
    }

