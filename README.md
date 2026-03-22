# DigMap: Explorer & Archaeology Journal

DigMap is a full-stack web application designed for treasure hunters, archaeologists, and metal detecting enthusiasts

## Tech Stack

**Backend:**
* C# / .NET 10
* ASP.NET Core Web API
* Entity Framework Core (MS SQL Server)
* JWT (JSON Web Tokens) Authentication

**Frontend:**
* Vanilla JavaScript (ES6+), Fetch API
* HTML5 / CSS3 (Bootstrap 5)
* Leaflet.js (Interactive Mapping & Geolocation)

## Core Features

* **Interactive Map:** Users can log the exact coordinates of their finds either via device GPS or by placing a custom marker directly on the interactive Leaflet map.
* **Polymorphic Entities:** Dynamic form rendering and backend processing based on the specific type of find (Coins require year/metal/denomination, Artifacts require era/material/class).
* **Secure Registry:** Private, token-secured dashboard to view, edit, and manage the history of expeditions.

## Local Setup Instructions

### Prerequisites
* .NET 10 SDK
* MS SQL Server (LocalDB or full instance)
* Any modern web browser

### Running the Backend
1. Clone the repository.
2. Open the solution in Visual Studio or your preferred IDE.
3. Update the `DefaultConnection` string in `appsettings.json` if necessary.
4. Open the Package Manager Console or Terminal and apply database migrations:
   `dotnet ef database update`
5. Run the API project.

### Running the Frontend
1. Navigate to the client folder containing `index.html`.
2. Open `index.html` in your browser.
3. Ensure the `API_BASE` variable in `script.js` matches your local running backend URL.
