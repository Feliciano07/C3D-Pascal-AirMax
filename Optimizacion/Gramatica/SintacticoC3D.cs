using System;
using System.Collections.Generic;
using System.Text;
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

            return true;
        }
    }
}
