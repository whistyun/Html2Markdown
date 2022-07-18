Param($ApiKey, $Source)

dotnet pack -c Release -o .\packages

$nupkgList=dir .\packages\*.nupkg
$nupkg=$nupkgList[0].FullName

dotnet nuget push $nupkg --api-key $ApiKey --source $Source
