namespace TarefasToDo.Models.Conjutos
{
    public class ConjuntoLista
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int UsuarioId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Dataformatada
        {
            get
            {
                return CreatedAt.ToLocalTime().ToString("dd/MM/yyyy 'às' HH:mm");
            }
        }
    }
}
