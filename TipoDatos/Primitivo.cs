using C3D_Pascal_AirMax.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.TipoDatos
{
    public class Primitivo : Objeto
    {
        public object valor;

        public Primitivo(TipoObjeto tipo, object valor) : base(tipo)
        {
            this.valor = valor;
        }

        public override object getValor()
        {
            // TODO: esto se puede mejorar
            return this.valor;
        }
    }
}
