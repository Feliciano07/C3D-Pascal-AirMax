using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Optimizacion.Arbol;
using Irony.Parsing;

namespace C3D_Pascal_AirMax.Optimizacion.Gramatica
{
    public class SintacticoC3D
    {
        public bool Analizar(string texto)
        {
            GramaticaC3D gramatica = new GramaticaC3D();
            LanguageData languageData = new LanguageData(gramatica);
            Parser parser = new Parser(languageData);
            ParseTree parseTree = parser.Parse(texto);
            ParseTreeNode raiz = parseTree.Root;
            if(raiz == null)
            {
                return false;
            }

            Encabezado(raiz.ChildNodes[0]);

            return true;
        }

        public string Encabezado(ParseTreeNode entrada)
        {
            string header = Libreria_estructuras();
            header += Temporales(entrada.ChildNodes[3]);
            header += Temporales(entrada.ChildNodes[4]);

            return header;
        }

        public string Libreria_estructuras()
        {
            string salida = "#include <stdio.h>\n";
            salida += "float stack[100000]; \n";
            salida += "float heap[100000]; \n";
            salida += "int SP; \n";
            salida += "int HP; \n";
            return salida;
        }

        public string Temporales(ParseTreeNode entrada)
        {
            string salida = entrada.ChildNodes[0].Token.Text +" ";
            salida += Lista_temporales(entrada.ChildNodes[1]);
            salida += "\n";
            return salida;
        }

        public string Lista_temporales(ParseTreeNode entrada)
        {
            string salida = "";
            int contador = 0;
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                if(contador == 0)
                {
                    salida += node.Token.Text;
                }
                else
                {
                    salida += "," + node.Token.Text;
                }
                contador++;
            }
            return salida;
        }

        public void Instrucciones(ParseTreeNode entrada)
        {
            foreach(ParseTreeNode node in entrada.ChildNodes)
            {

            }
        }

        public void instruccion(ParseTreeNode entrada)
        {
            string type = entrada.Term.Name;

            switch (type)
            {
                case "funcion":
                    break;
                case "asignar":
                    break;
                case "no_condicional":
                    break;
                case "punto":
                    break;
                case "retorno":
                    break;
                case "llamada":
                    break;
                case "printf":
                    break;
                case "fin":
                    break;
            }
        }

        public Nodo getFuncion(ParseTreeNode entrada)
        {
            string id = entrada.ChildNodes[1].Token.Text;
            int fila = entrada.Span.Location.Line;

            return new Funcion(fila, id);
        }

        public Nodo getEtiqueta(ParseTreeNode entrada)
        {
            string label = entrada.ChildNodes[0].Token.Text;
            int fila = entrada.Span.Location.Line;

            return new Etiqueta(fila, label);
        }

        public Nodo getRetorno(ParseTreeNode entrada)
        {
            string retorno = "return";
            int fila = entrada.Span.Location.Line;

            return new Retorno(fila, retorno);
        }

        public Nodo getLlamada(ParseTreeNode entrada)
        {
            string id = entrada.ChildNodes[0].Token.Text;
            int fila = entrada.Span.Location.Line;

            return new Llamada(fila, id);
        }

        public Nodo getSalto(ParseTreeNode entrada)
        {
            string label = entrada.ChildNodes[1].Token.Text;
            int fila = entrada.Span.Location.Line;
            return new Salto(fila, label);
        }

    }
}
