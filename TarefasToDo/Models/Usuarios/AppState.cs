using TarefasToDo.Models.Conjutos;

namespace TarefasToDo.Models.Usuarios
{
    public static class AppState
    {
        public static UsuarioLogin? UsuarioAtual { get; set; }
        public static ConjuntoLista? ConjuntoSelecionado { get; set; }
    }
}
