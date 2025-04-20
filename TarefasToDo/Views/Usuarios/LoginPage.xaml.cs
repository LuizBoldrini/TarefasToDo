using TarefasToDo.Service;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Views.Conjuntos;
namespace TarefasToDo.Views.Usuarios;


public partial class LoginPage : ContentPage
{
	private readonly ServiceAPI _api = new();

	public LoginPage() 
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        AppState.UsuarioAtual = null;
        EmailEntry.Text = string.Empty;
        SenhaEntry.Text = string.Empty;
    }
    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text;
        var senha = SenhaEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            await DisplayAlert("Aviso", "Preencha todos os campos", "OK");
            return;
        }
        try
        {
            var usuario = await _api.LoginUsuario(email, senha);
            if (usuario != null)
            {
                AppState.UsuarioAtual = null;
                AppState.UsuarioAtual = usuario;
                Console.WriteLine($"✅ Login efetuado com: {usuario.Email}");
                await Shell.Current.GoToAsync("///ConjuntoPage");
            }
            else
            {
                await DisplayAlert("Aviso", "Usuário ou senha inválidos", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no login:{ex.Message}");
            await DisplayAlert("Aviso", ex.Message, "OK");
        }
    }

    private async void CadastrarButton_Clicked(object sender,EventArgs e)
    {
        await Shell.Current.GoToAsync("///CadastroPage");
    }
}