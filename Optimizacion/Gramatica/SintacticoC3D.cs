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

            string salida = Encabezado(raiz.ChildNodes[0]);// obtiene el encabezado
            LinkedList<Nodo> instrucciones =  Instrucciones(raiz.ChildNodes[1]); // obtiene istrucciones
            Interprete interprete = new Interprete(salida, instrucciones);
            interprete.Mirrilla();
            interprete.GenerarCodigo();

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
            salida += "; \n";
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

        public LinkedList<Nodo> Instrucciones(ParseTreeNode entrada)
        {
            LinkedList<Nodo> salida = new LinkedList<Nodo>();

            foreach(ParseTreeNode node in entrada.ChildNodes)
            {
                salida.AddLast(instruccion(node));
            }
            return salida;
        }

        public Nodo instruccion(ParseTreeNode entrada)
        {
            string type = entrada.ChildNodes[0].Term.Name;

            switch (type)
            {
                case "funcion":
                    return getFuncion(entrada.ChildNodes[0]);
                case "asignar":
                    return getAsignacion(entrada.ChildNodes[0]);
                case "condicional":
                    return getCondicion(entrada.ChildNodes[0]);
                case "punto":
                    return getEtiqueta(entrada.ChildNodes[0]);
                case "retorno":
                    return getRetorno(entrada.ChildNodes[0]);
                case "llamada":
                    return getLlamada(entrada.ChildNodes[0]);
                case "printf":
                    return getPrint(entrada.ChildNodes[0]);
                case "fin":
                    return getFin(entrada.ChildNodes[0]);
                case "no_condicional":
                    return getSalto(entrada.ChildNodes[0]);
                case "operacion":
                    return getOperacion(entrada.ChildNodes[0]);

            }
            return null;
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

        public Nodo getPrint(ParseTreeNode entrada)
        {
            int fila = entrada.Span.Location.Line;
            string formato = entrada.ChildNodes[2].ChildNodes[0].Token.Text;
            string tipo = entrada.ChildNodes[5].ChildNodes[0].Token.Text;
            string valor = getTerminal(entrada.ChildNodes[7]);
            return new Printf(fila, formato, tipo, valor);
        }

        public Nodo getOperacion(ParseTreeNode entrada)
        {
            string id = getTerminal(entrada.ChildNodes[0]);
            string left = getAux_terminal(entrada.ChildNodes[2]);
            string right = getAux_terminal(entrada.ChildNodes[4]);
            string simbolo = entrada.ChildNodes[3].Token.Text;
            int fila = entrada.Span.Location.Line;

            return new Operacion(fila, id, left, simbolo, right);
        }

        public Nodo getAsignacion(ParseTreeNode entrada)
        {
            int fila = entrada.Span.Location.Line;

            if(entrada.ChildNodes.Count == 3)
            {
                string direccion = getTerminal(entrada.ChildNodes[0]);
                string valor = getAux_terminal(entrada.ChildNodes[2]);
                return new Asignacion1(fila, direccion, valor);
            }
            else
            {
                string tipo = entrada.ChildNodes[0].Term.Name;
                switch (tipo)
                {
                    case "no_terminal":
                        {
                            string direccion = getNo_terminal(entrada.ChildNodes[0]);
                            string posicion = getAux_terminal(entrada.ChildNodes[2]);
                            string valor = getAux_terminal(entrada.ChildNodes[5]);
                            return new Asignacion2(fila, direccion, posicion, valor);
                        }
                    default:
                        {
                            string direccion = getTerminal(entrada.ChildNodes[0]);
                            string valor = getNo_terminal(entrada.ChildNodes[2]);
                            string posicion = getAux_terminal(entrada.ChildNodes[4]);
                            return new Asignacion3(fila, direccion, valor, posicion);
                        }
                }
            }
        }

        public Nodo getCondicion(ParseTreeNode entrada)
        {
            int fila = entrada.Span.Location.Line;

            string left = getAux_terminal(entrada.ChildNodes[2]);
            string right = getAux_terminal(entrada.ChildNodes[4]);
            string operacion = entrada.ChildNodes[3].Token.Text;

            string label = entrada.ChildNodes[7].Token.Text;

            return new Condicion(fila, left, right, operacion, label);
        }

        public Nodo getFin(ParseTreeNode entrada)
        {
            int fila = entrada.Span.Location.Line;
            return new Fin(fila);
        }

        public string getTerminal(ParseTreeNode entrada)
        {
            string type = entrada.ChildNodes[0].Token.Text;

            return type;
        }

        public string getAux_terminal(ParseTreeNode entrada)
        {
            if(entrada.ChildNodes.Count == 2)
            {
                string salida = "-" + getTerminal(entrada.ChildNodes[1]);
                return salida;
            }
            else
            {
                string salida = getTerminal(entrada.ChildNodes[0]);
                return salida;
            }
        }

        public string getNo_terminal(ParseTreeNode entrada)
        {
            string salida = entrada.ChildNodes[0].Token.Text;
            return salida;
        }

        

    }
}
