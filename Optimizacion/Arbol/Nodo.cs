using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public abstract class Nodo
    {
        public int index;
        public int fila;
        public bool isEnable;// sirve para saber si se incluye en la salida

        public Nodo(int fila)
        {
            this.fila = fila;
            this.index = -1;
            this.isEnable = true;
        }

        public abstract void Mirilla(Interprete interprete);

        public abstract string getOriginal();

    }
}
