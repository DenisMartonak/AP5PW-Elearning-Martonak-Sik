# PW.ELearning

E-learningová webová aplikace (ASP.NET Core MVC, EF Core, MySQL).
Návrh aplikace (funkční požadavky, ER diagram, architektura): [docs/NAVRH.md](docs/NAVRH.md).

## Spuštění

0. Spusťte MySQL v Dockeru (heslo `rootroot`, databáze `Elearning`, port 3306):

   ```bash
   docker compose up -d
   ```

1. Nastavte connection string k MySQL (ukládá se mimo repozitář do user-secrets):

   ```bash
   dotnet user-secrets set "ConnectionStrings:MySQL" "server=localhost;port=3306;database=Elearning;user=root;password=<heslo>" --project PW.ELearning.Web
   ```

2. Vytvořte / aktualizujte databázi:

   ```bash
   dotnet ef database update -p PW.Elearning.Infrastructure -s PW.ELearning.Web
   ```

3. Spusťte aplikaci:

   ```bash
   dotnet run --project PW.ELearning.Web
   ```
