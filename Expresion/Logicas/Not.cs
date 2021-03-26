using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Logicas
{
    public class Not : Nodo
    {
        
        private Nodo right;
        public Not(int linea, int columna, Nodo right) : base(linea, columna)
        {
            this.right = right;
        }
        public override Retorno compilar(Entorno entorno)
        {
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;

            this.right.trueLabel = this.falseLabel;
            this.right.falseLabel = this.trueLabel;

            Retorno res_right = this.right.compilar(entorno);

            if(res_right.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                Retorno retorno = new Retorno("", true, TipoDatos.Objeto.TipoObjeto.BOOLEAN);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;
            }
            Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                   "No se puede operar NOT a un tipos de datos: " + res_right.getTipo().ToString());
            Master.getInstancia.addError(error);
            throw new Exception("La operacion logica NOT solo puede hacerse con valores booleanos");

        }
    }
}
