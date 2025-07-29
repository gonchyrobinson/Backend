# Git Flow - Guía de Uso

Este proyecto utiliza Git Flow como metodología de branching para organizar el desarrollo de manera estructurada.

## 🏗️ Estructura de Branches

### **Branches Principales**
- **`main`** - Código en producción (estable)
- **`develop`** - Código en desarrollo (integración)

### **Branches de Soporte**
- **`feature/`** - Nuevas características
- **`bugfix/`** - Corrección de bugs
- **`release/`** - Preparación de releases
- **`hotfix/`** - Correcciones urgentes en producción
- **`support/`** - Soporte para versiones antiguas

## 🚀 Comandos Principales

### **Iniciar una Nueva Feature**
```bash
git flow feature start nombre-feature
# Desarrollar...
git flow feature finish nombre-feature
```

### **Iniciar un Bugfix**
```bash
git flow bugfix start nombre-bugfix
# Corregir...
git flow bugfix finish nombre-bugfix
```

### **Crear un Release**
```bash
git flow release start 1.0.0
# Preparar release...
git flow release finish 1.0.0
```

### **Crear un Hotfix**
```bash
git flow hotfix start nombre-hotfix
# Corregir urgentemente...
git flow hotfix finish nombre-hotfix
```

## 📋 Flujo de Trabajo

### **1. Desarrollo de Features**
```bash
# Crear feature desde develop
git flow feature start nueva-funcionalidad

# Desarrollar y hacer commits
git add .
git commit -m "feat: implementar nueva funcionalidad"

# Finalizar feature (merge a develop)
git flow feature finish nueva-funcionalidad
```

### **2. Preparación de Release**
```bash
# Crear release desde develop
git flow release start v1.0.0

# Hacer ajustes finales
git add .
git commit -m "chore: preparar release v1.0.0"

# Finalizar release (merge a main y develop)
git flow release finish v1.0.0
```

### **3. Hotfix Urgente**
```bash
# Crear hotfix desde main
git flow hotfix start correccion-urgente

# Corregir y hacer commit
git add .
git commit -m "fix: corrección urgente"

# Finalizar hotfix (merge a main y develop)
git flow hotfix finish correccion-urgente
```

## 🏷️ Convenciones de Commits

### **Tipos de Commit**
- `feat:` - Nueva característica
- `fix:` - Corrección de bug
- `docs:` - Documentación
- `style:` - Formato, punto y coma faltante, etc.
- `refactor:` - Refactorización de código
- `test:` - Agregar o corregir tests
- `chore:` - Tareas de construcción, configuraciones, etc.

### **Ejemplos**
```bash
git commit -m "feat: agregar autenticación JWT"
git commit -m "fix: corregir error en validación de email"
git commit -m "docs: actualizar README con instrucciones de instalación"
git commit -m "refactor: simplificar lógica de mapeo en StudentService"
```

## 🔄 Flujo de Integración

### **Desarrollo Diario**
1. Trabajar en `feature/` branches
2. Hacer commits frecuentes con mensajes descriptivos
3. Finalizar features y merge a `develop`
4. Integrar cambios en `develop` regularmente

### **Releases**
1. Crear `release/` desde `develop`
2. Hacer ajustes finales (versionado, documentación)
3. Finalizar release (merge a `main` y `develop`)
4. Tag automático con versión

### **Hotfixes**
1. Crear `hotfix/` desde `main`
2. Corregir problema urgente
3. Finalizar hotfix (merge a `main` y `develop`)
4. Tag automático con versión

## 📊 Estado Actual del Proyecto

- **Branch Principal**: `main` (producción)
- **Branch de Desarrollo**: `develop` (integración)
- **Último Commit**: `dd56507` - "first commit"

## 🛠️ Configuración Git Flow

```bash
# Verificar configuración
git flow version

# Ver branches actuales
git branch -a

# Ver configuración de Git Flow
git config --list | grep gitflow
```

## 📝 Notas Importantes

- **Nunca** trabajar directamente en `main` o `develop`
- **Siempre** crear branches específicos para features/bugfixes
- **Usar** mensajes de commit descriptivos
- **Mantener** `develop` estable para integración
- **Taggear** releases con versiones semánticas

## 🎯 Próximos Pasos

1. Crear primera feature: `git flow feature start configuracion-inicial`
2. Desarrollar funcionalidades en branches específicos
3. Integrar cambios en `develop`
4. Preparar releases cuando esté listo
5. Mantener `main` estable para producción