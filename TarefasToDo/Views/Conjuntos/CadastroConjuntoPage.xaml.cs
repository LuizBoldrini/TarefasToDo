using TarefasToDo.Models.Conjutos;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Conjuntos;

public partial class CadastroConjuntoPage : ContentPage
{
	private readonly ServiceAPI _api = new();
	public CadastroConjuntoPage()
	{
		InitializeComponent();
	}

    private async void CadastrarButton_Clicked(object sender, EventArgs e)
    {
        var nome = NomeEntry.Text;
        var descricao = DescricaoEntry.Text;
        var _usuario = AppState.UsuarioAtual;

        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(descricao))
        {
            await DisplayAlert("Alerta", "Preencha todos os campos", "Ok");
            return;
        }

        if (_usuario == null)
        {
            await DisplayAlert("Alerta", "Usuário não identificado, realize o login novamente!", "Ir para login");
            await Shell.Current.GoToAsync("///LoginPage");
            return;
        }

        try
        {
            var novoConjuto = new ConjuntoCadastro
            {
                Nome = nome,
                Descricao = descricao,
                UsuarioId = _usuario.Id
            };

            var conjunto = await _api.CadastraConjunto(novoConjuto);
            await DisplayAlert("Sucesso", "Grupo criado com sucesso", "Ok");
            await Shell.Current.GoToAsync("///ConjuntoPage");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no Cadastro de conjunto:{ex.Message}");
            await DisplayAlert("Aviso", ex.Message, "OK");
        }
    }
    private async void VoltarButton_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Aviso", "Deseja cancelar o cadastro?", "Sim", "Não");

        if (confirmar)
        {
            await Shell.Current.GoToAsync("///ConjuntoPage", true);
        }
    }
}