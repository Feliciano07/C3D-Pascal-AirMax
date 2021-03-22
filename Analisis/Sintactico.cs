using System;
using System.Collections.Generic;
using System.Text;
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

            return true;

        }

        public void GenerarAST(ParseTreeNode raiz)
        {
            Utilidades.Dot.GenerarAST(raiz);
        }
    }
}
