using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
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

        private Nodo anterior;

        public AccesoArray(int linea, int columna, string nombre, LinkedList<Nodo> dimension):base(linea, columna)
        {
            this.id = nombre;
            this.dimensiones = dimension;
            this.cantidad = this.dimensiones.Count;
        }

        public AccesoArray() : base(0, 0)
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
                return Arreglo_Global(entorno);
            }
            else
            {
                Retorno res_anterior = this.anterior.compilar(entorno);

                if (res_anterior.getTipo() == Objeto.TipoObjeto.OBJECTS)
                {
                    return Obtener_Atributo_Objeto(res_anterior, entorno);
                }
            }
            return null;
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

            if (sym.getGlobal())
            {
                SimboloArreglo simbolo_aux = sym.getObjeto().symArray;

                // me ubico en la posicion del stack donde esta la variable
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                string posicion_heap = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addUnaria(posicion_stack, sym.getPosicion());


                // obtengo la posicion del heap que se guardaba en el stack
                Master.getInstancia.addGetStack(posicion_heap, posicion_stack);

                string posicion_arreglo = Ubicar_posicion(entorno, simbolo_aux);

                string posicion_absoluta = Master.getInstancia.newTemporalEntero();

                Master.getInstancia.addBinaria(posicion_absoluta, posicion_heap, posicion_arreglo, "+");

                //extraemos el valor del heap
                string valor = Master.getInstancia.newTemporal();
                Master.getInstancia.addGetHeap(valor, posicion_absoluta);
                if (simbolo_aux.objeto.getTipo() != Objeto.TipoObjeto.BOOLEAN)
                {
                    return new Retorno(valor, true, simbolo_aux.objeto,
                        new Simbolo(this.id, simbolo_aux.objeto,
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false), posicion_absoluta);
                }
                Retorno retorno = new Retorno("", false, simbolo_aux.objeto,
                    new Simbolo(this.id, simbolo_aux.objeto,
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false), posicion_absoluta);
                this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
                Master.getInstancia.addGoto(this.falseLabel);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;
            }
            else
            {
                //TODO: cambiar el retorno
                if(sym.isReferencia == false)
                {
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
                    if (simbolo_aux.objeto.getTipo() != Objeto.TipoObjeto.BOOLEAN)
                    {
                        return new Retorno(valor, true, simbolo_aux.objeto,
                            new Simbolo(this.id, simbolo_aux.objeto,
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false), posicion_absoluta);
                    }
                    Retorno retorno = new Retorno("", false, simbolo_aux.objeto,
                        new Simbolo(this.id, simbolo_aux.objeto,
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false), posicion_absoluta);
                    this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                    Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
                    Master.getInstancia.addGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }
                else
                {
                    SimboloArreglo simbolo_aux = sym.getObjeto().symArray;
                    string posicion_arreglo = Ubicar_posicion(entorno, simbolo_aux);
                    string posicion_x = Variable_Referencia(sym);
                    string posicion_absoluta = Master.getInstancia.newTemporalEntero();
                    Master.getInstancia.addBinaria(posicion_absoluta, posicion_x, posicion_arreglo, "+");
                    string valor = Master.getInstancia.newTemporal();
                    Master.getInstancia.addGetHeap(valor, posicion_absoluta);
                    if (simbolo_aux.objeto.getTipo() != Objeto.TipoObjeto.BOOLEAN)
                    {
                        return new Retorno(valor, true, simbolo_aux.objeto, sym, posicion_absoluta);
                    }
                    Retorno retorno = new Retorno("", false, simbolo_aux.objeto, sym, posicion_absoluta);
                    this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                    Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
                    Master.getInstancia.addGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }
            }


        }

        public string Variable_Referencia(Simbolo sym)
        {
            string posicion_stack = Master.getInstancia.newTemporalEntero();
            string valor = Master.getInstancia.newTemporal();
            string posicion = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, (sym.posicion + 1).ToString(), "+");
            Master.getInstancia.addGetStack(valor, posicion_stack);

            string label_true = Master.getInstancia.newLabel();
            string label_false = Master.getInstancia.newLabel();
            string salida = Master.getInstancia.newLabel();

            Master.getInstancia.addBinaria(posicion_stack, posicion_stack, "1", "-");
            Master.getInstancia.addGetStack(posicion, posicion_stack);

            Master.getInstancia.addif(valor, "0", "==", label_true);
            Master.getInstancia.addGoto(label_false);

            Master.getInstancia.addLabel(label_true);
            Master.getInstancia.addGetStack(valor, posicion);

            Master.getInstancia.addGoto(salida);

            Master.getInstancia.addLabel(label_false);
            Master.getInstancia.addGetHeap(valor, posicion);

            Master.getInstancia.addLabel(salida);
            return valor;
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


        public Retorno Obtener_Atributo_Objeto(Retorno res_anterior, Entorno entorno)
        {
            SimboloObjeto simboloObjeto = res_anterior.getObjeto().symObj;
            /*
             * Retorna el atributo y la posicion respecto al objeto
             */
            Atributo_Index atributo = simboloObjeto.getAtributo(this.id);
            if (atributo == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
                Master.getInstancia.addError(error);
                throw new Exception("El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
            }

            string pos_heap = Master.getInstancia.newTemporalEntero();
            /*
             * pos_heap = nos permite almacena la posicion del atributo en el heap
             * res_anterior.getValor() = retorna la posicion de donde inicia el objeto
             * atributo.index = es la posicion del atributo
             * T12 = 2 + 0;
             */
            Master.getInstancia.addBinaria(pos_heap, res_anterior.getValor(), atributo.index.ToString(), "+");


            string valor = Master.getInstancia.newTemporal();
            /*
             * (primitivo) valor = guarda el valor como tal del atributo
             * (types) valor = guarda una posicion en el heap del atributo
             */
            Master.getInstancia.addGetHeap(valor, pos_heap);

            if(atributo.atributo.getObjeto().getTipo() == Objeto.TipoObjeto.ARRAY)
            {
                return Arreglo_Objeto(atributo.atributo, valor, entorno);
            }
            else if (atributo.atributo.getObjeto().getTipo() != Objeto.TipoObjeto.BOOLEAN)
            {
                return new Retorno(valor, true, atributo.atributo.getObjeto(),
                    new Simbolo(this.id, atributo.atributo.getObjeto(),
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, atributo.index, "", false), pos_heap);
            }
            

            Retorno retorno = new Retorno("", false, atributo.atributo.getObjeto(),
                new Simbolo(this.id, atributo.atributo.getObjeto(),
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, atributo.index, "", false), pos_heap);
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
            Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
            Master.getInstancia.addGoto(this.falseLabel);
            retorno.trueLabel = this.trueLabel;
            retorno.falseLabel = this.falseLabel;
            return retorno;
        }


        public Retorno Arreglo_Objeto(Atributo atributo, string inicio_arreglo, Entorno entorno)
        {
            if(this.dimensiones == null)
            {
                return new Retorno(inicio_arreglo, true, atributo.getObjeto());
            }
            else
            {
                SimboloArreglo simboloArreglo = atributo.getObjeto().symArray;

                string posicion_arreglo = Ubicar_posicion(entorno, simboloArreglo);

                string posicion_absoluta = Master.getInstancia.newTemporalEntero();

                Master.getInstancia.addBinaria(posicion_absoluta, inicio_arreglo, posicion_arreglo, "+");


                string valor = Master.getInstancia.newTemporal();
                Master.getInstancia.addGetHeap(valor, posicion_absoluta);
                if (simboloArreglo.objeto.getTipo() != Objeto.TipoObjeto.BOOLEAN)
                {
                    return new Retorno(valor, true, simboloArreglo.objeto,
                        new Simbolo(this.id, simboloArreglo.objeto,
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false), posicion_absoluta);
                }
                Retorno retorno = new Retorno("", false, simboloArreglo.objeto,
                    new Simbolo(this.id, simboloArreglo.objeto,
                Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP, 0, "", false), posicion_absoluta);

                this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
                Master.getInstancia.addGoto(this.falseLabel);
                retorno.trueLabel = this.trueLabel;
                retorno.falseLabel = this.falseLabel;
                return retorno;

            }
        }

    }
}
