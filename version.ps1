function Update-Version([string] $version, [string] $assemblyInfoPath) {
    $tmpFile = "AssemblyInfo.cs"

    Get-Content $assemblyInfoPath | 
    %{ $_ -replace "(Assembly.*Version)\(""[0-9]+.[0-9]+.[0-9]+.[0-9]+""\)", "`$1(""$version"")" } |
    Set-Content $tmpFile

    Move-Item $tmpFile $assemblyInfoPath -Force
}

function Set-Version ([string] $version) {
    $build = if (Test-Path env:TRAVIS_BUILD_NUMBER) { $env:TRAVIS_BUILD_NUMBER } else { 0 }
    
    $assemblyVersion = "$version.$build"

    Get-ChildItem -Path AssemblyInfo.cs -Recurse | ForEach-Object { Update-Version $assemblyVersion $_ }
}

Set-Version "1.0.0"