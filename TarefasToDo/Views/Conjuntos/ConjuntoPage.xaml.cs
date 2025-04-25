using System.Collections.ObjectModel;
using TarefasToDo.Models.Conjutos;
using TarefasToDo.Models.Usuarios;
using TarefasToDo.Service;

namespace TarefasToDo.Views.Conjuntos;

public partial class ConjuntoPage : ContentPage
{
    private readonly ServiceAPI _api = new();
    public ObservableCollection<ConjuntoLista> Conjuntos { get; set; } = new();

    public ConjuntoPage()
    {
        InitializeComponent();
        BindingContext = this;


    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        var label = new Label
        {
            Text = "",
            Style = Application.Current?.Resources["ShellTitleHidden"] as Style ?? new Style(typeof(Label))
        };

        Shell.SetTitleView(this, label);

        var _usuario = AppState.UsuarioAtual;

        if (_usuario != null)
        {
            WelcomeLabel.Text = $"Bem-vindo, {_usuario.Nome}!";
            CarregaConjuntos(_usuario.Id);
        }
        else
        {
            DisplayAlert("Alerta", "Usuário não identificado, realize o login novamente!", "Ir para login");
            Shell.Current.GoToAsync("///LoginPage");
        }
    }

    private async void CarregaConjuntos(int usuarioId)
    {
        try
        {
            var conjuntos = await _api.ListaConjuntosUsuario(usuarioId);
            Conjuntos.Clear();

            if (conjuntos.Count == 0)
            {
                SemConjuntoLabel.IsVisible = true;
            }
            else
            {
                SemConjuntoLabel.IsVisible = false;
                foreach (var conjunto in conjuntos)
                {
                    Conjuntos.Add(conjunto);
                }
                ConjuntoCollectionView.ItemsSource = Conjuntos;
            }
        }
        catch
        {
            await DisplayAlert("Erro", "Falha ao carregar conjuntos", "Ok");
        }
    }

    private async void MostraOpcoes_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var conjuntoSelecionado = button?.BindingContext as ConjuntoLista;

        if (conjuntoSelecionado == null)
        {
            await DisplayAlert("Erro", "Conjunto não encontrado!", "Ok");
            return;
        }

        string opcao = await DisplayActionSheet("Opções", "Cancelar", null, "Editar", "Excluir");

        switch (opcao)
        {
            case "Editar":
                if (conjuntoSelecionado != null)
                {
                    AppState.ConjuntoSelecionado = conjuntoSelecionado;
                    await Shell.Current.GoToAsync("///EditarConjuntoPage");
                }
                break;
            case "Excluir":
                bool confirmar = await DisplayAlert("Confirmar", $"Tem certeza que deseja excluir o conjunto \"{conjuntoSelecionado.Nome}\"?", "Sim", "Cancelar");

                if (confirmar)
                {
                    try
                    {
                        bool sucesso = await _api.DeletarConjunto(conjuntoSelecionado.Id);

                        if (sucesso)
                        {
                            Conjuntos.Remove(conjuntoSelecionado);
                            await DisplayAlert("Sucesso", "Conjunto excluído com sucesso!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Erro", "Falha ao excluir o conjunto.", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", ex.Message, "OK");
                    }
                }
                break;
        }
    }

    private async void CriarConjunto_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///CadastroConjuntoPage");
    }

    private async void Conjunto_Tapped(object sender, EventArgs e)
    {
        if (sender is Border border && border.BindingContext is ConjuntoLista conjunto)
        {
            try
            {
                await border.ScaleTo(0.95, 100, Easing.CubicIn);
                await border.ScaleTo(1, 100, Easing.CubicOut);
                AppState.ConjuntoSelecionado = conjunto;
                await Shell.Current.GoToAsync("///TarefasPage", true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao abrir conjunto: {ex.Message}", "OK");
            }
        }
    }
}
