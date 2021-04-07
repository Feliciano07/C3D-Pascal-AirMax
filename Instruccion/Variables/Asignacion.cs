using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Instruccion.Variables
{
    public class Asignacion : Nodo
    {
        private Nodo variable;
        private Nodo valor;

        public Asignacion(int linea, int columna, Nodo var, Nodo valor):base(linea, columna)
        {
            this.variable = var;
            this.valor = valor;
        }

        public override Retorno compilar(Entorno entorno)
        {
            Retorno asig = this.variable.compilar(entorno);
            Retorno value = this.valor.compilar(entorno);

            if(!base.Verificar_Tipo(value.getObjeto(), asig.getObjeto()))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes");
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes");
            }

            Simbolo simbolo = asig.sym;

            if (simbolo.getGlobal())
            {
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                

                if (asig.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                {
                    string aux = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(value.trueLabel);
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, "1");
                    Master.getInstancia.addGoto(aux);
                    Master.getInstancia.addLabel(value.falseLabel);
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, "0");
                    Master.getInstancia.addLabel(aux);
                    return new Retorno(simbolo.getPosicion(), false, simbolo.getObjeto(), simbolo);
                }else if(asig.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
                {
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    string inicial = Master.getInstancia.newTemporalEntero();
                    Master.getInstancia.addGetStack(inicial, posicion_stack);
                    Copiar_Objeto(asig.getObjeto().symObj, inicial, value.getValor());
                }
                else
                {
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, simbolo.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, value.getValor());
                    return new Retorno(simbolo.getPosicion(), false, simbolo.getObjeto(), simbolo);
                }
            }else if(simbolo.pointer == Simbolo.Pointer.HEAP)
            {
                

                if(asig.getTipo() == TipoDatos.Objeto.TipoObjeto.BOOLEAN)
                {
                    string aux = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(value.trueLabel);
                   /*
                    * asig.getValor() = retorna la posicion del heap donde esta guardado un atributo
                    */
                    Master.getInstancia.addSetHeap(asig.getValor(), "1");
                    Master.getInstancia.addGoto(aux);
                    Master.getInstancia.addLabel(value.falseLabel);
                    
                    Master.getInstancia.addSetHeap(asig.getValor(), "0");
                    Master.getInstancia.addLabel(aux);
                    return new Retorno(asig.getValor(), false, simbolo.getObjeto(), simbolo);
                }else if(asig.getTipo() == TipoDatos.Objeto.TipoObjeto.OBJECTS)
                {
                    //TODO: hacer una copia de los atributos
                }
                else
                {
                    
                    Master.getInstancia.addSetHeap(asig.getValor(), value.getValor());
                    return new Retorno(asig.getValor(), false, simbolo.getObjeto(), simbolo);
                }
            }

            return null;
        }

        public void Copiar_Objeto(SimboloObjeto simboloObjeto, string inicial_asig, string inicial_valor)
        {
            int contador = 0;
            foreach(Atributo atributo in simboloObjeto.GetAtributos())
            {
                switch (atributo.objeto.getTipo())
                {
                    case Objeto.TipoObjeto.INTEGER:
                    case Objeto.TipoObjeto.REAL:
                    case Objeto.TipoObjeto.BOOLEAN:
                        {
                            Copiar_Interger_real_bool(inicial_asig, inicial_valor, contador.ToString());
                            break;
                        }
                    case Objeto.TipoObjeto.STRING:
                        Copiar_cadena(inicial_asig, inicial_valor, contador.ToString());
                        break;
                    case Objeto.TipoObjeto.OBJECTS:
                        Copiar_Objeto(inicial_asig, inicial_valor, contador.ToString(),atributo);
                        break;
                    case Objeto.TipoObjeto.ARRAY:
                        break;
                }
                contador++;
            }
            
        }
        public void Copiar_Interger_real_bool(string inicial_asig, string inicial_valor, string contador)
        {
            //Obtengo la posicion del heap del objeto valor
            string tem_valor = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem_valor, inicial_valor, contador, "+");
            string valor = Master.getInstancia.newTemporal();
            Master.getInstancia.addGetHeap(valor, tem_valor);

            // Obtengo la posicion del heap del objeto asignar y guardo el valor
            string tem_asig = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem_asig, inicial_asig, contador, "+");
            Master.getInstancia.addSetHeap(tem_asig, valor);
        }

        public void Copiar_cadena(string inicial_asig, string inicial_valor, string contador)
        {
            // Me ubico en el inicio de la cadena
            string tem_valor = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem_valor, inicial_valor, contador, "+");

           
            string inicio_cadena = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addUnaria(inicio_cadena, Master.getInstancia.heap_p);

            string true_if1 = Master.getInstancia.newLabel();
            string false_if1 = Master.getInstancia.newLabel();
            string retorno = Master.getInstancia.newLabel();

            string valor = Master.getInstancia.newTemporal();

            string cadena_valor = Master.getInstancia.newTemporalEntero();// posicion de la cadena
            Master.getInstancia.addGetHeap(cadena_valor, tem_valor);


            Master.getInstancia.addLabel(retorno);



            Master.getInstancia.addGetHeap(valor, cadena_valor);
            Master.getInstancia.addif(valor, "-1", "!=", true_if1);
            Master.getInstancia.addGoto(false_if1);

            Master.getInstancia.addLabel(true_if1);
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, valor);
            Master.getInstancia.nextHeap();
            Master.getInstancia.addBinaria(cadena_valor, cadena_valor, "1", "+");
            Master.getInstancia.addGoto(retorno);
            Master.getInstancia.addLabel(false_if1);


            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
            Master.getInstancia.nextHeap();
            string tem_asig = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(tem_asig, inicial_asig, contador, "+");
            Master.getInstancia.addSetHeap(tem_asig, inicio_cadena);

        }
    
        public void Copiar_Objeto(string inicial_asig, string inicial_valor, string contador, Atributo atributo)
        {
            string asig1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(asig1, inicial_asig, contador.ToString(), "+");
            string asig2 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addGetHeap(asig2, asig1);

            string valor1 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addBinaria(valor1, inicial_valor, contador.ToString(), "+");
            string valor2 = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addGetHeap(valor2, valor1);

            Copiar_Objeto(atributo.objeto.symObj, asig2, valor2);
        }
    }
}
