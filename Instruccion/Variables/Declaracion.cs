﻿using C3D_Pascal_AirMax.Abstract;
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

        public Declaracion() : base(0, 0)
        {

        }

        //TODO: primero guardar el simbolo y luego genero el codigo
        public override Retorno compilar(Entorno entorno)
        {
            /*
             * se encarga de poder crear una variable que no se haya definido como tipo types
             */
            if(this.tipo.getTipo() != Objeto.TipoObjeto.TYPES)
            {
                if(base.pre_compilar == true)
                {
                    Crear_Simbolo_primtivia(entorno);
                }
                else
                {
                    Crear_Variable_Primitiva(entorno);
                }


            }
            else
            {
                /*
                 * Esta parte se encarga de ir a buscar si existe un type object y que se guarde la variable como ese tipo
                 */
                if(base.pre_compilar == true)
                {
                    if (Crear_Simbolo_Objeto(entorno))
                    {
                        return null;
                    }
                    if (Crear_simbolo_arreglo(entorno))
                    {
                        return null;
                    }
                }
                else
                {
                    if (Crear_Variable_Objeto(entorno))
                    {
                        return null;
                    }

                    if (Crear_Variable_Array(entorno))
                    {
                        return null;
                    }
                }
                


            }
            return null;
        }

        public void Crear_Simbolo_primtivia(Entorno entorno)
        {
            
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
            }
        }

        public bool Crear_Simbolo_Objeto(Entorno entorno)
        {
            SimboloObjeto sym_obj = entorno.searchObjeto(this.tipo.getObjetoId());
            if (sym_obj == null)
            {
                return false;
            }

            
            foreach (string nombre in this.ids)
            {
                
                Objeto tipo = new Objeto(Objeto.TipoObjeto.OBJECTS, sym_obj, sym_obj.id);

                Simbolo newVar = entorno.addSimbolo(nombre, tipo, Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK);
                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + nombre + " ya existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + nombre + " ya existe en el ambito");
                }
            }

            return true;
        }

        public bool Crear_simbolo_arreglo(Entorno entorno)
        {
            SimboloArreglo simboloArreglo = entorno.searchArreglo(this.tipo.getObjetoId());

            if (simboloArreglo == null)
            {
                return false;
            }
           

            foreach (string nombre in this.ids)
            {
                
                Objeto tipo = new Objeto(Objeto.TipoObjeto.ARRAY, simboloArreglo, simboloArreglo.id);
                Simbolo newVar = entorno.addSimbolo(nombre, tipo, Simbolo.Rol.VARIABLE, Simbolo.Pointer.STACK);
                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + nombre + " ya existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + nombre + " ya existe en el ambito");
                }
            }
            return true;
        }



        public void Crear_Variable_Primitiva(Entorno entorno)
        {
            Retorno valor = this.expresion.compilar(entorno);
            if (!base.Verificar_Tipo(valor.getObjeto(), this.tipo))
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
                Master.getInstancia.addError(error);
                throw new Exception("Tipos de datos diferentes: " + valor.getTipo().ToString() + "," + this.tipo.ToString());
            }

            foreach (string str in ids)
            {
                Simbolo newVar = entorno.getSimbolo(str);
                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + str + " no existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + str + " no existe en el ambito");
                }

                Master.getInstancia.addComentario("variable: " + str);
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                

                if (this.tipo.getTipo() == Objeto.TipoObjeto.BOOLEAN)
                {
                    string label_salida = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(valor.trueLabel);
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, "1");
                    Master.getInstancia.addGoto(label_salida);
                    Master.getInstancia.addLabel(valor.falseLabel);
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, "0");
                    Master.getInstancia.addLabel(label_salida);
                }
                else
                {
                    Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                    Master.getInstancia.addSetStack(posicion_stack, valor.getValor());
                }

            }
        }


        public bool Crear_Variable_Objeto(Entorno entorno)
        {
            SimboloObjeto sym_obj = entorno.searchObjeto(this.tipo.getObjetoId());
            if(sym_obj == null)
            {
                return false;
            }

            Master.getInstancia.addComentario("Inicia declarando una variable de type: " + this.tipo.getObjetoId());
            foreach(string nombre in this.ids)
            {
                // guarda la posicion inicial del objeto en el heap
                string inicio_objeto = Master.getInstancia.newTemporal();
                Master.getInstancia.addUnaria(inicio_objeto, Master.getInstancia.heap_p);

                Simbolo newVar = entorno.getSimbolo(nombre);

                if (newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + nombre + " no existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + nombre + " no existe en el ambito");
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

                /*
                 * Verifica si dentro del objeto a declarar existe un atributo 
                 * tipo objeto, de ser asi realiza la reservacion del objeto en el heap
                 */
                Reservar_Espacio_Objeto(sym_obj, inicio_objeto);
                /*
                 * Verifica si dentro del objeto a declarar existe un atributo de tipo array
                 * de ser asi realiza la reservacion de espacio en el heap
                 */
                Reservar_Espacio_Arreglo(sym_obj, inicio_objeto);

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
                            Reservar_Espacio_Arreglo(auxiliar, inicio_objeto);
                        }
                        break;
                }
                contador++;
            }
        }


        public void Reservar_Espacio_Arreglo(SimboloObjeto simboloObjeto, string posicionInicial)
        {
            int contador = 0;

            foreach(Atributo atributo in simboloObjeto.GetAtributos())
            {
                if(atributo.getObjeto().getTipo() == Objeto.TipoObjeto.ARRAY)
                {
                    SimboloArreglo simboloArreglo = atributo.getObjeto().symArray;
                    string inicio_arreglo = Master.getInstancia.newTemporalEntero();
                    Master.getInstancia.addUnaria(inicio_arreglo, Master.getInstancia.heap_p);

                    switch (simboloArreglo.objeto.getTipo())
                    {
                        case Objeto.TipoObjeto.INTEGER:
                        case Objeto.TipoObjeto.REAL:
                        case Objeto.TipoObjeto.BOOLEAN:
                            Llenar_Arreglo_Primitivos(simboloArreglo);
                            break;
                        case Objeto.TipoObjeto.STRING:
                        case Objeto.TipoObjeto.OBJECTS:
                            //TODO: hacer array de array?
                        case Objeto.TipoObjeto.ARRAY:
                            Llenar_Cadenas(simboloArreglo);
                            break;
                    }
                    string posicion_heap = Master.getInstancia.newTemporalEntero();
                    Master.getInstancia.addBinaria(posicion_heap, posicionInicial, contador.ToString(), "+");
                    Master.getInstancia.addSetHeap(posicion_heap, inicio_arreglo);
                    Llenar_Arreglo_Objetos(simboloArreglo, inicio_arreglo);
                }
                contador++;
            }
        }


        public bool Crear_Variable_Array(Entorno entorno)
        {
            SimboloArreglo simboloArreglo = entorno.searchArreglo(this.tipo.getObjetoId());

            if(simboloArreglo == null)
            {
                return false;
            }
            Master.getInstancia.addComentario("Inicia la declaracion de variable tipo arreglo");

            foreach(string nombre in this.ids)
            {
                // guarda la posicion inicial del objeto en el heap
                string inicio_arreglo = Master.getInstancia.newTemporal();
                Master.getInstancia.addUnaria(inicio_arreglo, Master.getInstancia.heap_p);

                Simbolo newVar = entorno.getSimbolo(nombre);

                if(newVar == null)
                {
                    Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                        "La variable: " + nombre + " no existe en el ambito");
                    Master.getInstancia.addError(error);
                    throw new Exception("La variable: " + nombre + " no existe en el ambito");
                }

                switch (simboloArreglo.objeto.getTipo())
                {
                    case Objeto.TipoObjeto.INTEGER:
                    case Objeto.TipoObjeto.REAL:
                    case Objeto.TipoObjeto.BOOLEAN:
                        Llenar_Arreglo_Primitivos(simboloArreglo);
                        break;
                    case Objeto.TipoObjeto.STRING:
                    case Objeto.TipoObjeto.OBJECTS:
                    case Objeto.TipoObjeto.ARRAY:
                        Llenar_Cadenas(simboloArreglo);
                        break;
                }
                string posicion_stack = Master.getInstancia.newTemporalEntero();
                Master.getInstancia.addBinaria(posicion_stack, Master.getInstancia.stack_p, newVar.getPosicion(), "+");
                Master.getInstancia.addSetStack(posicion_stack, inicio_arreglo);

                Llenar_Arreglo_Objetos(simboloArreglo, inicio_arreglo);

            }
            return true;
        }

        public void Llenar_Arreglo_Primitivos(SimboloArreglo simboloArreglo)
        {
            string contador = Master.getInstancia.newTemporalEntero();
            string tope = simboloArreglo.Espacios_Utilizar();
            Master.getInstancia.addUnaria(contador, "0");

            string retorno = Master.getInstancia.newLabel();
            string true_if = Master.getInstancia.newLabel();
            string false_if = Master.getInstancia.newLabel();

            Master.getInstancia.addLabel(retorno);
            Master.getInstancia.addif(contador, tope, "<", true_if);
            Master.getInstancia.addGoto(false_if);

            Master.getInstancia.addLabel(true_if);
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "0");
            Master.getInstancia.nextHeap();
            Master.getInstancia.addBinaria(contador, contador, "1", "+");
            Master.getInstancia.addGoto(retorno);
            Master.getInstancia.addLabel(false_if);

        }

        public void Llenar_Cadenas(SimboloArreglo simboloArreglo)
        {
            string contador = Master.getInstancia.newTemporalEntero();
            string tope = simboloArreglo.Espacios_Utilizar();
            Master.getInstancia.addUnaria(contador, "0");

            string retorno = Master.getInstancia.newLabel();
            string true_if = Master.getInstancia.newLabel();
            string false_if = Master.getInstancia.newLabel();

            Master.getInstancia.addLabel(retorno);
            Master.getInstancia.addif(contador, tope, "<", true_if);
            Master.getInstancia.addGoto(false_if);

            Master.getInstancia.addLabel(true_if);
            Master.getInstancia.addSetHeap(Master.getInstancia.heap_p, "-1");
            Master.getInstancia.nextHeap();
            Master.getInstancia.addBinaria(contador, contador, "1", "+");
            Master.getInstancia.addGoto(retorno);
            Master.getInstancia.addLabel(false_if);
        }


        public bool Llenar_Arreglo_Objetos(SimboloArreglo simboloArreglo, string inicio_arreglo)
        {
            if(simboloArreglo.objeto.getTipo() != Objeto.TipoObjeto.OBJECTS)
            {
                return true;
            }

            string contador = Master.getInstancia.newTemporalEntero();
            string tope = simboloArreglo.Espacios_Utilizar();
            Master.getInstancia.addUnaria(contador, "0");

            string retorno = Master.getInstancia.newLabel();
            string true_if = Master.getInstancia.newLabel();
            string false_if = Master.getInstancia.newLabel();
            string posicion = Master.getInstancia.newTemporalEntero();

            Master.getInstancia.addLabel(retorno);
            Master.getInstancia.addif(contador, tope, "<", true_if);
            Master.getInstancia.addGoto(false_if);

            Master.getInstancia.addLabel(true_if);
            
            Master.getInstancia.addBinaria(posicion, inicio_arreglo, contador, "+");

            SimboloObjeto simboloObjeto = simboloArreglo.objeto.symObj;

            string inicio_objeto = Master.getInstancia.newTemporal();
            Master.getInstancia.addUnaria(inicio_objeto, Master.getInstancia.heap_p);


            foreach (Atributo atributo in simboloObjeto.GetAtributos())
            {
                switch (atributo.getObjeto().getTipo())
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

            Master.getInstancia.addSetHeap(posicion, inicio_objeto);
            Reservar_Espacio_Objeto(simboloObjeto, inicio_objeto);
            Reservar_Espacio_Arreglo(simboloObjeto, inicio_objeto);

            Master.getInstancia.addBinaria(contador, contador, "1", "+");
            Master.getInstancia.addGoto(retorno);

            Master.getInstancia.addLabel(false_if);

            return true;
        }

    }
}
