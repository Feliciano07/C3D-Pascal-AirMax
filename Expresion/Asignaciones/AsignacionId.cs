using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Asignaciones
{
    public class AsignacionId : Nodo
    {
        private string id;
        private Nodo anterior;

        public AsignacionId(int linea, int columna, string id, Nodo anterior):base(linea, columna)
        {
            this.id = id;
            this.anterior = anterior;
        }
        public  AsignacionId() : base(0, 0)
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


                if (sym.getGlobal())
                {
                    return new Retorno(sym.getPosicion(), false, sym.getObjeto(),sym);
                }
                else
                {
                    //TODO: variables que no son locales
                }
            }
            else
            {
                Retorno res_retorno = this.anterior.compilar(entorno);
                SimboloObjeto simboloObjeto = res_retorno.getObjeto().symObj;
                if(res_retorno.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
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

                    //TODO: validar si debo acceder al heap o stack
                    if(res_retorno.sym.pointer == Simbolo.Pointer.HEAP)
                    {
                        Master.getInstancia.addGetHeap(pos_heap, res_retorno.getValor());
                    }
                    else
                    {
                        //inicio del objeto
                        Master.getInstancia.addGetStack(pos_heap, res_retorno.getValor());
                    }
                    
                    //posicion del atributo
                    Master.getInstancia.addBinaria(valor, pos_heap, atributo.index.ToString(), "+");

                    return new Retorno(valor, true, atributo.atributo.tipo, new Simbolo(this.id, atributo.atributo.tipo, Simbolo.Rol.VARIABLE, Simbolo.Pointer.HEAP,
                        atributo.index, "", false));
                }
            }
            return null;
        }
    }
}
