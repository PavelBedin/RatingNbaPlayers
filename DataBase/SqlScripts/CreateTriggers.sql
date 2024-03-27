-- Create a trigger to check the uniqueness of the name 
-- when adding an entry to the Players table
CREATE TRIGGER IF NOT EXISTS prevent_duplicate_player_name
    BEFORE INSERT
    ON Players
    FOR EACH ROW
BEGIN
    SELECT CASE
               WHEN EXISTS (SELECT 1 FROM Players WHERE Name = NEW.Name) THEN
                   RAISE(IGNORE)
               END;
END;


-- Create a trigger to check the uniqueness of the team name 
-- when adding an entry to the Teams table
CREATE TRIGGER IF NOT EXISTS prevent_duplicate_team_name
    BEFORE INSERT
    ON Teams
    FOR EACH ROW
BEGIN
    SELECT CASE
               WHEN EXISTS (SELECT 1 FROM Players WHERE Name = NEW.Name) THEN
                   RAISE(IGNORE)
               END;
END;

-- Create a trigger to check for an existing record
-- when adding an entry to the Rating table
CREATE TRIGGER IF NOT EXISTS update_existing_player_rating
    BEFORE INSERT
    ON Rating
    FOR EACH ROW
BEGIN
    UPDATE Rating
    SET Rating = NEW.Rating
    WHERE Player_Id = NEW.Player_Id;

    SELECT CASE
               WHEN changes() > 0 THEN
                   RAISE(IGNORE)
               END;
END;


-- Creating a trigger to check for an existing record
-- when adding to the Player_In_Team table
CREATE TRIGGER IF NOT EXISTS update_existing_player_in_team
    BEFORE INSERT
    ON Player_In_Team
    FOR EACH ROW
BEGIN
    UPDATE Player_In_Team
    SET Team_Id = NEW.Team_Id
    WHERE Player_Id = NEW.Player_Id;

    SELECT CASE
               WHEN changes() > 0 THEN
                   RAISE(IGNORE)
               END;
END;


-- Creating a trigger to check for an existing record 
-- when adding to the Traditional_Statistics table
CREATE TRIGGER IF NOT EXISTS update_existing_player_in_traditional_statistics
    BEFORE INSERT
    ON Traditional_Statistics
    FOR EACH ROW
BEGIN
    UPDATE Traditional_Statistics
    SET Game_Played    = NEW.Game_Played,
        Minutes_Played = NEW.Minutes_Played,
        PPG            = NEW.PPG,
        FGM            = NEW.FGM,
        FGA            = NEW.FGA,
        FGP            = NEW.FGP,
        TPM            = NEW.TPM,
        TPA            = NEW.TPA,
        TPP            = NEW.TPP,
        FTM            = NEW.FTM,
        FTA            = NEW.FTA,
        FTP            = NEW.FTP,
        OREB           = NEW.OREB,
        DRED           = NEW.DRED,
        REB            = NEW.REB,
        AST            = NEW.AST,
        TOV            = NEW.TOV,
        STL            = NEW.STL,
        BLK            = NEW.BLK,
        PF             = NEW.PF,
        DD2            = NEW.DD2,
        TD3            = NEW.TD3,
        PM             = NEW.PM
    WHERE Player_Id = NEW.Player_Id;

    SELECT CASE
               WHEN changes() > 0 THEN
                   RAISE(IGNORE)
               END;
END;

-- Creating a trigger to check for an existing record 
-- when adding to the Advanced_Statistics table
CREATE TRIGGER IF NOT EXISTS update_existing_player_in_advanced_statistics
    BEFORE INSERT
    ON Advanced_Statistics
    FOR EACH ROW
BEGIN
    UPDATE Advanced_Statistics
    SET OFFRTG = NEW.OFFRTG,
        DEFRTG = NEW.DEFRTG,
        NETRTG = NEW.NETRTG,
        ASTP   = NEW.ASTP,
        ASTTO  = NEW.ASTTO,
        ASTR   = NEW.ASTR,
        OREBP  = NEW.OREBP,
        DREBP  = NEW.DREBP,
        REBP   = NEW.REBP,
        TOR    = NEW.TOR,
        EFGP   = NEW.EFGP,
        TSP    = NEW.TSP,
        USGP   = NEW.USGP,
        PACE   = NEW.PACE,
        PIE    = NEW.PIE
    WHERE Player_Id = NEW.Player_Id;

    SELECT CASE
               WHEN changes() > 0 THEN
                   RAISE(IGNORE)
               END;
END;