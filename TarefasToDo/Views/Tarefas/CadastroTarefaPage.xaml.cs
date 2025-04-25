using TarefasToDo.Models.Tarefas;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Tarefas;

public partial class CadastroTarefaPage : ContentPage
{
	private readonly ServiceAPI _api = new();
	public CadastroTarefaPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        NomeEntry.Text = string.Empty;
        DescricaoEntry.Text = string.Empty;
    }

    private async void CadastrarButton_Clicked(object sender, EventArgs e)
    {
        var nome = NomeEntry.Text;
        var descricao = DescricaoEntry.Text;
        var _conjunto = AppState.ConjuntoSelecionado;

        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(descricao))
        {
            await DisplayAlert("Alerta", "Preencha todos os campos", "Ok");
            return;
        }

        if (_conjunto == null)
        {
            await DisplayAlert("Alerta", "Conjunto não identificado, selecione o conjunto novamente!", "Ok");
            await Shell.Current.GoToAsync("///ConjuntoPage", true);
            return;
        }

        try
        {
            var novaTarefa = new TarefaCadastro
            {
                Nome = nome,
                Descricao = descricao,
                ConjuntoId = _conjunto.Id
            };

            var tarefa = await _api.CadastraTarefa(novaTarefa);
            await DisplayAlert("Sucesso", "Tarefa cadastrada com sucesso!", "Ok");
            await Shell.Current.GoToAsync("///TarefasPage");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar tarefa: {ex.Message}");
            await DisplayAlert("Erro", ex.Message, "Ok");
        }
    }

    private async void VoltarButton_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Aviso", "Deseja cancelar o cadastro?", "Sim", "Não");

        if (confirmar)
        {
            await Shell.Current.GoToAsync("///TarefasPage", true);
        }
    }
}