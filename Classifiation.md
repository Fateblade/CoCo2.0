# Ziel
Hilfestellung bei der Einteilung der von Logik-Komponenten in die vier Bereiche **Business**, **Domain**,**Foundation** und **Integration**.
# Definitionen
## Business
Hier werden alle Komponenten gelistet, die sich um Workflows kümmern. Diese verwenden meist verschiedene Komponenten aus dem Domain Bereich.
## Domain
Hier werden alle Komponenten gelistet, die sich um die Kern-Domänen-Funktionalitäten kümmern. Die einzelnen Komponenten dürften im Normalfall untereinander keinerlei Kopplung haben.
## Foundation
Hier werden alle Komponenten gelistet, die (Low-Level)-Funktionalitäten bereitstellen, aber nichts mit der Domäne zu tun haben. 
Beispielsweise eine Localization-Komponente oder Security-Komponente.
## Integration
Hier werden alle Komponenten gelistet, deren Zweck die Kommunikation/Verwendung mit anderen (Legacy-)Anwendungen ist. 

# Quelle
Klassifizierung grob übernommen aus: https://www.youtube.com/watch?v=dNKOo_bd17M&t=2715s
