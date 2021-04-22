using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Condicion : Nodo
    {
        private string left;
        private string right;
        private string operacion;
        private string etiqueta;

        public Condicion(int fila, string left, string right, string operacion, string etiqueta) : base(fila)
        {
            this.left = left;
            this.right = right;
            this.operacion = operacion;
            this.etiqueta = etiqueta;
        }

        public override string getOriginal()
        {
            throw new NotImplementedException();
        }

        public override void Mirilla(Interprete interprete)
        {
            throw new NotImplementedException();
        }
    }
}
