
# 🛠️ Guide d'Installation et de Configuration

Ce guide détaille la procédure pour installer l'application et configurer la base de données à partir des fichiers fournis.

---

## 📋 Prérequis

Avant de commencer, assurez-vous que les éléments suivants sont installés sur votre machine :

- **SQL Server Express Basic** : [Télécharger ici](https://www.microsoft.com/sql-server/sql-server-downloads)
- **SQL Server Management Studio (SSMS)** : [Télécharger ici](https://aka.ms/ssms)

---

## ☁️ 1. Récupération des fichiers

Accédez au dossier partagé via le lien suivant :  
🔗 **[OneDrive - Lycée Fulbert](https://lyceefulbert-my.sharepoint.com/:f:/g/personal/paul_redler_lyceefulbert_fr/EtiEjREviJFNkLEwAhEntPQBrCCU2BlnoAiEan7Hyqhv3Q?e=6ordKP)**

Téléchargez les deux fichiers suivants :
- `GestVino.zip` (.exe de l’application)
- `cave.bak` (Sauvegarde de la base de données)

---



## 🧩 3. Installation de l’application

1.  **Décompressez** le fichier `GestVino.zip`.
2.  Une fois installé, localisez le fichier `.exe`.

-----

## 🗄️ 4. Restauration de la base de données

1.  Copiez le fichier `cave.bak` dans le dossier de backup SQL pour garantir les droits d'accès :
    `C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup`
2.  Ouvrez **SQL Server Management Studio (SSMS)**.
3.  Connectez-vous à votre instance (ex: `.\SQLEXPRESS`).
4.  Clic droit sur **Bases de données** \> **Restaurer la base de données...**
5.  Sélectionnez **Support**, cliquez sur `...` puis **Ajouter**.
6.  Sélectionnez le fichier `cave.bak` copié précédemment.
7.  Cliquez sur **OK** pour lancer la restauration.

-----

## 🚀 5. Test de l'application

1.  Lancez l'application installée.
2.  Allez dans l'espace admin et entrez le mot de passe **admin123**.
3.  Cliquez sur **Connexion**.
4.  Utilisez les identifiants suivants :
      - **Email :** `paul@cave.fr`
      - **Mot de passe :** `popo123`


**Fonctionnalités :**

  - 📉 **Stats :** Cliquez sur l'image en bas à droite.
  - ⚙️ **Gestion :** Cliquez sur l'image en bas à gauche.

<!-- end list -->

```
