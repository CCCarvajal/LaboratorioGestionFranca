using System;
using System.Data;
using CapaDatos;
using CapaModelos.BD;

namespace CapaNegocio
{
    public class Controlador
    {
        public Controlador() { }



        //Método que consulta un técnico de forma general
        public List<Object> ControladorConsultaTecnicoGeneral() {

            //Se consulta el técnico y devolvemos una lista de objetos con el resultado
            List<object> Tecnico = new Conexion().ConsultaTecnicoGeneral();

            return Tecnico;
        }
        
        
        //Método que consulta un técnico específico
        public Object ControladorConsultaTecnicoEspecifico(string Codigo) {

            //Se consulta el técnico y devolvemos una lista de objetos con el resultado
            Object Tecnico = new Conexion().ConsultaTecnicoEspecifico(Codigo);

            return Tecnico;
        }
        
        //Método que consulta los elementos que tiene asociado un técnico
        public List<Object> ControladorConsultaElementos(string Codigo) {

            //Se consulta los elementos de un técnico y devolvemos una lista de objetos con el resultado
            List<Object> Tecnico = new Conexion().ConsultaElementosTecnico(Codigo);

            return Tecnico;
        }

        public string AgregaELementos(RelacionTecnicoElemento TecnicoElemento)
        {
            string Result = new Conexion().AgregaELementos(TecnicoElemento);
            return Result;
        }

        public string AgregaTecnicos(Tecnico Tecnico)
        {
            string Result = new Conexion().AgregaTecnicos(Tecnico);
            return Result;
        }
        
        public string ModificaTecnico(Tecnico Tecnico)
        {
            string Result = new Conexion().ModificaTecnico(Tecnico);
            return Result;
        }
        
        public string EliminaTecnico(string Codigo)
        {
            string Result = new Conexion().EliminaTecnico(Codigo);
            return Result;
        }
        
        public List<Sucursal> ConsultaSucursales()
        {
            List<Sucursal> Result = new Conexion().ConsultaSucursales();
            return Result;
        }



    }
}