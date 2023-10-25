using Laboratorio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using CapaNegocio;
using System.Data;
using CapaDatos;
using System.Text.Json.Serialization;
using CapaModelos.BD;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Laboratorio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /***********************************************************************************************************
         * 
         * Este método carga la vista principal
         * 
         ***********************************************************************************************************/
        public IActionResult Index()
        {
            return View();
        }


        /***********************************************************************************************************
         * 
         * Este método Muestra vista de un técnico en específico
         * 
         ***********************************************************************************************************/
        [HttpGet]
        public IActionResult DetalleTecnico(string Codigo)
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        /***********************************************************************************************************
         * 
         * Este método consulta datos generales de un técnico, mostrandolos en la primera vista
         * 
         ***********************************************************************************************************/
        [HttpGet]
        public JsonResult ConsultarTecnicoGeneral()
        {
            //Instancia Capa de negocio
            Controlador ControladorCapaNegocio = new Controlador();
            
            //Consulta Datos del técnico y retornamos una lista de objetos con el fin de no construir la entidad
            List<Object> DatosTecnico = ControladorCapaNegocio.ControladorConsultaTecnicoGeneral();
           
            //Mostramos la respuesta
            return Json(DatosTecnico);
        }

        /***********************************************************************************************************
         * 
         * Este método consulta 
         * 
         ***********************************************************************************************************/
        [HttpPost]
        public IActionResult ConsultarTecnico([FromBody] Tecnico Codigo)
        {
            // Instancia Capa de negocio
            Controlador ControladorCapaNegocio = new Controlador();

            // Consulta Datos del técnico y retornamos una lista de objetos con el fin de no construir la entidad
            Object DatosTecnico = ControladorCapaNegocio.ControladorConsultaTecnicoEspecifico(Codigo.Codigo);

            // Configurar opciones de serialización
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32 // establece la profundidad máxima si es necesario
            };

            // Serializar el objeto y retornarlo como JSON
            string jsonString = JsonSerializer.Serialize(DatosTecnico, options);

            // Devolver la respuesta JSON
            return Content(jsonString, "application/json");

        }

            /***********************************************************************************************************
             * 
             * Este método consulta datos generales de un técnico, mostrandolos en la primera vista
             * 
             ***********************************************************************************************************/
            
        [HttpPost]
        public JsonResult ConsultarElementosTabla([FromBody] Tecnico Codigo)
        {
            //Instancia Capa de negocio
            Controlador ControladorCapaNegocio = new Controlador();

            //Consulta Datos del técnico y retornamos una lista de objetos con el fin de no construir la entidad
            List<Object> DatosTecnico = ControladorCapaNegocio.ControladorConsultaElementos(Codigo.Codigo);

            //Mostramos la respuesta
            return Json(DatosTecnico);
        }

        [HttpGet]
        public JsonResult AgregaELementos(RelacionTecnicoElemento TecnicoElemento)
        {
            string Result = new Conexion().AgregaELementos(TecnicoElemento);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult AgregaTecnicos(Tecnico Tecnico)
        {
            string Result = new Conexion().AgregaTecnicos(Tecnico);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult ModificaTecnico(Tecnico Tecnico)
        {
            string Result = new Conexion().ModificaTecnico(Tecnico);
            return Json(Result);
        }

        [HttpPost]
        public JsonResult EliminaTecnico(string Codigo)
        {
            string Result = new Conexion().EliminaTecnico(Codigo);
            return Json(Result);
        }
        
        
        [HttpPost]
        public JsonResult ConsultaSucursales()
        {
            List<Sucursal> Result = new Conexion().ConsultaSucursales();
            return Json(Result);
        }
        
        [HttpPost]
        public JsonResult ConsultarTodosElementos()
        {
            List<Elemento> Result = new Conexion().ConsultarElementos();
            return Json(Result);
        }




    }
}