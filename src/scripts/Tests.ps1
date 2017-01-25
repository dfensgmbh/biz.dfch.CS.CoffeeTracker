try{
	Add-Type -Path "..\biz.dfch.CS.CoffeeTracker.Core\bin\Debug\biz.dfch.CS.CoffeeTracker.Core.dll";
	Write-Host "success";
}
catch
{
	Write-Host "failed";
    write-host $_.Exception.LoaderExceptions;
}