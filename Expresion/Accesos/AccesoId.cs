using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
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
        public void setAnterior(AccesoId anterior)
        {
            this.anterior = anterior;
        }

        public override Retorno compilar(Entorno entorno)
        {
            //TODO: falta ver la forma de acceso a un objeto y acceso a locales
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
                string tem = Master.getInstancia.newTemporal();
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, sym.getPosicion(), "+");

                if (sym.getGlobal())
                {
                    Master.getInstancia.addGetStack(tem, posicion_stack);

                    if(sym.getTipo() != TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                    {
                        return new Retorno(tem, true, sym.getObjeto(), sym);
                    }
                    Retorno retorno = new Retorno("", false, sym.getObjeto(), sym);
                    this.trueLabel = this.trueLabel == "" ? Master.getInstancia.newLabel() : this.trueLabel;
                    this.falseLabel = this.falseLabel == "" ? Master.getInstancia.newLabel() : this.falseLabel;
                    Master.getInstancia.addif(tem, "1", "==", this.trueLabel);
                    Master.getInstancia.addGoto(this.falseLabel);
                    retorno.trueLabel = this.trueLabel;
                    retorno.falseLabel = this.falseLabel;
                    return retorno;
                }
                else
                {
                    //TODO: acceso variables no locales
                }
            }
            else
            {
                //TODO: acesso a objeto
                Retorno res_anterior = this.anterior.compilar(entorno);
                SimboloObjeto simboloObjeto = res_anterior.getObjeto().symObj;
                if(res_anterior.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
                {
                    Atributo_Index atributo = simboloObjeto.getAtributo(this.id);
                    if(atributo == null)
                    {
                        Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                            "El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
                        Master.getInstancia.addError(error);
                        throw new Exception("El objeto: " + simboloObjeto.id + " no tiene el atributo: " + this.id);
                    }
                    string pos_heap = Master.getInstancia.newTemporalEntero();
                    string valor = Master.getInstancia.newTemporalEntero();

                    //conseguir la posicion del atributo
                    Master.getInstancia.addBinaria(pos_heap, res_anterior.getValor(), atributo.index.ToString(), "+");
                    // conseguir la posicion en el heap
                    Master.getInstancia.addGetHeap(valor, pos_heap);
                    return new Retorno(valor, true, atributo.atributo.getObjeto());
                }
            }
            return null;
        }
    }
}
