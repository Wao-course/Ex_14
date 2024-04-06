```mermaid
erDiagram
    Product ||--o{ Recommendation : "Has"
    Cart ||--|{ Product : "Contains"
    StatsEntry ||--|{ Product : "Includes"
    StatsEntry ||--|{ Search : "Records"

```	