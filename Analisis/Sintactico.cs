using System;
using System.Collections.Generic;
using System.Text;
using C3D_Pascal_AirMax.Abstract;
using C3D_Pascal_AirMax.Manejador;
using C3D_Pascal_AirMax.Nativas;
using Irony.Parsing;

namespace C3D_Pascal_AirMax.Analisis
{
    public class Sintactico
    {
        
        public bool Analizar(string texto)
        {
            Gramatica gramatica= new Gramatica();
            LanguageData languageData = new LanguageData(gramatica);
            Parser parser = new Parser(languageData);
            ParseTree parseTree = parser.Parse(texto);
            ParseTreeNode raiz = parseTree.Root;

            if(raiz == null)
            {
                return false;
            }

            GenerarAST(raiz);

            CargarNativas();

            encabezado(raiz.ChildNodes[0]);
            Master.getInstancia.ejecutar();

            return true;

        }

        public void encabezado(ParseTreeNode actual)
        {
            if (actual.ChildNodes.Count == 5)
            {
                // declaracion de variables, objetos, arrays, funciones
                declaraciones(actual.ChildNodes[3]);

                //Instruciones(actual.ChildNodes[4].ChildNodes[1]);

            }
            else if (actual.ChildNodes.Count == 4)
            {
                //Instruciones(actual.ChildNodes[3].ChildNodes[1]);
            }
        }

        public void declaraciones(ParseTreeNode actual)
        {

            foreach (ParseTreeNode node in actual.ChildNodes)
            {

                Master.getInstancia.addInstruccion(declaracion(node.ChildNodes[0]));

            }
        }

        public Nodo declaracion(ParseTreeNode actual)
        {
            String toke = actual.Term.Name;

            switch (toke)
            {
                case "exp":
                    return Expresion.evaluar(actual);

            }
            return null;
        }

        public void CargarNativas()
        {
            //Master.getInstancia.addInstruccion(new Nativa_Concatenar(0,0));
            //Master.getInstancia.addInstruccion(new Nativa_Igual(0, 0));
            //Master.getInstancia.addInstruccion(new Nativa_MayorIgual(0, 0));
            //Master.getInstancia.addInstruccion(new Nativa_Mayor());
        }

        public void GenerarAST(ParseTreeNode raiz)
        {
            Utilidades.Dot.GenerarAST(raiz);
        }
    }
}
