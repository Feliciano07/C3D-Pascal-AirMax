using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;

namespace C3D_Pascal_AirMax.Expresion.Constantes
{
    public class PrimitivoC : Operacion
    {
        private Objeto.TipoObjeto tipo;
        private object valor;

        public PrimitivoC(int linea, int columna, Objeto.TipoObjeto tipo, object valor):base(linea,columna)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public override Retorno compilar()
        {
            switch (tipo)
            {
                case Objeto.TipoObjeto.INTEGER:
                    return new Retorno(this.valor.ToString(), false, Objeto.TipoObjeto.INTEGER);
                case Objeto.TipoObjeto.REAL:
                    return new Retorno(this.valor.ToString(), false, Objeto.TipoObjeto.REAL);
            }
            return null;
        }
    }
}
