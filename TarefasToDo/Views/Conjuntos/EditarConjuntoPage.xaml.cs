using TarefasToDo.Models.Conjutos;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Conjuntos;

public partial class EditarConjuntoPage : ContentPage
{
    private readonly ServiceAPI _api = new();
    private ConjuntoLista _conjuntoAtual = null!;

    public EditarConjuntoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (AppState.ConjuntoSelecionado != null)
        {
            _conjuntoAtual = AppState.ConjuntoSelecionado;
            NomeEntry.Text = _conjuntoAtual.Nome;
            DescricaoEntry.Text = _conjuntoAtual.Descricao;
        }
        else
        {
            DisplayAlert("Erro", "Nenhum conjunto selecionado para edição.", "Ok");
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

        var conjuntoEditado = new ConjuntoCadastro
        {
            Nome = NomeEntry.Text,
            Descricao = DescricaoEntry.Text,
        };

        try
        {
            await _api.EditarConjunto(_conjuntoAtual.Id, conjuntoEditado);
            await DisplayAlert("Sucesso", "Grupo atualizado com sucesso!", "Ok");
            await Shell.Current.GoToAsync("///ConjuntoPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao atualizar grupo: {ex.Message}", "Ok");
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
