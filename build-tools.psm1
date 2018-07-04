function Restore {
    nuget.exe restore .
}

function Build {
    Restore
    msbuild.exe .\Unit4.sln

    & ".\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4.Automation.Tests\bin\Debug\Unit4.Automation.Tests.dll --where="cat == RequiresExcelInstall"
}

function Test {
    & ".\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4.Automation.Tests\bin\Debug\Unit4.Automation.Tests.dll
}

function Help {
    & ".\Unit4\bin\Debug\unit4-automation.exe" help
}

function Installer {
    & ".\packages\WiX.3.11.1\tools\candle.exe" .\unit4-automation.wxs
    & ".\packages\WiX.3.11.1\tools\light.exe" -sice:ICE91 .\unit4-automation.wixobj
}

function Install {
    msiexec.exe /i unit4-automation.msi /passive
}

function Uninstall {
    msiexec.exe /x unit4-automation.msi /passive
}

function Inspect {
    inspectcode.exe -f=Html --output=InspectCodeReport.html --profile=Unit4.DotSettings .\Unit4.sln
    .\InspectCodeReport.html
}

function Release([string] $version) {
    If (-Not ($version -Match "^[0-9]+.[0-9]+.[0-9]+$")) {
        Write-Host "Invalid version"
        Return
    }

    $releaseBranch = "master"

    If (-Not (git branch | Where { $_ -match "(\* )(.*)" } | ForEach { $matches[2] -eq $releaseBranch })) {
        Write-Host "Not on $releaseBranch"
        Return
    }

    $lastVersion = git tag | Select -Last 1
    If (-Not ([System.Version]$version -gt [System.Version]$lastVersion)) {
        Write-Host "$version is not greater than $lastVersion"
        Return
    }

    $tmpFile = "updated-travis.wxs"
    $path = ".\.travis.yml"

    Get-Content $path | 
    % { $_ -replace "- UNIT4_AUTOMATION_VERSION=[0-9]+.[0-9]+.[0-9]+", "- UNIT4_AUTOMATION_VERSION=$version" } |
    Set-Content $tmpFile
    Move-Item $tmpFile $path -Force

    git stage .travis.yml
    git commit -m "Update version to $version"
    git tag -a $version -m "$version"

    git push origin $releaseBranch
    git push origin $version
}

export-modulemember -function Restore
export-modulemember -function Build
export-modulemember -function Test
export-modulemember -function Run
export-modulemember -function Help
export-modulemember -function Installer
export-modulemember -function Install
export-modulemember -function Uninstall
export-modulemember -function Inspect
export-modulemember -function Release