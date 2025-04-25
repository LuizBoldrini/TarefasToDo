using TarefasToDo.Models.Tarefas;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Tarefas;

public partial class EditarTarefaPage : ContentPage
{
    private readonly ServiceAPI _api = new();
    private TarefaLista _tarefaAtual = null!;

    public EditarTarefaPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AppState.TarefaSelecionada != null)
        {
            _tarefaAtual = AppState.TarefaSelecionada;
            NomeEntry.Text = _tarefaAtual.Nome;
            DescricaoEntry.Text = _tarefaAtual.Descricao;
        }
        else
        {
            DisplayAlert("Erro", "Nenhum conjunto ou tarefa selecionada para edição.", "Ok");
            Shell.Current.GoToAsync("///TarefasPage");
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
            await Shell.Current.GoToAsync("///TarefasPage", true);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao atualizar tarefa: {ex.Message}", "Ok");
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
