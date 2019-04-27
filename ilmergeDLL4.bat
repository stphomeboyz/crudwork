::@echo on
if %1.==. goto XIT **** Arg1 missing
set STEM=__ Installers\crudworkMerged_vs2010
set STPDLL=
set MERGED_DLL=crudwork_v40
cd /d %1
del %MERGED_DLL%_merge.log
ilmerge /xmldocs /wildcards /log:%MERGED_DLL%_merge.log /keyfile:%MERGED_DLL%.snk /t:library /out:%MERGED_DLL%.dll /targetplatform:v4 "%STEM%\bin\Debug"\crudwork.dll "%STEM%\bin\Debug"\crudwork.*.dll "%STEM%\bin\Debug"\QaDataSetTools.dll
if errorlevel 1 goto ABORT
goto XIT

:ABORT
 @echo Something went wrong!
 type %MERGED_DLL%_merge.log
 mawk "BEGIN{exit 1}"
 
:XIT
