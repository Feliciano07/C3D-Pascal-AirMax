﻿using C3D_Pascal_AirMax.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace C3D_Pascal_AirMax.Manejador
{
    public sealed class Master
    {
        private static readonly Master instancia = new Master();
        private LinkedList<Nodo> instrucciones = new LinkedList<Nodo>();
        private int temporal = 0;
        private int label = 0;
        private LinkedList<string> codigo = new LinkedList<string>();
        private LinkedList<string> storageTemp = new LinkedList<string>();

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

        public void ejecutar()
        {
            foreach(Nodo node in this.instrucciones)
            {
                node.compilar();
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

        
        public string newTemporal()
        {
            string tem = "T" + this.temporal;
            this.temporal++;
            this.storageTemp.AddLast(tem);
            return tem;
        }

        public void addBinaria(string tem, string left, string right, string operador)
        {
            this.codigo.AddLast(tem + " = " + left + " " + operador + " " + right);
        }


    }
}
