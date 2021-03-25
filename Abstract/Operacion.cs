using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Abstract
{
    public abstract class Operacion : Nodo
    {
        public string trueLabel;
        public string falseLabel;

        public Operacion(int linea, int columna):base(linea, columna)
        {
            this.trueLabel = "";
            this.falseLabel = "";
        }

        public string getLabelTrue()
        {
            return this.trueLabel;
        }
        public string getLabelFalse()
        {
            return this.falseLabel;
        }
    }
}
