using System.Data;
using System.Data.SqlClient;
using CapaModelos.BD;
using CapaModelos;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CapaDatos
{
    public class Modelo
    {
        //public DataTable ConsultarTecnico()
        //{
        //    //DataTable Tecnico = new Conexion().ConsultaSQL("SELECT * FROM Tecnico");
        //    //return Tecnico;
        //}

    }

    public class Conexion
    {
        /****************************************************************************************************
        *           Consulta técnicos
        * **************************************************************************************************/
        public List<Object> ConsultaTecnicoGeneral()
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            //se realiza consulta linq a la base de datos
            // Tecnico -> contiene toda la información de la entidad Tecnico
            // NumeroDeElementos -> Contiene el conteo de los elementos que contiene un técnico

            List<Object> Datos = (from TecnicoInfo in datos.Tecnicos
                                  join relacion in datos.RelacionTecnicoElementos
                                  on TecnicoInfo.Codigo equals relacion.CodigoTecnico
                                  into tecnicoRelacionGroup
                                  select new
                                  {
                                      Tecnico = TecnicoInfo,
                                      NumeroDeElementos = tecnicoRelacionGroup.Count()
                                  }).ToList<Object>();

            //se retorna la lista de objetos
            return Datos;

        }

        /****************************************************************************************************
        *           Consulta a un técnico
        * **************************************************************************************************/
        public Object ConsultaTecnicoEspecifico(string Codigo)
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            //se realiza consulta linq a la base de datos
            // Tecnico -> contiene toda la información de la entidad Tecnico
            // Sucursal -> Contiene Información de la sucursal del técnico
            var resultado = (from tecnico in datos.Tecnicos
                             join sucursal in datos.Sucursals
                             on tecnico.CodigoSucursal equals sucursal.Codigo
                             where tecnico.Codigo.Equals(Codigo)
                             select new
                             {
                                 Tecnico = tecnico,
                                 Sucursal = sucursal
                             }).FirstOrDefault();

            //se retorna la lista de objetos
            return resultado;

        }


        /****************************************************************************************************
        *           Consulta elementos de tecnico
        * **************************************************************************************************/
        public List<Object> ConsultaElementosTecnico(string Codigo)
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            //se realiza consulta linq a la base de datos
            // Tecnico -> contiene toda la información de la entidad Tecnico
            // NumeroDeElementos -> Contiene el conteo de los elementos que contiene un técnico
            var resultado = (from Elemento in datos.Elementos
                             join RelacionTecnicoElemento in datos.RelacionTecnicoElementos
                             on Elemento.Codigo equals RelacionTecnicoElemento.CodigoElemento
                             where RelacionTecnicoElemento.CodigoTecnico.Equals(Codigo)
                             select new
                             {
                                 Elemento = Elemento,
                                 RelacionTecnicoElemento = RelacionTecnicoElemento
                             }).ToList<Object>();


            //se retorna la lista de objetos
            return resultado;

        }

        /****************************************************************************************************
        *           Agrega elementos a un técnico
        * **************************************************************************************************/
        public string AgregaELementos(RelacionTecnicoElemento TecnicoElemento)
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();
            var guardaDatos = new LaboratorioContext();

            //validados que sea una cantidad válida
            if (TecnicoElemento.Cantidad < 1 || TecnicoElemento.Cantidad > 10)
            {
                return "La cantidad no es válidad";
            }

            //Verifica si ya existe una relación para el técnico y el elemento especificados
            var existeRelacion = datos.RelacionTecnicoElementos.Any(r => r.CodigoTecnico == TecnicoElemento.CodigoTecnico && r.CodigoElemento == TecnicoElemento.CodigoElemento);
            if (!existeRelacion)
            {
                //cremos el registro para la entidad
                RelacionTecnicoElemento NuevoRelacion = new RelacionTecnicoElemento
                {
                    CodigoTecnico = TecnicoElemento.CodigoTecnico,
                    CodigoElemento = TecnicoElemento.CodigoElemento,
                    Cantidad = TecnicoElemento.Cantidad
                };

                // Agrega la nueva relación al contexto y guarda los cambios en la base de datos
                guardaDatos.RelacionTecnicoElementos.Add(NuevoRelacion);
                try
                {
                    guardaDatos.SaveChanges();
                    return "Ok";
                }catch (Exception ex)
                {
                    return ex.Message;
                }


            }
            else
            {
                return "El técnico ya se contiene el elemento";
            }
        }

        /****************************************************************************************************
        *           Elimina elementos a un técnico
        * **************************************************************************************************/
        //public string AgregaELementos(RelacionTecnicoElemento TecnicoElemento)
        //{
        //    //Se instancia el contexto de la base de datos
        //    var datos = new LaboratorioContext();

        //    //validados que sea una cantidad válida
        //    if (TecnicoElemento.Cantidad < 1 || TecnicoElemento.Cantidad > 10)
        //    {
        //        return "La cantidad no es válidad";
        //    }

        //    //Verifica si ya existe una relación para el técnico y el elemento especificados
        //    var existeRelacion = datos.RelacionTecnicoElementos.Any(r => r.CodigoTecnico == TecnicoElemento.CodigoTecnico && r.CodigoElemento == TecnicoElemento.CodigoElemento);
        //    if (!existeRelacion)
        //    {
        //        //creamos el registro para la entidad
        //        RelacionTecnicoElemento NuevoRelacion = new RelacionTecnicoElemento
        //        {
        //            CodigoTecnico = TecnicoElemento.CodigoTecnico,
        //            CodigoElemento = TecnicoElemento.CodigoElemento,
        //            Cantidad = TecnicoElemento.Cantidad
        //        };

        //        // Agrega la nueva relación al contexto y guarda los cambios en la base de datos
        //        datos.RelacionTecnicoElementos.Add(NuevoRelacion);
        //        datos.SaveChanges();
        //        return "Ok";
        //    }
        //    else
        //    {
        //        return "El técnico ya se contiene el elemento";
        //    }
        //}


        /****************************************************************************************************
        *           Agrega a un técnico
        * **************************************************************************************************/
        public string AgregaTecnicos(Tecnico Tecnico)
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            // Expresión regular para validar que el código contiene solo letras y números
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if (regex.IsMatch(Tecnico.Codigo))
            {
                // Verifica si el código del técnico ya existe en la base de datos
                var tecnicoExistente = datos.Tecnicos.FirstOrDefault(t => t.Codigo == Tecnico.Codigo);

                if (tecnicoExistente == null)
                {
                    // Verifica si la sucursal existe en la base de datos
                    var sucursalExistente = datos.Sucursals.FirstOrDefault(s => s.Codigo == Tecnico.CodigoSucursal);

                    if (sucursalExistente != null)
                    {
                        // Si el técnico no existe, crea una nueva instancia de Tecnico
                        var nuevoTecnico = new Tecnico
                        {
                            Codigo = Tecnico.Codigo,
                            Nombre = Tecnico.Nombre,
                            SueldoBase = Tecnico.SueldoBase,
                            CodigoSucursal = Tecnico.CodigoSucursal
                        };

                        // Agrega el nuevo técnico al contexto y guarda los cambios en la base de datos
                        datos.Tecnicos.Add(nuevoTecnico);
                        datos.SaveChanges();

                        return "Ok";
                    }
                    else
                    {
                        // No se encontró la sucursal
                        return "No se encontró la sucursal.";
                    }

                }
                else
                {
                    // El técnico ya existe
                    return "El código de técnico ya está en uso.";
                }
            }
            else
            {
                return "El código no es válido.";
            }
        }

        /****************************************************************************************************
        *           Modifica a un técnico
        * **************************************************************************************************/
        public string ModificaTecnico(Tecnico Tecnico)
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            // Expresión regular para validar que el código contiene solo letras y números
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if (regex.IsMatch(Tecnico.Codigo))
            {
                // Encuentra el técnico a modificar por su código
                var tecnicoAModificar = datos.Tecnicos.FirstOrDefault(t => t.Codigo == Tecnico.Codigo);

                if (tecnicoAModificar != null)
                {
                    // Modifica los atributos del técnico
                    tecnicoAModificar.Nombre = Tecnico.Nombre;
                    tecnicoAModificar.SueldoBase = Tecnico.SueldoBase;
                    tecnicoAModificar.CodigoSucursal = Tecnico.CodigoSucursal;

                    // Guarda los cambios en la base de datos
                    datos.SaveChanges();

                    return "Ok.";
                }
                else
                {
                    return "El técnico especificado no fue encontrado en la base de datos.";
                }
            }
            else
            {
                return "El código no es válido.";
            }
        }


        /****************************************************************************************************
        *           Elimina a un técnico
        * **************************************************************************************************/
        public string EliminaTecnico(string Codigo)
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            // Encuentra el técnico a eliminar por su código
            var tecnicoAEliminar = datos.Tecnicos.FirstOrDefault(t => t.Codigo == Codigo);

            if (tecnicoAEliminar != null)
            {
                // Elimina el técnico de la base de datos
                datos.Tecnicos.Remove(tecnicoAEliminar);
                datos.SaveChanges();

                return "Ok";
            }
            else
            {
                return "El técnico especificado no fue encontrado en la base de datos.";
            }

        }

        /****************************************************************************************************
        *           Consulta sucursales
        * **************************************************************************************************/
        public List<Sucursal> ConsultaSucursales()
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            //se realiza consulta linq a la base de datos
            List<Sucursal> Datos = datos.Sucursals.ToList<Sucursal>();

            //se retorna la lista de objetos
            return Datos;

        }

        /****************************************************************************************************
        *           Consulta Elementos
        * **************************************************************************************************/
        public List<Elemento> ConsultarElementos()
        {
            //Se instancia el contexto de la base de datos
            var datos = new LaboratorioContext();

            //se realiza consulta linq a la base de datos
            List<Elemento> Datos = datos.Elementos.ToList<Elemento>();

            //se retorna la lista de objetos
            return Datos;

        }
    }
}
