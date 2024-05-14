# DesafioOriginSW
Desafío técnico. Consiste en una interfaz de tipo ATM (cajero automático) con su respectivo backend.

Este fue desarrollado usando ASP.NET Core API en su versión 7 y para el lado del cliente se creó un proyecto en ReactJS 18 utilizando JavaScript y JSX.

## Cómo configurar el proyecto en su equipo
Se debe clonar o descargar el proyecto desde [Github](https://github.com/RhonalG24/DesafioOriginSW). 


## Creación de la base de datos:
Dentro de la carperta "Recursos" se encuentran dos scripts SQL. 
Para crear la base de datos y las tablas se debe ejecutar dentro de SQLServer el archivo "CreateDatabaseAndTables.sql".
Una vez creada la base de datos y las tablas, se procede a ejecutar el archivo "FillTables.sql", con el cual se crean 3 registros de cuentas, 4 tarjetas bancarias, 3 estados para las tarjetas y 2 tipos de operaciones. 

## Conexión de la solucón con la base de datos SQLServer:
Dentro del archivo appsettings.json se encuentra la cadena de conexión (ConnectionStrings) para la base de datos. Escoja una de las dos dependiendo de si su base de datos se encuentra en la maquina local o si se encuentra en un servidor remoto, seteando los valores correspondientes de su Server, User ID y Password.

## Levantar el servidor de .NET:

Una vez se tenga el proyecto a disposición, para iniciar el servidor de .NET se debe abrir haciendo click en la solución del proyecto "DesafioOriginSW.sln" y haciendo click en ejecutar la solución una vez abierta. El servidor correrá en localhost:7210.

## Levantar el servidor del Cliente React:
Necesitaremos usar la consola de comandos (CMD) para verificar si se encuentran instaladas algunas herramientas. 

#### Abrir la Terminal de Comandos (CMD):
Para abrir la terminal de comando sigue los siguientes pasos:
1. Presiona la tecla Windows
2. Escribe "cmd" (sin comillas) en la barra de búsqueda.
3. Abre el programa "Consola de Comandos" desde los resultados de búsqueda. Esto abrirá la ventana de Consola.

#### Instalación de Node.js
Para verificar si se tiene Node.js instalado en el sistema operativo se debe ejecutar el siguiente comando en el CMD:
```
node -v
```
Esto mostrará la versión de Node.js instalada en su sistema.

En caso de no tener instalado Node.js verás un mensaje de error que te avisa que el comando no se reconoce en el sistema y se debe proceder con los siguientes pasos para instalar Node.js en Windows:

1. Descarga el instalador de Node.js desde el sitio oficial de [Node.js](https://nodejs.org/en/download). 
2. Ejecuta el instalador descargado y sigue las instrucciones del asistente de instalación.
3. Una vez completada la instalación, puedes verificar si Node.js se instaló correctamente abriendo la consola de comandos de Windows y ejecutando el comando node -v. Esto mostrará la versión de Node.js instalada en tu sistema.

Finalmente, para levantar el servidor de React se debe abrir la carpeta DesafioOriginSW_Client desde la terminal de comando haciendo click derecho en la carpeta y seleccionando la opcion "Abril en terminal". Una vez en la terminal se ejecuta el siguiente comando:
```
npm run start
```

El cliente se iniciará en localhost:3000.

La aplicación hace uso de 4 endpoints en total:
- https://localhost:7210//api/BankCard/check/number/{bank_card_number} para verificar si la tarjeta existe en el sistema.
- https://localhost:7210//api/BankCard/check/pin/{bank_card_id} para validar que el pin es correcto. Por cada intento fallido se va actualizando en la base de datos la cantidad total de intentos. Una vez llega a 4 intentos fallidos, se le cambia el estado de la tarjeta a "Bloqueada", lo cual no permite que se pueda pasar de la verificación de la tarjeta.
- https://localhost:7210//api/Operation/balance/{bank_card_id} para realizar la operación de consulta de saldo.
- https://localhost:7210//api/Operation/withdrawal para realizar la operación de retiro de dinero.