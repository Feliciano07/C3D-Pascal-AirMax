﻿using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Enviroment;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Manejador
{
    public sealed class Master
    {
        private static readonly Master instancia = new Master();
        private LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
        private LinkedList<Error> lista_errores = new LinkedList<Error>();

        private int temporal = 0;
        private int label = 0;
        private LinkedList<string> codigo = new LinkedList<string>();
        private LinkedList<string> storageTemp = new LinkedList<string>();
        private LinkedList<string> storageLabel = new LinkedList<string>();

        public string heap = "Heap";
        public string stack = "Stack";
        public string heap_p = "HP";
        public string stack_p = "SP";


        private int puntero_heap = 0;
        private int puntero_stack = 0;

        public static Master getInstancia
        {
            get
            {
                return instancia;
            }
        }

        public void addInstruccion(Nodo nodo)
        {
            this.instrucciones.AddLast(nodo);
        }
        public void addError(Error error)
        {
            this.lista_errores.AddLast(error);
        }

        public void ejecutar()
        {
            Entorno entorno = new Entorno();

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
        }

        public string getSalida()
        {
            string salida = "";
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

        public void addSetHeap(string posicion, string valor)
        {
            this.codigo.AddLast(this.heap + "[" + posicion + "] = "+ valor+";");
        }

        public void addGetHeap(string target, string posicion)
        {
            this.codigo.AddLast(target + " = " + this.heap_p + "[" + posicion + "];");
        }

        public void addif(string left, string right, string operador, string label)
        {
            this.codigo.AddLast("if " + left + operador + right + " goto " + label+";");
        }

        public void addGoto(string label)
        {
            this.codigo.AddLast("goto " + label + ";");
        }

        public void addLabel(string label)
        {
            this.codigo.AddLast(label + ":");
        }


    }
}
