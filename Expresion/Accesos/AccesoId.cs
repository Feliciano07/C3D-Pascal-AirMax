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
    public class AccesoId : Nodo
    {
        private string id;
        private Nodo anterior;

        public AccesoId(int linea, int columna, string id, Nodo anterior) : base(linea, columna)
        {
            this.id = id;
            this.anterior = anterior;
        }
        public AccesoId():base(0,0)
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
                Simbolo sym = entorno.getSimbolo(this.id);
                if(sym == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "No existe la variable: " + this.id);
                    Master.getInstancia.addError(error);
                    throw new Exception("No existe la variable: " + this.id);
                }
                /*
                 * (primitivo)tem: guarda el valor como tal de la variable
                 * (types)tem: guarda la posicion de donde inicia en el heap
                 */
                
                if (sym.getGlobal())
                {
                    string valor = Master.getInstancia.newTemporal();
                    string posicion_stack = Master.getInstancia.newTemporalEntero();
                    Master.getInstancia.addUnaria(posicion_stack, sym.getPosicion());
                    Master.getInstancia.addGetStack(valor, posicion_stack);
                    if (sym.getTipo() != Objeto.TipoObjeto.BOOLEAN)
                    {
                        return new Retorno(valor, true, sym.getObjeto(), sym, posicion_stack);
                    }
                    Retorno retorno = new Retorno("", false, sym.getObjeto(), sym, posicion_stack);
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
                    
                    if(sym.isReferencia == false)
                    {
                        string valor = Master.getInstancia.newTemporal();
                        string posicion_stack = Master.getInstancia.newTemporalEntero();
                        Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, sym.getPosicion(), "+");
                        Master.getInstancia.addGetStack(valor, posicion_stack);
                        if (sym.getTipo() != Objeto.TipoObjeto.BOOLEAN)
                        {
                            return new Retorno(valor, true, sym.getObjeto(), sym, posicion_stack);
                        }
                        Retorno retorno = new Retorno("", false, sym.getObjeto(), sym, posicion_stack);
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
                        return Variable_Referencia(sym);
                    }
                }

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


        public Retorno Variable_Referencia(Simbolo sym)
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

            if(sym.getTipo() != Objeto.TipoObjeto.BOOLEAN)
            {
                return new Retorno(valor, true, sym.getObjeto(), sym, posicion);
            }

            Retorno retorno = new Retorno("", false, sym.getObjeto(), sym, posicion);
            this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
            this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
            Master.getInstancia.addif(valor, "1", "==", this.trueLabel);
            Master.getInstancia.addGoto(this.falseLabel);
            retorno.trueLabel = this.trueLabel;
            retorno.falseLabel = this.falseLabel;
            return retorno;

        }

        public Retorno Obtener_Atributo_Objeto(Retorno res_anterior)
        {
            SimboloObjeto simboloObjeto = res_anterior.getObjeto().symObj;
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
            if (atributo.atributo.getObjeto().getTipo() != Objeto.TipoObjeto.BOOLEAN)
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

        public string Obtener_Posicion_Referencia(Simbolo sym)
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

    }
}
