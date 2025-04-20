using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Usuarios;

public partial class CadastroPage : ContentPage
{
	private readonly ServiceAPI _api = new();
	public CadastroPage()
	{
		InitializeComponent();
	}

	private async void CadastrarButton_Clicked(object sender, EventArgs e)
	{
        var nome = NomeEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim().ToLower();
        var telefone = TelefoneEntry.Text?.Trim();
        var senha = SenhaEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(senha))
		{
			await DisplayAlert("Alerta", "Preencha todos os campos", "Ok");
			return;
		}

		try
		{
			var novoUsuario = new UsuarioCadastro
			{
				Nome = nome,
				Email = email,
				Telefone = telefone,
				Senha = senha
			};

            

            var usuarioCadastrado = await _api.CadastraUsuario(novoUsuario);


            if (usuarioCadastrado != null)
			{
				await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso, realize o login!", "Ok");
				await Shell.Current.GoToAsync("///LoginPage");
			}
		} catch(Exception ex)
		{
			await DisplayAlert("Aviso", ex.Message, "Ok");
		}
    }
}