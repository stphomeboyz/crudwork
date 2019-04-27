::@echo on
if %1.==. goto XIT **** Arg1 missing
set STEM=crudwork
set STPDLL=
cd /d %1
del ilmerge.log
ilmerge /xmldocs /wildcards /log:ilmerge.log /keyfile:"%STEM%.snk" /t:library /out:%STEM%.dll /targetplatform:v2 %STEM%\bin\Debug\%STEM%.dll %STEM%\bin\Debug\%STEM%.*.dll %STEM%\bin\Debug\QaDataSetTools.dll
if errorlevel 1 goto ABORT
goto XIT

:ABORT
 @echo Something went wrong!
 type %TMP%\@setdll.bat
 type ilmerge.log
 mawk "BEGIN{exit 1}"
 
:XIT
