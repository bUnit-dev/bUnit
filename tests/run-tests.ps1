$maxRuns = $args[0]
$mode = $args[1]
$filter = $args[2]

dotnet build ..\bunit.sln -c $mode --nologo

for ($num = 1 ; $num -le $maxRuns ; $num++)
{
    Write-Output "### TEST RUN $num ###"
	
	if($filter)
	{
		dotnet test .\bunit.core.tests\bunit.core.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f net5.0 --nologo --filter $filter
		dotnet test .\bunit.web.tests\bunit.web.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f net5.0 --nologo --filter $filter
		dotnet test .\bunit.xunit.tests\bunit.xunit.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f net5.0 --nologo --filter $filter
	}
	else
	{
		dotnet test .\bunit.core.tests\bunit.core.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f net5.0 --nologo
		dotnet test .\bunit.web.tests\bunit.web.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f net5.0 --nologo
		dotnet test .\bunit.xunit.tests\bunit.xunit.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f net5.0 --nologo 
	}
}
