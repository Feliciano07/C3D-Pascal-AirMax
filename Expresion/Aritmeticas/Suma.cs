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
 

            //TODO: Falta concantenar
            switch (tipo_dominante)
            {
                case Objeto.TipoObjeto.INTEGER:
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "+");
                    return new Retorno(tem, true, tipo_dominante);
                case Objeto.TipoObjeto.REAL:
                    Master.getInstancia.addBinaria(tem, res_left.getValor(), res_right.getValor(), "+");
                    return new Retorno(tem, true, tipo_dominante);
                case Objeto.TipoObjeto.STRING:
                    return concatenar(res_left, res_right);
                default:
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "No se pueden sumas los tipos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
                    Master.getInstancia.addError(error);
                    throw new Exception("No se pueden sumas los tipos " + res_left.getTipo().ToString() + " con " + res_right.getTipo().ToString());
            }
            
        }

        public Retorno concatenar(Retorno left, Retorno right)
        {
            string tem = Master.getInstancia.newTemporal(); // guarda la posicion inicial de la nueva cadena
            string tem_posicion = Master.getInstancia.newTemporal();
            string tem_valor = Master.getInstancia.newTemporal();
            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();
            string label_salto = Master.getInstancia.newLabel();
            Master.getInstancia.addUnaria(tem, Master.getInstancia.heap_p); // guarda en el tem, la posicion del heap
            Master.getInstancia.addUnaria(tem_posicion, left.getValor());// primera cadena
            // meter label de retorno
            Master.getInstancia.addLabel(label_salto);
            //ingresar al heap
            Master.getInstancia.addGetHeap(tem_valor, tem_posicion);
            //if
            Master.getInstancia.addif(tem_valor, "-1", "!=", label_true);
            //goto
            Master.getInstancia.addGoto(label_false);
            //verdadero
            Master.getInstancia.addLabel(label_true);
            //meter al heap
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, tem_valor);
            // siguiente pos del heap
            Master.getInstancia.nextHeap();
            // siguiente posicion de la cadena
            Master.getInstancia.addBinaria(tem_posicion, tem_posicion, "1", "+");
            // retorno
            Master.getInstancia.addGoto(label_salto);
            // etiqueta falsa
            Master.getInstancia.addLabel(label_false);

            Master.getInstancia.addUnaria(tem_posicion, right.getValor());// segunda cadena
            string label_true1 = Master.getInstancia.newLabel();
            string label_false1 = Master.getInstancia.newLabel();
            string label_salto1 = Master.getInstancia.newLabel();
            // meter label de salto
            Master.getInstancia.addLabel(label_salto1);
            // ingresar al heap
            Master.getInstancia.addGetHeap(tem_valor, tem_posicion);
            //if
            Master.getInstancia.addif(tem_valor, "-1", "!=", label_true1);
            // got
            Master.getInstancia.addGoto(label_false1);
            // etiqueta verdadera
            Master.getInstancia.addLabel(label_true1);
            // guardar en el heap
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, tem_valor);
            // siguiente pos del heap
            Master.getInstancia.nextHeap();
            // siguiente posicion de la cadena
            Master.getInstancia.addBinaria(tem_posicion, tem_posicion, "1", "+");
            // retorno
            Master.getInstancia.addGoto(label_salto1);
            // etiqueta falsa
            Master.getInstancia.addLabel(label_false1);
            return new Retorno(tem, true, Objeto.TipoObjeto.STRING);
        }

    }
}
