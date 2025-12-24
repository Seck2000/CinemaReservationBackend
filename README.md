# 🎬 Cinema Reservation Backend – TP2

Ce projet correspond au **Travail Pratique II** du cours **Technologies du commerce électronique**.  
Il s’agit de la partie **back-end** d’un système de réservation de cinéma en ligne développé avec une **architecture microservices** en ASP.NET Core Web API.

L’objectif du travail est de démontrer la mise en place de microservices, d’une authentification sécurisée, d’un paiement électronique et d’une documentation Swagger unifiée.

---

## 🧩 Microservices du projet

Le système est composé de **six microservices indépendants** :

- **API Gateway** : point d’entrée unique de l’application
- **AuthentificationService** : inscription, connexion et gestion des rôles (JWT)
- **FilmsService** : gestion des films, séances et tarifs
- **SalleService** : gestion des salles et des sièges
- **ReservationService** : création et gestion des réservations
- **PaiementService** : gestion des paiements avec Stripe (mode test)

Chaque service possède **sa propre base de données MySQL**.

---

## 🛠️ Technologies utilisées

- ASP.NET Core Web API  
- Entity Framework Core  
- MySQL  
- JWT (authentification)  
- Ocelot (API Gateway)  
- Swagger / OpenAPI  
- Stripe (mode test)  
- Git & GitHub  

---

## ▶️ Petit guide d’exécution (local)

### 1️⃣ Prérequis
- .NET 8
- MySQL
- Visual Studio 2022

---

### 2️⃣ Bases de données
Chaque microservice utilise sa **propre base de données MySQL**.  
Les chaînes de connexion se trouvent dans les fichiers `appsettings.json`.

---

### 3️⃣ Migrations
Pour chaque microservice, exécuter :

