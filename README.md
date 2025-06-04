# Gestion de base de données - Projet VéloMax

Ce projet a pour objectif de concevoir et développer une base de données relationnelle complète pour l'entreprise fictive **VéloMax**, spécialisée dans la vente de vélos et de pièces détachées.

---

## Objectifs pédagogiques

- Modéliser une base de données relationnelle à partir d’un cahier des charges.
- Implémenter la base en SQL (MySQL).
- Créer une application C# pour manipuler la base (version console ou WPF).
- Mettre en place un scénario de démonstration fonctionnel.
- Présenter des requêtes avancées (jointures, unions, requêtes synchronisées).

---

## Technologies utilisées

- **MySQL** pour la base de données.
- **C#** avec Visual Studio pour l’application (WPF ou console).
- **SQL** pour les requêtes et scripts de gestion.
- Optionnel : utilisation de fichiers `.csv`, `.docx`, diagrammes E/A, etc.

---

## Structure du projet
gestion-de-base-donn-es/
├── projet.sln # Solution Visual Studio
├── projet.csproj # Projet C# principal
├── Program.cs # Application principale (console ou WPF)
├── velomax.sql # Script SQL complet de création de la base
├── Rapport de Projet.pdf # Note technique et justificatif des choix
├── Base de donnée.docx # Cahier des charges ou version annotée
├── Schéma E-A/ # Diagrammes entité-association
└── bin/, obj/, .vs/ # Dossiers générés (à ignorer via .gitignore)

---

## Fonctionnalités attendues

- Gestion des **clients particuliers** et **entreprises**
- Gestion des **commandes**, **stocks**, **vélos** et **pièces détachées**
- Suivi des **fournisseurs** et de leurs délais/qualité
- Gestion des **adhésions aux programmes Fidélio**
- Statistiques globales : CA par magasin, bonus des salariés, analyse des ventes
- Alerte automatique sur le **stock bas** et suggestion de réapprovisionnement

---

## Scripts et démonstration

### Scripts SQL
- Création de la base `VeloMax`
- Insertion des tuples
- Gestion des relations avec contraintes
- Utilisateur en lecture seule : `bozo/bozo`
- Utilisateur admin : `root/root`

### Requêtes personnalisées
- 1 requête synchronisée
- 1 auto-jointure
- 1 union

### Démonstration
- En mode **console** avec navigation par menu ou
- En mode **WPF** avec interface (boutons, datagrids, combobox, etc.)

---

## Installation de la base

1. Ouvrir MySQL ou un SGBD compatible.
2. Exécuter le script `velomax.sql` pour créer la base et les tables.
3. Lancer le projet Visual Studio et connecter l'application à la base.

---

## Bonnes pratiques

- Penser à bien gérer les dépendances lors des suppressions (clients, commandes, etc.)
- Prévoir une architecture modulaire du code (fichiers dédiés par entité ou fonctionnalité)
- Utiliser un `.gitignore` pour ne pas versionner les fichiers binaires (`bin/`, `obj/`, `.vs/`, `.pdf`, etc.)

---

## Équipe

Projet réalisé dans le cadre du module Base de Données / C# – 2024  
Encadrant : *à renseigner selon le cours*  
Étudiants : *à compléter (nom/prénom binôme ou trinôme)*

---

## Licence

Ce projet est académique et n’est pas destiné à un usage commercial.

