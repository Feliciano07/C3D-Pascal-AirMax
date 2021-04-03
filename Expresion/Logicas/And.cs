using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Logicas
{
    public class And : Nodo
    {
        private Nodo left;
        private Nodo right;

        public And(int linea, int columna, Nodo left, Nodo right) : base(linea, columna)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compilar(Entorno entorno)
        {
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;

            this.left.trueLabel = Master.getInstancia.newLabel();
            this.right.trueLabel = this.trueLabel;
            this.left.falseLabel = this.right.falseLabel = this.falseLabel;

            Retorno res_left = this.left.compilar(entorno);
            Master.getInstancia.addLabel(this.left.trueLabel);
            Retorno res_right = this.right.compilar(entorno);

            if(res_left.getTipo() == Objeto.TipoObjeto.BOOLEAN && res_right.getTipo() == Objeto.TipoObjeto.BOOLEAN)
            {
                Retorno retorno = new Retorno("", true, new Objeto(Objeto.TipoObjeto.BOOLEAN));
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.right.falseLabel;
                return retorno;
            }
            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                   "No se puede operar AND en tipos" + res_left.getTipo().ToString() + "con" + res_right.getTipo().ToString());
            Master.getInstancia.addError(error);
            throw new Exception("La operacion logica AND solo puede hacerse con valores booleanos");

        }
    }
}
