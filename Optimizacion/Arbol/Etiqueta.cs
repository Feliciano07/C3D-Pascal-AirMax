using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Etiqueta : Nodo
    {
        public string label;

        public Etiqueta(int fila, string label) : base(fila)
        {
            this.label = label;
        }

        public override string getOriginal()
        {
            return this.label + ":";
        }

        public override void Mirilla(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }
}
