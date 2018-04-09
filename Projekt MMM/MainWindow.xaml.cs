using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Projekt_MMM
{
    public partial class MainWindow : Window
    {
        public double param_a;
        public double param_A;
        public double param_T;
        public string param_pobudzenie;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Wykonaj(object sender, RoutedEventArgs e)
        {
            if (Sprawdz_Pola())
            {
                Canvas_Y.Children.Clear();
                Canvas_E.Children.Clear();
                
                param_a = double.Parse(TBox_parametr_a.Text);
                param_A = double.Parse(TBox_parametr_A.Text);
                param_T = double.Parse(TBox_parametr_T.Text);
                param_pobudzenie = TBox_parametr_pobudzenie.Text;

                TBlock_parametr_a.Text = TBox_parametr_a.Text;
                TBlock_parametr_A.Text = TBox_parametr_A.Text;
                TBlock_parametr_T.Text = TBox_parametr_T.Text;
                TBlock_parametr_pobudzenie.Text = TBox_parametr_pobudzenie.Text;

                dolny_pasek.Visibility = Visibility.Visible;

                Operacje();
            }
        }

        private bool Sprawdz_Pola()
        {
            if (TBox_parametr_a.Text == "" ||
                 TBox_parametr_A.Text == "" ||
                 TBox_parametr_T.Text == "" ||
                TBox_parametr_pobudzenie.Text == "")
                return false;
            else if (double.Parse(TBox_parametr_a.Text) <= 0 ||
                    double.Parse(TBox_parametr_A.Text) <= 0 ||
                    double.Parse(TBox_parametr_T.Text) <= 0)
                return false;
            else
                return true;
        }

        private void Operacje()
        {
            double akt_uchyb = 0;
            double akt_wyjście = 0;
            double akt_wejscie = 0;
            double akt_sygnal_przejsciowy_u1 = 0;
            double stan_przejsciowy_x1 = 0;
            double akt_czas = 0;
            double czas_pracy_ukladu = 20 * (1 / Calka.krok);

            List<double> Uchyb = new List<double>();
            List<double> Wyjscie = new List<double>();
            List<double> Wejscie = new List<double>();

            Calka calka_pierwsza = new Calka();
            Calka calka_druga = new Calka();

            for (akt_czas = 0; akt_czas <= czas_pracy_ukladu; akt_czas++)
            {
                akt_wejscie = Oblicz_wejscie(akt_czas,Wejscie);                                                                         //deklarujemy wartość wejścia w danym momencie t
                akt_uchyb = akt_wejscie - akt_wyjście;                                                                                  //obliczamy uchyb
                akt_sygnal_przejsciowy_u1 = Nieliniowosc(akt_uchyb);                                                                    //uchyb przepuszczamy przez człon nieliniowy
                akt_wyjście = Oblicz_wyjscie(calka_pierwsza, calka_druga, akt_sygnal_przejsciowy_u1, ref stan_przejsciowy_x1);

                Uchyb.Add(akt_uchyb);
                Wyjscie.Add(akt_wyjście);
            }

            Label[] wspolrzedne_y_wyjscie = 
                {
                     wsp_wyjscia_y11, wsp_wyjscia_y10, wsp_wyjscia_y9, wsp_wyjscia_y8, wsp_wyjscia_y7, wsp_wyjscia_y6, wsp_wyjscia_y5, wsp_wyjscia_y4, wsp_wyjscia_y3,
                     wsp_wyjscia_y2, wsp_wyjscia_y1, wsp_wyjscia_y0, wsp_wyjscia_ym1, wsp_wyjscia_ym2, wsp_wyjscia_ym3, wsp_wyjscia_ym4, wsp_wyjscia_ym5, wsp_wyjscia_ym6,
                     wsp_wyjscia_ym7, wsp_wyjscia_ym8, wsp_wyjscia_ym9, wsp_wyjscia_ym10, wsp_wyjscia_ym11
                };

            Label[] wspolrzedne_x_wyjscie =
               {
                     wsp_wyjscia_x1, wsp_wyjscia_x2, wsp_wyjscia_x3, wsp_wyjscia_x4, wsp_wyjscia_x5, wsp_wyjscia_x6, wsp_wyjscia_x7, wsp_wyjscia_x8, wsp_wyjscia_x9,
                     wsp_wyjscia_x10, wsp_wyjscia_x11, wsp_wyjscia_x12, wsp_wyjscia_x13, wsp_wyjscia_x14, wsp_wyjscia_x15, wsp_wyjscia_x16, wsp_wyjscia_x17, wsp_wyjscia_x18,
                     wsp_wyjscia_x19, wsp_wyjscia_x20
                };

            Label[] wspolrzedne_y_uchyb =
                {
                     wsp_uchybu_y11, wsp_uchybu_y10, wsp_uchybu_y9, wsp_uchybu_y8, wsp_uchybu_y7, wsp_uchybu_y6, wsp_uchybu_y5, wsp_uchybu_y4, wsp_uchybu_y3,
                     wsp_uchybu_y2, wsp_uchybu_y1, wsp_uchybu_y0, wsp_uchybu_ym1, wsp_uchybu_ym2, wsp_uchybu_ym3, wsp_uchybu_ym4, wsp_uchybu_ym5, wsp_uchybu_ym6,
                     wsp_uchybu_ym7, wsp_uchybu_ym8, wsp_uchybu_ym9, wsp_uchybu_ym10, wsp_uchybu_ym11
                };

            Label[] wspolrzedne_x_uchyb =
               {
                     wsp_uchybu_x1, wsp_uchybu_x2, wsp_uchybu_x3, wsp_uchybu_x4, wsp_uchybu_x5, wsp_uchybu_x6, wsp_uchybu_x7, wsp_uchybu_x8, wsp_uchybu_x9,
                     wsp_uchybu_x10, wsp_uchybu_x11, wsp_uchybu_x12, wsp_uchybu_x13, wsp_uchybu_x14, wsp_uchybu_x15, wsp_uchybu_x16, wsp_uchybu_x17, wsp_uchybu_x18,
                     wsp_uchybu_x19, wsp_uchybu_x20
                };

            Rysuj_wykres(Wyjscie, Brushes.Red, Canvas_Y, wspolrzedne_x_wyjscie, wspolrzedne_y_wyjscie, czas_pracy_ukladu * Calka.krok);
            if(RB_uchyb.IsChecked == true) Rysuj_wykres(Uchyb, Brushes.Blue, Canvas_E, wspolrzedne_x_uchyb, wspolrzedne_y_uchyb, czas_pracy_ukladu * Calka.krok);
            else Rysuj_wykres(Wejscie, Brushes.Blue, Canvas_E, wspolrzedne_x_uchyb, wspolrzedne_y_uchyb, czas_pracy_ukladu * Calka.krok);

        }

        private void Rysuj_wykres(List<double> Dane, SolidColorBrush kolor, Canvas canvas, Label[] wsp_x, Label[] wsp_y, double czas_pracy)
        {
            int liczba_elementow = Dane.Count;

            double x_start = linia_wyznacznik.X1;
            double y_start = linia_wyznacznik.Y1;

            double x_zakres = linia_zakres_gorny_x.X2 - linia_wyznacznik.X1;
            double y_zakres = linia_wyznacznik.Y1 - linia_zakres_gorny_y.Y1;

            double X1 = x_start;
            double Y1 = y_start;
            double X2, Y2;

            double skala = Math.Abs(Dane.Max()) >= Math.Abs(Dane.Min()) ? Math.Abs(Dane.Max()) : Math.Abs(Dane.Min());

            for (int i=0; i < liczba_elementow; i++)
            {
                X2 = x_start + (i+1) * x_zakres / liczba_elementow;
                Y2 = y_start - Dane[i] * y_zakres / skala;

                Line linia = new Line();

                linia.X1 = X1;
                linia.X2 = X2;
                linia.Y1 = Y1;
                linia.Y2 = Y2;

                linia.Stroke = kolor;
                linia.StrokeThickness = 0.5;
                
                canvas.Children.Add(linia);

                X1 = X2;
                Y1 = Y2;
            }

            double jednostka_y = skala / 11;
            double jednostka_x = czas_pracy / 20;

            for (int i = 0; i < wsp_y.Length; i++) wsp_y[i].Content = ((11 - i) * jednostka_y).ToString("F");
            
            for (int i = 0; i < wsp_x.Length; i++) wsp_x[i].Content = (jednostka_x * (i+1)).ToString("F");
            }

        private double Oblicz_wyjscie(Calka calka_pierwsza, Calka calka_druga, double wejscie, ref double stan_przejsciowy_x1)
        {
            double stan_przejsciowy_x2 = wejscie + (-1 / param_T) * stan_przejsciowy_x1;
            calka_pierwsza.probka_terazniejsza = stan_przejsciowy_x2;
            calka_pierwsza.wartosc += Calkowanie(calka_pierwsza.probka_poprzednia, calka_pierwsza.probka_terazniejsza);
            stan_przejsciowy_x1 = calka_pierwsza.wartosc;

            calka_druga.probka_terazniejsza = stan_przejsciowy_x1;                                                                              
            calka_druga.wartosc += Calkowanie(calka_druga.probka_poprzednia, calka_druga.probka_terazniejsza);
            double stan_przejsciowy_x0 = calka_druga.wartosc;
            
            calka_pierwsza.probka_poprzednia = calka_pierwsza.probka_terazniejsza;
            calka_druga.probka_poprzednia = calka_druga.probka_terazniejsza;

            return stan_przejsciowy_x0 * (1 / param_T);                                   
        }                                                                                                  

        private double Calkowanie(double wartosc_poprzednia, double wartosc_nastepna)
        {
            return (wartosc_poprzednia + wartosc_nastepna) * 0.5f * Calka.krok;
        }

        private double Nieliniowosc(double uchyb)
        {
            if (uchyb >= param_a) return param_A;
            else if (uchyb <= -param_a) return -param_A;
            else return param_A / param_a * uchyb;
        }

        private double Oblicz_wejscie(double czas, List<double> Wejscie)
        {
            double amplituda_ost = 0;
            double czas_rzeczywisty = czas * Calka.krok;
            switch (param_pobudzenie)
            {
                case "Fala prostokątna":
                    {
                        double okres = 8;
                        double amplituda = 1;
                        if ((int)(czas_rzeczywisty / (okres / 2)) % 2 == 0) amplituda_ost = amplituda;
                        else amplituda_ost = -amplituda;
                    }
                    break;
                case "Fala trójkątna":
                    {
                        double okres = 8;
                        double amplituda = 1;
                        double nachylenie_wykresu = amplituda / (okres / 4);

                        int zmienna_do_obliczen = (int)(czas_rzeczywisty / (okres / 4));

                        double do_dwoch = (czas_rzeczywisty - (zmienna_do_obliczen * (okres / 4)));
                        double argument = (zmienna_do_obliczen > 0) ? do_dwoch : czas_rzeczywisty;
                        
                        if (zmienna_do_obliczen % 4 == 0)
                            amplituda_ost = nachylenie_wykresu * argument;
                        else if (zmienna_do_obliczen % 4 == 1)
                            amplituda_ost = 1 - nachylenie_wykresu * argument;
                        else if (zmienna_do_obliczen % 4 == 2)
                            amplituda_ost = 0 - nachylenie_wykresu * argument;
                        else if (zmienna_do_obliczen % 4 == 3)
                            amplituda_ost = nachylenie_wykresu * argument - 1;
                    }
                    break;
                case "Sinusoida":
                    {
                        double okres = 8;
                        amplituda_ost = Math.Sin((2 * Math.PI * czas_rzeczywisty / okres));
                    }
                    break;
                case "Skok jednostkowy":
                    {
                        amplituda_ost = 1;
                    }
                    break;
            }
            Wejscie.Add(amplituda_ost);
            return amplituda_ost;
        }
        }
}
