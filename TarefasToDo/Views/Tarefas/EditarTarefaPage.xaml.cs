using TarefasToDo.Models.Tarefas;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Tarefas;

public partial class EditarTarefaPage : ContentPage
{
    private readonly ServiceAPI _api = new();
    private TarefaLista _tarefaAtual;

    public EditarTarefaPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _tarefaAtual = AppState.TarefaSelecionada;
        
        if (_tarefaAtual != null)
        {
            NomeEntry.Text = _tarefaAtual.Nome;
            DescricaoEntry.Text = _tarefaAtual.Descricao;
        }
        else
        {
            DisplayAlert("Erro", "Nenhum conjunto ou tarefa selecionada para edição.", "Ok");
            Shell.Current.GoToAsync("///ConjuntoPage");
        }
    }

    private async void Salvar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NomeEntry.Text))
        {
            await DisplayAlert("Aviso", "Preencha os campos", "Ok");
            return;
        }
        var tarefaEditada = new TarefaCadastro
        {
            Nome = NomeEntry.Text,
            Descricao = DescricaoEntry.Text,
        };
        try
        {
            await _api.EditarTarefa(_tarefaAtual.Id, tarefaEditada);
            await DisplayAlert("Sucesso", "Tarefa atualizada com sucesso!", "Ok");
            await Shell.Current.GoToAsync("///TarefasPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao atualizar tarefa: {ex.Message}", "Ok");
        }
    }
}