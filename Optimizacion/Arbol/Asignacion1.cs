using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Asignacion1 : Nodo
    {
        private string direccion;
        private string valor;

        public Asignacion1(int fila, string direccion, string valor) : base(fila)
        {
            this.direccion = direccion;
            this.valor = valor;
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
