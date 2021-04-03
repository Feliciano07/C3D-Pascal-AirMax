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
    public class Suma : Nodo
    {
        private Nodo left;
        private Nodo right;

        public Suma(int linea, int columna, Nodo left, Nodo right):base(linea, columna)
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
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "+");
                    return new Retorno(tem, true, new Objeto(tipo_dominante));
                case Objeto.TipoObjeto.REAL:
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "+");
                    return new Retorno(tem, true, new Objeto(tipo_dominante));
                case Objeto.TipoObjeto.STRING:
                    return Llamada_nativa_concatenar(res_left,res_right,entorno.getSize());
                default:
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "No se pueden sumas los tipos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error);
                    throw new Exception("No se pueden sumas los tipos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
            }
        }

        public Retorno Llamada_nativa_concatenar(Retorno res_left, Retorno res_right, int size)
        {
            string tem = Master.getInstancia.newTemporal();
            Master.getInstancia.addBinaria(tem, Master.getInstancia.stack_p, size.ToString() , "+");
            string tem1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem1, tem, "1", "+");
            Master.getInstancia.addSetStack(tem1, res_left.getValor());
            tem1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem1, tem, "2", "+");
            Master.getInstancia.addSetStack(tem1, res_right.getValor());
            //cambio entorno
            Master.getInstancia.plusStack(size.ToString());
            Master.getInstancia.callFuncion("native_concat_str");
            Master.getInstancia.addBinaria(tem1, Master.getInstancia.stack_p, "0", "+");
            string tem2 = Master.getInstancia.newTemporal();// posicion inicio de nueva cadena
            Master.getInstancia.addGetStack(tem2, tem1);
            Master.getInstancia.substracStack(size.ToString());
            return new Retorno(tem2, true, new Objeto(Objeto.TipoObjeto.STRING));
        }

    }
}
