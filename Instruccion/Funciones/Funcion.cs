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
    public class Funcion : Nodo
    {
        private string id;
        private LinkedList<Parametro> parametros;
        private LinkedList<Nodo> instrucciones;
        private Objeto objeto;

        public Funcion(int linea, int columna,string id, LinkedList<Parametro> parametros, LinkedList<Nodo> instrucciones, 
            Objeto objeto):base(linea, columna)
        {
            this.id = id;
            this.parametros = parametros;
            this.instrucciones = instrucciones;
            this.objeto = objeto;
        }

        public override Retorno compilar(Entorno entorno)
        {
            this.Validar_Parametros(entorno);

            if (entorno.addFuncion(this.id, this.objeto, this.parametros))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Ya existe una funcion con el id: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("Ya existe una funcion con el id: " + this.id);
            }

            //Obtengo la funcion
            SimboloFuncion simboloFuncion = entorno.getFuncion(this.id);
            //creo el nuevo entorno de la funcion
            Entorno entorno_fun = new Entorno(entorno, this.id);
            //creo una etiqueta que me mando al retorno
            string label_return = Master.getInstancia.newLabel();
            //creo una etiqueta del exit
            string label_exit = Master.getInstancia.newLabel();
            //seteo atributos al entorno, con respecto a la funcion
            entorno_fun.setFuncion(this.id, simboloFuncion, label_return);

            //verifico el tipo
            this.Validar_Retorno(entorno);
            //Mando a setear un simbolo que representa a la funcion
            this.Setear_Simbolo_Funcion(entorno_fun, simboloFuncion);
            this.Agregar_variables(entorno_fun);

            Master.getInstancia.addFuncion(simboloFuncion.id);
            //Genero el codigo de la funcion
            this.Compilar_body(entorno_fun);
            Master.getInstancia.addLabel(label_return);

            //TODO: deberia hacer algo para devolver el retorno?

            Master.getInstancia.Retorno_funcion();
            Master.getInstancia.addFinFuncion();
            return null;
        }

        public void Setear_Simbolo_Funcion(Entorno entorno, SimboloFuncion simboloFuncion)
        {
            Simbolo simbolo = new Simbolo(simboloFuncion.id.ToLower(), simboloFuncion.objeto, Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK,
                0, simboloFuncion.id, false);
            entorno.variables.Add(simboloFuncion.id.ToLower(), simbolo);
        }

        public void Agregar_variables(Entorno entorno)
        {

            foreach (Parametro parametro in this.parametros)
            {
                entorno.addSimboloFuncion(parametro.id, parametro.objeto, Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK, parametro.param);
            }
        }

        public void Compilar_body(Entorno entorno)
        {
            //ejecuta la sentencias para guardar simbolos
            foreach (Nodo instruccion in this.instrucciones)
            {
                try
                {
                    if(instruccion.pre_compilar == true)
                    {
                        instruccion.compilar(entorno);
                        instruccion.pre_compilar = false;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            // ejecuta todas las instrucciones
            foreach (Nodo instruccion in this.instrucciones)
            {
                try
                {
                    instruccion.compilar(entorno);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }




        public void Validar_Parametros(Entorno entorno)
        {
            HashSet<string> ids = new HashSet<string>();
            foreach (Parametro parametro in this.parametros)
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
                    if (parametro.objeto.getTipo() == Objeto.TipoObjeto.TYPES)
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

                        Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                            "La estructura: " + parametro.objeto.getObjetoId() + " no esta definida");
                        Master.getInstancia.addError(error);
                        throw new Exception("La estructura: " + parametro.objeto.getObjetoId() + " no esta definida");
                    }
                }
            }
        }

        public bool Validar_Retorno(Entorno entorno)
        {
            if(this.objeto.getTipo() == Objeto.TipoObjeto.TYPES)
            {
                SimboloObjeto simboloObjeto = entorno.searchObjeto(this.objeto.getObjetoId());
                if(simboloObjeto != null)
                {
                    this.objeto.seTipo(Objeto.TipoObjeto.OBJECTS);
                    this.objeto.symObj = simboloObjeto;
                    return true;
                }
                SimboloArreglo simboloArreglo = entorno.searchArreglo(this.objeto.getObjetoId());
                if(simboloArreglo != null)
                {
                    this.objeto.seTipo(Objeto.TipoObjeto.ARRAY);
                    this.objeto.symArray = simboloArreglo;
                    return true;
                }
            }
            return false;
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
