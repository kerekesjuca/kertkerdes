using System.Collections.Generic;
using System.Linq;

namespace KertKerdes.Helpers
{
    public static class CimkeModeraloHelper
    {
        private static readonly List<string> AlapTiltottSzavak = new List<string>
        {
            "hülye","idiota","barom","szar","szarházi","kurva","geci","picsa","fasz","faszfej",
            "basz","baszd","bazd","bazmeg","kibasz","kicsesz","szop","szopat","nyomorék",
            "retardált","segg","seggfej","seggnyaló","balfasz","anyáz","anyád","anyádat","apád",
            "rohad","rohadék","dög","dögölj","pusztulj","szemét","mocskos","kretén","gyökér",
            "hülyegyerek","idióta","baromarc","féreg","patkány","csicska","csíra","szánalmas",
            "szutyok","hányadék","okádék","fos","fosadék","szarfészek","kurvára","kurvás",
            "ribanc","lotyó","ringyó","cafka","kurafi","bunkó","tapló","paraszt","suttyó"
        };

        private static readonly List<string> Ragok = new List<string>
        {
            "", "k", "ok", "ek", "ak", "ban", "ben", "ra", "re", "hoz", "hez", "höz",
            "val", "vel", "ért", "ig", "ként", "t", "at", "et", "ot", "öt", "ul", "ül",
            "ni", "nál", "nél", "ról", "ről"
        };

        private static readonly List<string> TiltottSzavak = GeneralTiltottLista();

        private static List<string> GeneralTiltottLista()
        {
            var lista = new HashSet<string>();

            foreach (var szo in AlapTiltottSzavak)
            {
                foreach (var rag in Ragok)
                {
                    lista.Add(szo + rag);
                }

                lista.Add(szo.Replace("a", "á"));
                lista.Add(szo.Replace("e", "é"));
                lista.Add(szo.Replace("i", "í"));
                lista.Add(szo.Replace("o", "ó"));
                lista.Add(szo.Replace("u", "ú"));
            }

            return lista.ToList();
        }

        public static bool TiltottCimke(string szoveg)
        {
            if (string.IsNullOrWhiteSpace(szoveg))
                return false;

            var kisbetus = szoveg.Trim().ToLower();

            return TiltottSzavak.Any(t => kisbetus.Contains(t));
        }
    }
}