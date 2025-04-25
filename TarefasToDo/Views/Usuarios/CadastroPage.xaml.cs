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

    protected override void OnAppearing()
    {
        NomeEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        TelefoneEntry.Text = string.Empty;
        SenhaEntry.Text = string.Empty;
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
        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso", ex.Message, "Ok");
        }
    }

    private async void VoltarButton_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Aviso", "Deseja cancelar o cadastro?", "Sim", "Não");

        if (confirmar)
        {
            await Shell.Current.GoToAsync("///LoginPage", true);
        }
    }

    private void TelefoneEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == null)
            return;

        var numeros = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        if (numeros.Length > 11)
            numeros = numeros.Substring(0, 11);

        string formatado = string.Empty;

        if (numeros.Length >= 1)
            formatado = "(" + numeros.Substring(0, Math.Min(2, numeros.Length));

        if (numeros.Length >= 3)
            formatado += ") " + numeros.Substring(2, Math.Min(5, numeros.Length - 2));

        if (numeros.Length >= 7)
            formatado += "-" + numeros.Substring(7, Math.Min(4, numeros.Length - 7));

        if (TelefoneEntry.Text != formatado && formatado.Length <= 15)
        {
            Dispatcher.Dispatch(() =>
            {
                TelefoneEntry.Text = formatado;
            });
        }
    }
}