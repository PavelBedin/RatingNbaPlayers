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
    Rating   INTEGER,
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
    PPG           REAL, --Points Per Game
    FGM           REAL, --Field Goals Made
    FGA           REAL, --Field Goals Attempted
    FGP           REAL, --Field Goals Percentage
    TPM           REAL, --3 Point Field Goals Made
    TPA           REAL, --3 Point Field Goals Attempted
    TPP           REAL, --3 Point Field Goals Percentage
    FTM           REAL, --Free Throws Made
    FTA           REAL, --Free Throws Attempted
    FTP           REAL, --Free Throws Percentage
    OREB          REAL, --Offensive Rebounds
    DRED          REAL, --Defensive Rebounds
    REB           REAL, --Rebounds
    AST           REAL, --Assists
    TOV           REAL, --Turnovers
    STL           REAL, --Steals
    BLK           REAL, --Blocks
    PF            REAL, --Personal Fouls
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);

--Creating a table Advanced_Statistics
CREATE TABLE IF NOT EXISTS Advanced_Statistics
(
    PlayerId INTEGER,
    PER      REAL, -- Player efficiency rating
    TSP      REAL, -- True shooting percentage
    TPAR     REAL, -- 3 point attempt rate
    FTR      REAL, -- Free throw attempt rate
    OREBP    REAL, -- Offensive rebounds percentage
    DREBP    REAL, -- Defensive rebounds percentage
    REBP     REAL, -- Rebounds percentage
    ASTP     REAL, -- Percentage of assists
    STLP     REAL, -- Percentage of steal
    BLKP     REAL, -- Percentage of block
    TOVP     REAL, -- Percentage of turnover
    USGP     REAL, -- Usage percentage
    OWS      REAL, -- Offensive win shares
    DWS      REAL, -- Defensive win shares
    WS       REAL, -- Win shares
    WS48     REAL, -- Win shares per 48 minutes
    OBPM     REAL, -- Offensive box plus/minus
    DBPM     REAL, -- Defensive box plus/minus
    BPM      REAL, -- Box plus/minus
    VORP     REAL, -- Value over replacement player
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);