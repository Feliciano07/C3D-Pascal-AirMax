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
        private Nodo anterior;


        public AsignacionFun(int linea, int columna, string id, LinkedList<Nodo> nodos):base(linea, columna)
        {
            this.id = id;
            this.parametros = nodos;
        }

        public void setAnterior(Nodo anterior)
        {
            this.anterior = anterior;
        }


        public override Retorno compilar(Entorno entorno)
        {
            /*
             * Realizamos la busquedad de la funcion
             */
            SimboloFuncion simboloFuncion = entorno.searchFuncion(this.id);
            if (simboloFuncion == null)
            {
                Error error = new Error(base.getLinea(), base.getColumna(), Error.Errores.Semantico,
                    "La funcion: " + this.id + " no se encontro");
                Master.getInstancia.addError(error);
                throw new Exception("La funcion: " + this.id + " no se encontro");
            }

            if(string.Compare(simboloFuncion.id.ToLower(), entorno.getNombreEntorno().ToLower()) == 0)
            {
                return this.Recursiva(entorno, simboloFuncion);
            }
            else
            {
                return this.No_Recursiva(entorno, simboloFuncion);
            }

        }


        public Retorno Recursiva(Entorno entorno, SimboloFuncion simboloFuncion)
        {
            if (simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.VOID)
            {
                //TODO: tengo que mandar a guardar mis temporales, para manejar recursividad
                Master.getInstancia.addComentario("Guardo mis temporales");
                Master.getInstancia.saveTemporales(entorno);


                //Simulamos el cambio de entorno
                string temp = Master.getInstancia.newTemporalEntero();

                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, (entorno.size + 1).ToString(), "+");

                //correomos los parametros
                LinkedList<Retorno> salida = this.getRetornos(entorno);

                Master.getInstancia.addComentario("simulacion de cambio de entorno");
                //Setemos los valores de los parametros
                this.getValores(temp, simboloFuncion, salida);

                //Generamos el cambio de entorno formal
                Master.getInstancia.plusStack((entorno.size + 1).ToString());



                Master.getInstancia.callFuncion(simboloFuncion.id);
                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, "0", "+");
                string tem_retorno = Master.getInstancia.newTemporal();

                //retornamos al entorno anterior
                Master.getInstancia.addGetStack(tem_retorno, temp);
                Master.getInstancia.substracStack((entorno.size + 1).ToString());
                Master.getInstancia.addComentario("Recupero mis temporales");
                Master.getInstancia.RecoverTemporales(entorno);

                return this.Devolver_valor_funcion(tem_retorno, simboloFuncion);
            }
            else
            {
                //TODO: tengo que mandar a guardar mis temporales, para manejar recursividad
                Master.getInstancia.addComentario("Guardo mis temporales");
                Master.getInstancia.saveTemporales(entorno);


                //Simulamos el cambio de entorno
                string temp = Master.getInstancia.newTemporalEntero();

                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, (entorno.size + 1).ToString(), "+");


                //correomos los parametros
                LinkedList<Retorno> salida = this.getRetornos(entorno);

                Master.getInstancia.addComentario("simulacion de cambio de entorno");
                //Setemos los valores de los parametros

                //Setemos el valor del retorno, que estara en la posicion 0
                Master.getInstancia.addComentario("creamos el retorno de la funcionn");
                this.Generar_Retorno(simboloFuncion, temp);

                this.getValores(temp, simboloFuncion, salida);

                //Generamos el cambio de entorno formal
                Master.getInstancia.plusStack((entorno.size + 1).ToString());



                Master.getInstancia.callFuncion(simboloFuncion.id);
                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, "0", "+");
                string tem_retorno = Master.getInstancia.newTemporal();

                //retornamos al entorno anterior
                Master.getInstancia.addGetStack(tem_retorno, temp);
                Master.getInstancia.substracStack((entorno.size + 1).ToString());
                Master.getInstancia.addComentario("Recupero mis temporales");
                Master.getInstancia.RecoverTemporales(entorno);

                return this.Devolver_valor_funcion(tem_retorno, simboloFuncion);
            }
        }


        public Retorno No_Recursiva(Entorno entorno, SimboloFuncion simboloFuncion)
        {
            if (simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.VOID)
            {
                //Simulamos el cambio de entorno
                string temp = Master.getInstancia.newTemporalEntero();

                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, (entorno.size + 1).ToString(), "+");

                //correomos los parametros
                LinkedList<Retorno> salida = this.getRetornos(entorno);

                Master.getInstancia.addComentario("simulacion de cambio de entorno");
                //Setemos los valores de los parametros
                this.getValores(temp, simboloFuncion, salida);

                //Generamos el cambio de entorno formal
                Master.getInstancia.plusStack((entorno.size + 1).ToString());



                Master.getInstancia.callFuncion(simboloFuncion.id);
                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, "0", "+");
                string tem_retorno = Master.getInstancia.newTemporal();

                //retornamos al entorno anterior
                Master.getInstancia.addGetStack(tem_retorno, temp);
                Master.getInstancia.substracStack((entorno.size + 1).ToString());
               

                return this.Devolver_valor_funcion(tem_retorno, simboloFuncion);
            }
            else
            {

                //Simulamos el cambio de entorno
                string temp = Master.getInstancia.newTemporalEntero();

                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, (entorno.size + 1).ToString(), "+");


                

                //correomos los parametros
                LinkedList<Retorno> salida = this.getRetornos(entorno);

                Master.getInstancia.addComentario("simulacion de cambio de entorno");

                Master.getInstancia.addComentario("generamos el retorno");
                //Setemos el valor del retorno, que estara en la posicion 0
                this.Generar_Retorno(simboloFuncion, temp);
                //Setemos los valores de los parametros
                this.getValores(temp, simboloFuncion, salida);

                //Generamos el cambio de entorno formal
                Master.getInstancia.plusStack((entorno.size + 1).ToString());



                Master.getInstancia.callFuncion(simboloFuncion.id);
                Master.getInstancia.addBinaria(temp, Master.getInstancia.stack_p, "0", "+");
                string tem_retorno = Master.getInstancia.newTemporal();

                //retornamos al entorno anterior
                Master.getInstancia.addGetStack(tem_retorno, temp);
                Master.getInstancia.substracStack((entorno.size + 1).ToString());


                return this.Devolver_valor_funcion(tem_retorno, simboloFuncion);
            }
        }


        public void Generar_Retorno(SimboloFuncion simboloFuncion, string temp)
        {
            if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.OBJECTS)
            {
                //TODO: probar con esto
                string inicio = Crear_Objeto(simboloFuncion.objeto.symObj);
                Master.getInstancia.addSetStack(temp, inicio);

            }else if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.ARRAY)
            {
                //TODO: probar con esto
                string inicio = Crear_arreglo(simboloFuncion.objeto.symArray);
                Master.getInstancia.addSetStack(temp, inicio);

            }else if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.STRING)
            {
                Master.getInstancia.addSetStack(temp, "-1");
            }
            else
            {
                Master.getInstancia.addSetStack(temp, "0");
            }
        }

        public LinkedList<Retorno> getRetornos(Entorno entorno)
        {
            LinkedList<Retorno> salida = new LinkedList<Retorno>();
            foreach(Nodo instruccion in this.parametros)
            {
                salida.AddLast(instruccion.compilar(entorno));
            }
            return salida;
        }

        public void getValores(string temp, SimboloFuncion simboloFuncion, LinkedList<Retorno> valores)
        {
            
            if(valores.Count > 0)
            {
                Parametro[] aux_para = new Parametro[this.parametros.Count];
                simboloFuncion.parametros.CopyTo(aux_para, 0);
                
                int contador = 0;
                foreach (Retorno retorno in valores)
                {   
                    this.Simulacion_Entorno(retorno, temp, aux_para[contador]);
                    contador++;

                }
            }
            
            
        }

        public void Simulacion_Entorno(Retorno aux, string temp, Parametro parametro)
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
                return new Retorno(tem, true, simboloFuncion.objeto);

            }else if(simboloFuncion.objeto.getTipo() == Objeto.TipoObjeto.BOOLEAN)
            {
                Retorno retorno = new Retorno(tem, true, simboloFuncion.objeto);
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
                return new Retorno(tem, true, simboloFuncion.objeto);
            }
        }

    }
}
