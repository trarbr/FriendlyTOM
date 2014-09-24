Installation notes
==================

1. Install MS SQL
2. Install Friendly TOM
3. Give permission to the ms sql user: NT SERVICE\MSSQL$SQLEXPRESS

Try and do #3 programmaticly:

- need to know if first-run (app.config) - or just do it the hackisk way
  http://msdn.microsoft.com/en-us/library/a65txexh%28v=vs.110%29.aspx
  http://msdn.microsoft.com/en-us/library/a65txexh%28v=vs.100%29.aspx
- if first-run, run setup dialog, ask for backup dir - needs it for permissions
  or use the exe dir, and add and remove permissions... no way. Not a real
  solution
- set permissions on the folder 
  http://stackoverflow.com/questions/10175270/change-permissions-of-folders


"no database was found. This is probably because first run."
navigate to Settings tab
"please pick a path to store backups, then "
"no database found, please restore factory settings"
knap til restore factory settings - først skal man sætte backupPath

eller bare bruge en default sti, og kopiere install.bak derover, sætte
tilladelser automatisk, hmm...

