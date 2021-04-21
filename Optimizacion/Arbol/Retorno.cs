using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Retorno : Nodo
    {
        public string ret;

        public Retorno(int fila, string ret) : base(fila)
        {
            this.ret = ret;
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
