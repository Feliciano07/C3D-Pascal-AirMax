using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Asignaciones
{
    public class AsignacionArray : Nodo
    {
        private string id;
        private LinkedList<Nodo> dimensiones;
        private int cantidad;
        private Nodo anterior;

        public AsignacionArray(int linea, int columna, string nombre, LinkedList<Nodo> dimension) : base(linea, columna)
        {
            this.id = nombre;
            this.dimensiones = dimension;
            this.cantidad = this.dimensiones.Count;
        }

        public AsignacionArray() : base(0, 0)
        {

        }

        public void setAnterior(Nodo anterior)
        {
            this.anterior = anterior;
        }


        public override Retorno compilar(Entorno entorno)
        {
            if(this.anterior == null)
            {
                return Asignacion_Global(entorno);
            }
            else
            {
                Retorno res_anterior = this.anterior.compilar(entorno);
                if(res_anterior.getTipo() == Objeto.TipoObjeto.OBJECTS)
                {
                    return Obtener_Atributo_Objeto(res_anterior);
                }
            }

            return null;
        }

        public Retorno Asignacion_Global(Entorno entorno)
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


            return new Retorno(posicion_absoluta, true, simbolo_aux.objeto,
                new Simbolo("", simbolo_aux.objeto, Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false));

        }

        public string Ubicar_posicion(Entorno entorno, SimboloArreglo arreglo)
        {
            Nodo[] aux = new Nodo[this.dimensiones.Count];
            this.dimensiones.CopyTo(aux, 0);
            if (this.cantidad == 1)
            {
                Retorno r1 = aux[0].compilar(entorno);
                return arreglo.Posicion_Una_dimension(r1.getValor());
            }
            else if (this.cantidad == 2)
            {
                Retorno r1 = aux[0].compilar(entorno);
                Retorno r2 = aux[1].compilar(entorno);
                return arreglo.Posicion_Dos_Dimensiones(r1.getValor(), r2.getValor());
            }
            else if (this.cantidad == 3)
            {
                Retorno r1 = aux[0].compilar(entorno);
                Retorno r2 = aux[1].compilar(entorno);
                Retorno r3 = aux[2].compilar(entorno);
                return arreglo.Posicion_Tres_dimensiones(r1.getValor(), r2.getValor(), r3.getValor());
            }
            return "0";
        }

        public Retorno Obtener_Atributo_Objeto(Retorno res_retorno)
        {
            SimboloObjeto simboloObjeto = res_retorno.getObjeto().symObj;
            Atributo_Index atributo = simboloObjeto.getAtributo(this.id);
            if (atributo == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
            }
            string posicion = Master.getInstancia.newTemporalEntero();
            /*
             * verifico si el atributo esta en heap o stack
             * posicion = guarda la posicion donde inicial el objeto
             */
            string valor = Master.getInstancia.newTemporalEntero();
            /*
             * (primitivo) valor = guarda el valor como tal del atributo
             * (types) valor = guarda una posicion en el heap del atributo
             */
            if (res_retorno.sym.pointer == Simbolo.Pointer.HEAP)
            {

                Master.getInstancia.addGetHeap(posicion, res_retorno.getValor());
            }
            else
            {
                string contador = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(contador, Master.getInstancia.stack_p, res_retorno.getValor(), "+");
                Master.getInstancia.addGetStack(posicion, contador);
            }

            Master.getInstancia.addBinaria(valor, posicion, atributo.index.ToString(), "+");

            /*
             * retorno el valor y un simbolo que posee la posicion relativa al objeto encontrado
             */
            return new Retorno(valor, true, atributo.atributo.getObjeto(), new Simbolo(this.id, atributo.atributo.getObjeto(),
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, atributo.index, "", false));
        }

    }
}
