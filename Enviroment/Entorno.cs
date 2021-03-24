using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace C3D_Pascal_AirMax.Enviroment
{
    public class Entorno
    {
        private Dictionary<string, Simbolo> tabla;
        private string nombre_entorno;

        public Entorno( string nombre)
        {
            this.tabla = new Dictionary<string, Simbolo>();
            this.nombre_entorno = nombre;
        }

        public string getNombreEntorno()
        {
            return this.nombre_entorno;
        }

        public void addSimbolo(Simbolo sb)
        {
            string llave = sb.getNombre() + sb.getEntorno();
            this.tabla.Add(llave, sb);
        }

        public string Retornar_Simbolos()
        {
            string salida = "";
            foreach(KeyValuePair<string,Simbolo> kvp in this.tabla)
            {
                salida += "<tr>";
                salida += "<td>" + kvp.Value.getNombre() + "</td>\n";
                salida += "<td>" + kvp.Value.getTipo().ToString() + "</td>\n";
                salida += "<td>" + kvp.Value.getEntorno() + "</td>\n";
                salida += "<td>" + kvp.Value.getRol().ToString()+ "</td>\n";
                salida += "<td>" + kvp.Value.getApuntador() + "</td>\n";
                salida += "</tr>";
            }
            return salida;
        }


        public void TablaGeneral()
        {
            string ruta = @"C:\compiladores2";
            StreamWriter fichero = new StreamWriter(ruta + "\\" + "Tabla_general" + ".html");
            fichero.WriteLine("<html>");
            fichero.WriteLine("<head><title>Simbolos</title></head>");
            fichero.WriteLine("<body>");
            fichero.WriteLine("<h2>" + "Simbolos" + "</h2>");
            fichero.WriteLine("<br></br>");
            fichero.WriteLine("<center>" +
            "<table border=3 width=60% height=7%>");
            fichero.WriteLine("<tr>");
            fichero.WriteLine("<th>Nombre</th>");
            fichero.WriteLine("<th>Tipo</th>");
            fichero.WriteLine("<th>Ambito</th>");
            fichero.WriteLine("<th>Rol</th>");
            fichero.WriteLine("<th>Apuntador</th>");
            fichero.WriteLine("</tr>");
            fichero.Write(this.Retornar_Simbolos());
            fichero.Write("</table>");
            fichero.WriteLine("</center>" + "</body>" + "</html>");
            fichero.Close();
        }

    }
}
