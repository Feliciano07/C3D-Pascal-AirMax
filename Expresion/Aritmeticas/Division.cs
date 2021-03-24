using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using C3D_Pascal_AirMax.Enviroment;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Aritmeticas
{
    public class Division : Nodo
    {
        private Nodo left;
        private Nodo right;

        public Division(int linea, int columna, Nodo left, Nodo right):base(linea, columna)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno res_left = left.compilar(entorno);
            Retorno res_right = right.compilar(entorno);

            Objeto.TipoObjeto tipo_dominante = TablaTipo.tabla[res_left.getTipo().GetHashCode(), res_right.getTipo().GetHashCode()];

            string tem = Master.getInstancia.newTemporal();
            switch (tipo_dominante)
            {
                case Objeto.TipoObjeto.INTEGER:
                case Objeto.TipoObjeto.REAL:
                    // TODO: aqui deberia de ir que no se divida entre 0
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "/");
                    return new Retorno(tem, true, Objeto.TipoObjeto.INTEGER);
                default:
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                   "No se pueden sumas los tipos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error);
                    throw new Exception("No se pueden sumas los tipos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
            }

        }
    }
}
