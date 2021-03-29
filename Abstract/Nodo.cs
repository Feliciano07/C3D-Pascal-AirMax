using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Enviroment;
using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.TipoDatos;

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


        public bool Verificar_Tipo(Objeto.TipoObjeto tipo1, Objeto.TipoObjeto tipo2 )
        {
            if (tipo1 == tipo2)
            {
                return true;
            }
            else if (tipo1 == Objeto.TipoObjeto.INTEGER && tipo2 == Objeto.TipoObjeto.REAL)
            {
                return true;
            }
            return false;
        }


    }
}
