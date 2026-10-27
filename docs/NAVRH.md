# Návrh aplikace – E-learning.
## 1. Role uživatelů

| Role | Popis |
|---|---|
| **Admin** | Spravuje uživatele a má přístup ke všem kurzům. |
| **Teacher** (učitel) | Vytváří a spravuje své kurzy, lekce a úkoly, hodnotí odevzdání. |
| **Student** | Zapisuje se do kurzů, prochází lekce, odevzdává úkoly. |
| **Nepřihlášený návštěvník** | Vidí pouze veřejný seznam kurzů. |

## 2. Funkční požadavky

### Diagram případů užití

Přerušovaná šipka `«je»` znamená dědění role.

```mermaid
flowchart LR
    Guest["🧑 Návštěvník"]
    User["🧑 Přihlášený uživatel"]
    Student["🧑 Student"]
    Teacher["🧑 Učitel"]
    Admin["🧑 Admin"]

    Student -. «je» .-> User
    Teacher -. «je» .-> User
    Admin -. «je» .-> User

    subgraph SYS["E-learning"]
        direction TB
        subgraph AUTH["Uživatelé a autentizace"]
            UC01(["FR-01 Registrace"])
            UC02(["FR-02 Přihlášení / odhlášení"])
            UC05(["FR-05 Správa uživatelů a rolí"])
        end
        subgraph COURSES["Kurzy a lekce"]
            UC06(["FR-06 Zobrazit seznam kurzů"])
            UC07(["FR-07 Vytvořit kurz"])
            UC08(["FR-08 Upravit / smazat kurz"])
            UC10(["FR-10 Spravovat lekce"])
            UC12(["FR-12 Studovat obsah lekcí"])
        end
        subgraph ENROLL["Zápis do kurzu"]
            UC13(["FR-13 Zapsat se / odhlásit z kurzu"])
            UC15(["FR-15 Moje kurzy"])
            UC16(["FR-16 Seznam zapsaných studentů"])
        end
        subgraph TASKS["Úkoly a odevzdání"]
            UC17(["FR-17 Vytvořit úkol"])
            UC18(["FR-18 Odevzdat / upravit odevzdání"])
            UC19(["FR-19 Ohodnotit odevzdání"])
            UC20(["FR-20 Zobrazit hodnocení"])
        end
    end

    Guest --- UC01
    Guest --- UC06
    User --- UC02
    User --- UC06

    Student --- UC13
    Student --- UC15
    Student --- UC12
    Student --- UC18
    Student --- UC20

    Teacher --- UC07
    Teacher --- UC08
    Teacher --- UC10
    Teacher --- UC12
    Teacher --- UC16
    Teacher --- UC17
    Teacher --- UC19

    Admin --- UC05
    Admin --- UC08
    Admin --- UC12

    UC18 -. «include» ověření zápisu .-> UC13
```

### Uživatelé a autentizace
- **FR-01** Návštěvník se může zaregistrovat (uživatelské jméno, jméno, příjmení, e-mail, heslo). Nový účet má roli Student.
- **FR-02** Uživatel se může přihlásit a odhlásit.
- **FR-03** Uživatelské jméno i e-mail jsou v systému unikátní.
- **FR-04** Heslo se ukládá pouze jako hash, nikdy v čitelné podobě.
- **FR-05** Admin může zobrazit seznam uživatelů, měnit jejich role a mazat je.

### Kurzy
- **FR-06** Kdokoli může zobrazit seznam kurzů (název, popis, vyučující).
- **FR-07** Učitel může vytvořit kurz; stává se jeho vyučujícím.
- **FR-08** Učitel může upravit a smazat svůj kurz (Admin libovolný kurz).
- **FR-09** Učitele, který vyučuje alespoň jeden kurz, nelze smazat (nejdříve je nutné kurzy smazat nebo předat).

### Lekce
- **FR-10** Učitel může do svého kurzu přidávat, upravovat a mazat lekce.
- **FR-11** Lekce mají pořadí; v detailu kurzu se zobrazují seřazené.
- **FR-12** Obsah lekcí vidí pouze studenti zapsaní v kurzu, vyučující a Admin.

### Zápis do kurzu
- **FR-13** Student se může zapsat do kurzu a odhlásit se z něj.
- **FR-14** Student může být do jednoho kurzu zapsán nejvýše jednou.
- **FR-15** Student vidí seznam kurzů, do kterých je zapsán.
- **FR-16** Učitel vidí seznam studentů zapsaných ve svém kurzu.

### Úkoly a odevzdání
- **FR-17** Učitel může v kurzu vytvořit úkol (název, zadání, termín odevzdání, maximální počet bodů).
- **FR-18** Zapsaný student může úkol odevzdat; ke každému úkolu má nejvýše jedno odevzdání, které může do termínu upravit.
- **FR-19** Učitel vidí odevzdání ke svým úkolům a může je ohodnotit (body 0 až max. body, slovní zpětná vazba).
- **FR-20** Student vidí hodnocení a zpětnou vazbu ke svým odevzdáním.

### Mazání (kaskády)
- **FR-21** Smazáním kurzu se smažou jeho lekce, úkoly, odevzdání a zápisy.
- **FR-22** Smazáním studenta se smažou jeho zápisy a odevzdání.

## 3. Návrh entit a databáze

### Doménový model (entity v kódu)

```mermaid
classDiagram
    direction LR

    class BaseEntity {
        <<abstract>>
        +int Id
        +DateTime CreatedAt
    }
    class IUser {
        <<interface>>
        +string Username
        +string FirstName
        +string LastName
        +string Email
        +string PasswordHash
    }
    class Role {
        <<enumeration>>
        Admin
        Teacher
        Student
    }
    class User {
        +string Username
        +string FirstName
        +string LastName
        +string Email
        +string PasswordHash
        +Role Role
        +ICollection~Course~ TaughtCourses
        +ICollection~Enrollment~ Enrollments
        +ICollection~Submission~ Submissions
    }
    class Course {
        +string Title
        +string? Description
        +int TeacherId
        +User Teacher
        +ICollection~Lesson~ Lessons
        +ICollection~Enrollment~ Enrollments
        +ICollection~Assignment~ Assignments
    }
    class Lesson {
        +string Title
        +string? Content
        +int Order
        +int CourseId
        +Course Course
    }
    class Enrollment {
        +int StudentId
        +User Student
        +int CourseId
        +Course Course
    }
    class Assignment {
        +string Title
        +string? Description
        +DateTime? DueDate
        +int MaxPoints
        +int CourseId
        +Course Course
        +ICollection~Submission~ Submissions
    }
    class Submission {
        +string? Content
        +int? Points
        +string? Feedback
        +int AssignmentId
        +Assignment Assignment
        +int StudentId
        +User Student
    }

    BaseEntity <|-- User
    BaseEntity <|-- Course
    BaseEntity <|-- Lesson
    BaseEntity <|-- Enrollment
    BaseEntity <|-- Assignment
    BaseEntity <|-- Submission
    IUser <|.. User
    User --> Role

    User "1" -- "0..*" Course : vyučuje
    User "1" -- "0..*" Enrollment : je zapsán
    Course "1" *-- "0..*" Enrollment
    Course "1" *-- "0..*" Lesson
    Course "1" *-- "0..*" Assignment
    Assignment "1" *-- "0..*" Submission
    User "1" -- "0..*" Submission : odevzdává
```

### ER diagram

```mermaid
erDiagram
    Users ||--o{ Courses : "vyučuje (TeacherId)"
    Users ||--o{ Enrollments : "je zapsán (StudentId)"
    Courses ||--o{ Enrollments : "má zápisy"
    Courses ||--o{ Lessons : "obsahuje"
    Courses ||--o{ Assignments : "obsahuje"
    Assignments ||--o{ Submissions : "má odevzdání"
    Users ||--o{ Submissions : "odevzdává (StudentId)"

    Users {
        int Id PK
        varchar50 Username UK
        varchar100 FirstName
        varchar100 LastName
        varchar255 Email UK
        varchar255 PasswordHash
        varchar20 Role "Admin | Teacher | Student"
        datetime CreatedAt
    }
    Courses {
        int Id PK
        varchar200 Title
        varchar2000 Description "NULL"
        int TeacherId FK
        datetime CreatedAt
    }
    Lessons {
        int Id PK
        varchar200 Title
        text Content "NULL"
        int Order
        int CourseId FK
        datetime CreatedAt
    }
    Enrollments {
        int Id PK
        int StudentId FK
        int CourseId FK
        datetime CreatedAt
    }
    Assignments {
        int Id PK
        varchar200 Title
        varchar4000 Description "NULL"
        datetime DueDate "NULL"
        int MaxPoints
        int CourseId FK
        datetime CreatedAt
    }
    Submissions {
        int Id PK
        text Content "NULL"
        int Points "NULL"
        varchar2000 Feedback "NULL"
        int AssignmentId FK
        int StudentId FK
        datetime CreatedAt
    }
```

### Popis entit

Všechny entity dědí z `BaseEntity` (`Id` – primární klíč, `CreatedAt` – čas vytvoření v UTC).

| Entita | Význam | Klíčová pravidla |
|---|---|---|
| **User** | Uživatel systému s rolí Admin / Teacher / Student. | `Username` a `Email` unikátní; `Role` uložena jako text. |
| **Course** | Kurz vedený jedním učitelem. | FK `TeacherId → Users`, `ON DELETE RESTRICT` (FR-09). |
| **Lesson** | Výuková lekce v kurzu. | FK `CourseId → Courses`, kaskádové mazání; `Order` určuje pořadí. |
| **Enrollment** | Zápis studenta do kurzu – vazební tabulka M:N mezi `Users` a `Courses`. | Unikátní dvojice `(StudentId, CourseId)` (FR-14); kaskáda z obou stran. |
| **Assignment** | Úkol v kurzu. | FK `CourseId → Courses`, kaskádové mazání. |
| **Submission** | Odevzdání úkolu studentem, včetně hodnocení. | Unikátní dvojice `(AssignmentId, StudentId)` (FR-18); `Points`/`Feedback` vyplní učitel. |

### Vazby

| Vazba | Kardinalita | Realizace |
|---|---|---|
| Učitel – Kurz | 1 : N | `Courses.TeacherId` |
| Student – Kurz | M : N | přes `Enrollments` |
| Kurz – Lekce | 1 : N | `Lessons.CourseId` |
| Kurz – Úkol | 1 : N | `Assignments.CourseId` |
| Úkol – Odevzdání | 1 : N | `Submissions.AssignmentId` |
| Student – Odevzdání | 1 : N | `Submissions.StudentId` |

## 4. Architektura

| Projekt | Vrstva | Obsah |
|---|---|---|
| `PW.Elearning.Domain` | Doménová | Entity, enum `Role`, rozhraní `IRepository<T>`, `ICourseRepository`, `IUnitOfWork`. Nezávisí na ničem. |
| `PW.Elearning.Application` | Aplikační | Služby a DTO (např. `CourseService`, `CourseDto`). Závisí pouze na Domain. |
| `PW.Elearning.Infrastructure` | Infrastruktura | EF Core `ElearningDbContext`, Fluent API konfigurace, migrace, implementace `Repository<T>` a specifických repozitářů. |
| `PW.ELearning.Web` | Prezentační | ASP.NET Core MVC – controllery a views; skládá vrstvy pomocí DI. |
