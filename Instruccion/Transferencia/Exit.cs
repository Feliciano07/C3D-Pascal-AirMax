using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Instruccion.Variables;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Transferencia
{
    public class Exit : Nodo
    {
        private Nodo exp;

        public Exit(int linea, int columna, Nodo exp):base(linea, columna)
        {
            this.exp = exp;
        }

        public override Retorno compilar(Entorno entorno)
        {
            if (exp == null)
            {
                //exit de un procedimiento
                if(entorno.label_return == null)
                {
                    throw new Exception("Exit fuera del ambito");
                }
                Master.getInstancia.addGoto(entorno.label_return);

                return null;
            }
            else
            {
                Objeto objeto_funcion = entorno.actual_funcion.objeto;
                Retorno retorno = exp.compilar(entorno);

                this.Copiar_Exit(retorno, objeto_funcion);

                Master.getInstancia.addGoto(entorno.label_return);

            }

            return null;
        }

        public void Copiar_Exit(Retorno retorno, Objeto objeto)
        {
            string posicion_stack = Master.getInstancia.newTemporalEntero();
            

            if (!base.Verificar_Tipo(retorno.getObjeto(), objeto))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes");
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes");
            }

            if (objeto.getTipo() == Objeto.TipoObjeto.BOOLEAN)
            {
                string label_salida = Master.getInstancia.newLabel();
                Master.getInstancia.addLabel(retorno.trueLabel);
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, "0", "+");
                Master.getInstancia.addSetStack(posicion_stack, "1");
                Master.getInstancia.addGoto(label_salida);
                Master.getInstancia.addLabel(retorno.falseLabel);
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, "0", "+");
                Master.getInstancia.addSetStack(posicion_stack, "0");
                Master.getInstancia.addLabel(label_salida);

            }
            else if( objeto.getTipo() == Objeto.TipoObjeto.OBJECTS)
            {
                Simbolo simbolo = retorno.sym;

                if(simbolo.isReferencia == false)
                {
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, "0", "+");
                    string inicial = Master.getInstancia.newTemporalEntero();
                    Master.getInstancia.addGetStack(inicial, posicion_stack);
                    Asignacion asignacion = new Asignacion();
                    asignacion.Copiar_Objeto(retorno.getObjeto().symObj, inicial, retorno.getValor());
                }
                else
                {
                    Asignacion asignacion = new Asignacion();
                    string inicial = asignacion.Obtener_Posicion_Referencia(simbolo);
                    asignacion.Copiar_Objeto(retorno.getObjeto().symObj, inicial, retorno.getValor());
                }
            }else if(objeto.getTipo() == Objeto.TipoObjeto.ARRAY)
            {
                Simbolo simbolo = retorno.sym;

                if(simbolo.isReferencia == false)
                {
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, "0", "+");
                    string inicial = Master.getInstancia.newTemporal();
                    Master.getInstancia.addGetStack(inicial, posicion_stack);
                    Asignacion asignacion = new Asignacion();
                    asignacion.Copiar_Arreglo(retorno.getObjeto().symArray, inicial, retorno.getValor());
                }
                else
                {
                    Asignacion asignacion = new Asignacion();
                    string inicial = asignacion.Obtener_Posicion_Referencia(simbolo);
                    asignacion.Copiar_Arreglo(retorno.getObjeto().symArray, inicial, retorno.getValor());
                }

            }
            else
            {
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, "0", "+");
                Master.getInstancia.addSetStack(posicion_stack, retorno.getValor());
            }
        }
    }
}
