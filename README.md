# KingdomHospital - Système de Gestion Hospitalière

Ce projet est une API REST de gestion hospitalière développée avec **ASP.NET Core**. Il suit les principes de la **Clean Architecture** pour assurer une séparation claire des responsabilités, la testabilité et la maintenance du code.

## 📋 Description

L'application permet de gérer les différents aspects d'un hôpital, notamment :
*   La gestion des **Patients**.
*   La gestion des **Médecins** et leurs **Spécialités**.
*   La planification et le suivi des **Consultations**.
*   La gestion des **Médicaments** et la création d'**Ordonnances**.

## 🏗ure Architecture du Projet

Le projet est structuré selon les couches suivantes :

### 1. 📂 Domain (`/Domain`)
C'est le cœur du projet. Il contient les entités métier et ne dépend d'aucune autre couche.
*   **Entities** : Les classes représentant les objets du domaine (ex: `Patient`, `Doctor`, `Consultation`, `Ordonnance`).

### 2. 📂 Application (`/Application`)
Cette couche contient la logique applicative et fait le lien entre le domaine et l'extérieur.
*   **DTOs (Data Transfer Objects)** : Objets utilisés pour transférer les données entre l'API et le client.
*   **Mappers** : Logique de transformation entre les Entités et les DTOs.
*   **Services** : Interfaces et logique métier.
*   **Repositories (Interfaces)** : Contrats définissant l'accès aux données.

### 3. 📂 Infrastructure (`/Infrastructure`)
Cette couche gère l'accès aux données et l'implémentation des interfaces techniques.
*   **KingdomHospitalDbContext** : Le contexte de base de données Entity Framework Core.
*   **Repositories (Implémentations)** : Implémentation concrète de l'accès aux données.
*   **Migrations** : Historique des changements de schéma de base de données.
*   **Configurations** : Configuration des mappings Entity Framework.

### 4. 📂 Presentation (`/Presentation`)
Le point d'entrée de l'API.
*   **Controllers** : Contrôleurs API REST (ex: `DoctorsController`, `PatientsController`) qui exposent les endpoints HTTP.

## 🚀 Fonctionnalités Principales (Entités)

*   **Doctor** : Gestion des médecins.
*   **Specialty** : Spécialités médicales.
*   **Patient** : Dossiers patients.
*   **Consultation** : Rendez-vous et visites médicales.
*   **Medicament** : Référentiel des médicaments.
*   **Ordonnance** : Prescriptions médicales associées aux consultations.

## 🛠 Technologies Utilisées

*   .NET Core / ASP.NET Core
*   Entity Framework Core (ORM)
*   SQLite (Base de données utilisée par défaut : `KingdomHospital.db`)

## ▶️ Comment lancer le projet

1.  Assurez-vous d'avoir le SDK .NET installé.
2.  Ouvrez un terminal dans le dossier racine du projet.
3.  Lancez la commande :
    ```bash
    dotnet run
    ```
4.  L'API sera accessible (par défaut) sur `https://localhost:7198` ou `http://localhost:5037`.
