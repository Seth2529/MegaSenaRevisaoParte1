using JogoMegaSena.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json;

namespace JogoMegaSena.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MegaSenaController : ControllerBase
    {
        string _arquivojogos = @".\Data\jogosMega.json";

        #region Métodos arquivo
        private List<MegaSenaViewModel> ObterJogos()
        {
            if (!System.IO.File.Exists(_arquivojogos))
            {
                return new List<MegaSenaViewModel>();
            }
            Directory.CreateDirectory(_arquivojogos.Substring(0, _arquivojogos.LastIndexOf('\\') + 1));
            string Readjson = System.IO.File.ReadAllText(_arquivojogos);
            return JsonConvert.DeserializeObject<List<MegaSenaViewModel>>(Readjson);

        }

        private void RegistroJogo(List<MegaSenaViewModel> registro)
        {
            string Readjson = JsonConvert.SerializeObject(registro);
            System.IO.File.WriteAllText(_arquivojogos, Readjson);
        }
        #endregion

        #region Operações CRUD
        [HttpGet]
        public IActionResult ObterTodosOsJogos()
        {
            if(ObterJogos() != null) 
            {
                List<MegaSenaViewModel> registro = ObterJogos();
                return Ok(registro);
            }
            return BadRequest("Nenhum jogo salvo");

        }
        [HttpPost]
        public IActionResult RegistrarJogo([FromBody] MegaSenaViewModel megasena)
        {
            if (megasena.primeiroNro != megasena.segundoNro && megasena.primeiroNro != megasena.terceiroNro 
                && megasena.primeiroNro != megasena.quartoNro && megasena.primeiroNro != megasena.quintoNro
                && megasena.primeiroNro != megasena.sextoNro

                && megasena.segundoNro != megasena.terceiroNro && megasena.segundoNro != megasena.quartoNro 
                && megasena.segundoNro != megasena.quintoNro && megasena.segundoNro != megasena.sextoNro

                && megasena.terceiroNro != megasena.quartoNro && megasena.terceiroNro != megasena.quintoNro 
                && megasena.terceiroNro != megasena.sextoNro

                && megasena.quartoNro != megasena.quintoNro && megasena.terceiroNro != megasena.sextoNro

                && megasena.quintoNro != megasena.sextoNro)
            {
                List<MegaSenaViewModel> registro = ObterJogos();
                MegaSenaViewModel novoJogo = new MegaSenaViewModel()
                {
                    Nome = megasena.Nome,
                    Cpf = megasena.Cpf,
                    primeiroNro = megasena.primeiroNro,
                    segundoNro = megasena.segundoNro,
                    terceiroNro = megasena.terceiroNro,
                    quartoNro = megasena.quartoNro,
                    quintoNro = megasena.quintoNro,
                    sextoNro = megasena.sextoNro,
                    datajogo = megasena.datajogo
                };

                registro.Add(novoJogo);
                RegistroJogo(registro);

                return Ok("Jogo Registrado com sucesso!");
            }
            else
            {
                return BadRequest("Dados incorretos, existem números repetidos no jogo");
            }

        }
        #endregion  
    }
}