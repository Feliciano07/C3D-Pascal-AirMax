using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Relacionales
{
    public class MayorQ : Nodo
    {
        private Nodo left;
        private Nodo right;

        public MayorQ(int linea, int columna, Nodo left, Nodo right) : base(linea, columna)
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
                            Master.getInstancia.addif(res_left.getValor(), res_right.getValor(), ">", this.trueLabel);
                            Master.getInstancia.addGoto(this.falseLabel);
                            Retorno retorno = new Retorno("", false, Objeto.TipoObjeto.BOOLEAN);
                            retorno.trueLabel = this.trueLabel;
                            retorno.falseLabel = this.falseLabel;
                            return retorno;
                        default:
                            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                            "No se puede aplicar mayor que a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                            Master.getInstancia.addError(error);
                            throw new Exception("No se puede aplicar mayor  que a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    }
                case Objeto.TipoObjeto.STRING:
                    res_right = this.right.compilar(entorno);
                    if (res_right.getTipo() == Objeto.TipoObjeto.STRING)
                    {
                        return Llamada_Nativa_Comparar(res_left, res_right, entorno.getSize());
                    }
                    Error error1 = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "No se puede aplicar mayor  que a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error1);
                    throw new Exception("No se puede aplicar mayor  que a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());

                case Objeto.TipoObjeto.BOOLEAN:
                    string trueLabel = Master.getInstancia.newLabel();
                    string falseLabel = Master.getInstancia.newLabel();

                    Master.getInstancia.addLabel(res_left.trueLabel);
                    this.right.trueLabel = falseLabel;
                    this.right.falseLabel = trueLabel;
                    res_right = this.right.compilar(entorno);

                    Master.getInstancia.addLabel(res_left.falseLabel);
                    this.right.trueLabel = falseLabel;
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
                    "No se puede aplicar mayor que a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error2);
                    throw new Exception("No se puede aplicar mayor  que a tipos de datos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                default:
                    Error error3 = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "No se puede aplicar mayor que a tipos de datos " + res_left.getTipo().ToString());
                    Master.getInstancia.addError(error3);
                    throw new Exception("No se puede aplicar mayor  que a tipos de datos " + res_left.getTipo().ToString());
            }

           
        }

        public Retorno Llamada_Nativa_Comparar(Retorno res_left, Retorno res_right, int size)
        {
            string tem = Master.getInstancia.newTemporal();
            Master.getInstancia.addBinaria(tem, Master.getInstancia.stack_p, size.ToString(), "+");
            string tem1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem1, tem, "1", "+");
            Master.getInstancia.addSetStack(tem1, res_left.getValor());
            tem1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem1, tem, "2", "+");
            Master.getInstancia.addSetStack(tem1, res_right.getValor());
            //cambio entorno
            Master.getInstancia.plusStack(size.ToString());
            Master.getInstancia.callFuncion("native_mayorQ_str");
            Master.getInstancia.addBinaria(tem1, Master.getInstancia.stack_p, "0", "+");
            string tem2 = Master.getInstancia.newTemporal();// posicion  que tiene si se cumple la condicion o no
            Master.getInstancia.addGetStack(tem2, tem1);
            Master.getInstancia.substracStack(size.ToString());
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
            Master.getInstancia.addif(tem2, "1", "==", this.trueLabel);
            Master.getInstancia.addGoto(this.falseLabel);
            Retorno retorno = new Retorno("", false, Objeto.TipoObjeto.BOOLEAN);
            retorno.trueLabel = this.trueLabel;
            retorno.falseLabel = this.falseLabel;
            return retorno;

        }
    }
}
