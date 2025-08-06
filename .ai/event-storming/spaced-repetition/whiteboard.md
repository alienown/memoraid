```mermaid
flowchart LR
subgraph Legenda
  DE0[Domain Event]
  CMD0[Command]
  RM0[(Read Model)]
  POL0>Policy]
  AGG0{Aggregate}
  HS0/!/
  ACT0((Actor))
  EX0{{External System}}

  style DE0 fill:#FF9900,color:black
  style CMD0 fill:#1E90FF,color:white
  style RM0 fill:#32CD32,color:black
  style POL0 fill:#9932CC,color:white
  style AGG0 fill:#FFFF00,color:black
  style HS0 fill:#FF0000,color:white
  style ACT0 fill:#FFFF00,color:black
  style EX0 fill:#A9A9A9,color:white
end

subgraph "Proces powtórek - Pełny model Event Storming"
  %% Actors
  ACT1((Użytkownik))

  %% External Systems
  EX1{{Algorytm Spaced Repetition}}

  %% Read Models
  RM1[(Lista fiszek użytkownika)]
  RM2[(Dane fiszki)]
  RM3[(Stan sesji powtórek)]

  %% Policies
  POL1>Sprawdź dostępność fiszek]
  POL2>Oblicz następną fiszkę]
  POL3>Zapisz postęp sesji]

  %% Aggregates
  AGG1{Sesja powtórek}

  %% Commands  
  CMD1[Rozpocznij sesję powtórek]
  CMD2[Oceń fiszkę]
  CMD3[Zakończ sesję]

  %% Domain Events
  DE1[Sesja powtórek rozpoczęta]
  DE2[Następna fiszka wybrana]
  DE3[Odpowiedź oceniona]
  DE4[Sesja powtórek zakończona]

  %% Hot Spots
  HS1[/Wydajnosc przy duzej liczbie fiszek/]
  HS2[/Definicja zakonczenia sesji/]

  %% Przepływ Commands -> Aggregate -> Events
  ACT1 -.->|"wykonuje"| CMD1
  RM1 -->|"sprawdza"| POL1
  POL1 -->|"blokuje gdy brak fiszek"| CMD1
  CMD1 -.-> AGG1
  AGG1 --> DE1
  
  %% Automatyczny wybór pierwszej fiszki po rozpoczęciu sesji
  DE1 -->|"wyzwala"| POL2
  
  %% Automatyczny wybór następnej fiszki po ocenie
  DE3 -->|"wyzwala"| POL2
  
  %% Polityka obliczania następnej fiszki z zewnętrznym systemem
  RM1 -->|"używane do wyboru"| POL2
  RM2 -->|"używane do wyboru"| POL2
  RM3 -->|"używane do wyboru"| POL2
  POL2 -->|"wykorzystuje"| EX1
  POL2 ==> AGG1
  AGG1 --> DE2
  
  %% Ocena przez użytkownika
  DE2 --> DE3
  ACT1 -.->|"wykonuje"| CMD2
  CMD2 -.-> AGG1
  AGG1 --> DE3
  
  %% Zakończenie sesji może nastąpić po ocenie
  DE3 --> DE4
  
  %% Zakończenie sesji
  ACT1 -.->|"wykonuje"| CMD3
  CMD3 -.-> AGG1
  AGG1 --> DE4
  
  %% Automatyczny zapis postępu po każdym zdarzeniu
  DE1 -->|"wyzwala"| POL3
  DE2 -->|"wyzwala"| POL3
  DE3 -->|"wyzwala"| POL3
  DE4 -->|"wyzwala"| POL3
  POL3 -->|"zapisuje postęp"| RM3

  %% Hot Spots connections
  HS1 -.->|"dotyczy"| POL2
  HS2 -.->|"dotyczy"| DE4
  HS1 -.->|"dotyczy"| RM1

  %% Style
  style ACT1 fill:#FFFF00,color:black
  style EX1 fill:#A9A9A9,color:white
  style RM1 fill:#32CD32,color:black
  style RM2 fill:#32CD32,color:black
  style RM3 fill:#32CD32,color:black
  style POL1 fill:#9932CC,color:white
  style POL2 fill:#9932CC,color:white
  style POL3 fill:#9932CC,color:white
  style AGG1 fill:#FFFF00,color:black
  style CMD1 fill:#1E90FF,color:white
  style CMD2 fill:#1E90FF,color:white
  style CMD3 fill:#1E90FF,color:white
  style DE1 fill:#FF9900,color:black
  style DE2 fill:#FF9900,color:black
  style DE3 fill:#FF9900,color:black
  style DE4 fill:#FF9900,color:black
  style HS1 fill:#FF0000,color:white
  style HS2 fill:#FF0000,color:white
end