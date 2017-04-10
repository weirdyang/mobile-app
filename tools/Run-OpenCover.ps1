param($Configuration,$Token)

$OpenCoverDir = (Get-ChildItem -Path ./source/packages -Filter "OpenCover*")[0].Name
$ExePath = ".\source\packages\$OpenCoverDir\tools\OpenCover.Console.exe"

$NunitDir = (Get-ChildItem -Path ./source/packages -Filter "NUnit*ConsoleRunner*")[0].Name
$NunitExePath = ".\source\packages\$NunitDir\tools\nunit3-console.exe"

Write-Host "Using OpenCover NuGet package $OpenCoverDir"
Write-Host "Exe path: $ExePath"

Write-Host "Running OpenCover"
& $($ExePath) -register:user -target:"$NunitExePath" -targetargs:"--noheader source\LH.Forcas\LH.Forcas\bin\$Configuration\LH.Forcas.dll source\LH.Forcas.Tests\bin\$Configuration\LH.Forcas.Tests.dll" -returntargetcode -filter:"+[LH.Forcas]*" -excludebyattribute:*.ExcludeFromCodeCoverage* -hideskipped:All -output:.\Coverage.xml

# For NUnit Console Runner debugging
#& $NunitExePath .\source\LH.Forcas\LH.Forcas\bin\$Configuration\LH.Forcas.dll .\source\LH.Forcas.Tests\bin\$Configuration\LH.Forcas.Tests.dll --noheader --framework=4.5 --verbose --trace=Verbose

Write-Host "Installing Codecov..."
& pip install codecov

Write-Host "Uploading coverage report..."
& codecov -f "Coverage.xml" -X gcov -t $Token