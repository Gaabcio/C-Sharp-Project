# System Obsługi Parkingu

## 📝 Opis aplikacji

System Obsługi Parkingu to aplikacja desktopowa napisana w C# z wykorzystaniem WPF, służąca do zarządzania miejscami parkingowymi w obiektach komercyjnych. Aplikacja umożliwia:

- Logowanie użytkowników (Administrator)
- Zarządzanie pojazdami (dodawanie, edycja, usuwanie)
- Parkowanie i wyparkowanie pojazdów
- Wyszukiwanie zaparkowanych pojazdów
- Podgląd aktualnego stanu parkingu

## 🏗️ Technologie

- **Język:** C# (.NET)
- **Framework UI:** WPF (Windows Presentation Foundation)
- **ORM:** Entity Framework Core
- **Baza danych:** MySQL
- **Walidacja:** FluentValidation
- **Haszowanie haseł:** BCrypt.NET
- **Dependency Injection:** Microsoft.Extensions.DependencyInjection

## 🗄️ Struktura bazy danych

### Tabele:

1. **Users** - użytkownicy systemu
   - Id (PK), Username, PasswordHash, Email, FirstName, LastName, CreatedAt, LastLoginAt, IsActive

2. **VehicleTypes** - typy pojazdów
   - Id (PK), Name, Description, SpacesRequired, AllowedRows

3. **Vehicles** - pojazdy użytkowników
   - Id (PK), LicensePlate, UserId (FK), VehicleTypeId (FK), Brand, Model, Color, Year, CreatedAt

4. **ParkingSpaces** - miejsca parkingowe
   - Id (PK), Row, Column, IsOccupied, CreatedAt

5. **ParkingReservations** - rezerwacje miejsc parkingowych
   - Id (PK), VehicleId (FK), UserId (FK), ParkingSpaceId (FK), StartTime, IsActive

### Layout parkingu:
- **Rząd 0:** Motocykle (1 miejsce)
- **Rzędy 1-2:** Samochody (2 miejsca)
- **Rzędy 3-6:** Autobusy (4 miejsca)
- **Kolumny 0-9:** 10 kolumn


## 🚀 Instalacja i konfiguracja

### 1. Przygotowanie środowiska

```bash
# Zainstaluj .NET 8.0 SDK
# Pobierz z: https://dotnet.microsoft.com/download

# Zainstaluj XAMPP
# Pobierz z: https://www.apachefriends.org/download.html
```

### 2. Konfiguracja bazy danych

1. **Uruchom XAMPP Control Panel**
2. **Uruchom MySQL** (kliknij "Start" przy MySQL)

### 3. Konfiguracja aplikacji

1. **Sklonuj repozytorium:**
   ```bash
   git clone https://github.com/Gaabcio/C-Sharp-Project.git
   cd C-Sharp-Project
   ```

2. **Sprawdź plik appsettings.json:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ParkingManagementSystem;User=root;Password=;CharSet=utf8mb4;"
     },
     "ParkingSettings": {
       "MaxColumns": 10,
       "MaxRows": 7,
       "DefaultUser": "admin",
       "DefaultPassword": "admin"
     }
   }
   ```

3. **Przywróć pakiety NuGet:**
   ```bash
   dotnet restore
   ```

4. **Zbuduj aplikację:**
   ```bash
   dotnet build
   ```

5. **Uruchom aplikację:**
   ```bash
   dotnet run
   ```

### 4. Pierwszy uruch

Przy pierwszym uruchomieniu aplikacja automatycznie:
- Utworzy strukturę bazy danych
- Zainicjalizuje podstawowe dane
- Utworzy domyślne konto administratora

**Domyślne dane logowania:**
- **Login:** admin
- **Hasło:** admin

## 📖 Instrukcja obsługi

### Logowanie

1. **Uruchom aplikację**
2. **Wprowadź dane logowania:**
   - Login: `admin`
   - Hasło: `admin`
3. **Kliknij "Zaloguj"**

### Parkowanie pojazdu

1. **Na głównym ekranie wybierz typ pojazdu** (Motocykl/Samochód/Autobus)
2. **Wprowadź numer rejestracyjny** (np. WX12345)
3. **Wybierz kolumnę** z listy dostępnych
4. **Kliknij "Zaparkuj pojazd"**

**Uwaga:** Jeśli pojazd nie istnieje w systemie, zostanie automatycznie dodany.

### Używanie istniejącego pojazdu

1. **Wybierz pojazd z listy "Wybierz istniejący pojazd"**
2. **Pola zostaną automatycznie wypełnione**
3. **Wybierz kolumnę i zaparkuj**

### Wyparkowanie pojazdu

1. **Kliknij "Wyparkuj pojazd"**
2. **Wybierz pojazd z listy zaparkowanych**
3. **Sprawdź szczegóły pojazdu**
4. **Kliknij "Wyparkuj"**

**System automatycznie obliczy opłatę za parkowanie.**

### Wyszukiwanie pojazdu

1. **Kliknij "Wyszukaj pojazd"**
2. **Wprowadź numer rejestracyjny**
3. **Kliknij "Wyszukaj" lub naciśnij Enter**
4. **Sprawdź lokalizację pojazdu**

### Zarządzanie pojazdami

1. **Kliknij "Zarządzaj pojazdami"**
2. **Lista Twoich pojazdów:**
   - Przeglądaj wszystkie swoje pojazdy
   - Kliknij na pojazd aby go edytować
3. **Dodawanie nowego pojazdu:**
   - Wypełnij formularz po prawej stronie
   - Kliknij "Zapisz"
4. **Edytowanie pojazdu:**
   - Wybierz pojazd z listy
   - Zmień dane w formularzu
   - Kliknij "Aktualizuj"
5. **Usuwanie pojazdu:**
   - Wybierz pojazd z listy
   - Kliknij "Usuń"
   - Potwierdź usunięcie

**Uwaga:** Nie można usunąć pojazdu, który jest aktualnie zaparkowany.

### Podgląd stanu parkingu

**Główny ekran pokazuje:**
- **Siatka parkingu** - zielone miejsca (wolne), czerwone (zajęte)
- **Lista zaparkowanych pojazdów** - z numerami rejestracyjnymi i kolumnami
- **Podpowiedzi** - najedź na miejsce aby zobaczyć szczegóły

**Legenda:**
- 🟢 **Zielone** - miejsce wolne
- 🔴 **Czerwone** - miejsce zajęte
- **M** - miejsca dla motocykli (rząd 0)
- **S** - miejsca dla samochodów (rzędy 1-2)
- **A** - miejsca dla autobusów (rzędy 3-6)

## 🚨 Przypadki brzegowe i rozwiązywanie problemów

### Problem: Błąd połączenia z bazą danych

**Rozwiązanie:**
1. Sprawdź czy MySQL jest uruchomiony w XAMPP
2. Zweryfikuj connection string w `appsettings.json`
3. Upewnij się, że baza `ParkingManagementSystem` istnieje

### Problem: Nie można zaparkować pojazdu

**Możliwe przyczyny:**
- Brak wolnych miejsc dla danego typu pojazdu
- Pojazd już zaparkowany
- Wybrana kolumna zajęta

**Rozwiązanie:**
1. Sprawdź status parkingu na głównym ekranie
2. Wybierz inną kolumnę
3. Sprawdź czy pojazd nie jest już zaparkowany

### Problem: Nie można usunąć pojazdu

**Przyczyna:** Pojazd jest aktualnie zaparkowany

**Rozwiązanie:**
1. Najpierw wyparkuj pojazd
2. Następnie usuń go z zarządzania pojazdami

### Problem: Nieprawidłowy numer rejestracyjny

**Wymagania:**
- Tylko wielkie litery, cyfry i spacje
- Długość 2-20 znaków
- Unikalność w systemie

### Problem: Nie można się zalogować

**Rozwiązanie:**
1. Sprawdź domyślne dane: admin/admin
2. Upewnij się, że caps lock jest wyłączony
3. Sprawdź połączenie z bazą danych

## 👥 Autorzy

- **Gabriel Łasicki** (Gaabcio) - deweloper główny

## 📄 Licencja

Projekt edukacyjny - wszystkie prawa zastrzeżone.

---

*Ostatnia aktualizacja: 2025-06-04*