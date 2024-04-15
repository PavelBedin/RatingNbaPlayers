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
    REAL PER = 0, -- Player efficiency rating
    REAL TSP = 0, -- True shooting percentage
    REAL TPAR = 0, -- 3 point attempt rate
    REAL FTR = 0, -- Free throw attempt rate
    REAL OREBP = 0, -- Offensive rebounds percentage
    REAL DREBP = 0, -- Defensive rebounds percentage
    REAL REBP = 0, -- Rebounds percentage
    REAL ASTP = 0, -- Percentage of assists
    REAL STLP = 0, -- Percentage of steal
    REAL BLKP = 0, -- Percentage of block
    REAL TOVP = 0, -- Percentage of turnover
    REAL USGP = 0, -- Usage percentage
    REAL OWS = 0, -- Offensive win shares
    REAL DWS = 0, -- Defensive win shares
    REAL WS = 0, -- Win shares
    REAL WS48 = 0, -- Win shares per 48 minutes
    REAL OBPM = 0, -- Offensive box plus/minus
    REAL DBPM = 0, -- Defensive box plus/minus
    REAL BPM = 0, -- Box plus/minus
    REAL VORP = 0 -- Value over replacement player
    FOREIGN KEY (PlayerId) REFERENCES Players (Id)
);