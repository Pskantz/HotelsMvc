# HotelsApp

HotelsApp är en webbapplikation för att hantera hotellbokningar. Användare kan söka efter hotell, boka rum, och administratörer kan lägga till, redigera och ta bort hotell.

---

## 🚀 Funktioner

### För användare:
- **Sök hotell:** Filtrera hotell efter dina behov via sökfältet.
- **Boka rum:** Inloggade användare kan boka hotellrum direkt.
- **Visa bokningar:** Se och hantera dina bokningar via "Reservations"-sidan.

### För administratörer:
- **Lägg till hotell:** Lägg till nya hotell i systemet.
- **Redigera hotell:** Uppdatera hotellinformation.
- **Ta bort hotell:** Radera hotell från systemet.

---

## 🛠️ Installation

Följ dessa steg för att installera och köra projektet lokalt:

### Förutsättningar
- [**.NET 9.0 SDK**](https://dotnet.microsoft.com/download/dotnet/9.0) installerat.
- [**PostgreSQL**](https://www.postgresql.org/download/) installerat och konfigurerat.

### 1⃣ Klona projektet
```bash
git clone https://github.com/Pskantz/HotelsMvc.git
cd HotelsMvc/HotelsApp
```

### 2⃣ Konfigurera databasanslutning
Uppdatera `appsettings.json` med din PostgreSQL-anslutningssträng:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=hotelsappdb;Username=din_användare;Password=din_lösenord"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 3⃣ Skapa och migrera databasen
Kör följande kommando:
```bash
dotnet ef database update
```

### 4⃣ Bygg och kör applikationen
Bygg och starta applikationen:
```bash
dotnet run
```
Applikationen är nu tillgänglig på:
- https://localhost:7169
- http://localhost:5135

---

## 🧑‍💻 Användning

### 🔑 Inloggning och registrering
- **Registrera:** Klicka på "Register" i menyn och fyll i formuläret.
- **Logga in:** Klicka på "Login" i menyn och fyll i dina uppgifter.

### 📋 Administratörskonto
Ett administratörskonto skapas automatiskt vid första körningen:
- **E-post:** `admin@example.com`
- **Lösenord:** `Admin123!`

---

## 🤝 Bidra

Vill du bidra till projektet? Följ dessa steg:
1. Skapa en **fork** av projektet.
2. Gör dina ändringar i en egen branch.
3. Skicka in en **pull request** eller öppna en **issue**.

---

## 📜 Licens

Detta projekt är licensierat under MIT-licensen. Läs mer i [LICENSE](LICENSE)-filen.

---

Tack för att du använder HotelsApp! ✨

