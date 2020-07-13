using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pdb_parser
{
    public partial class Form1 : Form
    {
        private String dosyaYolu;   //secilen .pdb uzantili dosyamizin bulundugu konum.
        private String proteinAdi;  //secilen .pdb uzantili dosyanin ait oldugu protein.
        //atomVeriYapisi -> atom numarasi, atom adi, x kordinatından olusuyor.
        //veriler listesi bu atomlarin koordinatlarinin kiyaslanmasi ve yazdirilmasinda kullanilacak
        private List<atomVeriYapisi> veriler = new List<atomVeriYapisi>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            temizlikci();   //programin guzel gozukmesi icin labellari temizliyor.

            if (dosyaSec()) //dosya sectiysek. (iptal tusu yada exit tusuna basilmadiysa)
            {
                dosyaOku_verileriAl();  //dosyamizi okuyoruz gerekli verileri cekiyoruz.

                //en buyuk x kord. sahip atomu buluyoruz ve atomVeriYapisi cinsinden kaydediyoruz.
                atomVeriYapisi xKordiantiEnBuyukOlan = xKordinatiBuyukOlaniBul();

                proteinAdi2.Text = Convert.ToString(proteinAdi);    //ekrana yazdirma

                //atom sayisi atomla baslayan satir sayisina esittir.
                atomSayisi2.Text = Convert.ToString(veriler.Count); //ekrana yazdirma

                xAtomAdi2.Text = Convert.ToString(xKordiantiEnBuyukOlan.atomAdi);   //ekrana yazdirma

                xAtomNum2.Text = Convert.ToString(xKordiantiEnBuyukOlan.atomNumarasi);  //ekrana yazdirma              
            }
            //dosya seciminde iptal yada exit basildiysa hic birsey yapmaya gerek yok.
        }

        private Boolean dosyaSec()  //.pdb uzantili dosyamizi seciyoruz.
        {
            OpenFileDialog dosyaBulucu = new OpenFileDialog();
            dosyaBulucu.Title = "PDB Dosyası Seçin";    //baslik
            dosyaBulucu.Filter = "Protein Data Bank | *.pdb";   //yanlizca .pdb dosyalari gozukecek.
            DialogResult iptalEdildimi = dosyaBulucu.ShowDialog();  //dialog result iptal kontrolu icin.
            this.dosyaYolu = dosyaBulucu.FileName;  //secilen dosyanin adi(yoluyla beraber).
            if (iptalEdildimi == DialogResult.Cancel)   //button1_click methodunda acikladim
            {
                return false;
            }
            else  //button1_click methodunda acikladim
            {
                return true;
            }
        }

        private void dosyaOku_verileriAl()
        {
            //dosyadaki butun satirlar okunuyor.
            String[] butunSatirlar = System.IO.File.ReadAllLines(dosyaYolu);

            //ilk satir ozel olarak protein ismi icin parcalaniyor.
            String[] ilkSatir = butunSatirlar[0].Split(' ');
            List<String> geciciListe = new List<string>();    //elemanlari koymak icin gecici bir lsite.
            //ilk satirda bulunan boslukla ayrilmis butun elemanlara tek tek bakiyoruz.
            foreach (String eleman in ilkSatir)
            {
                if (eleman.Length != 0)   //elemanin uzunlugu 0 degilse birseyler yaziyor demektir.
                {
                    geciciListe.Add(eleman);    //elemani gecici listemize ekliyoruz.
                }
            }
            //ilk satirda yazan(geciciListe) son sey protein adi.
            proteinAdi = geciciListe[geciciListe.Count - 1];

            //butun satirlari isleme bolumu
            foreach (String satir in butunSatirlar)
            {
                //bir satirdaki butun elemanlar bosluklar ile ayriliyorlar.
                String[] satirdakiButunElemanlar = satir.Split(' ');
                //ilk elemani ATOM olan satirlari istiyoruz.
                if (satirdakiButunElemanlar[0].Equals("ATOM"))
                {
                    /*
                        satirdaki elemanlari ekliyoruz.
                        bu lsitenin olusturulma amaci, yukarida split(' ') yaptigimizda yani bosluk
                        basina ayirdigimizda, yan yan iki bosluk varsa ikisini ayiriyor ancak arada
                        kalan eleman 0 karakter uzunlugunda bir string.
                        satirdaki butun elemanlar icinde bu 0 uzunluktaki stringlerde var.
                        bu stringleri elemek ve kalanlari koymak icin ikinci bir liste olusturuyoruz.
                    */
                    List<String> satirdakiElemanlar = new List<string>();

                    //mevcut satirdaki butun elamanlari tek tek inceliyoruz.
                    foreach (String eleman in satirdakiButunElemanlar)
                    {
                        //0 uzunluklu stringler. yukarida aciklandi.
                        if (eleman.Length != 0)
                        {
                            satirdakiElemanlar.Add(eleman);
                        }
                    }
                    //veriler listesine okudugumuz satiri atom veri tipine cevirerek ekliyoruz.
                    veriler.Add(new atomVeriYapisi(
                            Convert.ToInt16(satirdakiElemanlar[1]), //2. sutun atom numarasi.
                            Convert.ToString(satirdakiElemanlar[2]),    //3. sutun atom adi.
                            Convert.ToDouble(satirdakiElemanlar[6])));  //7. sutun x kordinati.
                }
            }
        }

        private atomVeriYapisi xKordinatiBuyukOlaniBul()
        {
            //kiyaslama yapmak icin en buyuk x koordinatina sahip olacak bir nesne uretiyoruz.
            //kiyaslama yapilabilmesi icin ilk atom nesnesi baslangic degeri olarak ataniyor.
            atomVeriYapisi xEnBuyuk = veriler[0];

            foreach (atomVeriYapisi item in veriler)
            {
                //inceledigimiz atomun x kordinati, x kordinati en buyuk oldugunu dusundugumuz
                //atomdan daha buyuk cikarsa, yeni en buyuk atom suanda inceledigimiz atom olur.
                if (item.xKordinati > xEnBuyuk.xKordinati)
                {
                    xEnBuyuk = item;
                }
            }
            return xEnBuyuk;    //en buyuk x kordinatina sahip atomu donduruyoruz.
        }


        private void temizlikci()
        {
            //programin guzel gozukmesi icin butona tiklandiginda eski verileri gostermiyor.
            proteinAdi2.Text = "";
            atomSayisi2.Text = "";
            xAtomAdi2.Text = "";
            xAtomNum2.Text = "";
        }
    }
}
