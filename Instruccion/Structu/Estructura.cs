using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Structu
{

    /*
     * Permite poder definir un type <nombre> = object
     */

    public class Estructura : Nodo
    {
        private string id;
        private LinkedList<Atributo> atributos;

        public Estructura(int linea, int columna, string id, LinkedList<Atributo> atributos) : base(linea, columna)
        {
            this.id = id;
            this.atributos = atributos;
        }


        public override Retorno compilar(Entorno entorno)
        {
            
            if(entorno.addObjeto(this.id, this.atributos.Count, this.atributos))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Ya existe un type objet con el id: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("Ya existe un type objet con el id: " + this.id);
            }
            this.validarParametros(entorno);

            return null;
        }
        
        public void validarParametros(Entorno entorno)
        {
            //TODO: los atributos dentro de un objeto, por el momento solo son primitivos
            HashSet<string> ids = new HashSet<string>();
            foreach(Atributo atr in this.atributos)
            {
                if (ids.Contains(atr.getId()))
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "Ya existe un atributo en el objeto: " + this.id + " con el id de: " + atr.getId());
                    Master.getInstancia.addError(error);
                    throw new Exception("Ya existe un atributo en el objeto: " + this.id + " con el id de: " + atr.getId());
                }
            }
        }
    }
}
