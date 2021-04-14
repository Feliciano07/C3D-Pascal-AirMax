using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Instruccion.Variables;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.TipoDatos;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Expresion.Asignaciones
{
    public class AsignacionFun : Nodo
    {
        private string id;
        private LinkedList<Nodo> parametros;

        public AsignacionFun(int linea, int columna, string id, LinkedList<Nodo> nodos):base(linea, columna)
        {
            this.id = id;
            this.parametros = nodos;
        }

        public override Retorno compilar(Entorno entorno)
        {
            /*
             * Realizamos la busquedad de la funcion
             */
            //TODO: verificar si es funcion o proc
            SimboloFuncion simboloFuncion = entorno.searchFuncion(this.id);
            if(simboloFuncion == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La funcion: " + this.id + " no se encontro");
                Master.getInstancia.addError(error);
                throw new Exception("La funcion: " + this.id + " no se encontro");
            }

            
            
            //TODO: tengo que mandar a guardar mis temporales, cuando se hace una llamada

            string temp = Master.getInstancia.newTemporalEntero();
            Master.getInstancia.addComentario("simulacion de cambio de entorno");

            this.getValores(entorno, temp, simboloFuncion);
            /*
             * Cambio del entorno formal
             */
            Master.getInstancia.plusStack((entorno.size + 1).ToString());
            Master.getInstancia.callFuncion(simboloFuncion.id);
            Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, "0", "+");
            string tem2 = Master.getInstancia.newTemporal();// posicion inicio de nueva cadena
            Master.getInstancia.addGetStack(tem2, temp);
            Master.getInstancia.substracStack((entorno.size + 1).ToString());


            return this.Devolver_valor_funcion(tem2, simboloFuncion);
        }

        public void getValores(Entorno entorno, string temp, SimboloFuncion simboloFuncion)
        {
            
            if(this.parametros.Count > 0)
            {
                Parametro[] aux_para = new Parametro[this.parametros.Count];
                simboloFuncion.parametros.CopyTo(aux_para, 0);
                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, (entorno.size +1 ).ToString(), "+");
                int contador = 0;
                foreach (Nodo instruccion in this.parametros)
                {

                    Retorno retorno = instruccion.compilar(entorno);
                    this.Simulacion_Entorno(entorno, retorno, temp, aux_para[contador]);
                    contador++;
                }
            }
            
            
        }

        public void Simulacion_Entorno(Entorno entorno, Retorno aux, string temp, Parametro parametro)
        {
            if(parametro.param == Parametro.Tipo_Parametro.VALOR)
            {
                if(aux.getObjeto().getTipo() == Objeto.TipoObjeto.OBJECTS)
                {
                    Asignacion asignacion = new Asignacion();
                    string inicio_copia = Crear_Objeto(aux.getObjeto().symObj);
                    asignacion.Copiar_Objeto(aux.getObjeto().symObj, inicio_copia, aux.getValor());
                    Master.getInstancia.addBinaria(temp, temp, "1", "+");
                    Master.getInstancia.addSetStack(temp, inicio_copia);

                }
                else if(aux.getObjeto().getTipo() == Objeto.TipoObjeto.ARRAY)
                {
                    Asignacion asignacion = new Asignacion();
                    string inicio_copia = Crear_arreglo(aux.getObjeto().symArray);
                    asignacion.Copiar_Arreglo(aux.getObjeto().symArray, inicio_copia, aux.getValor());

                    Master.getInstancia.addBinaria(temp, temp, "1", "+");
                    Master.getInstancia.addSetStack(temp, inicio_copia);
                }
                else if (aux.getObjeto().getTipo() != Objeto.TipoObjeto.BOOLEAN)
                {
                    Master.getInstancia.addBinaria(temp, temp, "1", "+");
                    Master.getInstancia.addSetStack(temp, aux.getValor());
                }
                else
                {
                    string salida = Master.getInstancia.newLabel();
                    Master.getInstancia.addLabel(aux.trueLabel);
                    Master.getInstancia.addBinaria(temp, temp, "1", "+");
                    Master.getInstancia.addSetStack(temp, "1");
                    Master.getInstancia.addGoto(salida);
                    Master.getInstancia.addLabel(aux.falseLabel);
                    Master.getInstancia.addBinaria(temp, temp, "1", "+");
                    Master.getInstancia.addSetStack(temp, "0");
                    Master.getInstancia.addLabel(salida);
                }
            }
            else
            {
                //guardo 0 para indicar que esta en el stack
               if(aux.sym.pointer == Simbolo.Pointer.STACK)
               {
                    Setear_Stack(aux, temp);
               }
               //guardo 1 para indicar que esta en el heap
               else
               {
                    Setear_Heap(aux, temp);
               }
            }
        }

        public void Setear_Stack(Retorno aux, string temp)
        {
            if (aux.getObjeto().getTipo() == Objeto.TipoObjeto.OBJECTS)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "0");
            }
            else if (aux.getObjeto().getTipo() == Objeto.TipoObjeto.ARRAY)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "0");
            }
            else if (aux.getObjeto().getTipo() != Objeto.TipoObjeto.BOOLEAN)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "0");
            }
            else
            {
                string salida = Master.getInstancia.newLabel();
                Master.getInstancia.addLabel(aux.trueLabel);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "0");
                Master.getInstancia.addGoto(salida);
                Master.getInstancia.addLabel(aux.falseLabel);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "0");
                Master.getInstancia.addLabel(salida);
            }
        }

        public void Setear_Heap(Retorno aux, string temp)
        {
            if (aux.getObjeto().getTipo() == Objeto.TipoObjeto.OBJECTS)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "1");
            }
            else if (aux.getObjeto().getTipo() == Objeto.TipoObjeto.ARRAY)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "1");
            }
            else if (aux.getObjeto().getTipo() != Objeto.TipoObjeto.BOOLEAN)
            {
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "1");
            }
            else
            {
                string salida = Master.getInstancia.newLabel();
                Master.getInstancia.addLabel(aux.trueLabel);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "1");
                Master.getInstancia.addGoto(salida);
                Master.getInstancia.addLabel(aux.falseLabel);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, aux.posicion);
                Master.getInstancia.addBinaria(temp, temp, "1", "+");
                Master.getInstancia.addSetStack(temp, "1");
                Master.getInstancia.addLabel(salida);
            }
        }

        public string Crear_Objeto(SimboloObjeto sym_obj)
        {
            // guarda la posicion inicial del objeto en el heap
            string inicio_objeto = Master.getInstancia.newTemporal();
            Master.getInstancia.addUnaria(inicio_objeto, Master.getInstancia.heap_p);

            //Empiezo a recorrer los atributos para guardar el espacio del nuevo objeto
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
            // Reservo espacio de objetos internos si existen
            Declaracion declaracion = new Declaracion();
            declaracion.Reservar_Espacio_Objeto(sym_obj, inicio_objeto);
            //Reservo el espacio de arreglos si existen
            declaracion.Reservar_Espacio_Arreglo(sym_obj, inicio_objeto);

            return inicio_objeto;
        }

        public string Crear_arreglo(SimboloArreglo simboloArreglo)
        {
            // guarda la posicion inicial del objeto en el heap
            string inicio_arreglo = Master.getInstancia.newTemporal();
            Master.getInstancia.addUnaria(inicio_arreglo, Master.getInstancia.heap_p);

            //Utilizar variable Declaracion
            Declaracion declaracion = new Declaracion();

            switch (simboloArreglo.objeto.getTipo())
            {
                case Objeto.TipoObjeto.INTEGER:
                case Objeto.TipoObjeto.REAL:
                case Objeto.TipoObjeto.BOOLEAN:
                    declaracion.Llenar_Arreglo_Primitivos(simboloArreglo);
                    break;
                case Objeto.TipoObjeto.STRING:
                case Objeto.TipoObjeto.OBJECTS:
                case Objeto.TipoObjeto.ARRAY:
                    declaracion.Llenar_Cadenas(simboloArreglo);
                    break;
            }

            declaracion.Llenar_Arreglo_Objetos(simboloArreglo, inicio_arreglo);

            return inicio_arreglo;

        }

        public Retorno Devolver_valor_funcion(string tem, SimboloFuncion simboloFuncion)
        {
            if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.VOID)
            {
                return null;
            }else if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.BOOLEAN)
            {
                //TODO: retornar un booleano
                return null;
            }
            else
            {
                return new Retorno(tem, true, simboloFuncion.objeto);
            }
        }

    }
}
