using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Aritmeticas
{
    public class Suma : Nodo
    {
        private Nodo left;
        private Nodo right;

        public Suma(int linea, int columna, Nodo left, Nodo right):base(linea, columna)
        {
            this.left = left;
            this.right = right;
        }

        public override Retorno compilar()
        {
            Retorno res_left = left.compilar();
            Retorno res_right = right.compilar();
            Objeto.TipoObjeto tipo_dominante = TablaTipo.tabla[res_left.getTipo().GetHashCode(), res_right.getTipo().GetHashCode()];

            string tem = Master.getInstancia.newTemporal();

            //TODO: Falta concantenar y generar errores
            switch (tipo_dominante)
            {
                case Objeto.TipoObjeto.INTEGER:
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "+");
                    return new Retorno(tem, true, tipo_dominante);
                case Objeto.TipoObjeto.REAL:
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "+");
                    return new Retorno(tem, true, tipo_dominante);
            }
            return null;
        }
    }
}
