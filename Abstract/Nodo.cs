using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Enviroment;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Abstract
{
    public abstract class Nodo
    {
        private int linea;
        private int columna;
        public string trueLabel;
        public string falseLabel;

        public Nodo(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.trueLabel = "";
            this.falseLabel = "";
        }

        public int getLinea()
        {
            return this.linea;
        }
        public int getColumna()
        {
            return this.columna;
        }


        public abstract Retorno compilar(Entorno entorno);


    }
}
