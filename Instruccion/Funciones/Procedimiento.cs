using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Funciones
{
    public class Procedimiento : Nodo
    {
        private string id;
        private LinkedList<Parametro> parametros;
        private LinkedList<Nodo> instrucciones;
        private Objeto objeto;

        public Procedimiento(int linea, int columna, string id, LinkedList<Parametro> parametros, LinkedList<Nodo> nodos,
            Objeto objeto):base(linea, columna)
        {
            this.id = id;
            this.parametros = parametros;
            this.instrucciones = nodos;
            this.objeto = objeto;
        }

        public override Retorno compilar(Entorno entorno)
        {
            this.Validar_Parametros(entorno);
            

            //Guardo la funcion;
            if(entorno.addFuncion(this.id, this.objeto, this.parametros))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Ya existe una funcion con el id: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("Ya existe una funcion con el id: " + this.id);
            }

            SimboloFuncion simboloFuncion = entorno.getFuncion(this.id);

            Entorno entorno_fun = new Entorno(entorno, this.id);
            string label_return = Master.getInstancia.newLabel();

            

            this.Agregar_variables(entorno_fun);

            //Genero el codigo para la creacion de la funcion

            Master.getInstancia.addFuncion(simboloFuncion.id);

            this.Compilar_body(entorno_fun);

            Master.getInstancia.addLabel(label_return);
            Master.getInstancia.Retorno_funcion();
            Master.getInstancia.addFinFuncion();

            return null;
        }

        public void Agregar_variables(Entorno entorno)
        {
            
            foreach(Parametro parametro in this.parametros)
            {
                entorno.addSimbolo(parametro.id, parametro.objeto, Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK);
            }
        }

        public void Compilar_body(Entorno entorno)
        {
            foreach(Nodo instruccion in this.instrucciones)
            {
                try
                {
                    instruccion.compilar(entorno);
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        public void Validar_Parametros(Entorno entorno)
        {
            HashSet<string> ids = new HashSet<string>();
            foreach(Parametro parametro in this.parametros)
            {
                if (ids.Contains(parametro.id))
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "Ya existe un atributo en el procedimiento: " + this.id + " con el id de: " + parametro.id);
                    Master.getInstancia.addError(error);
                    throw new Exception("Ya existe un atributo en el objeto: " + this.id + " con el id de: " + parametro.id);
                }
                else
                {
                    if(parametro.objeto.getTipo() == Objeto.TipoObjeto.TYPES)
                    {
                        //busca primero una estructura de tipo objeto
                        if (Buscar_Objeto(parametro, entorno))
                        {
                            continue;
                        }
                        //buscar luego una estructura de tipo array
                        if (Buscar_Array(parametro, entorno))
                        {
                            continue;
                        }
                    }
                }
            }
        }
        public bool Buscar_Objeto(Parametro parametro, Entorno entorno)
        {
            SimboloObjeto simboloObjeto = entorno.searchObjeto(parametro.getObjeto().getObjetoId());
            if (simboloObjeto == null)
            {
                return false;
            }
            parametro.getObjeto().seTipo(Objeto.TipoObjeto.OBJECTS);
            parametro.getObjeto().symObj = simboloObjeto;
            return true;
        }

        public bool Buscar_Array(Parametro parametro, Entorno entorno)
        {
            SimboloArreglo simboloArreglo = entorno.searchArreglo(parametro.getObjeto().getObjetoId());
            if (simboloArreglo == null)
            {
                return false;
            }
            parametro.getObjeto().seTipo(Objeto.TipoObjeto.ARRAY);
            parametro.getObjeto().symArray = simboloArreglo;
            return true;
        }

    }
}
