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
    REAL     PER,   -- Player efficiency rating
    REAL     TSP,   -- True shooting percentage
    REAL     TPAR,  -- 3 point attempt rate
    REAL     FTR,   -- Free throw attempt rate
    REAL     OREBP, -- Offensive rebounds percentage
    REAL     DREBP, -- Defensive rebounds percentage
    REAL     REBP,  -- Rebounds percentage
    REAL     ASTP,  -- Percentage of assists
    REAL     STLP,  -- Percentage of steal
    REAL     BLKP,  -- Percentage of block
    REAL     TOVP,  -- Percentage of turnover
    REAL     USGP,  -- Usage percentage
    REAL     OWS,   -- Offensive win shares
    REAL     DWS,   -- Defensive win shares
    REAL     WS,    -- Win shares
    REAL     WS48,  -- Win shares per 48 minutes
    REAL     OBPM,  -- Offensive box plus/minus
    REAL     DBPM,  -- Defensive box plus/minus
    REAL     BPM,   -- Box plus/minus
    REAL     VORP,  -- Value over replacement player
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);