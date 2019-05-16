md .\bin\Release\Lib

mv .\bin\Release\*.dll .\bin\Release\Lib -Force
mv .\bin\Release\*.xml .\bin\Release\Lib -Force
mv .\bin\Release\NetOp.TestRunnerLib.dll.config .\bin\Release\Lib -Force

del .\bin\Release\*.pdb

del .\bin\Release\Python\*
rmdir .\bin\Release\Python\
del .\bin\Release\HowTo.cs
del .\bin\Release\Test_16x.ico

copy .\TRAC_noAccountInfo.cfg .\bin\Release\TRAC.cfg -Force

