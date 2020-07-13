using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdb_parser
{
    /* .pdb uzantili dosyadaki atomlarin;
     * kiyaslanabilmesi,
     * depolanabilmesi,
     * yazdirilabilmesi için bu class olusturuldu.
     */
    class atomVeriYapisi
    {
        //kolay erisim icin public olarak etiketledik
        public int atomNumarasi;
        public String atomAdi;
        public double xKordinati;

        public atomVeriYapisi(int atomNumarasi, String atomAdi, double xKordinati)
        {
            this.atomNumarasi = atomNumarasi;
            this.atomAdi = atomAdi;
            this.xKordinati = xKordinati;
        }
    }
}
