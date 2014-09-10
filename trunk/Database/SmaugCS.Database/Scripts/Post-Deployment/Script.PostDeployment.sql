/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Populate all reference tables
:r .\POPULATE_RefData_dbo.BanTypes.sql
:r .\POPULATE_RefData_dbo.BoardTypes.sql
:r .\POPULATE_RefData_dbo.LogTypes.sql
:r .\POPULATE_RefData_dbo.NoteVoteTypes.sql
:r .\POPULATE_RefData_dbo.HemisphereTypes.sql
:r .\POPULATE_RefData_dbo.ClimateTypes.sql
:r .\POPULATE_RefData_live.OrganizationTypes.sql
:r .\POPULATE_RefData_live.SkillTypes.sql
:r .\POPULATE_RefData_live.StatisticTypes.sql

-- Populate some data tables (do this for Defaults!)
:r .\POPULATE_DefaultData_dbo.Boards.sql
:r .\POPULATE_DefaultData_live.GameState.sql


-- Populate some data tables (do this for testing!)
:r .\POPULATE_TestData_dbo.Bans.sql

