using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefasToDo
{
    public class GlobalSettings
    {
        public static string BaseUrl = "http://10.0.2.2:3000";
        public string Usuario { get; set; }
        public string Conjunto { get; set; }
        public string Tarefa { get; set; }

        public static GlobalSettings Instance { get; } = new GlobalSettings();

    public GlobalSettings()
        {
            Usuario = $"{BaseUrl}/usuarios";
            Conjunto = $"{BaseUrl}/conjuntos";
            Tarefa = $"{BaseUrl}/conjunto/tarefas";
        }
    }
}
