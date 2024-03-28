--Creating a table Players
CREATE TABLE IF NOT EXISTS Players
(
    Id   INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);

--Creating a table Rating
CREATE TABLE IF NOT EXISTS Rating
(
    Player_Id INTEGER,
    Rating    INTEGER,
    FOREIGN KEY (Player_Id
        ) REFERENCES Players (Id)

);


--Creating a table Teams
CREATE TABLE IF NOT EXISTS Teams
(
    Id   INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);

--Creating a table Player_In_Team
CREATE TABLE IF NOT EXISTS Player_In_Team
(
    Player_Id INTEGER,
    Team_Id   INTEGER,
    FOREIGN KEY (Player_Id) REFERENCES Players (Id),
    FOREIGN KEY (Team_Id) REFERENCES Teams (Id)
);

--Creating a table Traditional_Statistics
CREATE TABLE IF NOT EXISTS Traditional_Statistics
(
    Player_Id      INTEGER,
    Game_Played    INTEGER,
    Minutes_Played REAL,
    PPG            REAL,    --Points Per Game
    FGM            REAL,    --Field Goals Made
    FGA            REAL,    --Field Goals Attempted
    FGP            REAL,    --Field Goals Percentage
    TPM            REAL,    --3 Point Field Goals Made
    TPA            REAL,    --3 Point Field Goals Attempted
    TPP            REAL,    --3 Point Field Goals Percentage
    FTM            REAL,    --Free Throws Made
    FTA            REAL,    --Free Throws Attempted
    FTP            REAL,    --Free Throws Percentage
    OREB           REAL,    --Offensive Rebounds
    DRED           REAL,    --Defensive Rebounds
    REB            REAL,    --Rebounds
    AST            REAL,    --Assists
    TOV            REAL,    --Turnovers
    STL            REAL,    --Steals
    BLK            REAL,    --Blocks
    PF             REAL,    --Personal Fouls
    DD2            INTEGER, --Double Doubles
    TD3            INTEGER, --Triple Doubles
    PM             REAL,    --Plus-Minus
    FOREIGN KEY (Player_Id) REFERENCES Players (Id)
);

--Creating a table Advanced_Statistics
CREATE TABLE IF NOT EXISTS Advanced_Statistics
(
    Player_Id INTEGER,
    OFFRTG    REAL, --Offensive Rating
    DEFRTG    REAL, --Defensive Rating
    NETRTG    REAL, --NET Rating
    ASTP      REAL, --Assist Percentage
    ASTTO     REAL, --Assist to Turnover Ratio
    ASTR      REAL, --Assist Ratio
    OREBP     REAL, --Offensive Rebounds Percentage
    DREBP     REAL, --Defensive Rebounds Percentage
    REBP      REAL, --Rebounds Percentage
    TOR       REAL, --Turnover Ratio
    EFGP      REAL, --Effective Field Goals Percentage
    TSP       REAL, --True Shooting Percentage
    USGP      REAL, --Usage Percentage
    PACE      REAL,
    PIE       REAL, --Player Impact Estimate
    FOREIGN KEY (Player_Id) REFERENCES Players (Id)
);