@echo off
for /f "skip=1" %%x in ('wmic os get localdatetime') do if not defined MyDate set MyDate=%%x
set today=%MyDate:~6,2%.%MyDate:~4,2%.%MyDate:~0,4% 
echo %today%> CodeVersion.txt
PAUSE