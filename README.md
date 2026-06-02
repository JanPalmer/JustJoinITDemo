# JustJoinITDemo

## Treść zadania

Przygotuj prosty system składający się z backendu oraz frontendowego UI, który umożliwia użytkownikowi wysyłanie wielu promptów do przetworzenia oraz śledzenie ich statusu.

Backend powinien być napisany w C# i udostępniać API do dodawania promptów oraz pobierania ich aktualnych stanów. Każdy prompt powinien zostać zapisany w bazie danych.

Oddzielny proces przetwarzający powinien obsługiwać zadania i wykonywać je przy użyciu jednej z dostępnych bibliotek do komunikacji z modelami językowymi, np. z lokalnym modelem lub usługą zewnętrzną. Każde zadanie musi przechodzić przez stany: oczekujące, przetwarzane, zakończone lub nieudane.

Frontend w React lub Next.js ma umożliwiać dodanie wielu promptów oraz wyświetlać listę wszystkich z aktualnymi statusami i wynikami. Odświeżanie może odbywać się za pomocą prostego pollingu.

Mile widziana jest orkiestracja projektu, tak aby cały system dało się łatwo uruchomić jednym poleceniem, oraz dołączenie krótkiej instrukcji, mini dokumentacji, wyjaśniającej jak uruchomić środowisko i poszczególne komponenty.

## Opis

System dzieli się na dwa projekty - Backend oraz Frontend. Frontend zawiera stronę internetową napisaną w Next.js, natomiast Backend implementuje serwer z API, bazę danych oraz proces przetwarzający prompty (Worker). 

Aplikacja pozwala na wysyłanie pojedynczych promptów do wybranych modeli AI. Prompt jest wysyłany do serwisu Web, który wstawia je do bazy, po czym Worker przekazuje je dalej do wybranego modelu oraz zapisuje wynik zapytania. Aplikacja kliencka pozwala na monitorowanie statusu zapytania (poprzez serwis Web), wyświetlając kolejno statusy "Pending" -> "Processing" -> "Completed" lub "Failed".

Każdy prompt jest traktowany jako osobny czat - między kolejnymi wiadomościami nie jest przekazywana historia czatu.

Udostępnione są 3 modele AI oraz jeden 'model testowy' ('fake-model'). Aplikacja korzysta z API Google oraz Groq, pozwalając na korzystanie z następujących modeli:
- gemini-3.5-flash (Google)
- gemini-3.1-flash-lite (Google)
- llama-3.3-70b-versatile (Groq)
Modele te korzystają z API ww. serwisów, przez co wymagają użycia kluczy API. Własne klucze można dodać w pliku 'appsettings.json', zamieniając wartość "apikey" na wygenerowany klucz dla odpowiedniego serwisu. W przeciwnym razie zwrócony zostanie błąd połączenia się z modelem.

Model 'fake-model' (FakeModel) to funkcja niewymagająca kluczy API. Zwraca ona wiadomość zawierającą nazwę modelu oraz tekst z wpisanego prompta.

## Instalacja

Dołączone zostały pliki umożliwiające włączenie systemu poprzez Docker:
- W konsoli należy przejść do folderu zawierającego plik docker-compose.yml
- Wywołać 'docker compose up --build'
- Zaczekać aż proces 'backend' zacznie pokazywać co jakiś czas pobieranie danych o promptach/modelach z bazy, oraz proces frontend udostępni swój adres strony
- Frontend powinien być możliwy do sprawdzenia poprzez adres 'http://localhost:3000'

Jeśli chcemy włączyć projekt bez użycia Dockera, należy:
- Włączyć serwis JustJoinITBackend.Web (np. poprzez Visual Studio)
- Oddzielnie włączyć proces JustJoinITBackend.Worker (np. poprzez dotnet run -p:SolutionDir="...\Projekt\JustJoinITBackend", gdzie ... to ścieżka do folderu zawierającego plik .sln projektu backendowego)
- Zbudować projekt Next.js poprzez wywołanie 'npm run dev' poprzez wiersz poleceń, znajdując się w folderze zawierającym pliki aplikacji (wewnątrz justjoinit-frontend)

Baza danych SQLite jest tworzona w folderze Solution, tak aby oba projekty .Web oraz .Worker miały do niej dostęp. Przy uruchamianiu Workera poprzez konsolę należy jednak zawrzeć parametr wskazujący na ten folder, dlatego -p:SolutionDir=...