using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;


using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace JsonCommunicationNamespace
{
    class SingleLineJson
    {
        public string na { get; set; }
        public int il { get; set; }
        public string vtp { get; set; }
        public int pr { get; set; }
    }
    class SinglePaymentDataJson
    {
        public int ty { get; set; }
        public int wa { get; set; }
        public string na { get; set; }
        public bool re { get; set; }
    }
    class JsonReceivedCommunicate
    {
        public IList<SingleLineJson> lines { get; set; }
        public Dictionary<string, int> summary { get; set; }
        //public IList<SingleLineJson>? extralines { get; set; }
        public IList<SinglePaymentDataJson> payments { get; set; }
        public void WriteJsonCommunicate()
        {
            if (lines != null)
            {
                Console.WriteLine("Lines =");
                foreach (SingleLineJson s in lines)
                {
                    Console.WriteLine($"na = {s.na}     il = {s.il}     vtp = {s.vtp}     pr = {s.pr}");
                }
            }
            if (summary != null)
                Console.WriteLine($"Summary = {summary.Keys.First()} : {summary.Values.First()}");
            if (payments != null)
            {
                Console.WriteLine("Payments =");
                foreach (SinglePaymentDataJson p in payments)
                {
                    Console.WriteLine($"ty = {p.ty}     wa = {p.wa}     na = {p.na}     re = {p.re}");
                }
            }
        }
    }
    class JsonSendCommunicate
    {
        public bool ok { get; set; }
        public int code { get; set; }
        public string bn { get; set; }
        public string hn { get; set; }
        public int took { get; set; }
        public string message { get; set; }
        public long ts { get; set; }
        public long tsend { get; set; }
    }
}

namespace PosnetServerWinFormsApp
{
    using JsonCommunicationNamespace;
    using DFPrnNamespace;
    class HttpServer
    {
        public static HttpListener listener;
        //public static string url = "http://localhost:3050/paragon/";
        public static string url = "http://localhost:3050/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData = "";

        public static async Task HandleIncomingConnections()
        {
            Dictionary<int, string> errorDictionary = new Dictionary<int, string>();
            errorDictionary.Add(0, "Brak Bladu");
            errorDictionary.Add(1, "Brak pamieci");
            errorDictionary.Add(2, "Za krotka ramka");
            errorDictionary.Add(3, "Blad ramki odebranej z kasy");
            errorDictionary.Add(5, "Nie mozna otworzyc wskazanego urzadzenia");
            errorDictionary.Add(6, "Blad CRC w odebranej ramce");
            errorDictionary.Add(7, "Blad utworzenia obiektu IPC (Event)");
            errorDictionary.Add(8, "Blad komunikacji");
            errorDictionary.Add(9, "Blad krytyczny USB - urzadzenie nie bedzie funkcjonowa? poprawnie");
            errorDictionary.Add(10, "Nieudany import sterownika FTDI");
            errorDictionary.Add(11, "Blad ustawienia parametrow otwieranego portu");
            errorDictionary.Add(12, "Nie mozna otworzyc wskazanego urzadzenia - port zajety");
            errorDictionary.Add(13, "Nie mozna otworzyc wskazanego urzadzenia - port nie znaleziony");
            errorDictionary.Add(14, "Bladne parametry portu - baudrate");
            errorDictionary.Add(15, "Bladne parametry portu - databits");
            errorDictionary.Add(16, "Bladne parametry portu - parity");
            errorDictionary.Add(17, " Bladne parametry portu - stop bits");
            errorDictionary.Add(18, "Bladne parametry portu - handshake");
            errorDictionary.Add(32, "urzadzenie zajete");
            errorDictionary.Add(33, "urzadzenie zajete - podniesiona d�wignia");
            errorDictionary.Add(34, "urzadzenie zajete - Blad mechanizmu");
            errorDictionary.Add(35, "urzadzenie zajete - podniesiona pokrywa");
            errorDictionary.Add(36, "urzadzenie zajete - brak papieru");
            errorDictionary.Add(37, "urzadzenie zajete - zbyt wysoka temperatura");
            errorDictionary.Add(38, "urzadzenie zajete - chwilowy zanik zasilania");
            errorDictionary.Add(39, "urzadzenie zajete - Blad obcinacza");
            errorDictionary.Add(48, "Blad sieciowy - przerwane wywolanie systemowe");
            errorDictionary.Add(49, "Blad sieciowy - brak dostepu");
            errorDictionary.Add(50, "Blad sieciowy - operacja w toku");
            errorDictionary.Add(51, "Blad sieciowy - wymagany adres docelowy");
            errorDictionary.Add(52, "Blad sieciowy - adres w uzyciu");
            errorDictionary.Add(53, "Blad sieciowy - adres nieprawidlowy");
            errorDictionary.Add(54, "Blad sieciowy - siec jest wylaczona");
            errorDictionary.Add(55, "Blad sieciowy - siec jest nieosiegalna");
            errorDictionary.Add(56, "Blad sieciowy - siec roz��czy�a polaczenie");
            errorDictionary.Add(57, "Blad sieciowy - polaczenie zerwane przez aplikacj�");
            errorDictionary.Add(58, "Blad sieciowy - strona zdalna zerwa�a polaczenie");
            errorDictionary.Add(59, "Blad sieciowy - uplynal czas oczekiwania na odpowiedz");
            errorDictionary.Add(60, "Blad sieciowy - polaczenie odrzucone");
            errorDictionary.Add(61, "Blad sieciowy - serwer zdalny jest wy��czony");
            errorDictionary.Add(62, "Blad sieciowy - serwer zdalny jest nieosiagalny");
            errorDictionary.Add(63, "Blad sieciowy - serwer nieznaleziony");
            errorDictionary.Add(64, "Blad sieciowy - serwer nieznaleziony  sprobuj ponownie");
            errorDictionary.Add(112, "Blad odczytu pliku bitmapy do zaprogramowania");
            errorDictionary.Add(113, "Blad rozmiaru bitmapy");
            errorDictionary.Add(65536, "Rozkaz juz wykonany");
            errorDictionary.Add(65537, "Brak danych w kolejce");
            errorDictionary.Add(65538, "Bladna wartosc");
            errorDictionary.Add(65539, "Oczekiwanie zakonczone up�ynizciem czasu (timeout)");
            errorDictionary.Add(65540, "Polecenie w trakcie wykonywania");
            errorDictionary.Add(65541, "Bledny numer polecenia");
            errorDictionary.Add(65542, "Bledny uchwyt");
            errorDictionary.Add(65543, "Przekazany bufor znakowy jest za ma�y");
            errorDictionary.Add(65544, "Poza zakresem licznika");
            errorDictionary.Add(65545, "Bledny tryb kolejkowania");
            errorDictionary.Add(65546, "Rozkaz anulowany");
            errorDictionary.Add(65793, "Bledny 1 parametr polecenia");
            errorDictionary.Add(65794, "Bledny 2 parametr polecenia");
            errorDictionary.Add(65795, "Bledny 3 parametr polecenia");
            errorDictionary.Add(65796, "Bledny 4 parametr polecenia");
            errorDictionary.Add(65797, "Bledny 5 parametr polecenia");
            errorDictionary.Add(65798, "Bledny 6 parametr polecenia");
            errorDictionary.Add(65799, "Bledny 7 parametr polecenia");
            errorDictionary.Add(65800, "Bledny 8 parametr polecenia");
            errorDictionary.Add(65801, "Bledny 9 parametr polecenia");
            errorDictionary.Add(65802, "Bledny 10 parametr polecenia");
            errorDictionary.Add(65803, "Bledny 11 parametr polecenia");
            errorDictionary.Add(65804, "Bledny 12 parametr polecenia");
            errorDictionary.Add(65805, "Bledny 13 parametr polecenia");
            errorDictionary.Add(65806, "Bledny 14 parametr polecenia");
            errorDictionary.Add(65807, "Bledny 15 parametr polecenia");
            errorDictionary.Add(65808, "Bledny 16 parametr polecenia");
            errorDictionary.Add(65809, "Bledny 17 parametr polecenia");
            errorDictionary.Add(65810, "Bledny 18 parametr polecenia");
            errorDictionary.Add(65811, "Bledny 19 parametr polecenia");
            errorDictionary.Add(65812, "Bledny 20 parametr polecenia");
            errorDictionary.Add(65813, "Bledny 21 parametr polecenia");
            errorDictionary.Add(65814, "Bledny 22 parametr polecenia");
            errorDictionary.Add(4390942, "Blad nietypowy - rezygnacja  przerwanie funkcji");
            errorDictionary.Add(4390962, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4390963, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4390964, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4390965, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4390966, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4390967, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4390968, "Blad wykonywania operacji przez kasę.");
            errorDictionary.Add(4391235, "Funkcja zablokowana w konfiguracji");
            errorDictionary.Add(4391272, "znaleziono zwore serwisowa");
            errorDictionary.Add(4391273, "nie znaleziono zwory");
            errorDictionary.Add(4391274, "Blad weryfikacji danych klucza");
            errorDictionary.Add(4391275, "uplynal czas na odpowiedz od klucza");
            errorDictionary.Add(4391276, "Bladne haslo w sensie niezgodnosci z poprawnoscia skladniowa");
            errorDictionary.Add(4391277, "Bladne haslo w sensie niezgodnosci z tym z bazy");
            errorDictionary.Add(4391294, "proba wykonania raportu zerowego");
            errorDictionary.Add(4391295, "Brak raportu dobowego.");
            errorDictionary.Add(4391296, "Brak rekordu w pamieci.");
            errorDictionary.Add(4391312, "Bladna wartosc");
            errorDictionary.Add(4391316, "Wprowadzono nieprawidlowy kod kontrolny");
            errorDictionary.Add(4391372, "Blad zegara w trybie fiskalnym");
            errorDictionary.Add(4391373, "Blad zegara w trybie niefiskalnym");
            errorDictionary.Add(4391392, "drukarka juz autoryzowana  bezterminowo");
            errorDictionary.Add(4391393, "nie rozpoczeto jeszcze autoryzacji");
            errorDictionary.Add(4391394, "kod juz wprowadzony");
            errorDictionary.Add(4391395, "proba wprowadzenia Blednych wartosci");
            errorDictionary.Add(4391396, "minal czas pracy kasy  sprzedaz zablokowana");
            errorDictionary.Add(4391397, "Bledny kod autoryzacji");
            errorDictionary.Add(4391398, "Blokada autoryzacji. Wprowadz kod z klawiatury.");
            errorDictionary.Add(4391399, "U�yto juz maksymalnej liczby kodow");
            errorDictionary.Add(4391412, "przepelnienie statystyki minimalnej");
            errorDictionary.Add(4391413, "przepelnienie statystyki maksymalnej");
            errorDictionary.Add(4391414, "przepelnienie stanu kasy");
            errorDictionary.Add(4391415, "wartosc stanu kasy po wyplacie staje sie ujemna (przyjmuje sie stan zerowy kasy)");
            errorDictionary.Add(4391612, "Bledny adres IP");
            errorDictionary.Add(4391613, "Blad numeru tonu");
            errorDictionary.Add(4391614, "Blad dlugosci impulsu szuflady");
            errorDictionary.Add(4391615, "Blad stawki VAT");
            errorDictionary.Add(4391616, "Blad czasu wylogowania");
            errorDictionary.Add(4391617, "Blad czasu uspienia");
            errorDictionary.Add(4391618, "Blad czasu wylaczenia");
            errorDictionary.Add(4391625, "Bladne parametry konfiguracji");
            errorDictionary.Add(4391626, "Bladna wartosc kontrastu wyswietlacza");
            errorDictionary.Add(4391627, "Bladna wartosc podswietlenia wyswietlacza");
            errorDictionary.Add(4391628, "Bladna wartosc czasu zaniku podswietlenia");
            errorDictionary.Add(4391629, "za d�uga linia naglowka albo stopki");
            errorDictionary.Add(4391630, "Bladna konfiguracja komunikacji");
            errorDictionary.Add(4391631, "Bladna konfiguracja protokolu kom.");
            errorDictionary.Add(4391632, "Bledny identyfikator portu");
            errorDictionary.Add(4391633, "Bledny numer tekstu reklamowego");
            errorDictionary.Add(4391634, "podany czas wychodzi poza wymagany zakres");
            errorDictionary.Add(4391635, "podana data/czas niepoprawne");
            errorDictionary.Add(4391636, "inna godzina w roznicach czasowych 0<=>23");
            errorDictionary.Add(4391638, "Bladna zawartosc tekstu w linii wyswietlacza");
            errorDictionary.Add(4391639, "Bladna wartosc dla przewijania na wyswietlaczu");
            errorDictionary.Add(4391640, "Bladna konfiguracja portu");
            errorDictionary.Add(4391641, "Bladna konfiguracja monitora transakcji");
            errorDictionary.Add(4391642, "Port zajety przez komputer");
            errorDictionary.Add(4391643, "Port zajety przez TCP/IP");
            errorDictionary.Add(4391644, "Port zajety przez monitor");
            errorDictionary.Add(4391645, "Port zajety");
            errorDictionary.Add(4391646, "Port zajety przez tunelowanie");
            errorDictionary.Add(4391647, "Port zajety przez odczyt KE");
            errorDictionary.Add(4391650, "nieprawidlowa konfiguracja Ethernetu");
            errorDictionary.Add(4391651, "nieprawidlowy typ wyswietlacza");
            errorDictionary.Add(4391652, "Dla tego typu wyswietlacza nie mozna ustawi� czasu zaniku podswietlenia");
            errorDictionary.Add(4391653, "wartosc czasu spoza zakresu");
            errorDictionary.Add(4391657, "Bladna numer strony kodowe");
            errorDictionary.Add(4391658, "Bladna konfiguracja ramki monitora transakcji");
            errorDictionary.Add(4391659, "DHCP aktywne. Funkcja niedostepna.");
            errorDictionary.Add(4391660, "DHCP dozwolone tylko przy transmisji ethernet.");
            errorDictionary.Add(4391664, "Protok� Thermal jest niedostepny po interfejsie TCP/IP.");
            errorDictionary.Add(4391732, "negatywny wynik testu");
            errorDictionary.Add(4391733, "Brak testowanej opcji w konfiguracji");
            errorDictionary.Add(4391769, "brak pamieci na inicjalizacj� bazy drukarkowej");
            errorDictionary.Add(4391912, "Blad fatalny modulu fiskalnego.");
            errorDictionary.Add(4391913, "wypi�ta Pamiec fiskalna");
            errorDictionary.Add(4391914, "Blad zapisu");
            errorDictionary.Add(4391915, "Blad nie ujety w specyfikacji bios");
            errorDictionary.Add(4391916, "Bladne sumy kontrolne");
            errorDictionary.Add(4391917, "Blad w pierwszym bloku kontrolnym");
            errorDictionary.Add(4391918, "Blad w drugim bloku kontrolnym");
            errorDictionary.Add(4391919, "Bledny id rekordu");
            errorDictionary.Add(4391920, "Blad inicjalizacji adresu startowego");
            errorDictionary.Add(4391921, "adres startowy zainicjalizowany");
            errorDictionary.Add(4391922, "numer unikatowy juz zapisany");
            errorDictionary.Add(4391923, "brak numeru w trybie fiskalnym");
            errorDictionary.Add(4391924, "Blad zapisu numeru unikatowego");
            errorDictionary.Add(4391925, "przepelnienie numer�w unikatowych");
            errorDictionary.Add(4391926, "Bledny j�zyk w numerze unikatowym");
            errorDictionary.Add(4391927, "wi�cej niz jeden NIP");
            errorDictionary.Add(4391928, "drukarka w trybie do odczytu bez rekordu fiskalizacji");
            errorDictionary.Add(4391929, "przekroczono liczb� zerowa� RAM");
            errorDictionary.Add(4391930, "przekroczono liczb� raportow dobowych");
            errorDictionary.Add(4391931, "Blad weryfikacji numeru unikatowego");
            errorDictionary.Add(4391932, "Blad weryfikacji statystyk z RD.");
            errorDictionary.Add(4391933, "Blad odczytu danych z NVR do weryfikacji FM");
            errorDictionary.Add(4391934, "Blad zapisu danych z NVR do weryfikacji FM");
            errorDictionary.Add(4391935, "Pamiec fiskalna jest ma�a 1Mb zamiast 2Mb");
            errorDictionary.Add(4391936, "nie zainicjalizowany obszar danych w pamieci fiskalnej");
            errorDictionary.Add(4391937, "Bledny format numeru unikatowego");
            errorDictionary.Add(4391938, "za duzo Blednych blokow w FM");
            errorDictionary.Add(4391939, "Blad oznaczenia Bladnego bloku");
            errorDictionary.Add(4391940, "rekord w pamieci fiskalnej nie istnieje - obszar pusty");
            errorDictionary.Add(4391941, "rekord w pamieci fiskalnej z data pozniejsza od poprzedniego");
            errorDictionary.Add(4391942, "Blad odczytu skrotu raportu dobowego.");
            errorDictionary.Add(4391943, "Blad zapisu skrotu raportu dobowego");
            errorDictionary.Add(4391944, "Blad odczytu informacji o weryfikacji skrotu raportu dobowego.");
            errorDictionary.Add(4391945, "Blad zapisu informacji o weryfikacji skrotu raportu dobowego");
            errorDictionary.Add(4391946, "Blad odczytu etykiety nosnika");
            errorDictionary.Add(4391947, "Blad zapisu etykiety nosnika");
            errorDictionary.Add(4391948, "Niezgodnosc danych kopii elektonicznej");
            errorDictionary.Add(4391949, "Bladne dane w obszarze bitow faktur  brak ciaglosci  zaplatany gdzies bit lub podobne");
            errorDictionary.Add(4391950, "Blad w obszarze faktur. Obszar nie jest pusty");
            errorDictionary.Add(4391951, "Brak miejsca na nowe faktury");
            errorDictionary.Add(4391952, "Suma faktur z raportow dobowych jest wieksza od licznika faktur.");
            errorDictionary.Add(4391953, "Blad w obszarze ID modulu kopii");
            errorDictionary.Add(4391954, "Blad zapisu ID modulu kopii.");
            errorDictionary.Add(4391955, "Obszar ID modulu kopii Zapelniony.");
            errorDictionary.Add(4391956, "nieudana fiskalizacja");
            errorDictionary.Add(4392862, "przekroczony zakres totalizerow paragonu.");
            errorDictionary.Add(4392863, "wplata forma platnosci przekracza max. wplate.");
            errorDictionary.Add(4392864, "suma form platnosci przekracza max. wplate.");
            errorDictionary.Add(4392865, "formy platnosci pokrywaja juz do zaplaty.");
            errorDictionary.Add(4392866, "wplata reszty przekracza max. wplate.");
            errorDictionary.Add(4392867, "suma form platnosci przekracza max. wplate.");
            errorDictionary.Add(4392868, "przekroczony zakres total.");
            errorDictionary.Add(4392869, "przekroczony maksymalny zakres paragonu.");
            errorDictionary.Add(4392870, "przekroczony zakres wartosci opakowan.");
            errorDictionary.Add(4392871, "przekroczony zakres wartosci opakowan przy stornowaniu.");
            errorDictionary.Add(4392873, "wplata reszty zbyt duza");
            errorDictionary.Add(4392874, "wplata forma platnosci wartosci 0");
            errorDictionary.Add(4392892, "przekroczony zakres kwoty bazowej rabatu/narzutu");
            errorDictionary.Add(4392893, "przekroczony zakres kwoty po rabacie / narzucie");
            errorDictionary.Add(4392894, "Blad obliczania rabatu/narzutu");
            errorDictionary.Add(4392895, "wartosc bazowa ujemna lub rowna 0");
            errorDictionary.Add(4392896, "wartosc rabatu/narzutu zerowa");
            errorDictionary.Add(4392897, "wartosc po rabacie ujemna lub rowna 0");
            errorDictionary.Add(4392902, "Niedozwolone stornowanie towaru. Bledny stan transakcji.");
            errorDictionary.Add(4392903, "Niedozwolony rabat/narzut. Bledny stan transakcji.");
            errorDictionary.Add(4392912, "Blad pola VAT.");
            errorDictionary.Add(4392914, "brak naglowka");
            errorDictionary.Add(4392915, "zaprogramowany naglowek");
            errorDictionary.Add(4392916, "brak aktywnych stawek VAT.");
            errorDictionary.Add(4392917, "brak trybu transakcji.");
            errorDictionary.Add(4392918, "Blad pola cena ( cena <= 0)");
            errorDictionary.Add(4392919, "Blad pola ilosc ( ilosc <= 0)");
            errorDictionary.Add(4392920, "Blad kwoty total");
            errorDictionary.Add(4392921, "Blad kwoty total  rowna zero");
            errorDictionary.Add(4392922, "przekroczony zakres totalizerow dobowych.");
            errorDictionary.Add(4392933, "proba ponownego ustawienia zegara.");
            errorDictionary.Add(4392934, "zbyt duza roznica dat");
            errorDictionary.Add(4392935, "roznica wieksza niz godzina w trybie uzytkownika w trybie fiskalnym.");
            errorDictionary.Add(4392936, "zly format daty (np. 13 miesiec)");
            errorDictionary.Add(4392937, "data wczesniejsza od ostatniego zapisu do modulu");
            errorDictionary.Add(4392938, "Blad zegara.");
            errorDictionary.Add(4392939, "przekroczono maksymalna liczbe zmian stawek VAT");
            errorDictionary.Add(4392940, "proba zdefiniowana identycznych stawek VAT");
            errorDictionary.Add(4392941, "Bladne wartosci stawek VAT");
            errorDictionary.Add(4392942, "proba zdefiniowania stawek VAT wszystkich nieaktywnych");
            errorDictionary.Add(4392943, "Blad pola NIP.");
            errorDictionary.Add(4392944, "Blad numeru unikatowego pamieci fiskalnej.");
            errorDictionary.Add(4392945, "urzadzenie w trybie fiskalnym.");
            errorDictionary.Add(4392946, "urzadzenie w trybie niefiskalnym.");
            errorDictionary.Add(4392947, "niezerowe totalizery.");
            errorDictionary.Add(4392948, "urzadzenie w stanie tylko do odczytu.");
            errorDictionary.Add(4392949, "urzadzenie nie jest w stanie tylko do odczytu.");
            errorDictionary.Add(4392950, "urzadzenie w trybie transakcji.");
            errorDictionary.Add(4392951, "zerowe totalizery.");
            errorDictionary.Add(4392952, "Blad obliczen walut  przepelnienie przy mno�eniu lub dzieleniu.");
            errorDictionary.Add(4392953, "proba zakonczenia pozytywnego paragonu z wartosci� 0");
            errorDictionary.Add(4392954, "Blady format daty poczatkowej");
            errorDictionary.Add(4392955, "Blady format daty koncowej");
            errorDictionary.Add(4392956, "proba wykonania raportu miesiecznego w danym miesiecu");
            errorDictionary.Add(4392957, "data poczatkowa pozniejsza od biezacej daty");
            errorDictionary.Add(4392958, "data koncowa wczesniejsza od daty fiskalizacji");
            errorDictionary.Add(4392959, "numer poczatkowy lub koncowy rowny zero");
            errorDictionary.Add(4392960, "numer poczatkowy wiekszy od numeru koncowego");
            errorDictionary.Add(4392961, "numer raportu zbyt duzy");
            errorDictionary.Add(4392962, "data poczatkowa pozniejsza od daty koncowej");
            errorDictionary.Add(4392963, "brak pamieci w buforze tekstow.");
            errorDictionary.Add(4392964, "brak pamieci w buforze transakcji");
            errorDictionary.Add(4392966, "formy platnosci nie pokrywaja kwoty do zaplaty lub reszty");
            errorDictionary.Add(4392967, "Bladna linia");
            errorDictionary.Add(4392968, "tekst pusty");
            errorDictionary.Add(4392969, "przekroczony rozmiar");
            errorDictionary.Add(4392970, "Bladna liczba linii.");
            errorDictionary.Add(4392972, "Bledny stan transakcji");
            errorDictionary.Add(4392974, "jest wydrukowana czesc jakiegos dokumentu");
            errorDictionary.Add(4392975, "Blad parametru");
            errorDictionary.Add(4392976, "brak rozpoczecia wydruku lub transakcji");
            errorDictionary.Add(4392979, "Blad ustawien konfiguracyjnych wydruk�w / drukarki");
            errorDictionary.Add(4392982, "Data przegladu wczesniejsza od systemowej");
            errorDictionary.Add(4392992, "Nieparzysta liczba danych w formacie HEX");
            errorDictionary.Add(4392993, "Niepoprawna wartosc dla formatu HEX");
            errorDictionary.Add(4393013, "Zapelnienie bazy");
            errorDictionary.Add(4393014, "Stawka nieaktywna");
            errorDictionary.Add(4393015, "nieprawidlowa stawka VAT");
            errorDictionary.Add(4393016, "Blad nazwy");
            errorDictionary.Add(4393017, "Blad przypisania stawki");
            errorDictionary.Add(4393018, "Zablokowany");
            errorDictionary.Add(4393019, "Nie znaleziono w bazie drukarkowej");
            errorDictionary.Add(4393020, "baza nie jest Zapelniona");
            errorDictionary.Add(4393022, "Blad autoryzacji");
            errorDictionary.Add(4393413, "Bledny identyfikator raportu");
            errorDictionary.Add(4393414, "Bledny identyfikator linii raportu");
            errorDictionary.Add(4393415, "Bledny identyfikator naglowka raportu");
            errorDictionary.Add(4393416, "Zbyt malo parametrow raportu");
            errorDictionary.Add(4393417, "Raport nie rozpoczety");
            errorDictionary.Add(4393418, "Raport rozpoczety");
            errorDictionary.Add(4393419, "Bledny identyfikator komendy");
            errorDictionary.Add(4393420, "proba wydrukowania szerokiej formatki na papierze 57mm");
            errorDictionary.Add(4393433, "Raport juz rozpoczety");
            errorDictionary.Add(4393434, "Raport nie rozpoczety");
            errorDictionary.Add(4393435, "Bladna stawka VAT");
            errorDictionary.Add(4393444, "Bladna liczba kopii faktur");
            errorDictionary.Add(4393445, "Pusty numer faktury");
            errorDictionary.Add(4393446, "Bledny format wydruku");
            errorDictionary.Add(4393512, "Bledny typ rabatu/narzutu");
            errorDictionary.Add(4393513, "wartosc rabatu/narzutu spoza zakresu");
            errorDictionary.Add(4393613, "Blad identyfikatora stawki podatkowej.");
            errorDictionary.Add(4393614, "Bledny identyfikator dodatkowej stopki.");
            errorDictionary.Add(4393615, "Przekroczona liczba dodatkowych stopek.");
            errorDictionary.Add(4393616, "Zbyt slaby akumulator.");
            errorDictionary.Add(4393617, "Bledny identyfikator typu formy platnosci.");
            errorDictionary.Add(4393618, "Brak zasilacza");
            errorDictionary.Add(4393622, "Usluga o podanym identyfikatorze nie jest uruchomiona");
            errorDictionary.Add(4393713, "Blad weryfikacji wartosci rabatu/narzutu");
            errorDictionary.Add(4393714, "Blad weryfikacji wartosci linii sprzedazy");
            errorDictionary.Add(4393715, "Blad weryfikacji wartosci opakowania");
            errorDictionary.Add(4393716, "Blad weryfikacji wartosci formy platnosci");
            errorDictionary.Add(4393717, "Blad weryfikacji wartosci fiskalnej");
            errorDictionary.Add(4393718, "Blad weryfikacji wartosci opakowan dodatnich");
            errorDictionary.Add(4393719, "Blad weryfikacji wartosci opakowan ujemnych");
            errorDictionary.Add(4393720, "Blad weryfikacji wartosci wplaconych form platnosci");
            errorDictionary.Add(4393721, "Blad weryfikacji wartosci reszt");
            errorDictionary.Add(4393763, "Blad stornowania  Bladna ilosc");
            errorDictionary.Add(4393764, "Blad stornowania  Bladna wartosc");
            errorDictionary.Add(4393812, "Stan kopii elektronicznej nie pozwala na wydrukowanie tego dokumentu");
            errorDictionary.Add(4393813, "Brak nosnika lub operacja na nosniku trwa");
            errorDictionary.Add(4393814, "nosnik nie jest poprawnie zweryfikowany");
            errorDictionary.Add(4393815, "Pamiec podreczna kopii elektronicznej zawiera zbyt duza ilosc danych");
            errorDictionary.Add(4393818, "Uszkodzony bufor kopii elektronicznej");
            errorDictionary.Add(4393819, "Brak nosnika");
            errorDictionary.Add(4393820, "nosnik nieprawidlowy - nieodpowiedni dla wybranej operacji");
            errorDictionary.Add(4393823, "Brak pliku na nosniku");
            errorDictionary.Add(4393825, "nieprawidlowy wynik testu.");
            errorDictionary.Add(4393827, "Pusta Pamiec podreczna.");
            errorDictionary.Add(4393828, "Trwa weryfikacja nosnika");
            errorDictionary.Add(4393829, "Bledny typ dokumentu");
            errorDictionary.Add(4393830, "Dane niedostepne (nieaktualne)");
            errorDictionary.Add(4393963, "Nie mozna zmieniz 2 raz waluty ewidencyjnej po RD.");
            errorDictionary.Add(4393964, "proba ustawienia juz ustawionej waluty");
            errorDictionary.Add(4393965, "Bladna nazwa waluty");
            errorDictionary.Add(4393966, "Automatyczna zmiana waluty.");
            errorDictionary.Add(4393967, "Bladna wartosc przelicznika kursu");
            errorDictionary.Add(4393968, "Przekroczono maksymalna liczbe zmian walut");
            errorDictionary.Add(4393992, "proba zdefiniowania stawek VAT ze star� data");
            errorDictionary.Add(4393996, "Automatyczna zmiana stawek VAT");
            errorDictionary.Add(4393997, "Brak pola daty");
            errorDictionary.Add(4394002, "nieprawidlowy kod autoryzacji formatki");
            errorDictionary.Add(4394003, "Autoryzacja formatki zablokowana");
            errorDictionary.Add(4394004, "Formatka zablokowana");
            errorDictionary.Add(4394012, "Brak parametru autoryzacji fiskalizacji");
            errorDictionary.Add(4394013, "nieprawidlowy kod autoryzacji fiskalizacji");
            errorDictionary.Add(4394022, "Blad napiecia szuflady");
            errorDictionary.Add(4394112, "proba wydruku pustego kodu");
            errorDictionary.Add(4394113, "Kod przekracza obszar papieru");
            errorDictionary.Add(4394114, "nieprawidlowa wartosc skali wydruku");
            errorDictionary.Add(4394115, "nieprawidlowa wartosc parametru Y2X ratio");
            errorDictionary.Add(4394117, "Kod przekracza dopuszczalny obszar pamieci");
            errorDictionary.Add(4394118, "Strumien wejsciowy przekracza dopuszczalna dlugosc");
            errorDictionary.Add(4394119, "Liczba kolumn poza zakresem");
            errorDictionary.Add(4394120, "Liczba wierszy poza zakresem");
            errorDictionary.Add(4394121, "Poziom korekcji bladow poza zakresem");
            errorDictionary.Add(4394122, "Liczba pikseli na modul poza zakresem");
            errorDictionary.Add(4394132, "Nieobslugiwany typ kodu kreskowego");
            errorDictionary.Add(4394133, "Blad zapisu kodu kreskowego");
            errorDictionary.Add(4394134, "Blad odczytu kodu kreskowego");
            errorDictionary.Add(4394162, "numer grafiki poza zakresem");
            errorDictionary.Add(4394163, "brak grafiki w slocie");
            errorDictionary.Add(4394164, "grafika tylko do odczytu");
            errorDictionary.Add(4394165, "niepoprawny rozmiar grafiki");
            errorDictionary.Add(4394166, "Przekroczony rozmiar pamieci przeznaczony na grafik�.");
            errorDictionary.Add(4394167, "Blad zapisu grafiki na kopi� elektroniczn�");
            errorDictionary.Add(4394168, "Blad zapisu grafiki");
            errorDictionary.Add(4394169, "poziom drukowalnosci grafik z kopii poza zakresem");
            errorDictionary.Add(4394170, "niepoprawny rozmiar danych");
            errorDictionary.Add(4456449, "Nierozpoznana komenda");
            errorDictionary.Add(4456450, "Brak obowiazkowego pola");
            errorDictionary.Add(4456451, "Blad konwersji pola ");
            errorDictionary.Add(4456452, "Bledny token");
            errorDictionary.Add(4456453, "Zla suma kontrolna");
            errorDictionary.Add(4456454, "Puste pole (kolejno dwa tabulatory)");
            errorDictionary.Add(4456455, "niewlasciwa dlugosc nazwy rozkazu");
            errorDictionary.Add(4456456, "niewlasciwa dlugosc tokena");
            errorDictionary.Add(4456457, "niewlasciwa dlugosc sumy kontrolnej");
            errorDictionary.Add(4456458, "niewlasciwa dlugosc pola danych");
            errorDictionary.Add(4456459, "Zapelniony bufor odbiorczy");
            errorDictionary.Add(4456460, "Nie mozna wykonac rozkazu w trybie natychmiastowym");
            errorDictionary.Add(4456461, "Nie znaleziono rozkazu o podanym tokenie");
            errorDictionary.Add(4456462, "Zapelniona kolejka wejsciowa");
            errorDictionary.Add(4456463, "Blad budowy ramki");


            bool runServer = true;
            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                pageData = "";
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                if (req.Url != null)
                    Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();
                Console.WriteLine(req.ContentLength64);
                Console.WriteLine(req.ContentType);
                Console.WriteLine("Body: " + req.HasEntityBody);

                int printerErrorCode = 0;

                if (req.HasEntityBody)
                {
                    string body;
                    using (Stream receiveStream = req.InputStream)
                    {
                        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            body = readStream.ReadToEnd();
                        }
                    }
                    Console.WriteLine(body + "\n\n");
                    JsonReceivedCommunicate receipt = JsonSerializer.Deserialize<JsonReceivedCommunicate>(body);
                    if (receipt != null)
                        receipt.WriteJsonCommunicate();

                    DFPrnCommunication paragon = new DFPrnCommunication();
                    printerErrorCode = paragon.PrintReceipt(body);
                    paragon.CloseDF();
                }

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
//                if ((req.HttpMethod == "POST") && (req.Url != null && req.Url.AbsolutePath == "/shutdown"))
                if ((req.HttpMethod == "POST") && (req.Url != null && req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }
                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url != null && req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                // Write the response info
                if (req.HasEntityBody)
                {
                    if(printerErrorCode == 0)
                    {
                        resp.StatusCode = 200;
                        JsonSendCommunicate responseBody = new JsonSendCommunicate
                        {
                            ok = true,
                            code = -1,
                            bn = "1500100900",
                            /*hn = "59",
                            took = 4234,
                            message = "",
                            ts = 1659489230623,
                            tsend = 1659489234857*/
                        };
                        pageData = "{" + JsonSerializer.Serialize(responseBody) + "}";
                    }
                    else
                    {
                        resp.StatusCode = 500;
                        //string responseBody = "{{\"ok\":false,\"code\":1,\"message\":\"Kod: "+Convert.ToString(printerErrorCode, 10)+" BLAD DRUKARKI\"}}";
                        string responseBody = "{{\"ok\":false,\"code\":1,\"message\":\"Kod: "+Convert.ToString(printerErrorCode, 10)+" "+errorDictionary[printerErrorCode]+"\"}}";
                        pageData = responseBody;
                    }
                    resp.AddHeader("Content-Type", "application/json;charset=utf-8");
                    resp.AddHeader("X-Rate-Limit-Limit", "5");
                    resp.AddHeader("X-Rate-Limit-Remaining", "4");
                    resp.AddHeader("Access-Control-Allow-Headers", "Origin,X-Requested-With,Content-Type,Accept");

                }
                else
                {
                    resp.StatusCode = 204;
                    resp.AddHeader("Access-Control-Allow-Methods", "GET,POST,DELETE,UPDATE,PUT,PATCH");
                    resp.AddHeader("Vary", "Access-Control-Request-Headers");
                    resp.AddHeader("Access-Control-Allow-Headers", "content-type");
                }

                resp.AddHeader("X-Powered-By", "Express");
                resp.AddHeader("Access-Control-Allow-Origin", "*");
                resp.AddHeader("Connection", "keep-alive");
                resp.AddHeader("Keep-Alive", "timeout=5");

                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }
    }
}

namespace DFPrnNamespace
{
    using JsonCommunicationNamespace;
    public class DFPrnCommunication
    {
        //        public const string PosLibDLL = "D:\\Zapisy_programow_C#\\woocommerce\\Git\\PosnetServerWinFormsApp\\Lib\\libposcmbth.dll";
        public const string PosLibDLL = "C:\\FINA\\libposcmbth.dll";
        [DllImport(PosLibDLL, EntryPoint = "POS_CreateDeviceHandle", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr POS_CreateDeviceHandle(uint deviceType);
        [DllImport(PosLibDLL, EntryPoint = "POS_DestroyDeviceHandle")]
        public static extern uint POS_DestroyDeviceHandle(IntPtr hDevice);
        [DllImport(PosLibDLL, EntryPoint = "POS_SetDeviceParam", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern uint POS_SetDeviceParam(IntPtr hDevice, uint paramCode, IntPtr paramValue);
        [DllImport(PosLibDLL, EntryPoint = "POS_OpenDevice")]
        public static extern IntPtr POS_OpenDevice(IntPtr hDevice);
        //        POSNET_STATUS __stdcall     POS_CloseDevice(POSNET_HANDLE hLocalDevice)
        [DllImport(PosLibDLL, EntryPoint = "POS_CloseDevice")]
        public static extern uint POS_CloseDevice(IntPtr hLocalDevice);
        [DllImport(PosLibDLL, EntryPoint = "POS_GetPrnDeviceStatus")]
        public static extern uint POS_GetPrnDeviceStatus(IntPtr hLocalDevice, byte mode, IntPtr globalStatus, IntPtr printerStatus);

        //        POSNET_API POSNET_STATUS __stdcall POS_GetError(POSNET_HANDLE hLocalDevice); // Podaj kod statusu związany z uchwytem urządzenia.
        //        POSNET_API const char* __stdcall    POS_GetErrorString(POSNET_STATUS code, char* lang); // Zwróć tekstowy opis błędu.
        [DllImport(PosLibDLL, EntryPoint = "POS_GetError")]
        public static extern uint POS_GetError(IntPtr hLocalDevice);
        [DllImport(PosLibDLL, EntryPoint = "POS_GetErrorString")]
        public static extern char[] POS_GetErrorString(uint code, IntPtr lang);
        //        POSNET_STATUS __stdcall POS_PostRequest(POSNET_HANDLE hRequest, unsigned char mode);  // Fukcja umieszcza obiekt rozkazowy w kolejce rozkazów do wykonania.
        [DllImport(PosLibDLL, EntryPoint = "POS_CreateRequest")]
        public static extern IntPtr POS_CreateRequest(IntPtr hLocalDevice, IntPtr command);
        //        POSNET_HANDLE __stdcall     POS_CreateRequestEx(POSNET_HANDLE hLocalDevice, const char* command, const char* parameters)
        [DllImport(PosLibDLL, EntryPoint = "POS_CreateRequestEx")]
        public static extern IntPtr POS_CreateRequestEx(IntPtr hLocalDevice, IntPtr command, IntPtr parameters);
        [DllImport(PosLibDLL, EntryPoint = "POS_PostRequest")]
        public static extern uint POS_PostRequest(IntPtr hRequest, byte mode);
        //        POSNET_STATUS __stdcall     POS_WaitForRequestCompleted(POSNET_HANDLE hRequest, unsigned long timeout);   // Czekaj na zakończenie rozkazu.
        [DllImport(PosLibDLL, EntryPoint = "POS_WaitForRequestCompleted")]
        public static extern uint POS_WaitForRequestCompleted(IntPtr hRequest, uint timeout);
        //        POSNET_STATUS __stdcall POS_GetRequestStatus(POSNET_HANDLE hRequest)
        [DllImport(PosLibDLL, EntryPoint = "POS_GetRequestStatus")]
        public static extern uint POS_GetRequestStatus(IntPtr hRequest);
        //        POSNET_STATUS __stdcall     POS_DestroyRequest(POSNET_HANDLE hRequest);  // Zniszczenie obiektu rozkazowego i zwolnienie zajmowanej przezeń pamięci.
        [DllImport(PosLibDLL, EntryPoint = "POS_DestroyRequest")]
        public static extern uint POS_DestroyRequest(IntPtr hRequest);


        const uint POSNET_INTERFACE_RS232 = 0x0001;   // Podłączenie przez RS232.
        const uint POSNET_INTERFACE_USB = 0x0002;   // Podłączenie przez USB.
        const uint POSNET_INTERFACE_ETH = 0x0003;   // Podłączenie przez Ethernet.

        const uint POSNET_DEV_PARAM_COMSETTINGS = 0x00020001;  // Parametry portu szeregowego.
        const uint POSNET_DEV_PARAM_SENDTIMEOUT = 0x00020004;  // Czas w[s] po jakim ma być zaniechane wysyłanie ramki.
        const uint POSNET_DEV_PARAM_IP = 0x00020005;  // Adres IP kasy dla urządzenia typu POSNET_INTERFACE_ETH.
        const uint POSNET_DEV_PARAM_IPPORT = 0x00020006;  // Port dla protokołu TCP/ IP dla urządzenia typu POSNET_INTERFACE_ETH.
        const uint POSNET_DEV_PARAM_USBSERIAL = 0x00020007;  //Numer seryjny drukarki do otwarcia przez typ urządzenia POSNET_INTERFACE_USB.
        const uint POSNET_DEV_PARAM_LISTUSBSERIALS = 0x00020008;  // Odczyt wszystkich numerów seryjnych drukarek podłączonych do komputera poprzez interfejs USB i sterownik FTDI -D2XX.
        const uint POSNET_DEV_PARAM_OUTQUEUELENGTH = 0x00020009;  // Długość kolejki wysyłkowej, po przekroczeniu, której rozkazy traktowane są jak wysyłane w trybie natychmiastowym.
        const uint POSNET_DEV_PARAM_STATUSPOLLINGINTERVAL = 0x0002000A;  // Interwał pomiędzy automatycznymi odpytaniami o status drukarki.
        const uint POSNET_DEV_PARAM_FILEHANDLE = 0x0002000E;  // Pobranie uchwytu portu szeregowego.

        // #define POSNET_REQMODE_SPOOL   0x00  // Tryb kolejkowania[domyślny], w tym trybie rozkaz umieszczany jest na.
        // #define POSNET_REQMODE_SPOOLSPECIAL   0x02  // Tryb kolejkowania specjalnego, w którym.

        const byte POSNET_REQMODE_SPOOL = 0x00;
        const byte POSNET_REQMODE_SPOOLSPECIAL = 0x02;

        // typedef void* POSNET_HANDLE;    // Definicja typu uchwytu urządzenia.
        // typedef unsigned long POSNET_STATUS;
        // typedef unsigned long POSNET_STATE;
        //        uint POSNET_STATUS;  // Definicja typu wartości zwracanej jako status.
        //        uint POSNET_STATE;  // Definicja typu wartości zwracanej jako stan rozkazu.

        /*
        const uint POSNET_STATUS_OK = 0x00000000;
        const uint OUTOFMEMORY = 0x00000001;
        const uint POSNET_STATUS_TIMEOUT = 0x00010003;
        */
        const uint POSNET_STATUS_OK = 0x00000000;
        const uint POSNET_STATUS_OUTOFMEMORY = 0x00000001;
        const uint POSNET_STATUS_FRAMETOOSHORT = 0x00000002;
        const uint POSNET_STATUS_FRAMINGERROR = 0x00000003;
        const uint POSNET_STATUS_COULDNOTOPEN = 0x00000005;
        const uint POSNET_STATUS_CRCERROR = 0x00000006;
        const uint POSNET_STATUS_IPCERROR = 0x00000007;
        const uint POSNET_STATUS_COMMERROR = 0x00000008;
        const uint POSNET_STATUS_USBERROR = 0x00000009;
        const uint POSNET_STATUS_FTLIBIMPORTFAIL = 0x0000000A;
        const uint POSNET_STATUS_COULDNOTSETUPPORT = 0x0000000B;
        const uint POSNET_STATUS_COULDNOTOPEN_ACCESSDENIED = 0x0000000C;
        const uint POSNET_STATUS_COULDNOTOPEN_FILENOTFOUND = 0x0000000D;
        const uint POSNET_STATUS_SETUP_INVALIDBAUD = 0x0000000E;
        const uint POSNET_STATUS_SETUP_INVALIDDATA = 0x0000000F;
        const uint POSNET_STATUS_SETUP_INVALIDPARITY = 0x00000010;
        const uint POSNET_STATUS_SETUP_INVALIDSTOP = 0x00000011;
        const uint POSNET_STATUS_SETUP_INVALIDHANDSHAKE = 0x00000012;
        const uint POSNET_STATUS_INVALIDSTATE = 0x00000013;
        const uint POSNET_STATUS_DEVICE_BUSY = 0x00000014;
        const uint POSNET_STATUS_BUSY = 0x00000020;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEINTR = 0x00000030;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEACCES = 0x00000031;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEINPROGRESS = 0x00000032;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEDESTADDRREQ = 0x00000033;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEADDRINUSE = 0x00000034;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEADDRNOTAVAIL = 0x00000035;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAENETDOWN = 0x00000036;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAENETUNREACH = 0x00000037;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAENETRESET = 0x00000038;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAECONNABORTED = 0x00000039;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAECONNRESET = 0x0000003A;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAETIMEDOUT = 0x0000003B;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAECONNREFUSED = 0x0000003c;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEHOSTDOWN = 0x0000003d;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAEHOSTUNREACH = 0x0000003e;
        const uint POSNET_STATUS_COULDNOTOPEN_WSAHOSTNOTFOUND = 0x0000003f;
        const uint POSNET_STATUS_COULDNOTOPEN_WSATRYAGAIN = 0x00000040;
        const uint POSNET_STATUS_ADVBMP_READERROR = 0x00000070;
        const uint POSNET_STATUS_ADVBMP_BADSIZE = 0x00000071;
        const uint POSNET_STATUS_ALREADY_COMPLETED = 0x00010000;
        const uint POSNET_STATUS_EMPTY = 0x00010001;
        const uint POSNET_STATUS_INVALIDVALUE = 0x00010002;
        const uint POSNET_STATUS_TIMEOUT = 0x00010003;
        const uint POSNET_STATUS_PENDING = 0x00010004;
        const uint POSNET_STATUS_INVALIDCOMMAND = 0x00010005;
        const uint POSNET_STATUS_INVALIDHANDLE = 0x00010006;
        const uint POSNET_STATUS_BUFFERTOOSHORT = 0x00010007;
        const uint POSNET_STATUS_OUTOFRANGE = 0x00010008;
        const uint POSNET_STATUS_INVALIDSPOOLMODE = 0x00010009;
        const uint POSNET_STATUS_CANCELLED = 0x0001000A;
        const uint POSNET_STATUS_INVALID_PARAM1 = 0x00010101;
        const uint POSNET_STATUS_INVALID_PARAM2 = 0x00010102;
        const uint POSNET_STATUS_INVALID_PARAM3 = 0x00010103;
        const uint POSNET_STATUS_INVALID_PARAM4 = 0x00010104;
        const uint POSNET_STATUS_INVALID_PARAM5 = 0x00010105;
        const uint POSNET_STATUS_INVALID_PARAM6 = 0x00010106;
        const uint POSNET_STATUS_CASHREGBASE = 0x00430000;
        const uint POSNET_STATUS_CASHREGCOMMBASE = 0x00440000;


        IntPtr hDevice = new IntPtr(0);
        IntPtr hLocalDevice = new IntPtr(0);
        IntPtr hRequest = new IntPtr(0);


        public DFPrnCommunication()
        {
            OpenDF();
        }

        public int PrintReceipt(string body)
        {
            int printerErrorCode = 0;
            JsonReceivedCommunicate receipt = JsonSerializer.Deserialize<JsonReceivedCommunicate>(body);
            bool cancel = true;
            IntPtr hRequest;
            do
            {
                hRequest = POS_CreateRequestEx(hLocalDevice, Marshal.StringToHGlobalAnsi("trinit"), Marshal.StringToHGlobalAnsi("bm,0"));
                if (hRequest == IntPtr.Zero)
                {
                    PrintStatus(POS_GetError(hLocalDevice), ref printerErrorCode);
                    break;
                }
                if (PrintStatus(POS_PostRequest(hRequest, POSNET_REQMODE_SPOOL), ref printerErrorCode) != POSNET_STATUS_OK) break;
                if (!WaitForCompleted(hRequest, hLocalDevice)) break;
                if (PrintStatus(POS_GetRequestStatus(hRequest), ref printerErrorCode) != POSNET_STATUS_OK) break;
                POS_DestroyRequest(hRequest);

                foreach (SingleLineJson s in receipt.lines)
                {
                    /*int vat = 0;
                    if (s.vtp == "23,00")
                        vat = 0;
                    if (s.vtp == "8,00")
                        vat = 1;
                    if (s.vtp == "5,00")
                        vat = 2;
                    if (s.vtp == "0,00")
                        vat = 3;*/
                    int vat = 4;  // Poplawska 
                    string sss = "na," + s.na + "\nvt," + Convert.ToString(vat, 10) + "\npr," + Convert.ToString(s.pr, 10) + "\nil," + Convert.ToString(s.il, 10);
                    hRequest = POS_CreateRequestEx(hLocalDevice, Marshal.StringToHGlobalAnsi("trline"), Marshal.StringToHGlobalAnsi(sss));
                    if (hRequest == IntPtr.Zero)
                    {
                        PrintStatus(POS_GetError(hLocalDevice), ref printerErrorCode);
                        break;
                    }
                    if (PrintStatus(POS_PostRequest(hRequest, POSNET_REQMODE_SPOOL), ref printerErrorCode) != POSNET_STATUS_OK) break;
                    if (!WaitForCompleted(hRequest, hLocalDevice)) break;
                    if (PrintStatus(POS_GetRequestStatus(hRequest), ref printerErrorCode) != POSNET_STATUS_OK) break;
                    POS_DestroyRequest(hRequest);
                }
                if (printerErrorCode != POSNET_STATUS_OK)
                    break;

                int zaplacono = 0;
                bool rodzajPlatnosci = false;
                if (receipt.payments != null)
                {
                    rodzajPlatnosci = true;
                    foreach (SinglePaymentDataJson p in receipt.payments)
                    {
                        zaplacono += p.wa;
                        int re = p.re ? 1 : 0;
                        string ppp = "ty," + Convert.ToString(p.ty, 10) + "\nwa," + Convert.ToString(p.wa, 10) + "\nna," + p.na + "\nre," + Convert.ToString(re, 10);
                        hRequest = POS_CreateRequestEx(hLocalDevice, Marshal.StringToHGlobalAnsi("trpayment"), Marshal.StringToHGlobalAnsi(ppp));
                        if (hRequest == IntPtr.Zero)
                        {
                            PrintStatus(POS_GetError(hLocalDevice), ref printerErrorCode);
                            break;
                        }
                        if (PrintStatus(POS_PostRequest(hRequest, POSNET_REQMODE_SPOOL), ref printerErrorCode) != POSNET_STATUS_OK) break;
                        if (!WaitForCompleted(hRequest, hLocalDevice)) break;
                        if (PrintStatus(POS_GetRequestStatus(hRequest), ref printerErrorCode) != POSNET_STATUS_OK) break;
                        POS_DestroyRequest(hRequest);
                    }
                    if (printerErrorCode != POSNET_STATUS_OK)
                        break;
                }

                string summaryKey = receipt.summary.Keys.First();
                string summaryValue = Convert.ToString(receipt.summary.Values.First(), 10);
                string summaryData = summaryKey + "," + summaryValue;
                if (rodzajPlatnosci) //jesli wybiera sie inny rodzaj platnosci to w podsumowaniu trzeba sprawdzic czy reszta sie zgadza
                {
                    int reszta = zaplacono - receipt.summary.Values.First();
                    summaryData = summaryKey + "," + summaryValue + "\nre," + reszta + "\nfp," + zaplacono;
                }
                hRequest = POS_CreateRequestEx(hLocalDevice, Marshal.StringToHGlobalAnsi("trend"), Marshal.StringToHGlobalAnsi(summaryData));
                if (hRequest == IntPtr.Zero)
                {
                    PrintStatus(POS_GetError(hLocalDevice), ref printerErrorCode);
                    break;
                }
                if (PrintStatus(POS_PostRequest(hRequest, POSNET_REQMODE_SPOOL), ref printerErrorCode) != POSNET_STATUS_OK) break;
                if (!WaitForCompleted(hRequest, hLocalDevice)) break;
                if (PrintStatus(POS_GetRequestStatus(hRequest), ref printerErrorCode) != POSNET_STATUS_OK) break;
                POS_DestroyRequest(hRequest);

                hRequest = IntPtr.Zero;
                cancel = false;
            }
            while (false);
            if (cancel)
            {
                if (hRequest != IntPtr.Zero) POS_DestroyRequest(hRequest);
                hRequest = POS_CreateRequest(hLocalDevice, Marshal.StringToHGlobalAnsi("prncancel"));
                PrintStatus(POS_PostRequest(hRequest, POSNET_REQMODE_SPOOL));
                PrintStatus(POS_WaitForRequestCompleted(hRequest, 5000));
                POS_DestroyRequest(hRequest);
            }
            //Console.WriteLine(printerErrorCode);
            return printerErrorCode;
        }
        public uint PrintStatus(uint status, ref int errorCode)
        {
            if (status != 0)
            {
                errorCode = (int)status;
            }
            return status;
        }
        private void OpenDF()
        {
            string settingPrinterPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            settingPrinterPath += "printerSetting.txt";
            string fileSettingsContent = File.ReadAllText(settingPrinterPath);
            char separatorEndline = '\n';
            string[] printerSettings = fileSettingsContent.Split(separatorEndline);
            for(int i=0; i < printerSettings.Length; i++)
                printerSettings[i] = printerSettings[i].Replace("\n", "").Replace("\r", "");
            if (printerSettings.Length > 0)
            {
                if(printerSettings[0] == "rs232")
                {
                    uint deviceType = POSNET_INTERFACE_RS232;
                    hDevice = POS_CreateDeviceHandle(deviceType);
                    string comSetting = printerSettings[1];
                    var result = POS_SetDeviceParam(hDevice, POSNET_DEV_PARAM_COMSETTINGS, Marshal.StringToHGlobalAnsi(comSetting));
                    hLocalDevice = POS_OpenDevice(hDevice);
                }
                else if(printerSettings[0] == "eth")
                {
                    uint deviceType = POSNET_INTERFACE_ETH;
                    hDevice = POS_CreateDeviceHandle(deviceType);
                    string ipSetting = printerSettings[1];
                    var result = POS_SetDeviceParam(hDevice, POSNET_DEV_PARAM_IP, Marshal.StringToHGlobalAnsi(ipSetting));
                    string portSetting = printerSettings[2];
                    var result2 = POS_SetDeviceParam(hDevice, POSNET_DEV_PARAM_IPPORT, Marshal.StringToHGlobalAnsi(portSetting));
                    hLocalDevice = POS_OpenDevice(hDevice);
                }
                else
                {
                    Console.WriteLine($"Odczytano plik printerSetting.txt ale jest bledna 1 linia pliku. Prawidlowo powinno byc \"rs232\" albo \"eth\". Natomiast odczytano {printerSettings[0]}");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono pliku lub jest on pusty. Upewnij sie ze plik \"printerSetting.txt\" znajduje sie w folderze z plikiem exe.");
            }
        }
        public void CloseDF()
        {
            POS_CloseDevice(hLocalDevice);
            POS_DestroyDeviceHandle(hDevice);
        }
        public uint PrintStatus(uint status)
        {
            if (status != 0)
            {
                /*//                string txt = new string(POS_GetErrorString(status, Marshal.StringToHGlobalAnsi("pl")));
                                *//*               
                                                printf("Błąd: %d - %s\n", (int)status, txt ? (char*)txt : "(brak opisu)");

                    if S = '' then
                      S := '(brak opisu)';
                    ShowMessage(Format('Błąd: %d - %s', [status, S]));
                                 */
            }
            return status;
        }

        /* oczekiwanie na zakończenie z mozliwoscia przedluzenia oczekiwania */
        /*
                bool WaitForCompleted(POSNET_HANDLE hRequest, POSNET_HANDLE hLocalDevice)
                {
                    POSNET_STATUS status;
                    do
                    {
                        // oczekujemy 10 s  na zakońzcenie rozkazu
                        status = POS_WaitForRequestCompleted(hRequest, 10000);
                        if (status == POSNET_STATUS_OK) return true;
                        if (status != POSNET_STATUS_TIMEOUT)
                        {
                            // jeśli status nie jest POSNET_STATUS_TIMEOUT ani POSNET_STATUS_OK to
                            // nastąpił błąd wykonania, wyświtlamy go i kończymy
                            PrintStatus(status);
                            return false;
                        }
                        // jeśli jest TIMEOUT to sprawdźmy i wydrukujmy status drukarki
                        long globalStatus = 0, printerStatus = 0;
                        PrintStatus(POS_GetPrnDeviceStatus(hLocalDevice, 1, &globalStatus, &printerStatus));
                        printf("Status drukarki - Drukarka: %ld, Mechanizm: %ld\n", globalStatus, printerStatus);
                        // dajemy użytkownikowi szansę na przedłużenie oczekiwania
                        printf("Nie otrzymano odpowiedzi na polecenie, czy czekać dalej (T/N + ENTER)\n");
                        do
                        {
                            char c;
                            if (scanf("%c", &c) <= 0) { fflush(stdin); continue; }
                            if (c == 'N' || c == 'n') return false;
                            if (c == 'T' || c == 't') break;
                        }
                        while (1);
                    } while (1);
                    return true;
                }
        */

        /* oczekiwanie na zakończenie z mozliwoscia przedluzenia oczekiwania */
        bool WaitForCompleted(IntPtr hRequest, IntPtr hLocalDevice)
        {
            uint status;
            do
            {
                // oczekujemy 10 s  na zakońzcenie rozkazu
                status = POS_WaitForRequestCompleted(hRequest, 10000);
                if (status == POSNET_STATUS_OK) return true;
                if (status != POSNET_STATUS_TIMEOUT)
                {
                    // jeśli status nie jest POSNET_STATUS_TIMEOUT ani POSNET_STATUS_OK to
                    // nastąpił błąd wykonania, wyświtlamy go i kończymy
                    PrintStatus(status);
                    return false;
                }

                break;
                /*
                                // jeśli jest TIMEOUT to sprawdźmy i wydrukujmy status drukarki
                                long globalStatus = 0, printerStatus = 0;
                                PrintStatus(POS_GetPrnDeviceStatus(hLocalDevice, 1, &globalStatus, &printerStatus));
                                printf("Status drukarki - Drukarka: %ld, Mechanizm: %ld\n", globalStatus, printerStatus);
                                // dajemy użytkownikowi szansę na przedłużenie oczekiwania
                                printf("Nie otrzymano odpowiedzi na polecenie, czy czekać dalej (T/N + ENTER)\n");
                                do
                                {
                                    char c;
                                    if (scanf("%c", &c) <= 0) { fflush(stdin); continue; }
                                    if (c == 'N' || c == 'n') return false;
                                    if (c == 'T' || c == 't') break;
                                }
                                while (1);
                */
            } while (true);
            return true;
        }
    }
}

