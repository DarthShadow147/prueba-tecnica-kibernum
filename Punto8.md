# PROBLEMAS DE CODIGO LEGADO

## 1. Credenciales "Hardcodeadas"
```C#
string connStr = "Server=PROD-SERVER;Database=IntergalaxyDB;User Id=admin;Password=admin123;";
```

### Problemas
- Exposicion de credenciales
- No configurable

## 2. Riesgo de SQL Injection
```C#
string sql = "INSERT INTO Solicitudes (...) VALUES (" +
ddlPersonaje.SelectedValue + ", '" +
txtSolicitante.Text + "', '" +
txtEvento.Text + "', '" +
txtFechaEvento.Text + "', 0, '" +
DateTime.Now + "')";
```

### Problemas
- Vulnerable a ataques

## 3. Logica mezclada
Todo esta mezclado en una sola capa

- Acceso a datos
- Validaciones
- Logica de negocio
- UI

## 4. No hay manejo de errores
Si falla se rompe toda la ejecucion

