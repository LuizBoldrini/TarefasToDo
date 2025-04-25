using System.Collections.ObjectModel;
using TarefasToDo.Models.Tarefas;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Tarefas;

public partial class TarefasPage : ContentPage
{
	private readonly ServiceAPI _api = new();
	public ObservableCollection<TarefaLista> Tarefas { get; set; } = new();

    public TarefasPage()
	{
		InitializeComponent();
        BindingContext = this;
        
        var label = new Label
        {
            Text = "",
            Style = Application.Current?.Resources["ShellTitleHidden"] as Style ?? new Style(typeof(Label))
        };
    
        Shell.SetTitleView(this, label);
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();

        var _usuario = AppState.UsuarioAtual;
        var _conjunto = AppState.ConjuntoSelecionado;
        if (_conjunto != null && _usuario != null)
        {
            ConjuntoSpan.Text = _conjunto.Nome;
            CarregaTarefas(_conjunto.Id);
        }
        else
        {
            DisplayAlert("Alerta", "Grupo não identificado, retorne a tela inicial!", "Ok");
            Shell.Current.GoToAsync("///ConjuntoPage");
        }
    }

    private async void CarregaTarefas(int conjuntoId)
    {
        try
        {
            var tarefas = await _api.ListaTarefasConjunto(conjuntoId);
            Tarefas.Clear();

            if (tarefas.Count == 0)
            {
                SemTarefaLabel.IsVisible = true;
            }
            else
            {
                SemTarefaLabel.IsVisible = false;
                foreach (var tarefa in tarefas.OrderBy(t => ObterPrioridade(t.Status)).ThenBy(t => t.Nome))
                {
                    Tarefas.Add(tarefa);
                }
                TarefaCollectionView.ItemsSource = Tarefas;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar tarefas: {ex.Message}");
            await DisplayAlert("Erro", "Falha ao carregar tarefas", "Ok");
        }
    }

    private async void Voltar_Clicked(object sender, EventArgs e)
    {
        AppState.ConjuntoSelecionado = null;
        await Shell.Current.GoToAsync("///ConjuntoPage", true);
    }

    private async void CriarTarefa_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///CadastroTarefaPage", true);
    }

    private async void MostraOpcoes_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is TarefaLista tarefasSelecionado)
        {
            string opcao = await DisplayActionSheet("Opções", "Cancelar", null, "Editar", "Excluir");

            switch (opcao)
            {
                case "Editar":
                    AppState.TarefaSelecionada = tarefasSelecionado;
                    await Shell.Current.GoToAsync("///EditarTarefaPage");
                    break;
                case "Excluir":
                    bool confirmar = await DisplayAlert("Aviso", "Deseja realmente excluir?", "Sim", "Cancelar");
                    if (confirmar)
                    {
                        bool sucesso = await _api.DeletarTarefa(tarefasSelecionado.Id);
                        if (sucesso)
                        {
                            Tarefas.Remove(tarefasSelecionado);
                            await DisplayAlert("Sucesso", "Tarefa excluída com sucesso!", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Erro", "Falha ao excluir tarefa", "Ok");
                        }
                    }
                    break;
            }
        }
        else
        {
            await DisplayAlert("Erro", "Tarefa não encontrada", "Ok");
        }
    }

    private async void Tarefa_Tapped(object sender, EventArgs e)
    {
        if (sender is Border border && border.BindingContext is TarefaLista tarefa)
        {
            await border.ScaleTo(0.95, 100, Easing.CubicIn);
            await border.ScaleTo(1, 100, Easing.CubicOut);
            await border.FadeTo(0.5, 100);
            await border.FadeTo(1, 100);

            string novoStatus = await DisplayActionSheet(
                "Alterar status da tarefa",
                "Cancelar", null, "Aberta", "Em andamento", "Completa");

            if(novoStatus == "Cancelar" || string.IsNullOrWhiteSpace(novoStatus) || novoStatus == tarefa.Status)
            {
                return;
            }
            try
            {
                var payload = new { status = novoStatus };
                var tarefaAtualizada = await _api.AlterarStatusTarefa(tarefa.Id, payload);

                var tarefaExistente = Tarefas.FirstOrDefault(t => t.Id == tarefa.Id);
                if (tarefaExistente != null)
                {
                    tarefaExistente.Status = tarefaAtualizada.Status;
                    ReordenarTarefas();
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"{ex.Message}", "OK");
            }
        }
    }

    private void ReordenarTarefas()
    {
        var tarefasOrdenadas = Tarefas
            .OrderBy(t => ObterPrioridade(t.Status))
            .ThenBy(t => t.Nome)
            .ToList();

        Tarefas.Clear();
        foreach (var tarefa in tarefasOrdenadas)
        {
            Tarefas.Add(tarefa);
        }
    }

    private static int ObterPrioridade(string status)
    {
        return status switch
        {
            "Aberta" => 0,
            "Em andamento" => 1,
            "Completa" => 2,
            _ => 3
        };
    }
}