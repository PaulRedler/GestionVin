# Architecture — ProjetCaveVin / Grand Monarque 🍷

Application de **gestion de cave à vins** développée avec **.NET MAUI Blazor Hybrid**.  
MAUI = app native (Windows/Android/iOS). Blazor = UI web (HTML + C#) embarquée dans l'app.

---

## Arborescence commentée

```
ProjetCaveVin/
├── MauiProgram.cs              ← Point d'entrée + injection de dépendances (DI)
│                                  Ici on enregistre tous les Repository avec AddScoped<>
├── appsettings.json            ← Chaîne de connexion SQL Server (DefaultConnection)
├── GlobalUsing.cs              ← using globaux pour tout le projet C#
│
├── Classes/                    ── MODÈLES DE DONNÉES (simples classes C#)
│   ├── Bouteille.cs            ← Représente une bouteille (Id, Libelle, Prix, Zone…)
│   ├── Zone.cs                 ← Zone de la cave (Code, Libelle)
│   ├── Emplacement.cs          ← Case physique dans une zone
│   ├── TypeBouteille.cs        ← Type de vin (Rouge, Blanc, Rosé…)
│   ├── Utilisateur.cs          ← Compte utilisateur (Nom, Email, PasswordHash…)
│   ├── Role.cs                 ← Rôle (Sommelier, Serveur, Administrateur)
│   ├── RoleAccess.cs           ← Association rôle ↔ droits d'accès
│   ├── HistoriqueDeplacement.cs← Trace chaque déplacement de bouteille
│   ├── Origine.cs              ← Origine géographique du vin
│   └── Session.cs              ← Session utilisateur connecté
│
├── Connexion/                  ── CONNEXION BASE DE DONNÉES
│   ├── DatabaseConnexion.cs    ← ISqlConnectionFactory + SqlConnectionFactory
│   │                              Lit la connection string dans appsettings.json
│   └── Connexion.cs
│
├── Repositories/               ── ACCÈS BASE DE DONNÉES (toutes les requêtes SQL ici)
│   ├── BouteilleRepository.cs  ← GetAllBouteilles(), GetBouteilleById(), Add(), Delete()…
│   ├── ZoneRepository.cs       ← GetAllZones()…
│   ├── UtilisateurRepository.cs← GetAllUtilisateur(), AddUtilisateur()…
│   ├── RoleRepository.cs       ← GetAllRoles()…
│   ├── RARepository.cs         ← ValidateRoleAccess() (connexion par rôle + mdp)
│   └── HDRepository.cs         ← GetHistorique(), AddDeplacement()…
│
├── Helpers/                    ── UTILITAIRES
│   ├── BaseViewModel.cs        ← Classe de base (INotifyPropertyChanged si besoin)
│   ├── PasswordHelper.cs       ← HashPassword(), VerifyPassword() avec sel
│   └── RoleAccessHelper.cs     ← CheckAccess(role, page)
│
└── Components/                 ── INTERFACE BLAZOR (pages + mise en page)
    ├── Routes.razor            ← Routeur : associe chaque URL à la bonne page
    ├── _Imports.razor          ← @using globaux pour tous les fichiers .razor
    ├── Layout/
    │   ├── MainLayout.razor    ← GABARIT : structure commune à toutes les pages
    │   │                          @Body = ici s'affiche le contenu de la page
    │   ├── MainLayout.razor.css← Styles du gabarit
    │   ├── NavMenu.razor       ← Barre de navigation (liens vers les pages)
    │   └── NavMenu.razor.css
    └── Pages/                  ← UNE PAGE = UN FICHIER .razor
        ├── RoleAccess.razor    ← @page "/"            Connexion par rôle
        ├── Bottles.razor       ← @page "/bottles"     Cave à vins (grille de bouteilles)
        ├── Stats.razor         ← @page "/stats"       Statistiques & historique
        ├── Inscription.razor   ← @page "/inscription" Créer un compte utilisateur
        ├── Login.razor         ← @page "/login"       Connexion utilisateur standard
        └── Login-Admin.razor   ← @page "/login-admin" Espace administrateur
```

---

## Flux complet — exemple "afficher la cave"

```
1. Utilisateur clique "Cave à vins" dans NavMenu
       ↓
2. Routes.razor trouve la page avec @page "/bottles"
       ↓
3. MainLayout.razor enveloppe la page (header, sidebar, @Body)
       ↓
4. Bottles.razor s'affiche dans @Body
   → @inject BouteilleRepository BouteilleRepo
   → @inject ZoneRepository ZoneRepo
       ↓
5. OnInitializedAsync() appelle :
   → ZoneRepo.GetAllZones()       → requête SQL → List<Zone>
   → BouteilleRepo.GetAllBouteilles() → requête SQL → List<Bouteille>
       ↓
6. Blazor re-rend le HTML avec les données → cartes bouteilles affichées
```

---

## Flux connexion par rôle

```
@page "/"  →  RoleAccess.razor
    Sélection du rôle (Sommelier / Serveur / Administrateur)
    + saisie mot de passe
    → RARepository.ValidateRoleAccess(roleName, password)
    → SQL : vérifie hash mdp du rôle
    ↓
    Si Sommelier ou Serveur  →  Navigation.NavigateTo("/bottles")
    Si Administrateur        →  Navigation.NavigateTo("/stats")
    Si erreur                →  Message d'erreur affiché
```

---

## Directives Blazor — aide-mémoire

| Directive | Rôle |
|-----------|------|
| `@page "/route"` | Définit l'URL de la page (obligatoire sur chaque page) |
| `@inject MonRepo Repo` | Injecte un service enregistré dans `MauiProgram.cs` |
| `@bind="MaVariable"` | Liaison bidirectionnelle : champ HTML ↔ variable C# |
| `@bind:event="oninput"` | Met à jour la variable à chaque frappe (pas au blur) |
| `@onclick="MaMethode"` | Appelle une méthode C# au clic |
| `@onclick="() => Methode(param)"` | Appelle une méthode avec paramètre |
| `@if (condition) { }` | Affichage conditionnel |
| `@foreach (var x in liste) { }` | Boucle d'affichage |
| `@code { }` | Bloc C# de la page (variables, méthodes, cycle de vie) |
| `@Body` | Dans MainLayout uniquement : où la page s'insère |

---

## Cycle de vie d'un composant Blazor

```csharp
@code {
    // S'exécute une fois au chargement (synchrone)
    protected override void OnInitialized() { }

    // S'exécute une fois au chargement (asynchrone — préféré pour les appels BDD)
    protected override async Task OnInitializedAsync()
    {
        Bouteilles = BouteilleRepo.GetAllBouteilles();
    }

    // S'exécute à chaque changement de paramètre
    protected override void OnParametersSet() { }

    // Forcer le re-rendu manuellement
    StateHasChanged();
}
```

---

## Enregistrer un nouveau Repository (MauiProgram.cs)

```csharp
// 1. Créer la classe dans Repositories/MonRepository.cs
// 2. L'enregistrer dans MauiProgram.cs :
builder.Services.AddScoped<MonRepository>();

// 3. L'injecter dans une page :
@inject MonRepository MonRepo
```
