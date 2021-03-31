using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Asignaciones
{
    public class AsignacionId : Nodo
    {
        private string id;
        private Nodo anterior;

        public AsignacionId(int linea, int columna, string id, Nodo anterior):base(linea, columna)
        {
            this.id = id;
            this.anterior = anterior;
        }

        public override Retorno compilar(Entorno entorno)
        {
            if(this.anterior == null)
            {
                Simbolo sym = entorno.getSimbolo(this.id);
                if(sym == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "No existe la variable: " + this.id);
                    Master.getInstancia.addError(error);
                    throw new Exception("No existe la variable: " + this.id);
                }
                if (sym.getGlobal())
                {
                    return new Retorno(sym.getPosicion(), false, sym.getTipo(),sym);
                }
                else
                {
                    //TODO: variables que no son locales
                }
            }
            else
            {
                //TODO: falta ver como acceder a objetos
            }
            return null;
        }
    }
}
