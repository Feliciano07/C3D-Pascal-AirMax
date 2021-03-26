using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Relacionales
{
    public class NoIgual : Nodo
    {
        private Nodo left;
        private Nodo right;

        public NoIgual(int linea, int columna, Nodo left, Nodo right):base(linea, columna)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno res_left = this.left.compilar(entorno);
            Retorno res_right = null;

            switch (res_left.getTipo())
            {
                case Objeto.TipoObjeto.INTEGER:
                case Objeto.TipoObjeto.REAL:
                    res_right = this.right.compilar(entorno);
                    switch (res_right.getTipo())
                    {
                        case Objeto.TipoObjeto.INTEGER:
                        case Objeto.TipoObjeto.REAL:
                            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                            Master.getInstancia.addif(res_left.getValor(), res_right.getValor(), "!=", this.trueLabel);
                            Master.getInstancia.addGoto(this.falseLabel);
                            Retorno retorno = new Retorno("", false, Objeto.TipoObjeto.BOOLEAN);
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                        default:
                            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                            "No se puede aplicar no igual a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                            Master.getInstancia.addError(error);
                            throw new Exception("No se puede aplicar no igual a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    }
                case Objeto.TipoObjeto.BOOLEAN:
                    string trueLabel = Master.getInstancia.newLabel();
                    string falseLabel = Master.getInstancia.newLabel();

                    Master.getInstancia.addLabel(res_left.trueLabel);
                    this.right.trueLabel = falseLabel;
                    this.right.falseLabel = trueLabel;
                    res_right = this.right.compilar(entorno);

                    Master.getInstancia.addLabel(res_left.falseLabel);
                    this.right.trueLabel = trueLabel;
                    this.right.falseLabel = falseLabel;
                    res_right = this.right.compilar(entorno);
                    if (res_right.getTipo() == Objeto.TipoObjeto.BOOLEAN)
                    {
                        Retorno retorno1 = new Retorno("", false, res_left.getTipo());
                        retorno1.trueLabel = trueLabel;
                        retorno1.falseLabel = falseLabel;
                        return retorno1;
                    }
                    Error error2 = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "No se puede aplicar no igual a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error2);
                    throw new Exception("No se puede aplicar no igual a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                default:
                    Error error3 = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                   "No se puede aplicar no igual a tipos de datos " + res_left.getTipo().ToString());
                    Master.getInstancia.addError(error3);
                    throw new Exception("No se puede aplicar no igual a tipos de datos " + res_left.getTipo().ToString());

            }

        }
    }
}
