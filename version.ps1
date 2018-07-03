function Update-Version([string] $version, [string] $assemblyInfoPath) {
    $tmpFile = "AssemblyInfo.cs"

    Get-Content $assemblyInfoPath | 
    %{ $_ -replace "(Assembly.*Version)\(""[0-9]+.[0-9]+.[0-9]+.[0-9]+""\)", "`$1(""$version"")" } |
    Set-Content $tmpFile

    Move-Item $tmpFile $assemblyInfoPath -Force
}

function Update-InstallerVersion([string] $version) {
    $tmpFile = "installer.wxs"

    $path = ".\unit4-automation.wxs"

    Get-Content $path | 
    % { $_ -replace "\<\?define ProductVersion=""1.0.0""\?\>", "<?define ProductVersion=""$version""?>" } |
    Set-Content $tmpFile

    Move-Item $tmpFile $path -Force
}

function Set-Version () {
    $version = if (Test-Path env:UNIT4_AUTOMATION_VERSION) { $env:UNIT4_AUTOMATION_VERSION } else { "1.0.0" }
    $build = if (Test-Path env:TRAVIS_BUILD_NUMBER) { $env:TRAVIS_BUILD_NUMBER } else { 0 }
    
    $assemblyVersion = "$version.$build"
    
    Get-ChildItem -Path AssemblyInfo.cs -Recurse | ForEach-Object { Update-Version $assemblyVersion $_ }

    Update-InstallerVersion $version
}

Set-Version