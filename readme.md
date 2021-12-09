# WildPay

## Introduction

Ce projet s'inscrit dans le cadre de la formation au développement en C# de la Wild Code School, promotion sxb-csharp-0921. Il s'agit de créer une application web permettant de gérer des dépenses dans un groupe ([Voir sujet](https://github.com/WildCodeSchool/2109-CSharp-Projet2-Subject)).

Il s'agit du projet de l'équipe CSK, composée de Cléa, Sébastien et Kévin.

## Mise en place

Pour utiliser ce projet, veuillez suivre les instructions ci-dessous :

1. Récupérer ce repo en local.

2. Ouvrir la solution dans Visual Studio.

3. Installer les packages nécessaires.

4. Ouvrir la console de gestion des packages nugets et exécuter la commande "Update-Database". Cela devrait créer la base de données automatiquement. Vous pouvez vérifier que cette action s'est bien déroulée en recherchant la base de données SQL nommée "WildPay-1" sur votre ordinateur (dans l'explorateur de serveur de votre choix, sur le serveur localhost).

5. Démarrez le projet sur le sevreur IIS depuis Visual Studio.

6. Amusez-vous :)

## Fonctionnalités

Une fois un compte créé, vous pourrez profiter des fonctionnalités de l'application. Par défaut, tous les utilisateurs sont présents dans un même groupe.

Pour l'instant, la création de plusieurs groupes n'est pas implémentée mais cela reste envisageable pour l'évolution du projet. Les bases sont déjà posées pour le permettre, il suffira d'y apporter quelques ajustements.

Vous pourrez gérer les informations de votre compte, ajouter, modifier ou supprimer des dépenses, et gérer les catégories du groupe.

Par défaut, 4 catégories sont présentes sur le groupe et ne sont pas supprimables :

- Courses
- Hébergement
- Restaurant
- Transport

Une dépense peut ne pas avoir de catégorie. A la suppression d'une catégorie, les dépenses ayant cette catégorie seront conservées mais il faudra resaisir manuellement les catégories des dépenses impactées.

L'application a été adaptée pour être responsive sur différents appareils.

## Retour

Nous serions ravis d'avoir votre retour sur cette application ! :)
Merci !
