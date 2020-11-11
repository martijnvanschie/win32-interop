$key = Get-Content nuget.key #   'oy2psvwh74gn7txeoiragjktrcurygoepol7ml2hp7vaya'
$source = 'https://api.nuget.org/v3/index.json'

Write-Host "Start packing packages from solution" -ForegroundColor Cyan
dotnet pack '.\Win32 Interop.sln' -o packages

Get-ChildItem "packages" -Filter *.nupkg | 
Foreach-Object {
    Write-Host "Start pushing $($_.FullName)" -ForegroundColor Cyan
    dotnet nuget push $_.FullName --api-key $key --source $source --skip-duplicate
}
