using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using C3D_Pascal_AirMax.Optimizacion;
using C3D_Pascal_AirMax.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace C3D_Pascal_AirMax.Manejador
{
    public sealed class Master
    {
        private static readonly Master instancia = new Master();


        // Maneja lo que esta en el body
        private LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
        //  Maneja variables, types
        private LinkedList<Nodo> compilacion = new LinkedList<Nodo>();
        // Contiene funciones y procedimientos
        private LinkedList<Nodo> compilar_funcion = new LinkedList<Nodo>();
        // Maneja funciones nativas
        public LinkedList<Nodo> nativas = new LinkedList<Nodo>();

        public LinkedList<Nodo> types_objetos = new LinkedList<Nodo>();

        public LinkedList<Simbolo> todas_variables = new LinkedList<Simbolo>();


        private LinkedList<Error> lista_errores = new LinkedList<Error>();

        private int temporal = 0;
        private int label = 0;
        private LinkedList<string> codigo = new LinkedList<string>();

        private LinkedList<string> storageTemp = new LinkedList<string>();
        private LinkedList<string> storageTempInt = new LinkedList<string>();

        private LinkedList<string> storageLabel = new LinkedList<string>();

        public string heap = "Heap";
        public string stack = "Stack";
        public string heap_p = "HP";
        public string stack_p = "SP";
        public string prt = "ptr";


        private int puntero_heap = 0;
        private int puntero_stack = 0;

        //Estructura para controlar la optimizacion
        public LinkedList<Optimized> optimizeds = new LinkedList<Optimized>();

        public void clear()
        {
            this.instrucciones.Clear();
            this.compilacion.Clear();
            this.compilar_funcion.Clear();
            this.nativas.Clear();
            this.types_objetos.Clear();
            this.todas_variables.Clear();
            this.lista_errores.Clear();
            this.temporal = 0;
            this.label = 0;
            this.codigo.Clear();
            this.storageTemp.Clear();
            this.storageTempInt.Clear();
            this.storageLabel.Clear();
            this.puntero_heap = 0;
            this.puntero_stack = 0;
            this.optimizeds.Clear();
        }


        public static Master getInstancia
        {
            get
            {
                return instancia;
            }
        }

        public void addSimbolo(Simbolo simbolo)
        {
            this.todas_variables.AddLast(simbolo);
        }

        public void addInstruccion(Nodo nodo)
        {
            this.instrucciones.AddLast(nodo);
        }

        public void addCompilar(Nodo nodo)
        {
            this.compilacion.AddLast(nodo);
        }

        public void compilar_fun(Nodo nodo)
        {
            this.compilar_funcion.AddLast(nodo);
        }

        public void addTypes(Nodo nodo)
        {
            this.types_objetos.AddLast(nodo);
        }

        public void addError(Error error)
        {
            this.lista_errores.AddLast(error);
        }

        public int AumentarStack()
        {
            int aux = this.puntero_stack;
            this.puntero_stack++;
            return aux;
        }
        /*
         * EJECUTA PARA QUE SE CREEN LAS FUNCIONES NATIVA
         */
        public void ejecutar_nativas()
        {
            foreach(Nodo node in this.nativas)
            {
                node.compilar(null);
            }
        }

        /*
         * Ejecuta el flujo basico del programa
         */
        public void ejecutar()
        {
            Entorno entorno = new Entorno("Global");

            /*
             * Ejecuta las instrucciones que son definicion de types objects y types arrays
             */
            foreach(Nodo node in this.types_objetos)
            {
                try
                {
                    node.compilar(entorno);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            /*
             * Ejecuta las sentencias que se deben de precompilar, esto para guardar la tabla de simbolo necesaria
             */
            foreach (Nodo node in this.compilacion)
            {
                try
                {
                    if (node.pre_compilar == true)
                    {
                        node.compilar(entorno);
                        node.pre_compilar = false;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            /*
             * Ejecuta las instrucciones necesarias para crear las funciones
             */
            foreach (Nodo node in this.compilar_funcion)
            {
                try
                {
                    node.compilar(entorno);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            /*
             * Ejecuta el flujo para que se pueda llenar la tabla de simbolos
             */
            this.codigo.AddLast("void main(){");
            foreach (Nodo node in this.compilacion)
            {
                try
                {
                    node.compilar(entorno);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            /*
             * Ejecuta todo lo que este dentro del main
             */
            
            foreach(Nodo node in this.instrucciones)
            {
                try
                {
                    node.compilar(entorno);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            this.codigo.AddLast("return ; }");
            entorno.TablaGeneral();
        }
        public string getEncabezado()
        {
            string salida = "#include <stdio.h>\n";
            salida += "float Heap[100000];\n";
            salida += "float Stack[100000];\n";
            salida += "int SP;\n";
            salida += "int HP;\n";
            
            int contador = 0;
            if(storageTemp.Count > 0)
            {
                salida += "float ";
                foreach (string st in storageTemp)
                {
                    if (contador == 0)
                    {
                        salida += st;
                    }
                    else
                    {
                        salida += "," + st;
                    }
                    contador++;
                }
                
                salida += ";\n";

            }
            if(storageTempInt.Count > 0)
            {
                salida += "int ";
                contador = 0;
                foreach (string st in storageTempInt)
                {
                    if (contador == 0)
                    {
                        salida += st;
                    }
                    else
                    {
                        salida += "," + st;
                    }
                    contador++;
                }
                salida += ";\n";
            }
            return salida;
        }


        public string getSalida()
        {
            string salida = "";
            salida += getEncabezado();

            foreach(string str in this.codigo)
            {
                salida += str + "\n";
            }
            return salida;
        }

        
        // ********************* Generar codigo ********************
        public string newTemporal()
        {
            string tem = "T" + this.temporal;
            this.temporal++;
            this.storageTemp.AddLast(tem);
            return tem;
        }
        public string newTemporalEntero()
        {
            string tem = "T" + this.temporal;
            this.temporal++;
            this.storageTempInt.AddLast(tem);
            return tem;
        }

        public string newLabel()
        {
            string lab = "L" + this.label;
            this.label++;
            this.storageLabel.AddLast(lab);
            return lab;
        }

        public void addBinaria(string tem, string left, string right, string operador)
        {
            this.codigo.AddLast(tem + " = " + left + " " + operador + " " + right+";");
        }

        public void addUnaria(string tem, string rigth)
        {
            this.codigo.AddLast(tem + " = " + rigth + ";");
        }

        public void nextHeap()
        {
            this.codigo.AddLast("HP = HP + 1;");
        }

        public void plusStack(string aumento)
        {
            this.codigo.AddLast(this.stack_p + " = " + this.stack_p + " + " + aumento+";");
        }
        public void substracStack(string disminucion)
        {
            this.codigo.AddLast(this.stack_p + " = " + this.stack_p + " - " + disminucion+";");
        }

        public void addSetHeap(string posicion, string valor)
        {
            this.codigo.AddLast(this.heap + "[" + posicion + "] = "+ valor+";");
        }
        public void addSetStack(string posicion, string valor)
        {
            this.codigo.AddLast(this.stack +  "[" + posicion + "] = " + valor + ";");
        }

        public void addGetHeap(string target, string posicion)
        {
            this.codigo.AddLast(target + " = " + this.heap + "[" + posicion + "];");
        }

        public void addGetStack(string target, string posicion)
        {
            this.codigo.AddLast(target + " = " + this.stack + "[" + posicion + "];");
        }

        public void addif(string left, string right, string operador, string label)
        {
            this.codigo.AddLast("if (" + left + operador + right + ") goto " + label+";");
        }

        public void addGoto(string label)
        {
            this.codigo.AddLast("goto " + label + ";");
        }

        public void addLabel(string label)
        {
            this.codigo.AddLast(label + ":");
        }

        public void Retorno_funcion()
        {
            this.codigo.AddLast("return;");
        }
        public void addFinFuncion()
        {
            this.codigo.AddLast("}");
        }

        public void addFuncion(string nombre)
        {
            this.codigo.AddLast("void " + nombre + "(){");
        }

        public void callFuncion(string nombre)
        {
            this.codigo.AddLast(nombre + "();");
        }

        public void addPrint(string formato, string valor, string conversion)
        {
            this.codigo.AddLast("printf(" + "\"%" + formato + "\"" + "," +"("+conversion+")"+ valor+ ");");
        }

        public void addPrintTrue()
        {
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'T'+");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'R' + ");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'U' + ");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'E' + ");");
        }
        public void addPrintFalse()
        {
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'F' + ");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'A' + ");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'L' + ");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'S' + ");");
            this.codigo.AddLast("printf(\"%c\",(char)" + (int)'E' + ");");
        }

        public void addComentario(string comentario)
        {
            this.codigo.AddLast("/***" + comentario + "*/");
        }

        public int saveTemporales(Entorno entorno)
        {
            string aux = this.newTemporalEntero();
            int contador = 0;

            this.addBinaria(aux, stack_p, (entorno.size + 1).ToString(), "+");

            int total = this.storageTemp.Count + this.storageTempInt.Count;

            LinkedList<string> storage = new LinkedList<string>(); ;

            foreach (string tem in this.storageTemp)
            {
                contador++;
                this.addSetStack(aux, tem);
                storage.AddLast(tem);
                if (contador != total)
                this.addBinaria(aux, aux, "1" , "+");

                
            }

            foreach(string tem in this.storageTempInt)
            {
                contador++;
                this.addSetStack(aux, tem);
                storage.AddLast(tem);
                if (contador != total)
                this.addBinaria(aux, aux, "1" , "+");

            }

            int tamanio_anterior = entorno.size;
            entorno.size = tamanio_anterior + contador;
            Llamada llamada = new Llamada(tamanio_anterior, storage);
            entorno.llamadas.Push(llamada);
            return tamanio_anterior;
        }

        public void RecoverTemporales(Entorno entorno)
        {
            string aux = this.newTemporalEntero();
            Llamada llamada = entorno.llamadas.Pop();

            this.addBinaria(aux, stack_p, (llamada.inicio + 1).ToString(), "+");

            int total = llamada.storageTemp.Count;
            int contador = 0;

            foreach (string tem in llamada.storageTemp)
            {
                contador++;
                this.addGetStack(tem, aux);
                if(contador != total)
                this.addBinaria(aux, aux, "1", "+");
            }
            entorno.size = llamada.inicio;
        }



        public void ReccorerErrores()
        {
            string ruta = @"C:\compiladores2";
            StreamWriter fichero = new StreamWriter(ruta + "\\" + "reporte_errores" + ".html");
            fichero.WriteLine("<html>");
            fichero.WriteLine("<head><title>Errores</title></head>");
            fichero.WriteLine("<body>");
            fichero.WriteLine("<h2>" + "Errores" + "</h2>");
            fichero.WriteLine("<br></br>");
            fichero.WriteLine("<center>" +
            "<table border=3 width=60% height=7%>");
            fichero.WriteLine("<tr>");
            fichero.WriteLine("<th>Tipo</th>");
            fichero.WriteLine("<th>Descripcion</th>");
            fichero.WriteLine("<th>Linea</th>");
            fichero.WriteLine("<th>Columna</th>");
            fichero.WriteLine("</tr>");
            fichero.WriteLine(Errores_encontrados());
            fichero.Write("</table>");
            fichero.WriteLine("</center>" + "</body>" + "</html>");
            fichero.Close();

        }
        public string Errores_encontrados()
        {
            string salida = "";
            foreach (Error error in this.lista_errores)
            {
                salida += "<tr>";
                salida += "<td>" + error.tipoError + "</td>\n";
                salida += "<td>" + error.descripcion + "</td>\n";
                salida += "<td>" + error.linea + "</td>\n";
                salida += "<td>" + error.columna + "</td>\n";
                salida += "</tr>";
            }
            return salida;
        }


        /*
         * Parte de optimizacion de codigo 3 direcciones
         */

        public void addOptimizedCode(string codigo)
        {
            this.codigo.AddLast(codigo);
        }

        public void addOptimized(int fila, string original, string nueva, Optimized.Regla regla)
        {
            Optimized optimized = new Optimized(fila, original, nueva, regla);
            this.optimizeds.AddLast(optimized);
        }


        public void ReporteOptimizacion()
        {
            string ruta = @"C:\compiladores2";
            StreamWriter fichero = new StreamWriter(ruta + "\\" + "reporte_optimizacion" + ".html");
            fichero.WriteLine("<html>");
            fichero.WriteLine("<head><title>Optimizacion</title></head>");
            fichero.WriteLine("<body>");
            fichero.WriteLine("<h2>" + "Mirilla" + "</h2>");
            fichero.WriteLine("<br></br>");
            fichero.WriteLine("<center>" +
            "<table border=3 width=60% height=7%>");
            fichero.WriteLine("<tr>");
            fichero.WriteLine("<th>Fila</th>");
            fichero.WriteLine("<th>Codigo Eliminado</th>");
            fichero.WriteLine("<th>Codigo Agregado</th>");
            fichero.WriteLine("<th>Regla</th>");
            fichero.WriteLine("</tr>");
            fichero.WriteLine(ListaOptimizar());
            fichero.Write("</table>");
            fichero.WriteLine("</center>" + "</body>" + "</html>");
            fichero.Close();
        }

        public string ListaOptimizar()
        {
            string salida = "";

            foreach(Optimized optimized in this.optimizeds)
            {
                salida += "<tr>";
                salida += "<td>" + optimized.fila + "</td>\n";
                salida += "<td>" + optimized.original + "</td>\n";
                salida += "<td>" + optimized.nueva + "</td>\n";
                salida += "<td>" + optimized.regla + "</td>\n";
                salida += "</tr>";
            }

            return salida;
        }
    }
}
