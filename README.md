# 🎬 ChallengeApi

**ChallengeApi** es una API REST desarrollada en **ASP.NET Core** para gestionar información relacionada con películas, como sus géneros, actores y portadas.  
La aplicación utiliza tecnologías modernas como **Entity Framework Core**, **DTOs**, **AutoMapper**, **OData v4** y se ejecuta dentro de un contenedor Docker, junto con una base de datos **MySQL**.

-----------------

## Tecnologías Utilizadas

- **ASP.NET Core**
- **C#**
- **Entity Framework Core**
- **MySQL**
- **REST + OData v4**
- **Patrón DTO**
- **AutoMapper**
- **Docker**

-----------------

##  Requisitos

Antes de empezar, asegurate de tener instalados:

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/) (recomendado)
- Git

-----------------

## 🛠 Cómo levantar el proyecto

## 1. Clonar el repositorio

## bash
-----------------
git clone https://github.com/DevKapo/ChallengeApi.git
cd ChallengeApi
-----------------
 2. Levantar los contenedores con Docker
 Asegurate de que el puerto 5046 no esté ocupado (es el que usa la API). MySQL usará el puerto 3306.

Pon en el cmd docker-compose up --build

Una vez que el contenedor esté corriendo, accedé a:
http://localhost:5046
---
