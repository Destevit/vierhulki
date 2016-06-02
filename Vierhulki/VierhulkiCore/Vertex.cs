using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{



    class Węzeł
    {
        public String wartość;
        public Węzeł lewy;
        public Węzeł prawy;
    }
    class Drzewo
    {
        public Węzeł korzeń;


        static Węzeł UtwórzWęzeł(String wartość)
        {
            Węzeł węzeł = new Węzeł();
            węzeł.wartość = wartość;
            return węzeł;
        }
        static void DodajLewy(Węzeł węzeł, Węzeł dziecko)
        {
            węzeł.lewy = dziecko;
        }
        static void DodajPrawy(Węzeł węzeł, Węzeł dziecko)
        {
            węzeł.prawy = dziecko;
        }


    }



    public class Vertex
    {
        public List<Vertex> NextVertexes { get; set; }



    }


}
