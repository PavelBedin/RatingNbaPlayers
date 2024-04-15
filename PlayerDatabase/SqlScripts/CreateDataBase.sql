--Creating a table Players
CREATE TABLE IF NOT EXISTS Players
(
    Id   INTEGER PRIMARY KEY,
    Name TEXT NOT NULL
);

--Creating a table Rating
CREATE TABLE IF NOT EXISTS Rating
(
    PlayerId INTEGER,
    Rating    INTEGER,
    FOREIGN KEY (PlayerId
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
    PlayerId INTEGER,
    TeamId   INTEGER,
    FOREIGN KEY (PlayerId) REFERENCES Players (Id),
    FOREIGN KEY (TeamId) REFERENCES Teams (Id)
);

--Creating a table Traditional_Statistics
CREATE TABLE IF NOT EXISTS Traditional_Statistics
(
    PlayerId      INTEGER,
    GamePlayed    INTEGER,
    MinutesPlayed REAL,
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
    PF             REAL     --Personal Fouls
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);

--Creating a table Advanced_Statistics
CREATE TABLE IF NOT EXISTS Advanced_Statistics
(
    PlayerId INTEGER,
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
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);