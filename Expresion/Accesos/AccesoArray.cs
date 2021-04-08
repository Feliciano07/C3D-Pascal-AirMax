using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Accesos
{
    public class AccesoArray : Nodo
    {
        private string id;
        private LinkedList<Nodo> dimensiones;
        private int cantidad;

        public AccesoArray(int linea, int columna, string nombre, LinkedList<Nodo> dimension):base(linea, columna)
        {
            this.id = nombre;
            this.dimensiones = dimension;
            this.cantidad = this.dimensiones.Count;
        }


        public override Retorno compilar(Entorno entorno)
        {
            return Arreglo_Global(entorno);   
        }

        public Retorno Arreglo_Global(Entorno entorno)
        {
            Simbolo sym = entorno.getSimbolo(this.id);
            if (sym == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "No existe la variable: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("No existe la variable: " + this.id);
            }

            SimboloArreglo simbolo_aux = sym.getObjeto().symArray;

            // me ubico en la posicion del stack donde esta la variable
            string posicion_stack = Master.getInstancia.newTemporalEntero();
            string posicion_heap = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, sym.getPosicion(), "+");


            // obtengo la posicion del heap que se guardaba en el stack
            Master.getInstancia.addGetStack(posicion_heap, posicion_stack);

            string posicion_arreglo = Ubicar_posicion(entorno, simbolo_aux);

            string posicion_absoluta = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addBinaria(posicion_absoluta, posicion_heap, posicion_arreglo, "+");

            //extraemos el valor del heap
            string valor = Master.getInstancia.newTemporal();
            Master.getInstancia.addGetHeap(valor, posicion_absoluta);
            if(simbolo_aux.objeto.getTipo() != TipoDatos.Objeto.TipoObjeto.BOOLEAN)
            {
                return new Retorno(valor, true, simbolo_aux.objeto, sym);
            }
            Retorno retorno = new Retorno("", false, simbolo_aux.objeto, sym);
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
            Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
            Master.getInstancia.addGoto(this.falseLabel);
            retorno.trueLabel = this.trueLabel;
            retorno.falseLabel = this.falseLabel;
            return retorno;
        }

        public string Ubicar_posicion(Entorno entorno, SimboloArreglo arreglo)
        {
            Nodo[] aux = new Nodo[this.dimensiones.Count];
            this.dimensiones.CopyTo(aux, 0);
            if(this.cantidad == 1)
            {
                Retorno r1 = aux[0].compilar(entorno);
                return arreglo.Posicion_Una_dimension(r1.getValor());
            }else if(this.cantidad == 2)
            {
                Retorno r1 = aux[0].compilar(entorno);
                Retorno r2 = aux[1].compilar(entorno);
                return arreglo.Posicion_Dos_Dimensiones(r1.getValor(), r2.getValor());
            }else if(this.cantidad == 3)
            {
                Retorno r1 = aux[0].compilar(entorno);
                Retorno r2 = aux[1].compilar(entorno);
                Retorno r3 = aux[2].compilar(entorno);
                return arreglo.Posicion_Tres_dimensiones(r1.getValor(), r2.getValor(), r3.getValor());
            }
            return "0";
        }
    }
}
