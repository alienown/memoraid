# Event Storming - Proces Powtórek Opartych o Fiszki

**Data sesji:** 6 sierpnia 2025  
**Uczestnicy:** Zespół Memoraid  
**Moderator:** GitHub Copilot  
**Zakres:** Modelowanie procesu powtórek jako kolejny etap rozwoju aplikacji Memoraid (MVP już gotowe)

## 🎯 Cel Sesji

Zmodelowanie procesu powtórek opartego o fiszki z wykorzystaniem algorytmu spaced repetition jako następnego etapu rozwoju aplikacji Memoraid.

## 📋 Odkrycia i Wyniki

### Domain Events (Zdarzenia Domenowe)
1. **Sesja powtórek rozpoczęta** - użytkownik rozpoczyna sesję nauki
2. **Następna fiszka wybrana** - system wybiera fiszkę do wyświetlenia (połączenie wyboru i wyświetlenia)
3. **Odpowiedź oceniona** - użytkownik ocenia swoją znajomość fiszki
4. **Sesja powtórek zakończona** - sesja kończy się (ręcznie lub automatycznie)

### Commands (Polecenia)
1. **Rozpocznij sesję powtórek** - inicjalizacja przez użytkownika
2. **Oceń fiszkę** - ocena znajomości przez użytkownika
3. **Zakończ sesję** - ręczne zakończenie przez użytkownika

### Actors (Aktorzy)
- **Użytkownik** - wykonuje polecenia i ocenia fiszki

*Uwaga: Usunięto aktora "System" - jego funkcje reprezentują polityki*

### Read Models (Modele Odczytu)
1. **Lista fiszek użytkownika** - dane do algorytmu wyboru
2. **Dane fiszki** - treść fiszki (przód/tył, identyfikator, ostatnia ocena)
3. **Stan sesji powtórek** - postęp sesji, ocenione fiszki, kolejność

### Aggregates (Agregaty)
- **Sesja powtórek** - główny agregat kontrolujący spójność procesu, zarządzający parametrami algorytmu spaced repetition

### Policies (Polityki)
1. **Sprawdź dostępność fiszek** - warunek wstępny przed rozpoczęciem sesji
2. **Oblicz następną fiszkę** - logika algorytmu spaced repetition z wykorzystaniem zewnętrznego systemu
3. **Zapisz postęp sesji** - automatyczna aktualizacja stanu po każdym zdarzeniu

### External Systems (Systemy Zewnętrzne)
- **Algorytm Spaced Repetition** - zewnętrzny system obliczający kolejną fiszkę do wyświetlenia

## 🔥 Hot Spots (Obszary Ryzyka)

1. **Wydajność przy dużej liczbie fiszek** 
   - Dotyczy: Polityka "Oblicz następną fiszkę"
   - Problem: Sprawdzanie wszystkich fiszek może być wolne przy tysiącach fiszek

2. **Definicja zakończenia sesji**
   - Dotyczy: Zdarzenie "Sesja powtórek zakończona"
   - Problem: Algorytm automatycznego zakończenia wciąż w rozwoju

## 🔄 Przepływ Procesu

### Główna Ścieżka
1. Użytkownik → `Rozpocznij sesję powtórek` → Agregat → `Sesja powtórek rozpoczęta`
2. Zdarzenie wyzwala politykę → `Oblicz następną fiszkę` → Zewnętrzny algorytm → Agregat → `Następna fiszka wybrana`
3. Użytkownik → `Oceń fiszkę` → Agregat → `Odpowiedź oceniona`
4. **Pętla:** Zdarzenie ponownie wyzwala politykę obliczania następnej fiszki (powrót do kroku 2)
5. **Zakończenie:** Użytkownik → `Zakończ sesję` LUB Automatyczne zakończenie → `Sesja powtórek zakończona`

### Polityka Zapisu Postępu
- Reaguje na **wszystkie zdarzenia** (rozpoczęcie, wybór fiszki, ocena, zakończenie)
- Zapewnia ciągłą aktualizację stanu sesji
- Umożliwia wznowienie sesji po nieoczekiwanym przerwaniu

## 🎓 Kluczowe Insights

1. **Cykliczność procesu**: Po ocenie fiszki system może automatycznie wybrać następną lub zakończyć sesję
2. **Zewnętrzny algorytm**: Logika spaced repetition będzie w osobnym systemie
3. **Spójność przez agregat**: Wszystkie operacje przechodzą przez agregat "Sesja powtórek"
4. **Polityki jako automatyzm**: Zastąpiły aktora "System" - bardziej precyzyjne modelowanie
5. **Ciągły zapis postępu**: Umożliwia wznowienie sesji i analizę użytkowania

## 📝 Decyzje Architektoniczne

### Połączenie Zdarzeń
- **Odrzucono**: Osobne zdarzenia "Następna fiszka wybrana" i "Fiszka wyświetlona"
- **Przyjęto**: Jedno zdarzenie "Następna fiszka wybrana" (wybór + wyświetlenie)

### Polityki vs Commands
- **Odrzucono**: Dodatkowe commands wywoływane przez polityki
- **Przyjęto**: Polityki bezpośrednio wykonują akcje (odpowiedni poziom abstrakcji)

### Typy Strzałek w Diagramie
- `-.->` Commands prowadzą do Domain Events
- `-->` Przepływ chronologiczny między Events, Events wyzwalają Policies
- `==>` Policies wywołują Aggregates

## 🚀 Następne Kroki

1. **Implementacja agregatu** "Sesja powtórek" z metodami biznesowymi
2. **Integracja zewnętrzna** z algorytmem spaced repetition
3. **Optymalizacja wydajności** dla dużych zbiorów fiszek (indeksy, cache, paginacja)
4. **Doprecyzowanie reguł** automatycznego zakończenia sesji
5. **Obsługa scenariuszy brzegowych** (brak fiszek, przerwanie sesji)

## 📊 Metryki do Śledzenia

- Czas odpowiedzi algorytmu wyboru fiszki
- Liczba fiszek obsłużonych w sesji
- Częstotliwość automatycznego vs ręcznego zakończenia sesji
- Wydajność przy różnych rozmiarach zbiorów fiszek

---

*Ten dokument stanowi dokumentację sesji Event Stormingu i punkt odniesienia dla kolejnych iteracji modelowania procesu powtórek w aplikacji Memoraid.*
