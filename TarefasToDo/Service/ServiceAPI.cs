using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TarefasToDo.Models.Conjutos;
using TarefasToDo.Models.Tarefas;
using TarefasToDo.Models.Usuarios;


namespace TarefasToDo.Service
{
    public class ServiceAPI
    {
        private readonly HttpClient _http;

        public ServiceAPI()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(GlobalSettings.BaseUrl)
            };
        }

        public async Task<UsuarioCadastro> CadastraUsuario(UsuarioCadastro user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{GlobalSettings.Instance.Usuario}/cadastrar", user);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsByteArrayAsync();
                    var utf8String = Encoding.UTF8.GetString(erro);

                    var json = JsonDocument.Parse(utf8String);
                    string mensagemErro = "Erro desconhecido.";

                    if (json.RootElement.TryGetProperty("mensagem", out var mensagem))
                    {
                        if (mensagem.ValueKind == JsonValueKind.Array)
                        {
                            var mensagemArray = mensagem.EnumerateArray().Select(x => x.GetString());
                            mensagemErro = string.Join("\n", mensagemArray);
                        }
                        else
                        {
                            mensagemErro = mensagem.GetString() ?? mensagemErro;
                        }
                    }
                    else if (json.RootElement.TryGetProperty("error", out var error))
                    {
                        mensagemErro = error.GetString() ?? mensagemErro;
                    }

                    throw new Exception(mensagemErro);
                }

                var usuario = await response.Content.ReadFromJsonAsync<UsuarioCadastro>();

                if (usuario == null)
                {
                    throw new InvalidOperationException("A resposta da API retornou um objeto nulo.");
                }

                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Erro em CadastraUsuario: {ex.Message}");
                throw;
            }
        }

        public async Task<UsuarioLogin> LoginUsuario(string email, string senha)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{GlobalSettings.Instance.Usuario}/acessar", new { email, senha });

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsByteArrayAsync();
                    var utf8String = Encoding.UTF8.GetString(erro);

                    var json = JsonDocument.Parse(utf8String);
                    var mensagem = json.RootElement.GetProperty("error").GetString();

                    throw new Exception(mensagem);
                }

                var usuarioLogado = await response.Content.ReadFromJsonAsync<UsuarioLogin>();

                if (usuarioLogado == null)
                {
                    throw new InvalidOperationException("A resposta da API retornou um objeto nulo.");
                }

                return usuarioLogado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Erro em LoginUsuario: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ConjuntoLista>> ListaConjuntosUsuario(int usuarioLogadoId)
        {
            try
            {
                var response = await _http.GetAsync($"{GlobalSettings.Instance.Conjunto}/{usuarioLogadoId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<ConjuntoLista>();
                }
                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsByteArrayAsync();
                    var utf8String = Encoding.UTF8.GetString(erro);

                    var json = JsonDocument.Parse(utf8String);
                    var mensagem = json.RootElement.GetProperty("message").GetString();

                    throw new Exception(mensagem);
                }

                var conjunto = await response.Content.ReadFromJsonAsync<List<ConjuntoLista>>();

                if (conjunto == null)
                {
                    throw new InvalidOperationException("A resposta da API retornou um objeto nulo.");
                }

                return conjunto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Erro em Buscar Conjunto: {ex.Message}");
                throw;
            }
        }

        public async Task<ConjuntoCadastro> CadastraConjunto(ConjuntoCadastro conjuntoCadastro)
        {
            try
            {
                var response = await _http.PostAsJsonAsync($"{GlobalSettings.Instance.Conjunto}/cadastrar", conjuntoCadastro);

                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsByteArrayAsync();
                    var utf8String = Encoding.UTF8.GetString(erro);

                    var json = JsonDocument.Parse(utf8String);
                    string mensagemErro = "Erro desconhecido.";

                    if (json.RootElement.TryGetProperty("mensagem", out var mensagem))
                    {
                        if (mensagem.ValueKind == JsonValueKind.Array)
                        {
                            var mensagemArray = mensagem.EnumerateArray().Select(x => x.GetString());
                            mensagemErro = string.Join("\n", mensagemArray);
                        }
                        else
                        {
                            mensagemErro = mensagem.GetString() ?? mensagemErro;
                        }
                    }
                    else if (json.RootElement.TryGetProperty("error", out var error))
                    {
                        mensagemErro = error.GetString() ?? mensagemErro;
                    }

                    throw new Exception(mensagemErro);
                }

                var conjunto = await response.Content.ReadFromJsonAsync<ConjuntoCadastro>();

                if (conjunto == null)
                {
                    throw new InvalidOperationException("A resposta da API retornou um objeto nulo.");
                }

                return conjunto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Erro em CadastraUsuario: {ex.Message}");
                throw;
            }
        }

        public async Task<ConjuntoCadastro> EditarConjunto(int conjuntoId, ConjuntoCadastro conjuntoEditado)
        {
            try
            {
                var response = await _http.PutAsJsonAsync($"{GlobalSettings.Instance.Conjunto}/editar/{conjuntoId}", conjuntoEditado);
                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsByteArrayAsync();
                    var utf8String = Encoding.UTF8.GetString(erro);

                    string mensagemErro = "Erro ao excluir conjunto.";

                    try
                    {
                        var json = JsonDocument.Parse(utf8String);
                        if (json.RootElement.TryGetProperty("mensagem", out var mensagem))
                        {
                            mensagemErro = mensagem.GetString() ?? mensagemErro;
                        }
                        else if (json.RootElement.TryGetProperty("error", out var error))
                        {
                            mensagemErro = error.GetString() ?? mensagemErro;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Erro desconhecido!");
                    }

                    throw new Exception(mensagemErro);
                }

                var conjuntoAtualizado = await response.Content.ReadFromJsonAsync<ConjuntoCadastro>();
                if (conjuntoAtualizado == null)
                {
                    throw new InvalidOperationException("A resposta da API retornou um objeto nulo");
                }
                return conjuntoAtualizado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em editar conjunto: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> DeletarConjunto(int conjuntoId)
        {
            try
            {
                var response = await _http.DeleteAsync($"{GlobalSettings.Instance.Conjunto}/{conjuntoId}");
                if (!response.IsSuccessStatusCode)
                {
                    var erro = await response.Content.ReadAsByteArrayAsync();
                    var utf8String = Encoding.UTF8.GetString(erro);

                    string mensagemErro = "Erro ao excluir conjunto.";

                    try
                    {
                        var json = JsonDocument.Parse(utf8String);
                        if (json.RootElement.TryGetProperty("error", out var error))
                        {
                            mensagemErro = error.GetString() ?? mensagemErro;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Erro desconhecido!");
                    }

                    throw new Exception(mensagemErro);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗ Erro em DeletarConjunto: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Tarefa>> ListaTarefasConjunto(int conjuntoId)
        {
            var response = await _http.GetAsync($"/conjunto/tarefas/{conjuntoId}");
            response.EnsureSuccessStatusCode();
            var tarefa = await response.Content.ReadFromJsonAsync<List<Tarefa>>();
            
            if(tarefa == null)
            {
                throw new InvalidOperationException("A resposta da API retornou um objeto nulo.");
            }

            return tarefa;
        }
    }
}
