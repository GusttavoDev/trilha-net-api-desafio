using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar a Tarefa no banco utilizando o EF
            var task = _context.Tarefas.FirstOrDefault(t => t.id == id);

            // Validar o tipo de retorno
            if (task == null)
            {
                // Se não encontrar a tarefa, retornar NotFound
                return NotFound();
            }
            else
            {
                // Caso contrário, retornar OK com a tarefa encontrada
                return Ok(task);
            }
        }


        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando o EF
            var tasks = _context.Tarefas.ToList();

            // Retornar as tarefas encontradas
            return Ok(tasks);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Buscar as tarefas no banco utilizando o EF, que contenham o título recebido por parâmetro
            var tasks = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();

            // Validar se foram encontradas tarefas
            if (tasks.Count == 0)
            {
                // Se não foram encontradas tarefas, retornar NotFound
                return NotFound();
            }
            else
            {
                // Caso contrário, retornar OK com as tarefas encontradas
                return Ok(tasks);
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData


            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adicionar a tarefa recebida no EF
            _context.Tarefas.Add(tarefa);

            // Salvar as mudanças
            _context.SaveChanges();

            // Retornar uma resposta indicando que a tarefa foi criada com sucesso, junto com a tarefa criada
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;

            // Atualizar a variável tarefaBanco no EF e salvar as mudanças
            _context.SaveChanges();

            // Retornar uma resposta indicando que a tarefa foi atualizada com sucesso
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Remover a tarefa encontrada através do EF
            _context.Tarefas.Remove(tarefaBanco);

            // Salvar as mudanças
            _context.SaveChanges();

            // Retornar uma resposta indicando que a tarefa foi removida com sucesso
            return NoContent();
        }

    }
}
