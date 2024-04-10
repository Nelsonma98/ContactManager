Junto a la Api agregué un coleccion de Postman para facilitar la revisión de su funcionamiento.
Decidí crear los tokens de autentificacion en la misma Api.
Agregué las rutas:\
http://localhost:5052/api/auth/register\
para registrar un usuario, y:\
http://localhost:5052/api/auth/login\
para loguearse y así obtener el token.
Añadí un campo Role a los Usuarios.
Para la hora de eliminar un Contacto, ya que solo lo pueden hacer los administradores de Cuba, en el analisis de la nacionalidad del usuario decidí hacerlo mediante la información que ofrece el IP a la hora del login. Si se prueba de forma local el IP no ofrecerá tal información. Por tanto, en el archivo AuthController.cs de la carpeta Controllers la linea 48 toma esta información del IP de donde se realiza la petición, en la linea 49 se brinda un IP de Cuba para pruebas locales, en la linea 50 se brinda un IP de Cuba para pruebas locales, solo bastaría descomentar la linea que se desea utilizar y comentar las otras dos.
