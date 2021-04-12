using C3D_Pascal_AirMax.Instruccion.Funciones;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class SimboloFuncion
    {
        public Objeto objeto;
        public string id;
        public int size;
        public LinkedList<Parametro> parametros;

        public SimboloFuncion(string id, LinkedList<Parametro> parametros, Objeto objeto)
        {
            this.objeto = objeto;
            this.id = id.ToLower();
            this.size = parametros.Count;
            this.parametros = parametros;
        }

    }
}
