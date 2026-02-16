# How to Run the MTG Card Lookup App

You need **two terminals** running at the same time.

---

## Terminal 1 — API (ASP.NET Core)

```
Directory: d:\SoftwareProjects\Abstract-Factory
```

```powershell
dotnet run --project src/AbstractFactory.Api
```

- Runs on **http://localhost:5000**
- Swagger UI at **http://localhost:5000**

---

## Terminal 2 — Frontend (Next.js)

```
Directory: d:\SoftwareProjects\Abstract-Factory\src\frontend
```

```powershell
npm install   # only needed the first time
npm run dev
```

- Runs on **http://localhost:3000**

---

## Quick Start (copy-paste)

**Terminal 1:**
```powershell
cd d:\SoftwareProjects\Abstract-Factory
dotnet run --project src/AbstractFactory.Api
```

**Terminal 2:**
```powershell
cd d:\SoftwareProjects\Abstract-Factory\src\frontend
npm run dev
```

---

## Build Check

To verify everything compiles before running:

```
Directory: d:\SoftwareProjects\Abstract-Factory
```

```powershell
dotnet build
```

---

## URLs

| Service  | URL                      | Purpose              |
|----------|--------------------------|----------------------|
| Frontend | http://localhost:3000     | MTG Card Lookup UI   |
| API      | http://localhost:5000     | Swagger UI / API     |
