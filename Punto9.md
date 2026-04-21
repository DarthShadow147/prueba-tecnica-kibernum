# PREGUNTAS DE LIDERAZGO TECNICO
## ¿Cómo planificarías la migración completa del sistema legado en etapas graduales?

Planificaria la migracion utilizando el patron "Strangler Fig", y los principales objetivos serian
- Identificar modulos independientes del sistema legacy
- Priorizar los que tienen mayor impacto o menor dependencia
- Implementacion de nuevos servicios de forma paralela
- Redirigir el trafico gradualmente desde el legacy a lo nuevo
- Validacion en cada etapa

Esto permite reducir riesgos, evitar interrupciones del servicio y asegurar una transicion controlada.

## ¿Qué estrategia usarías si el sistema legado debe operar en paralelo durante la transición?
- Implementar un API Gateway, centralizar llamadas externas o generar una capa de enrutamiento
- Sincronizar datos entre sistemas si es necesario
- Monitorear comportamiento y errores en produccion
- Plan de rollback, definir cómo volver al sistema legado si la nueva arquitectura presenta fallos

## ¿Cómo organizarías a un equipo de 3 desarrolladores para este módulo?

*Roles*
1. Desarrollador A – Integración y Legacy Bridge
- Encargado de analizar el sistema heredado y construir las interfaces de integración (APIs, adaptadores, conectores).
- Su foco es garantizar que el módulo nuevo pueda convivir con el legado sin romper la operación.
- Responsable de pruebas de compatibilidad y sincronización de datos.

2. Desarrollador B – Nueva Arquitectura / Core
- Diseña y desarrolla el módulo en la nueva arquitectura (ej. microservicio en .NET Core).
- Implementa patrones modernos (CQRS, SAGA, etc.) y asegura buenas prácticas de escalabilidad.
- Responsable de documentación técnica del nuevo código.

3. Desarrollador C – QA / Automatización / DevOps
- Configura pipelines de CI/CD, pruebas automatizadas y despliegues paralelos (blue-green, canary).
- Supervisa métricas de rendimiento y asegura rollback rápido en caso de fallos.
- Responsable de monitoreo y alarmas para detectar inconsistencias entre legacy y nuevo módulo.

*Code review*
- Pull Request obligatorio
- Aprobaciones antes de un merge
- Validacion de buenas practicas, cobertura en pruebas, consistencia con arquitectura