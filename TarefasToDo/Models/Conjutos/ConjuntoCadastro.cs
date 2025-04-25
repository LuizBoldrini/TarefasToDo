namespace TarefasToDo.Models.Conjutos
{
    public class ConjuntoCadastro
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}
