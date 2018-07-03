function Update-Build([int] $build, [string] $assemblyInfoPath) {
    $tmpFile = "AssemblyInfo.cs"

    Get-Content $assemblyInfoPath | 
    %{ $_ -replace "(Assembly.*Version)\(""([0-9]+).([0-9]+).([0-9]+).[0-9]+""\)", "`$1(""`$2.`$3.`$4.$build"")" } |
    Set-Content $tmpFile

    Move-Item $tmpFile $assemblyInfoPath -Force
}

function Set-Build {
    $build = $env:TRAVIS_BUILD_NUMBER
    Get-ChildItem -Path AssemblyInfo.cs -Recurse | ForEach-Object { Update-Build $build $_ }
}

Set-Build