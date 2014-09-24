DECLARE @spid varchar(10)
SELECT @spid = spid
FROM master.sys.sysprocesses
where dbid = DB_ID('FTOM')
BEGIN
  EXEC('KILL ' + @spid)
END

use master
restore database FTOM
from disk = 'C:\Users\troels\baks\15-09-2014.bak'