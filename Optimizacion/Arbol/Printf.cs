using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Optimizacion.Arbol
{
    public class Printf : Nodo
    {
        private string formato;
        private string tipo;
        private string valor;
        public Printf(int fila, string formato, string tipo, string valor) : base(fila)
        {
            this.formato = formato;
            this.tipo = tipo;
            this.valor = valor;
        }

        public override string getOriginal()
        {
            return "printf("+this.formato+", ("+this.tipo+")"+this.valor+");";
        }

        public override void Mirilla(Interprete interprete)
        {
            interprete.IP++;
        }
    }
}
