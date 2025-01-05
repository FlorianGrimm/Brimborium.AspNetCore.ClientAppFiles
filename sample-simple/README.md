# sample-simple - Brimborium.AspNetCore.ClientAppFiles

Steps to create:

1. Create a Solution

    ```cmd
    mkdir sample-i18n
    cd sample-i18n
    
    mkdir ClientApp
    dotnet new sln
    ```

1. Create a WebApp

    ```cmd
    mkdir WebApp
    cd WebApp

    dotnet new webapi --auth Windows

    cd ..

    dotnet solution add WebApp
    ```

1. Create a ClientApp

    Create the Angular App choose No Server-Side Rendering

    ```cmd
    ng new ClientApp
    ```

1. optional add the ClientApp to the solution

    This depends if you want that the ClientApp is shown in VisualStudio or not.
    If you are not sure skip this.

    ```cmd
    cd ClientApp

    dotnet new classlib

    cd ..

    dotnet solution add ClientApp
    ```

1. Change ClientApp/angular.json

    If you are using Angular V19 (or higher???)
    a. You have to adjust the builder.

    ```diff
    - "builder": "@angular-devkit/build-angular:application",
    + "builder": "@angular-devkit/build-angular:browser",
    ```

    Change the outputpath and add the basehref.

    ```diff
    - "outputPath": "dist/client-app",
    + "outputPath": "../WebApp/wwwRoot/app",
    + "baseHref": "/app/",
    ```

    b. Adjust the entry point

    ```diff
    - "browser": "src/main.ts",
    + "main": "src/main.ts",
    ```

    The result looks like:

    ```json
    {
        "$schema": "./node_modules/@angular/cli/lib/config/schema.json",
        "version": 1,
        "newProjectRoot": "projects",
        "projects": {
            "ClientApp": {
            "projectType": "application",
            "schematics": {
                "@schematics/angular:component": {
                "style": "scss"
                }
            },
            "root": "",
            "sourceRoot": "src",
            "prefix": "app",
            "architect": {
                "build": {
                "builder": "@angular-devkit/build-angular:browser",
                "options": {
                    "outputPath": "../WebApp/wwwRoot/app/",
                    "baseHref": "/app/",
                    "index": "src/index.html",
                    "main": "src/main.ts",
                    "extractLicenses": false,
                    "polyfills": [
    ```
