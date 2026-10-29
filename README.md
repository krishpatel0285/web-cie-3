# AutoHaven — Premium Car Showroom

AutoHaven is a student-friendly ASP.NET Core MVC showroom website with a refined automotive editorial design, responsive layout, sample vehicle catalogue, search/filter/sort, vehicle detail pages, account registration/login, and showroom visit requests.

## Features
- Premium dark-olive, cream and brass visual identity
- Responsive landing page and curated vehicle cards
- Eight sample cars with external image URLs
- Search by make/model/category, category and fuel filters, price/year sorting
- Vehicle details and specifications
- Register/login with salted PBKDF2 password hashes and session-based sign-in
- Showroom visit request form with validation and confirmation reference
- Local JSON persistence; no SQL server or third-party NuGet packages required
- `.gitignore` excludes build output and local data

## Requirements
- .NET 10 SDK
- Internet access for Google Fonts and remote Unsplash images (layout still works without them)

## Run locally
Open the folder containing `AutoHaven.csproj` in VS Code, then run:

```bash
dotnet restore
dotnet build
dotnet run
```

Open the `http://localhost:...` or `https://localhost:...` URL shown in the terminal.

The app creates `AutoHaven_Data/` on first run, containing `cars.json`, `users.json`, and `inquiries.json`. These files are local demo data and are intentionally excluded from Git. Deleting this folder resets the demo data and removes local demo accounts/requests.

## Important demo notes
- This is an educational project, not a production dealership system.
- Accounts and inquiries are stored in local JSON files on the machine running the app; multiple hosted instances will not share data.
- No payments, inventory management, admin dashboard, email delivery, or real appointment confirmation is implemented. Visit requests are saved and shown a reference number; staff confirmation is simulated.
- Never commit real passwords, customer data, API keys, or secrets.
- Vehicle imagery is loaded from Unsplash URLs; replace with locally licensed images if required for an offline presentation.
