using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Aritmeticas
{
    public class Negativo : Nodo
    {
        private Nodo right;

        public Negativo(int linea, int columna, Nodo right) : base(linea, columna)
        {
            this.right = right;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno res_right = this.right.compilar(entorno);

            string tem = Master.getInstancia.newTemporal();

            switch (res_right.getTipo())
            {
                case TipoDatos.Objeto.TipoObjeto.INTEGER:
                case TipoDatos.Objeto.TipoObjeto.REAL:
                    Master.getInstancia.addUnaria(tem, "-" + res_right.getValor());
                    return new Retorno(tem, true, res_right.getTipo());
                default:
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "No se pueden asignar valor negativo a tipo de datos: " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error);
                    throw new Exception("No se pueden asignar valor negativo a tipo de datos: " + res_right.getTipo().ToString());
            }

        }
    }
}
