using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Enviroment;

namespace C3D_Pascal_AirMax.Expresion.Constantes
{
    public class PrimitivoC : Nodo
    {
        private Objeto.TipoObjeto tipo;
        private object valor;

        public PrimitivoC(int linea, int columna, Objeto.TipoObjeto tipo, object valor):base(linea,columna)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public override Retorno compilar(Entorno entorno)
        {
            switch (tipo)
            {
                case Objeto.TipoObjeto.INTEGER:
                    return new Retorno(this.valor.ToString(), false, Objeto.TipoObjeto.INTEGER);
                case Objeto.TipoObjeto.REAL:
                    return new Retorno(this.valor.ToString(), false, Objeto.TipoObjeto.REAL);
                case Objeto.TipoObjeto.BOOLEAN:
                    Retorno retorno = new Retorno("", false, Objeto.TipoObjeto.BOOLEAN);
                    this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                    bool booleano = bool.Parse(this.valor.ToString());
                    if (booleano)
                    {
                        Master.getInstancia.addGoto(this.trueLabel);
                    }
                    else
                    {
                        Master.getInstancia.addGoto(this.falseLabel);
                    }
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
            }
            return null;
        }
    }
}
