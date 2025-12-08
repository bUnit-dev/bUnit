$maxRuns = $args[0]
$mode = $args[1]
$filter = $args[2]

dotnet build ..\bunit.slnx -c $mode --nologo

for ($num = 1 ; $num -le $maxRuns ; $num++)
{
    Write-Output "### TEST RUN $num ###"
	
	if($filter)
	{
		dotnet test ..\bunit.slnx -c $mode --no-restore --no-build --blame --nologo --filter $filter
	}
	else
	{
		dotnet test ..\bunit.slnx -c $mode --no-restore --no-build --blame --nologo
	}
}
