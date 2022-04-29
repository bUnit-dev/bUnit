$maxRuns = $args[0]
$mode = $args[1]
$filter = $args[2]

dotnet build ..\bunit.sln -c $mode --nologo

for ($num = 1 ; $num -le $maxRuns ; $num++)
{
    Write-Output "### TEST RUN $num ###"
	
	if($filter)
	{
		dotnet test .\bunit.core.tests\bunit.core.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s --nologo --filter $filter --logger:"console;verbosity=normal"
		dotnet test .\bunit.web.tests\bunit.web.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s --nologo --filter $filter --logger:"console;verbosity=normal"
		dotnet test .\bunit.web.testcomponents.tests\bunit.web.testcomponents.teststests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s --nologo --filter $filter --logger:"console;verbosity=normal"
	}
	else
	{
		dotnet test .\bunit.core.tests\bunit.core.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s --nologo --logger:"console;verbosity=normal"
		dotnet test .\bunit.web.tests\bunit.web.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s --nologo --logger:"console;verbosity=normal"
		dotnet test .\bunit.web.testcomponents.tests\bunit.web.testcomponents.tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s --nologo --logger:"console;verbosity=normal"
	}
}
