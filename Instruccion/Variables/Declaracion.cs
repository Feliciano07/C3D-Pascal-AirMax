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
    public class Declaracion : Nodo
    {
        private string[] ids;
        private Nodo expresion;
        private Objeto tipo;


        public Declaracion(int linea, int columna, string[] ids, Nodo exp, Objeto tipo) : base(linea, columna)
        {
            this.ids = ids;
            this.expresion = exp;
            this.tipo = tipo;
        }
        //TODO: falta manejar variables locales
        public override Retorno compilar(Entorno entorno)
        {
            /*
             * se encarga de poder crear una variable que no se haya definido como tipo types
             */
            if(this.tipo.getTipo() != Objeto.TipoObjeto.TYPES)
            {
                Crear_Variable_Primitiva(entorno);
            }

            else
            {
                /*
                 * Esta parte se encarga de ir a buscar si existe un type object y que se guarde la variable como ese tipo
                 */
                Crear_Variable_Objeto(entorno);
            }
            return null;
        }

        public void Crear_Variable_Primitiva(Entorno entorno)
        {
            Retorno valor = this.expresion.compilar(entorno);
            if (!base.Verificar_Tipo(valor.getTipo(), this.tipo.getTipo()))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
            }

            foreach (string str in ids)
            {
                Simbolo newVar = entorno.addSimbolo(str, this.tipo, Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK);
                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + str + " ya existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + str + " ya existe en el ambito");
                }

                Master.getInstancia.addComentario("variable: " + str);
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");

                if (this.tipo.getTipo() == Objeto.TipoObjeto.BOOLEAN)
                {
                    string label_salida = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(valor.trueLabel);
                    Master.getInstancia.addSetStack(posicion_stack, "1");
                    Master.getInstancia.addGoto(label_salida);
                    Master.getInstancia.addLabel(valor.falseLabel);
                    Master.getInstancia.addSetStack(posicion_stack, "0");
                    Master.getInstancia.addLabel(label_salida);
                }
                else
                {
                    Master.getInstancia.addSetStack(posicion_stack, valor.getValor());
                }

            }
        }


        public bool Crear_Variable_Objeto(Entorno entorno)
        {
            SimboloObjeto sym_obj = entorno.searchObjeto(this.tipo.getObjetoId());
            if(sym_obj == null)
            {
                // TODO: retornar un false
                throw new Exception("El objeto: " + this.tipo.getObjetoId() + " No existe");
            }

            Master.getInstancia.addComentario("Inicia declarando una variable de type: " + this.tipo.getObjetoId());
            foreach(string nombre in this.ids)
            {
                // guarda la posicion inicial del objeto en el heap
                string inicio_objeto = Master.getInstancia.newTemporal();
                Master.getInstancia.addUnaria(inicio_objeto, Master.getInstancia.heap_p);
                /*
                 * Guarda un tipo objeto OBJECT
                 * guarda la definicion del simbolo objeto, para poder manipular sus atributos
                 * guarda el nombre de la estructura (cuadro, carro, circulo, etc)
                 */
                Objeto tipo = new Objeto(Objeto.TipoObjeto.OBJECTS, sym_obj, sym_obj.id);

                Simbolo newVar = entorno.addSimbolo(nombre, tipo , Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK);

                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + nombre + " ya existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + nombre + " ya existe en el ambito");
                }

                foreach (Atributo atr in sym_obj.GetAtributos())
                {
                    switch (atr.getObjeto().getTipo())
                    {
                        //se guarda el valor por defecto
                        case Objeto.TipoObjeto.INTEGER:
                        case Objeto.TipoObjeto.REAL:
                        case Objeto.TipoObjeto.BOOLEAN:
                            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "0");
                            break;
                        // se manda -1 debido a que va a guardar una direccion del heap 
                        case Objeto.TipoObjeto.STRING:
                        case Objeto.TipoObjeto.OBJECTS:
                        case Objeto.TipoObjeto.ARRAY:
                            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
                            break;

                    }
                    Master.getInstancia.nextHeap();
                }
                // aca se obtiene en que posicion del stack se va a guardar la variable
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                Master.getInstancia.addSetStack(posicion_stack, inicio_objeto);

                Reservar_Espacio_Objeto(sym_obj, inicio_objeto);

            }
            
            Master.getInstancia.addComentario("Fin de la variable objeto");
            return true;
        }


        public void Reservar_Espacio_Objeto(SimboloObjeto simboloObjeto, string posicionInicial)
        {
            int contador = 0;

            foreach (Atributo atr in simboloObjeto.GetAtributos())
            {
                switch (atr.objeto.getTipo())
                {

                    case Objeto.TipoObjeto.OBJECTS:
                        {
                            SimboloObjeto auxiliar = atr.getObjeto().symObj;
                            string inicio_objeto = Master.getInstancia.newTemporal();
                            Master.getInstancia.addUnaria(inicio_objeto, Master.getInstancia.heap_p);

                            foreach (Atributo interno in auxiliar.GetAtributos())
                            {
                                switch (interno.getObjeto().getTipo())
                                {
                                    //se guarda el valor por defecto
                                    case Objeto.TipoObjeto.INTEGER:
                                    case Objeto.TipoObjeto.REAL:
                                    case Objeto.TipoObjeto.BOOLEAN:
                                        Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "0");
                                        break;
                                    // se manda -1 debido a que va a guardar una direccion del heap 
                                    case Objeto.TipoObjeto.STRING:
                                    case Objeto.TipoObjeto.OBJECTS:
                                    case Objeto.TipoObjeto.ARRAY:
                                        Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
                                        break;

                                }
                                Master.getInstancia.nextHeap();
                            }
                            string posicion_heap = Master.getInstancia.newTemporalEntero();
                            Master.getInstancia.addBinaria(posicion_heap, posicionInicial, contador.ToString(), "+");
                            Master.getInstancia.addSetHeap(posicion_heap, inicio_objeto);
                            Reservar_Espacio_Objeto(auxiliar, inicio_objeto);
                        }
                        break;
                }
                contador++;
            }
        }

    }
}
