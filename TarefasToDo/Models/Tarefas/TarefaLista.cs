using System.ComponentModel;

namespace TarefasToDo.Models.Tarefas
{
    public class TarefaLista : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string _nome = string.Empty;
        public string Nome
        {
            get => _nome;
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        private string _descricao = string.Empty;
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    OnPropertyChanged(nameof(Descricao));
                }
            }
        }

        private string _status = string.Empty; 
        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private int _conjuntoId;
        public int ConjuntoId
        {
            get => _conjuntoId;
            set
            {
                if (_conjuntoId != value)
                {
                    _conjuntoId = value;
                    OnPropertyChanged(nameof(ConjuntoId));
                }
            }
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged(nameof(CreatedAt));
                }
            }
        }

        private DateTime _updatedAt;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value;
                    OnPropertyChanged(nameof(UpdatedAt));
                }
            }
        }
    }
}
