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
        public bool pre_compilar;

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


        public bool Verificar_Tipo(Objeto tipo1, Objeto tipo2 )
        {
            if (tipo1.getTipo() == tipo2.getTipo())
            {

                if(tipo1.getTipo() == Objeto.TipoObjeto.OBJECTS)
                {
                    Verificar_Id_Objeto(tipo1, tipo2);
                }

                return true;
            }
            else if (tipo1.getTipo() == Objeto.TipoObjeto.INTEGER && tipo2.getTipo() == Objeto.TipoObjeto.REAL)
            {
                return true;
            }
            return false;
        }

        public bool Verificar_Id_Objeto(Objeto tipo1, Objeto tipo2)
        {
            if(string.Compare(tipo1.getObjetoId(), tipo2.getObjetoId(),true) == 0)
            {
                return true;
            }
            return false;
        }


    }
}
