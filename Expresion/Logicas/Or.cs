using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Logicas
{
    public class Or : Nodo
    {
        private Nodo left;
        private Nodo right;

        public Or(int linea, int columna, Nodo left, Nodo right):base(linea, columna)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compilar(Entorno entorno)
        {
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;

            this.left.trueLabel = this.right.trueLabel = this.trueLabel;
            this.right.falseLabel = Master.getInstancia.newLabel();

            Retorno res_left = this.left.compilar(entorno);
            Master.getInstancia.addLabel(this.left.falseLabel);
            Retorno res_right = this.right.compilar(entorno);

            if(res_left.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN && res_right.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                Retorno retorno = new Retorno("", false, new Objeto(TipoDatos.Objeto.TipoObjeto.BOOLEAN));
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.right.falseLabel;
                return retorno;
            }
            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                  "No se puede operar OR en tipos" + res_left.getTipo().ToString() + "con" + res_right.getTipo().ToString());
            Master.getInstancia.addError(error);
            throw new Exception("La operacion logica OR solo puede hacerse con valores booleanos");

        }
    }
}
