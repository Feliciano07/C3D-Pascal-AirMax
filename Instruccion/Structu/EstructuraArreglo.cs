using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Structu
{
    public class EstructuraArreglo : Nodo
    {


        private string id;
        private LinkedList<Dimension> dimensiones;
        private Objeto objeto;

        public EstructuraArreglo(int linea, int columna, string id, LinkedList<Dimension> dimensions, Objeto objeto):base(linea, columna)
        {
            this.id = id;
            this.dimensiones = dimensions;
            this.objeto = objeto;
        }

        public override Retorno compilar(Entorno entorno)
        {
            //TODO: validar si el tipo ya existe y setear ese tipo aca
            if(entorno.addArreglo(id, dimensiones, objeto))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Ya existe un type array con el id: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("Ya existe un type array con el id: " + this.id);
            }
            

            return null;
        }

       
    }
}
