using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace C3D_Pascal_AirMax.Utilidades
{
    public class Dot
    {
        private static int Contador;
        private static String grafo;


        private static string path = @"C:\compiladores2\ast.txt";

        public static void GenerarAST(ParseTreeNode raiz)
        {
            String grafodot = getDot(raiz);

            SaveFile(grafodot, path);

            string imagen = path.Replace(".txt", ".jpg");

            try
            {
                var command = string.Format("dot -Tjpg {0} -o {1}", path, imagen);

                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);

                var proc = new System.Diagnostics.Process();

                proc.StartInfo = procStartInfo;

                proc.Start();

                proc.WaitForExit();

            }
            catch (Exception e)
            {

            }

        }

        public static void SaveFile(String cadena, string direccion)
        {
            TextWriter tw = new StreamWriter(direccion);
            tw.WriteLine(cadena);
            tw.Close();

        }



        public static String getDot(ParseTreeNode raiz)
        {
            grafo = "";
            grafo = "digraph  G {";
            grafo += "nodo0[label=\"" + raiz.ToString() + "\" ]; \n";
            Contador = 1;
            recorrerAST("nodo0", raiz);
            grafo += "}";
            return grafo;
        }

        public static void recorrerAST(String padre, ParseTreeNode hijos)
        {

            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                String nombreHijo = "nodo" + Contador.ToString();
                grafo += nombreHijo + "[label=\"" + Escapar(hijo.ToString()) + "\"];\n";
                grafo += padre + "->" + nombreHijo + ";\n";
                Contador++;
                recorrerAST(nombreHijo, hijo);

            }

        }
        public static String Escapar(String cade)
        {
            cade = cade.Replace("\\", "\\\\");
            cade = cade.Replace("\"", "\\\"");
            return cade;
        }

    }

}
