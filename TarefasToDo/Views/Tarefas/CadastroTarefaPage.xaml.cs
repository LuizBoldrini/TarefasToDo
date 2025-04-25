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

	private async void CadastrarButton_Clicked(object sender, EventArgs e)
	{
		var nome = NomeEntry.Text;
		var descricao = DescricaoEntry.Text;
		var _conjunto = AppState.ConjuntoSelecionado;

		if(string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(descricao))
		{
			await DisplayAlert("Alerta", "Preencha todos os campos", "Ok");
			return;
		}

		if (_conjunto == null)
		{
			await DisplayAlert("Alerta", "Conjunto não indetificado, selecione o conjunto novamente!", "Ok");
			await Shell.Current.GoToAsync("///ConjuntoPage");
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
}