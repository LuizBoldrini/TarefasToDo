using TarefasToDo.Models.Conjutos;
using TarefasToDo.Models.Tarefas;

namespace TarefasToDo.Models.Usuarios
{
    public static class AppState
    {
        public static UsuarioLogin? UsuarioAtual { get; set; }
        public static ConjuntoLista? ConjuntoSelecionado { get; set; }
        public static TarefaLista? TarefaSelecionada { get; set; }
    }
}
